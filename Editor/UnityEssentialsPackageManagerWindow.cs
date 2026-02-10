#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Threading;
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

        private Vector2 _scroll;

        private string _filter;

        private bool _isFetching;
        private bool _isInstalling;

        private readonly List<UnityEssentialsPackageManager.Repo> _repos = new();
        private readonly List<string> _repoDisplayNames = new();
        private readonly List<bool> _selected = new();

        private DateTime _lastRepaint;

        [MenuItem("Tools/Install & Update UnityEssentials", priority = -10000)]
        public static void ShowWindow()
        {
            var w = GetWindow<UnityEssentialsPackageManagerWindow>(true, "UnityEssentials Package Manager", true);
            w.minSize = new Vector2(520, 520);
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

            // Ensure UI stays responsive during long-running ops.
            if ((_isFetching || _isInstalling) && (DateTime.UtcNow - _lastRepaint).TotalMilliseconds > 200)
            {
                _lastRepaint = DateTime.UtcNow;
                Repaint();
            }
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
                    if (GUILayout.Button("Refresh Repositories", EditorStyles.toolbarButton))
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
                        SetAllSelection(true);
                    if (GUILayout.Button("None", GUILayout.Width(56)))
                        SetAllSelection(false);
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label($"Total Repositories: {_repoDisplayNames.Count}", EditorStyles.miniBoldLabel);
            }

            GUILayout.Space(4);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            for (int i = 0; i < _repoDisplayNames.Count; i++)
            {
                if (i >= _selected.Count)
                    _selected.Add(false);

                using (new EditorGUI.DisabledScope(_isInstalling))
                {
                    _selected[i] = EditorGUILayout.ToggleLeft(_repoDisplayNames[i], _selected[i]);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawFooter()
        {
            if (_isFetching)
                return;

            int selectedCount = 0;
            for (int i = 0; i < _selected.Count; i++)
                if (_selected[i]) selectedCount++;

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
            _repoDisplayNames.Clear();
            _selected.Clear();

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

                    // Keep unfiltered list in memory; templates are filtered in ApplyFilterAndSelectionReset.
                    _repos.Add(r);
                }

                ApplyFilterAndSelectionReset();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                _isFetching = false;
            }
        }

        private void ApplyFilterAndSelectionReset()
        {
            _repoDisplayNames.Clear();
            _selected.Clear();

            for (int i = 0; i < _repos.Count; i++)
            {
                var repoName = _repos[i].name;
                if (string.IsNullOrEmpty(repoName))
                    continue;

                if (!string.IsNullOrEmpty(_filter) && repoName.IndexOf(_filter, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                _repoDisplayNames.Add(repoName);
                _selected.Add(false);
            }
        }

        private void SetAllSelection(bool value)
        {
            for (int i = 0; i < _selected.Count; i++)
                _selected[i] = value;
        }

        private void InstallOrUpdateSelected()
        {
            if (_isInstalling) return;

            _isInstalling = true;
            try
            {
                // Cache installed packages once.
                EditorUtility.DisplayProgressBar("UnityEssentials", "Reading installed packages…", 0f);
                ListRequest listReq = Client.List();
                while (!listReq.IsCompleted)
                    Thread.Sleep(20);

                var installedByName = new Dictionary<string, UnityEditor.PackageManager.PackageInfo>(StringComparer.OrdinalIgnoreCase);
                if (listReq.Status == StatusCode.Success && listReq.Result != null)
                    foreach (var p in listReq.Result)
                        if (!string.IsNullOrEmpty(p.name) && !installedByName.ContainsKey(p.name))
                            installedByName.Add(p.name, p);

                var selectedRepos = new List<UnityEssentialsPackageManager.Repo>();
                for (int i = 0; i < _repoDisplayNames.Count; i++)
                {
                    if (!_selected[i]) continue;
                    var repo = _repos.Find(r => string.Equals(r.name, _repoDisplayNames[i], StringComparison.Ordinal));
                    if (repo != null) selectedRepos.Add(repo);
                }

                int installedCount = 0;
                int updatedCount = 0;
                int skippedCount = 0;
                int failedCount = 0;

                for (int i = 0; i < selectedRepos.Count; i++)
                {
                    var repo = selectedRepos[i];
                    float progress = (float)i / Math.Max(1, selectedRepos.Count);

                    EditorUtility.DisplayProgressBar("UnityEssentials", $"Checking {repo.name} ({i + 1}/{selectedRepos.Count})…", progress);

                    if (!UnityEssentialsPackageManager.TryGetPackageNameFromRepo(repo, out var packageName, out _))
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
                        Thread.Sleep(40);
                    }

                    if (addReq.Status == StatusCode.Success)
                    {
                        if (isInstalled) updatedCount++; else installedCount++;

                        if (!string.IsNullOrEmpty(addReq.Result?.name))
                            installedByName[addReq.Result.name] = addReq.Result;
                    }
                    else
                    {
                        failedCount++;
                    }
                }

                EditorUtility.DisplayDialog(
                    "UnityEssentials",
                    $"Install/Update complete.\n\nInstalled: {installedCount}\nUpdated: {updatedCount}\nSkipped: {skippedCount}\nFailed: {failedCount}",
                    "OK");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                EditorUtility.DisplayDialog("UnityEssentials", "Unexpected error: " + ex.Message, "OK");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                _isInstalling = false;
            }
        }
    }
}
#endif

