#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityEssentials
{
    public static class UnityEssentialsAddressablesRegistrar
    {
        private const string MenuPath = "Tools/UnityEssentials/Register Addressables";
        private const int MenuPriority = -9999; // just below Install & Update (-10000)
        private const string GroupName = "UnityEssentials";

        [MenuItem(MenuPath, priority = MenuPriority)]
        public static void RegisterAddressables()
        {
            try
            {
                var settings = GetAddressableSettings();
                if (settings == null)
                {
                    EditorUtility.DisplayDialog(
                        "Addressables not set up",
                        "Could not find Addressables settings.\n\n" +
                        "If Addressables is installed, create settings via:\n" +
                        "Window → Asset Management → Addressables → Groups → Create Addressables Settings\n\n" +
                        "Then run Tools → UnityEssentials → Register Addressables again.",
                        "OK");
                    return;
                }

                var existingGroup = FindGroup(settings, GroupName);
                if (existingGroup != null)
                {
                    Selection.activeObject = existingGroup;
                    EditorGUIUtility.PingObject(existingGroup);
                    EditorUtility.DisplayDialog("Addressables already registered", $"An Addressables group named '{GroupName}' is already present in the project settings.", "OK");
                    return;
                }

                var groupAsset = FindBestGroupAsset(GroupName);
                if (groupAsset == null)
                {
                    EditorUtility.DisplayDialog(
                        "Group not found",
                        $"Could not find an Addressables group asset named '{GroupName}'.\n\n" +
                        "Expected to find a group in the imported UnityEssentials packages.\n" +
                        "Make sure the package that ships the Addressables group is installed.",
                        "OK");
                    return;
                }

                var groupAssetPath = AssetDatabase.GetAssetPath(groupAsset);
                var destGroup = EnsureGroupCopiedIntoAssets(groupAssetPath);
                if (destGroup == null)
                {
                    EditorUtility.DisplayDialog(
                        "Copy failed",
                        $"Failed to copy the '{GroupName}' Addressables group into Assets/AddressableAssetsData.",
                        "OK");
                    return;
                }

                if (!TryRegisterGroup(settings, destGroup))
                {
                    EditorUtility.DisplayDialog(
                        "Register failed",
                        $"Copied group asset, but could not add it to Addressables settings.\n\n" +
                        "Check the Console for details.",
                        "OK");
                    return;
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Selection.activeObject = destGroup;
                EditorGUIUtility.PingObject(destGroup);

                EditorUtility.DisplayDialog(
                    "Addressables registered",
                    $"Registered Addressables group '{GroupName}' into the project's Addressables settings.",
                    "OK");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorUtility.DisplayDialog("Register Addressables failed", "An unexpected error occurred. Check the Console for details.", "OK");
            }
        }

        private static UnityEngine.Object GetAddressableSettings()
        {
            // Avoid hard dependency on Addressables by using reflection.
            var defaultObjectType = Type.GetType("UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject, Unity.Addressables.Editor");
            if (defaultObjectType == null)
                return null;

            var settingsProp = defaultObjectType.GetProperty("Settings", BindingFlags.Public | BindingFlags.Static);
            if (settingsProp == null)
                return null;

            return settingsProp.GetValue(null) as UnityEngine.Object;
        }

        private static UnityEngine.Object FindGroup(UnityEngine.Object settings, string groupName)
        {
            if (settings == null)
                return null;

            var settingsType = settings.GetType();

            var findGroup = settingsType.GetMethod("FindGroup", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null);
            if (findGroup != null)
                return findGroup.Invoke(settings, new object[] { groupName }) as UnityEngine.Object;

            // Fallback: scan settings.groups
            var groupsProp = settingsType.GetProperty("groups", BindingFlags.Public | BindingFlags.Instance);
            if (groupsProp == null)
                return null;

            if (groupsProp.GetValue(settings) is not IEnumerable enumerable)
                return null;

            foreach (var g in enumerable)
            {
                if (g is not UnityEngine.Object obj)
                    continue;
                if (string.Equals(obj.name, groupName, StringComparison.Ordinal))
                    return obj;
            }

            return null;
        }

        private static UnityEngine.Object FindBestGroupAsset(string groupName)
        {
            // Note: `t:AddressableAssetGroup` works as a string filter even without compile-time reference.
            var guids = AssetDatabase.FindAssets($"t:AddressableAssetGroup {groupName}");
            if (guids == null || guids.Length == 0)
                return null;

            string[] paths = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(p => !string.IsNullOrEmpty(p))
                .ToArray();

            // Prefer Assets/AddressableAssetsData (already copied) then Assets/, then Packages/.
            string bestPath = paths
                .OrderBy(p => ScoreGroupPath(p))
                .FirstOrDefault();

            if (string.IsNullOrEmpty(bestPath))
                return null;

            return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(bestPath);
        }

        private static int ScoreGroupPath(string path)
        {
            if (path.StartsWith("Assets/AddressableAssetsData/", StringComparison.OrdinalIgnoreCase)) return 0;
            if (path.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase)) return 1;
            if (path.StartsWith("Packages/", StringComparison.OrdinalIgnoreCase)) return 2;
            return 3;
        }

        private static UnityEngine.Object EnsureGroupCopiedIntoAssets(string sourcePath)
        {
            if (string.IsNullOrEmpty(sourcePath))
                return null;

            const string destRoot = "Assets/AddressableAssetsData/AssetGroups";
            EnsureFolder(destRoot);

            var destPath = $"{destRoot}/{GroupName}.asset";

            // If a group already exists at destination, use it.
            var existingDest = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(destPath);
            if (existingDest != null)
                return existingDest;

            // Copy from package/other location into Assets.
            if (!AssetDatabase.CopyAsset(sourcePath, destPath))
            {
                Debug.LogError($"[UnityEssentials] Failed to copy Addressables group from '{sourcePath}' to '{destPath}'.");
                return null;
            }

            AssetDatabase.ImportAsset(destPath, ImportAssetOptions.ForceUpdate);

            var copied = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(destPath);
            if (copied == null)
            {
                Debug.LogError($"[UnityEssentials] Copied Addressables group asset, but failed to load it at '{destPath}'.");
                return null;
            }

            return copied;
        }

        private static void EnsureFolder(string folderPath)
        {
            folderPath = folderPath.Replace('\\', '/');
            if (AssetDatabase.IsValidFolder(folderPath))
                return;

            var parent = Path.GetDirectoryName(folderPath)?.Replace('\\', '/');
            var name = Path.GetFileName(folderPath);

            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(name))
                return;

            if (!AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);

            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder(parent, name);
        }

        private static bool TryRegisterGroup(UnityEngine.Object settings, UnityEngine.Object group)
        {
            if (settings == null || group == null)
                return false;

            var settingsType = settings.GetType();

            // Ensure group.Settings points at our settings (best-effort).
            TryRelinkGroupToSettings(group, settings);
            EnsureUniqueGroupInternalGuid(settings, group);

            // Add to settings.groups if not present.
            var groupsProp = settingsType.GetProperty("groups", BindingFlags.Public | BindingFlags.Instance);
            if (groupsProp == null)
            {
                Debug.LogError("[UnityEssentials] Addressables settings has no 'groups' property (API mismatch)." );
                return false;
            }

            var listObj = groupsProp.GetValue(settings);
            if (listObj is not IList list)
            {
                Debug.LogError("[UnityEssentials] Addressables settings 'groups' is not an IList (API mismatch)." );
                return false;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (ReferenceEquals(list[i], group))
                    return true;
                if (list[i] is UnityEngine.Object o && string.Equals(o.name, group.name, StringComparison.Ordinal))
                    return true;
            }

            list.Add(group);

            // Notify Addressables (best-effort). If we can't, still mark dirty.
            TrySetDirty(settings, group, "GroupAdded");

            EditorUtility.SetDirty(settings);
            EditorUtility.SetDirty(group);

            return true;
        }

        private static void TryRelinkGroupToSettings(UnityEngine.Object group, UnityEngine.Object settings)
        {
            try
            {
                var groupType = group.GetType();
                var settingsType = settings.GetType();

                var settingsProp = groupType.GetProperty("Settings", BindingFlags.Public | BindingFlags.Instance);
                if (settingsProp != null && settingsProp.CanWrite && settingsProp.PropertyType.IsAssignableFrom(settingsType))
                {
                    settingsProp.SetValue(group, settings);
                    return;
                }

                var settingsField = groupType.GetField("m_Settings", BindingFlags.NonPublic | BindingFlags.Instance);
                if (settingsField != null && settingsField.FieldType.IsAssignableFrom(settingsType))
                {
                    settingsField.SetValue(group, settings);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[UnityEssentials] Failed to relink group to settings: {e.Message}");
            }
        }

        private static void EnsureUniqueGroupInternalGuid(UnityEngine.Object settings, UnityEngine.Object group)
        {
            try
            {
                var settingsType = settings.GetType();
                var groupType = group.GetType();

                string groupGuid = GetGroupInternalGuid(group);
                if (string.IsNullOrEmpty(groupGuid))
                    return;

                var groupsProp = settingsType.GetProperty("groups", BindingFlags.Public | BindingFlags.Instance);
                if (groupsProp == null)
                    return;

                var groupsEnumerable = groupsProp.GetValue(settings) as IEnumerable;
                if (groupsEnumerable == null)
                    return;

                bool duplicateFound = false;
                foreach (var g in groupsEnumerable)
                {
                    if (g is not UnityEngine.Object other || ReferenceEquals(other, group))
                        continue;

                    string otherGuid = GetGroupInternalGuid(other);
                    if (string.IsNullOrEmpty(otherGuid))
                        continue;

                    if (string.Equals(otherGuid, groupGuid, StringComparison.OrdinalIgnoreCase))
                    {
                        duplicateFound = true;
                        break;
                    }
                }

                if (!duplicateFound)
                    return;

                var guidField = groupType.GetField("m_GUID", BindingFlags.NonPublic | BindingFlags.Instance)
                               ?? groupType.GetField("m_Guid", BindingFlags.NonPublic | BindingFlags.Instance);

                if (guidField != null && guidField.FieldType == typeof(string))
                {
                    guidField.SetValue(group, System.Guid.NewGuid().ToString());
                    EditorUtility.SetDirty(group);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[UnityEssentials] Failed to ensure unique group GUID: {e.Message}");
            }
        }

        private static string GetGroupInternalGuid(UnityEngine.Object group)
        {
            if (group == null)
                return null;

            var t = group.GetType();
            var prop = t.GetProperty("Guid", BindingFlags.Public | BindingFlags.Instance)
                    ?? t.GetProperty("guid", BindingFlags.Public | BindingFlags.Instance);

            if (prop != null && prop.PropertyType == typeof(string))
                return prop.GetValue(group) as string;

            var field = t.GetField("m_GUID", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?? t.GetField("m_Guid", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null && field.FieldType == typeof(string))
                return field.GetValue(group) as string;

            return null;
        }

        private static void TrySetDirty(UnityEngine.Object settings, UnityEngine.Object group, string modificationEventName)
        {
            try
            {
                var settingsType = settings.GetType();

                var setDirty = settingsType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m =>
                    {
                        if (!string.Equals(m.Name, "SetDirty", StringComparison.Ordinal))
                            return false;
                        var p = m.GetParameters();
                        return p.Length == 4;
                    });

                if (setDirty == null)
                    return;

                var pInfo = setDirty.GetParameters();
                var modEventType = pInfo[0].ParameterType;
                if (!modEventType.IsEnum)
                    return;

                object modEvent = Enum.Parse(modEventType, modificationEventName);

                // (ModificationEvent, object eventData, bool settingsModified, bool postEvent)
                setDirty.Invoke(settings, new object[] { modEvent, group, true, true });
            }
            catch
            {
                // Ignore - best effort only.
            }
        }
    }
}
#endif
