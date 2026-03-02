#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace UnityEssentials
{
    /// <summary>
    /// Window to install/update UnityEssentials packages hosted as public GitHub repos.
    /// </summary>
    public sealed class UnityEssentialsPackageManagerWindow : EditorWindow
    {
        private const string GitHubUser = "CanTalat-Yakan";

        private sealed class PackageAddFailure
        {
            public string repoName;
            public string packageName;
            public string gitUrl;
            public bool wasInstalled;
            public string error;
            public string retryError;
            public bool retrySucceeded;
        }

        private Vector2 _scroll;
        private string _filter;
        private bool _isFetching;
        private bool _isInstalling;

        private readonly List<UnityEssentialsPackageManager.Repo> _repos = new();
        private readonly List<string> _repoDisplayNames = new();
        private readonly List<bool> _userSelected = new();

        private readonly Dictionary<string, UnityEssentialsPackageManager.PackageMeta> _metaByRepoName = new(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _repoNameByPackageName = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _forcedRepoNames = new(StringComparer.Ordinal);

        private readonly Dictionary<string, UnityEssentialsPackageManager.Repo> _repoByName = new(StringComparer.Ordinal);

        [MenuItem("Tools/UnityEssentials/Install & Update", priority = -10000)]
        public static void ShowWindow()
        {
            var w = GetWindow<UnityEssentialsPackageManagerWindow>(true, "UnityEssentials Package Manager", true);
            w.minSize = new Vector2(400, 500);
            w.Show();
        }
        
        private void OnEnable()
        {
            if (_repos.Count == 0 && !_isFetching)
                FetchRepositories();
        }

        private void OnGUI()
        {
            DrawHeader();
            GUILayout.Space(6);
            DrawBody();
            GUILayout.Space(6);
            DrawFooter();
        }

        private void DrawHeader()
        {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Space(4);

                string oldFilter = _filter;
                _filter = GUILayout.TextField(_filter ?? string.Empty, EditorStyles.toolbarSearchField);
                if (!string.Equals(_filter, oldFilter, StringComparison.Ordinal))
                    ApplyFilterAndSelectionReset();

                GUILayout.Space(4);

                using (new EditorGUI.DisabledScope(_isFetching || _isInstalling))
                {
                    if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(100)))
                    {
                        FetchRepositories();
                        GUI.FocusControl(null);
                    }
                }
            }
        }

        private void DrawBody()
        {
            if (_isFetching)
            {
                EditorGUILayout.HelpBox("Fetching repositories from GitHub…", MessageType.Info);
                return;
            }

            if (_repos.Count == 0)
            {
                EditorGUILayout.HelpBox("No repositories found. Hit Refresh.", MessageType.Warning);
                return;
            }

            using (new GUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(_isInstalling))
                {
                    if (GUILayout.Button("All", GUILayout.Width(48)))
                        SetAllUserSelection(true);
                    if (GUILayout.Button("None", GUILayout.Width(56)))
                        SetAllUserSelection(false);
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label($"Total Repositories: {_repoDisplayNames.Count}", EditorStyles.miniBoldLabel);
            }

            GUILayout.Space(4);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            for (int i = 0; i < _repoDisplayNames.Count; i++)
            {
                if (i >= _userSelected.Count)
                    _userSelected.Add(false);

                var repoName = _repoDisplayNames[i];
                bool isForced = _forcedRepoNames.Contains(repoName);
                bool effectiveChecked = _userSelected[i] || isForced;

                using (new EditorGUI.DisabledScope(_isInstalling || isForced))
                {
                    bool newValue = EditorGUILayout.ToggleLeft(repoName, effectiveChecked);
                    if (!isForced && newValue != _userSelected[i])
                    {
                        _userSelected[i] = newValue;
                        RecomputeForcedStates();
                        Repaint();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawFooter()
        {
            if (_isFetching)
                return;

            int selectedCount = UnityEssentialsPackageManagerUtilities.GetEffectiveSelectedRepos(
                _repoDisplayNames,
                _userSelected,
                _forcedRepoNames).Count;

            if (selectedCount == 0)
                return;

            GUILayout.Space(-1);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                string repositoryString = selectedCount == 1 ? "Repository" : "Repositories";

                using (new EditorGUI.DisabledScope(_isInstalling))
                {
                    if (GUILayout.Button($"Install / Update Selected {repositoryString}", GUILayout.Height(24)))
                    {
                        InstallOrUpdateSelected();
                        GUI.FocusControl(null);
                    }
                }
            }
        }

        private void FetchRepositories()
        {
            if (_isFetching || _isInstalling) return;

            _isFetching = true;
            _repos.Clear();
            _repoByName.Clear();

            _repoDisplayNames.Clear();
            _userSelected.Clear();

            _metaByRepoName.Clear();
            _repoNameByPackageName.Clear();
            _forcedRepoNames.Clear();

            try
            {
                EditorUtility.DisplayProgressBar("UnityEssentials", "Querying GitHub repositories…", 0f);
                var repos = UnityEssentialsPackageManager.FetchAllRepos(GitHubUser);

                if (repos == null)
                    return;

                foreach (var r in repos)
                {
                    if (r == null || string.IsNullOrEmpty(r.name)) continue;
                    if (!r.name.StartsWith("Unity.", StringComparison.Ordinal)) continue;

                    _repos.Add(r);
                    if (!_repoByName.ContainsKey(r.name))
                        _repoByName[r.name] = r;
                }

                ApplyFilterAndSelectionReset();

                // Build packageName -> repoName index synchronously so dependency forcing is instant.
                BuildPackageIndexBlocking();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                _isFetching = false;
            }
        }

        private void BuildPackageIndexBlocking()
        {
            if (_repos.Count == 0)
                return;

            for (int i = 0; i < _repos.Count; i++)
            {
                var repo = _repos[i];
                if (repo == null)
                    continue;

                float progress = (float)i / Math.Max(1, _repos.Count);
                EditorUtility.DisplayProgressBar("UnityEssentials", $"Indexing packages & dependencies… ({i + 1}/{_repos.Count})", progress);

                // Cache full meta so selecting later doesn't fetch anything.
                if (UnityEssentialsPackageManager.TryGetPackageMetaFromRepo(repo, out var meta, out _))
                {
                    if (!_metaByRepoName.ContainsKey(repo.name))
                        _metaByRepoName[repo.name] = meta;

                    if (!string.IsNullOrEmpty(meta?.packageName) && !_repoNameByPackageName.ContainsKey(meta.packageName))
                        _repoNameByPackageName[meta.packageName] = repo.name;
                }
                else
                {
                    // Negative cache so we don't retry later.
                    if (!_metaByRepoName.ContainsKey(repo.name))
                        _metaByRepoName[repo.name] = null;
                }
            }

            RecomputeForcedStates();
        }

        private void ApplyFilterAndSelectionReset()
        {
            // Preserve user selections by repoName across filter changes.
            var previousUser = new Dictionary<string, bool>(StringComparer.Ordinal);
            for (int i = 0; i < _repoDisplayNames.Count && i < _userSelected.Count; i++)
                previousUser[_repoDisplayNames[i]] = _userSelected[i];

            _repoDisplayNames.Clear();
            _userSelected.Clear();

            for (int i = 0; i < _repos.Count; i++)
            {
                var repoName = _repos[i].name;
                if (string.IsNullOrEmpty(repoName))
                    continue;

                if (!string.IsNullOrEmpty(_filter) && repoName.IndexOf(_filter, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                _repoDisplayNames.Add(repoName);
                _userSelected.Add(previousUser.TryGetValue(repoName, out var v) && v);
            }

            // Filter changes shouldn't trigger network calls; forced state will be re-evaluated
            // using whatever dependency info we've already cached.
            RecomputeForcedStates();
            Repaint();
        }

        private void SetAllUserSelection(bool value)
        {
            for (int i = 0; i < _userSelected.Count; i++)
                _userSelected[i] = value;

            RecomputeForcedStates();
            Repaint();
        }

        private void RecomputeForcedStates()
        {
            _forcedRepoNames.Clear();

            var forced = UnityEssentialsPackageManagerUtilities.ComputeForcedRepos(
                _repoDisplayNames,
                _userSelected,
                _metaByRepoName,
                _repoNameByPackageName);

            foreach (var r in forced)
                _forcedRepoNames.Add(r);
        }

        private void InstallOrUpdateSelected()
        {
            if (_isInstalling) return;

            // Dialog 1: choose the operation for the selected repositories.
            int op = EditorUtility.DisplayDialogComplex(
                "UnityEssentials",
                "What do you want to do?",
                "Install / Update Packages",
                "Cancel",
                "Clone Repositories");

            // 0 = left/OK, 1 = cancel, 2 = right/alt
            if (op == 1)
                return;

            bool doInstallPackages = (op == 0);
            bool doCloneRepos = (op == 2);

            _isInstalling = true;
            try
            {
                // Effective selection = user-selected + dependency-forced.
                var selectedRepoNames = UnityEssentialsPackageManagerUtilities.GetEffectiveSelectedRepos(
                    _repoDisplayNames,
                    _userSelected,
                    _forcedRepoNames);

                var selectedRepos = new List<UnityEssentialsPackageManager.Repo>();
                foreach (var repoName in selectedRepoNames)
                {
                    var repo = _repos.Find(r => string.Equals(r.name, repoName, StringComparison.Ordinal));
                    if (repo != null) selectedRepos.Add(repo);
                }

                if (doCloneRepos)
                {
                    // Dialog 2: when cloning, choose how to handle existing destinations.
                    int modeIdx = EditorUtility.DisplayDialogComplex(
                        "UnityEssentials",
                        "Clone repositories into Assets/Repositories.\n\nIf destination exists:",
                        "Skip",
                        "Pull",
                        "Reclone");

                    UnityEssentialsCloneMode cloneMode = modeIdx switch
                    {
                        0 => UnityEssentialsCloneMode.SkipIfExists,
                        1 => UnityEssentialsCloneMode.PullIfExists,
                        _ => UnityEssentialsCloneMode.RecloneIfExists,
                    };

                    if (!UnityEssentialsGitUtilities.IsGitAvailable(out var verOrError))
                    {
                        EditorUtility.DisplayDialog(
                            "UnityEssentials",
                            "Git isn't available on PATH, so cloning can't run.\n\nError: " + verOrError,
                            "OK");
                        return;
                    }

                    string reposRoot = UnityEssentialsGitUtilities.GetRepositoriesRootAbsolute();
                    Directory.CreateDirectory(reposRoot);

                    int cloneOk = 0;
                    int cloneFailed = 0;
                    int cloneSkipped = 0;

                    for (int i = 0; i < selectedRepos.Count; i++)
                    {
                        var repo = selectedRepos[i];
                        float progress = (float)i / Math.Max(1, selectedRepos.Count);

                        if (EditorUtility.DisplayCancelableProgressBar(
                                "UnityEssentials",
                                $"Cloning {repo.name} ({i + 1}/{selectedRepos.Count})…",
                                progress))
                        {
                            break;
                        }

                        string dest = Path.Combine(reposRoot, UnityEssentialsGitUtilities.SanitizeFolderName(repo.name));
                        bool existed = Directory.Exists(dest);

                        string repoUrl = $"https://github.com/{repo.owner?.login}/{repo.name}.git";

                        bool ok = UnityEssentialsGitUtilities.CloneOrUpdate(
                            repoUrl,
                            repo.default_branch,
                            dest,
                            cloneMode,
                            out var err);

                        if (ok)
                        {
                            if (existed && cloneMode == UnityEssentialsCloneMode.SkipIfExists)
                                cloneSkipped++;
                            else
                                cloneOk++;
                        }
                        else
                        {
                            cloneFailed++;
                            Debug.LogError($"[UnityEssentials] Failed to clone/update '{repo.name}' -> '{dest}': {err}");
                        }
                    }

                    EditorUtility.ClearProgressBar();
                    AssetDatabase.Refresh();

                    EditorUtility.DisplayDialog(
                        "UnityEssentials",
                        $"Clone complete.\n\nOK: {cloneOk}\nSkipped: {cloneSkipped}\nFailed: {cloneFailed}\n\nDestination: Assets/Repositories",
                        "OK");

                    return;
                }

                if (doInstallPackages)
                {
                    // Install dependencies first.
                    // These packages reference each other by (name, version) in package.json, so adding a dependent
                    // before its dependency can fail with "Package ... cannot be found" until the dependency exists.
                    EnsureMetaForSelectedRepos(selectedRepos);
                    selectedRepos = OrderReposByDependencies(selectedRepos, _metaByRepoName, _repoNameByPackageName);

                    // Install/update the selected repositories as UPM git packages.
                    // Cache installed packages once.
                    EditorUtility.DisplayProgressBar("UnityEssentials", "Reading installed packages…", 0f);
                    ListRequest listReq = Client.List();
                    while (!listReq.IsCompleted)
                        System.Threading.Thread.Sleep(20);

                    var installedByName = new Dictionary<string, UnityEditor.PackageManager.PackageInfo>(StringComparer.OrdinalIgnoreCase);
                    if (listReq.Status == StatusCode.Success && listReq.Result != null)
                        foreach (var p in listReq.Result)
                            if (!string.IsNullOrEmpty(p.name) && !installedByName.ContainsKey(p.name))
                                installedByName.Add(p.name, p);

                    int installedCount = 0;
                    int updatedCount = 0;
                    int skippedCount = 0;

                    var failures = new List<PackageAddFailure>();

                    for (int i = 0; i < selectedRepos.Count; i++)
                    {
                        var repo = selectedRepos[i];
                        float progress = (float)i / Math.Max(1, selectedRepos.Count);

                        EditorUtility.DisplayProgressBar("UnityEssentials", $"Checking {repo.name} ({i + 1}/{selectedRepos.Count})…", progress);

                        string packageName = null;
                        if (_metaByRepoName.TryGetValue(repo.name, out var meta) && !string.IsNullOrEmpty(meta?.packageName))
                            packageName = meta.packageName;

                        if (string.IsNullOrEmpty(packageName) && !UnityEssentialsPackageManager.TryGetPackageNameFromRepo(repo, out packageName, out _))
                        {
                            skippedCount++;
                            continue;
                        }

                        bool isInstalled = installedByName.ContainsKey(packageName);
                        string action = isInstalled ? "Updating" : "Installing";

                        var gitUrl = $"https://github.com/{repo.owner?.login}/{repo.name}.git#{repo.default_branch}";

                        EditorUtility.DisplayProgressBar("UnityEssentials", $"{action} {packageName}…", progress);

                        AddRequest addReq = Client.Add(gitUrl);
                        var start = DateTime.UtcNow;
                        while (!addReq.IsCompleted)
                        {
                            var elapsed = (float)(DateTime.UtcNow - start).TotalSeconds;
                            EditorUtility.DisplayProgressBar("UnityEssentials", $"{action} {packageName}… ({elapsed:0}s)", progress);
                            System.Threading.Thread.Sleep(40);
                        }

                        if (addReq.Status == StatusCode.Success)
                        {
                            if (isInstalled) updatedCount++; else installedCount++;

                            if (!string.IsNullOrEmpty(addReq.Result?.name))
                                installedByName[addReq.Result.name] = addReq.Result;
                        }
                        else
                        {
                            var err = GetRequestErrorMessage(addReq);
                            failures.Add(new PackageAddFailure
                            {
                                repoName = repo.name,
                                packageName = packageName,
                                gitUrl = gitUrl,
                                wasInstalled = isInstalled,
                                error = err,
                                retryError = null,
                                retrySucceeded = false,
                            });

                            Debug.LogError($"[UnityEssentials] Failed to {(isInstalled ? "update" : "install")} '{packageName}' from '{repo.name}'. URL: {gitUrl}\nError: {err}");
                        }
                    }

                    // Retry once, at the end, for any packages that failed.
                    int retryAttempted = failures.Count;
                    int retrySucceeded = 0;

                    if (failures.Count > 0)
                    {
                        for (int i = 0; i < failures.Count; i++)
                        {
                            var f = failures[i];
                            float progress = (float)i / Math.Max(1, failures.Count);

                            string action = f.wasInstalled ? "Retrying Update" : "Retrying Install";
                            EditorUtility.DisplayProgressBar("UnityEssentials", $"{action} {f.packageName}… ({i + 1}/{failures.Count})", progress);

                            AddRequest retryReq = Client.Add(f.gitUrl);
                            var start = DateTime.UtcNow;
                            while (!retryReq.IsCompleted)
                            {
                                var elapsed = (float)(DateTime.UtcNow - start).TotalSeconds;
                                EditorUtility.DisplayProgressBar("UnityEssentials", $"{action} {f.packageName}… ({elapsed:0}s)", progress);
                                System.Threading.Thread.Sleep(40);
                            }

                            if (retryReq.Status == StatusCode.Success)
                            {
                                f.retrySucceeded = true;
                                retrySucceeded++;

                                // Count the final outcome.
                                bool nowInstalled = installedByName.ContainsKey(f.packageName);
                                if (nowInstalled) updatedCount++; else installedCount++;

                                if (!string.IsNullOrEmpty(retryReq.Result?.name))
                                    installedByName[retryReq.Result.name] = retryReq.Result;
                            }
                            else
                            {
                                f.retryError = GetRequestErrorMessage(retryReq);
                                Debug.LogError($"[UnityEssentials] Retry failed for '{f.packageName}' from '{f.repoName}'. URL: {f.gitUrl}\nError: {f.retryError}");
                            }
                        }
                    }

                    // Build final report (also copied to clipboard).
                    int finalFailedCount = 0;
                    for (int i = 0; i < failures.Count; i++)
                        if (!failures[i].retrySucceeded)
                            finalFailedCount++;

                    var sb = new StringBuilder(512);
                    sb.AppendLine("Install/Update complete.");
                    sb.AppendLine();
                    sb.AppendLine($"Installed: {installedCount}");
                    sb.AppendLine($"Updated: {updatedCount}");
                    sb.AppendLine($"Skipped: {skippedCount}");
                    sb.AppendLine($"Failed: {finalFailedCount}");

                    if (retryAttempted > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine($"Retried once: {retryAttempted} (Succeeded: {retrySucceeded}, Still failed: {finalFailedCount})");
                    }

                    if (finalFailedCount > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine("Failures:");
                        for (int i = 0; i < failures.Count; i++)
                        {
                            var f = failures[i];
                            if (f.retrySucceeded)
                                continue;

                            string err = string.IsNullOrEmpty(f.retryError) ? f.error : f.retryError;
                            sb.AppendLine($"- {f.packageName} ({f.repoName}): {err}");
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine("(Summary copied to clipboard)");

                    string summary = sb.ToString();
                    Debug.Log(summary);
                    EditorGUIUtility.systemCopyBuffer = summary;

                    EditorUtility.DisplayDialog(
                        "UnityEssentials",
                        summary,
                        "OK");
                }
            }
            finally
            {
                _isInstalling = false;
                EditorUtility.ClearProgressBar();
            }
        }

        private static string GetRequestErrorMessage(UnityEditor.PackageManager.Requests.Request req)
        {
            if (req == null)
                return "Unknown error (request was null)";

            try
            {
                if (req.Error != null)
                {
                    if (!string.IsNullOrEmpty(req.Error.message))
                        return req.Error.message;

                    return "Package Manager error";
                }
            }
            catch
            {
                // Unity's PackageManager errors vary a bit by version; fall back to status.
            }

            return $"Package Manager request failed (status: {req.Status})";
        }

        private void EnsureMetaForSelectedRepos(List<UnityEssentialsPackageManager.Repo> selectedRepos)
        {
            if (selectedRepos == null || selectedRepos.Count == 0)
                return;

            for (int i = 0; i < selectedRepos.Count; i++)
            {
                var repo = selectedRepos[i];
                if (repo == null || string.IsNullOrEmpty(repo.name))
                    continue;

                // If meta is missing or negative-cached, try once more now.
                if (_metaByRepoName.TryGetValue(repo.name, out var cached) && cached != null)
                    continue;

                float progress = (float)i / Math.Max(1, selectedRepos.Count);
                EditorUtility.DisplayProgressBar("UnityEssentials", $"Reading package metadata… {repo.name}", progress);

                if (UnityEssentialsPackageManager.TryGetPackageMetaFromRepo(repo, out var meta, out _))
                {
                    _metaByRepoName[repo.name] = meta;
                    if (!string.IsNullOrEmpty(meta?.packageName) && !_repoNameByPackageName.ContainsKey(meta.packageName))
                        _repoNameByPackageName[meta.packageName] = repo.name;
                }
            }
        }

        private static List<UnityEssentialsPackageManager.Repo> OrderReposByDependencies(
            List<UnityEssentialsPackageManager.Repo> repos,
            IReadOnlyDictionary<string, UnityEssentialsPackageManager.PackageMeta> metaByRepoName,
            IReadOnlyDictionary<string, string> repoNameByPackageName)
        {
            if (repos == null || repos.Count <= 1)
                return repos;

            var repoByName = new Dictionary<string, UnityEssentialsPackageManager.Repo>(StringComparer.Ordinal);
            var originalIndex = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < repos.Count; i++)
            {
                var r = repos[i];
                if (r == null || string.IsNullOrEmpty(r.name))
                    continue;

                if (!repoByName.ContainsKey(r.name))
                    repoByName[r.name] = r;
                if (!originalIndex.ContainsKey(r.name))
                    originalIndex[r.name] = i;
            }

            var inDegree = new Dictionary<string, int>(StringComparer.Ordinal);
            var outgoing = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var kvp in repoByName)
            {
                inDegree[kvp.Key] = 0;
                outgoing[kvp.Key] = new List<string>();
            }

            foreach (var kvp in repoByName)
            {
                var repoName = kvp.Key;

                if (!metaByRepoName.TryGetValue(repoName, out var meta) || meta == null || meta.dependencies == null)
                    continue;

                for (int i = 0; i < meta.dependencies.Count; i++)
                {
                    var depPackageName = meta.dependencies[i];
                    if (string.IsNullOrEmpty(depPackageName))
                        continue;

                    if (!repoNameByPackageName.TryGetValue(depPackageName, out var depRepoName))
                        continue;

                    // Only order within the current selection.
                    if (!repoByName.ContainsKey(depRepoName))
                        continue;

                    // Edge: depRepoName -> repoName
                    outgoing[depRepoName].Add(repoName);
                    inDegree[repoName] = inDegree[repoName] + 1;
                }
            }

            int CompareByOriginalIndex(string a, string b)
            {
                int ia = originalIndex.TryGetValue(a, out var va) ? va : int.MaxValue;
                int ib = originalIndex.TryGetValue(b, out var vb) ? vb : int.MaxValue;
                return ia.CompareTo(ib);
            }

            var zeros = new List<string>();
            foreach (var kvp in inDegree)
                if (kvp.Value == 0)
                    zeros.Add(kvp.Key);
            zeros.Sort(CompareByOriginalIndex);

            var orderedNames = new List<string>(repoByName.Count);
            var orderedSet = new HashSet<string>(StringComparer.Ordinal);

            while (zeros.Count > 0)
            {
                var n = zeros[0];
                zeros.RemoveAt(0);
                orderedNames.Add(n);
                orderedSet.Add(n);

                var children = outgoing[n];
                for (int i = 0; i < children.Count; i++)
                {
                    var c = children[i];
                    inDegree[c] = inDegree[c] - 1;
                    if (inDegree[c] == 0)
                    {
                        zeros.Add(c);
                        zeros.Sort(CompareByOriginalIndex);
                    }
                }
            }

            // Handle cycles / missing metadata by appending any remaining items in original order.
            if (orderedNames.Count < repoByName.Count)
            {
                var remaining = new List<string>();
                foreach (var kvp in repoByName)
                    if (!orderedSet.Contains(kvp.Key))
                        remaining.Add(kvp.Key);

                remaining.Sort(CompareByOriginalIndex);
                orderedNames.AddRange(remaining);
            }

            var ordered = new List<UnityEssentialsPackageManager.Repo>(orderedNames.Count);
            for (int i = 0; i < orderedNames.Count; i++)
                if (repoByName.TryGetValue(orderedNames[i], out var r))
                    ordered.Add(r);

            return ordered;
        }
    }
}
#endif

