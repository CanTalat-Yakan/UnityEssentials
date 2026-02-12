#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace UnityEssentials
{
    public static class UnityEssentialsPackageManagerUtilities
    {
        public static HashSet<string> ComputeForcedRepos(
            IReadOnlyList<string> visibleRepoNames,
            IReadOnlyList<bool> userSelected,
            IReadOnlyDictionary<string, UnityEssentialsPackageManager.PackageMeta> metaByRepoName,
            IReadOnlyDictionary<string, string> repoNameByPackageName)
        {
            var forced = new HashSet<string>(StringComparer.Ordinal);

            if (visibleRepoNames == null || userSelected == null)
                return forced;

            var roots = new List<string>();
            int count = Math.Min(visibleRepoNames.Count, userSelected.Count);
            for (int i = 0; i < count; i++)
                if (userSelected[i])
                    roots.Add(visibleRepoNames[i]);

            if (roots.Count == 0)
                return forced;

            var queue = new Queue<string>(roots);
            var visited = new HashSet<string>(roots, StringComparer.Ordinal);

            while (queue.Count > 0)
            {
                var currentRepo = queue.Dequeue();

                if (!metaByRepoName.TryGetValue(currentRepo, out var meta) || meta == null)
                    continue;

                var deps = meta.dependencies;
                if (deps == null || deps.Count == 0)
                    continue;

                foreach (var depPackage in deps)
                {
                    if (string.IsNullOrEmpty(depPackage))
                        continue;

                    if (!depPackage.StartsWith("com.unityessentials.", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!repoNameByPackageName.TryGetValue(depPackage, out var depRepoName) || string.IsNullOrEmpty(depRepoName))
                        continue;

                    if (string.Equals(depRepoName, currentRepo, StringComparison.Ordinal))
                        continue;

                    if (!IsUserSelected(visibleRepoNames, userSelected, depRepoName))
                        forced.Add(depRepoName);

                    if (visited.Add(depRepoName))
                        queue.Enqueue(depRepoName);
                }
            }

            forced.RemoveWhere(r => IsUserSelected(visibleRepoNames, userSelected, r));
            return forced;
        }

        public static HashSet<string> GetEffectiveSelectedRepos(
            IReadOnlyList<string> visibleRepoNames,
            IReadOnlyList<bool> userSelected,
            IReadOnlyCollection<string> forcedRepos)
        {
            var result = new HashSet<string>(StringComparer.Ordinal);

            if (visibleRepoNames != null && userSelected != null)
            {
                int count = Math.Min(visibleRepoNames.Count, userSelected.Count);
                for (int i = 0; i < count; i++)
                    if (userSelected[i])
                        result.Add(visibleRepoNames[i]);
            }

            if (forcedRepos != null)
                foreach (var r in forcedRepos)
                    result.Add(r);

            return result;
        }

        public static bool IsUserSelected(IReadOnlyList<string> visibleRepoNames, IReadOnlyList<bool> userSelected, string repoName)
        {
            if (string.IsNullOrEmpty(repoName) || visibleRepoNames == null || userSelected == null)
                return false;

            int count = Math.Min(visibleRepoNames.Count, userSelected.Count);
            for (int i = 0; i < count; i++)
            {
                if (string.Equals(visibleRepoNames[i], repoName, StringComparison.Ordinal))
                    return userSelected[i];
            }

            return false;
        }
    }
}
#endif

