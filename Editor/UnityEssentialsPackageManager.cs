#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEssentials
{
    public static class UnityEssentialsPackageManager
    {
        private const int HttpTimeoutSeconds = 30;

        [Serializable]
        public class Owner { public string login; }

        [Serializable]
        public class Repo
        {
            public string name;
            public string default_branch;
            public Owner owner;
        }

        [Serializable]
        private class RepoListWrapper { public Repo[] items; }

        [Serializable]
        private class PackageJson
        {
            public string name;
        }

        [Serializable]
        public class PackageMeta
        {
            public string packageName;
            public List<string> dependencies = new();
        }

        /// <summary>
        /// Validates that a repo is a UPM package by reading its package.json and extracting the package name.
        /// </summary>
        public static bool TryGetPackageNameFromRepo(Repo repo, out string packageName, out string reason)
        {
            packageName = null;
            reason = null;

            if (repo == null || string.IsNullOrEmpty(repo.name) || string.IsNullOrEmpty(repo.default_branch))
            {
                reason = "Invalid repo metadata";
                return false;
            }

            var packageJsonUrl = $"https://raw.githubusercontent.com/{repo.owner?.login}/{repo.name}/{repo.default_branch}/package.json";
            var json = HttpGet(packageJsonUrl, out long code, out var _);
            if (code == 403)
            {
                reason = "Access forbidden while fetching package.json (rate limit?)";
                return false;
            }

            if (code < 200 || code >= 300 || string.IsNullOrEmpty(json))
            {
                reason = "package.json missing or unreachable";
                return false;
            }

            try
            {
                var pj = JsonUtility.FromJson<PackageJson>(json);
                if (pj == null || string.IsNullOrEmpty(pj.name))
                {
                    reason = "package.json has no 'name'";
                    return false;
                }

                if (!pj.name.StartsWith("com.", StringComparison.OrdinalIgnoreCase))
                {
                    reason = $"invalid package name '{pj.name}' (must start with 'com.')";
                    return false;
                }

                packageName = pj.name;
                return true;
            }
            catch (Exception e)
            {
                reason = "Failed to parse package.json: " + e.Message;
                return false;
            }
        }

        /// <summary>
        /// Reads the repo's package.json and returns package name + dependency package names (UPM dependencies).
        /// </summary>
        public static bool TryGetPackageMetaFromRepo(Repo repo, out PackageMeta meta, out string reason)
        {
            meta = null;
            reason = null;

            if (repo == null || string.IsNullOrEmpty(repo.name) || string.IsNullOrEmpty(repo.default_branch))
            {
                reason = "Invalid repo metadata";
                return false;
            }

            var packageJsonUrl = $"https://raw.githubusercontent.com/{repo.owner?.login}/{repo.name}/{repo.default_branch}/package.json";
            var json = HttpGet(packageJsonUrl, out long code, out var _);
            if (code == 403)
            {
                reason = "Access forbidden while fetching package.json (rate limit?)";
                return false;
            }

            if (code < 200 || code >= 300 || string.IsNullOrEmpty(json))
            {
                reason = "package.json missing or unreachable";
                return false;
            }

            try
            {
                var pj = JsonUtility.FromJson<PackageJson>(json);
                if (pj == null || string.IsNullOrEmpty(pj.name))
                {
                    reason = "package.json has no 'name'";
                    return false;
                }

                if (!pj.name.StartsWith("com.", StringComparison.OrdinalIgnoreCase))
                {
                    reason = $"invalid package name '{pj.name}' (must start with 'com.')";
                    return false;
                }

                var m = new PackageMeta { packageName = pj.name };

                // JsonUtility can't deserialize the dependencies object map, so extract keys from raw json.
                foreach (var dep in ExtractUpmDependencyKeys(json))
                {
                    if (!m.dependencies.Contains(dep))
                        m.dependencies.Add(dep);
                }

                meta = m;
                return true;
            }
            catch (Exception e)
            {
                reason = "Failed to parse package.json: " + e.Message;
                return false;
            }
        }

        private static readonly Regex DependenciesBlockRegex =
            new Regex("\\\"dependencies\\\"\\s*:\\s*\\{(?<body>[^}]*)\\}", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex DependencyKeyRegex =
            new Regex("\\\"(?<key>com\\.[^\\\"]+)\\\"\\s*:\\s*\\\"[^\\\"]*\\\"", RegexOptions.Compiled);

        private static IEnumerable<string> ExtractUpmDependencyKeys(string packageJson)
        {
            if (string.IsNullOrEmpty(packageJson))
                yield break;

            // Grab the first "dependencies": { ... } block (UPM format).
            var m = DependenciesBlockRegex.Match(packageJson);
            if (!m.Success)
                yield break;

            var body = m.Groups["body"].Value;
            if (string.IsNullOrEmpty(body))
                yield break;

            var matches = DependencyKeyRegex.Matches(body);
            for (int i = 0; i < matches.Count; i++)
            {
                var key = matches[i].Groups["key"].Value;
                if (string.IsNullOrEmpty(key))
                    continue;

                // Only package-like keys.
                if (!key.StartsWith("com.", StringComparison.OrdinalIgnoreCase))
                    continue;

                yield return key;
            }
        }

        public static List<Repo> FetchAllRepos(string user)
        {
            var all = new List<Repo>();
            int page = 1;
            while (page < 20)
            {
                string url = $"https://api.github.com/users/{user}/repos?per_page=100&page={page}";
                var json = HttpGet(url, out long code, out Dictionary<string, string> headers);

                if (code == 403 && headers != null && headers.TryGetValue("x-ratelimit-remaining", out var remaining) && remaining == "0")
                {
                    Debug.LogError("GitHub rate limit exceeded. Try again later.");
                    return null;
                }

                if (string.IsNullOrEmpty(json)) break;

                var wrapped = "{\"items\":" + json + "}";
                RepoListWrapper wrapper;
                try
                {
                    wrapper = JsonUtility.FromJson<RepoListWrapper>(wrapped);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to parse GitHub response: " + e);
                    return null;
                }

                if (wrapper?.items == null || wrapper.items.Length == 0) break;
                all.AddRange(wrapper.items);
                page++;
            }
            return all;
        }

        private static string HttpGet(string url, out long responseCode, out Dictionary<string, string> responseHeaders)
        {
            responseHeaders = null;

            using (var req = UnityWebRequest.Get(url))
            {
                req.timeout = HttpTimeoutSeconds;
                req.SetRequestHeader("User-Agent", "UnityEssentialsPackageInstaller");

                var op = req.SendWebRequest();
                var start = DateTime.UtcNow;
                while (!op.isDone)
                {
                    Thread.Sleep(10);
                    if ((DateTime.UtcNow - start).TotalSeconds > HttpTimeoutSeconds + 5)
                        break;
                }

                bool ok = req.result == UnityWebRequest.Result.Success;

                responseCode = req.responseCode;
                try { responseHeaders = req.GetResponseHeaders(); } catch { }

                return ok ? req.downloadHandler.text : null;
            }
        }
    }
}
#endif