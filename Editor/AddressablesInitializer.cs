#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

/// <summary>
/// Ensures Addressables settings exist after script reload and exposes a manual Tools menu action.
/// </summary>
[InitializeOnLoad]
public static class AddressablesInitializer
{
    private const string PackedAssetsTemplateName = "Packed Assets";
    private const int MaxInitializationRetries = 10;

    private static bool s_InitializationQueued;
    private static int s_InitializationRetryCount;

    static AddressablesInitializer() =>
        TryInitializeAddressables();

    [MenuItem("Tools/UnityEssentials/Initialize Addressables", priority = -9999)]
    public static void InitializeAddressablesFromMenu()
    {
        Debug.Log("Manually initializing Addressables settings...");
        TryInitializeAddressables();
    }

    private static void TryInitializeAddressables()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            ScheduleInitializationRetry();
            return;
        }

        try
        {
            EnsureAddressablesSettingsExist();
            s_InitializationRetryCount = 0;
        }
        catch (System.NullReferenceException exception)
        {
            if (s_InitializationRetryCount < MaxInitializationRetries)
            {
                s_InitializationRetryCount++;
                Debug.LogWarning($"Addressables initialization failed during editor startup. Retrying ({s_InitializationRetryCount}/{MaxInitializationRetries}).\n{exception.Message}");
                ScheduleInitializationRetry();
                return;
            }

            Debug.LogException(exception);
        }
    }

    private static void ScheduleInitializationRetry()
    {
        if (s_InitializationQueued)
            return;

        s_InitializationQueued = true;
        EditorApplication.delayCall += RetryInitialization;
    }

    private static void RetryInitialization()
    {
        s_InitializationQueued = false;
        TryInitializeAddressables();
    }

    private static void EnsureAddressablesSettingsExist()
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
        if (settings == null)
        {
            Debug.LogWarning("Addressables settings could not be created or loaded.");
            return;
        }

        bool updated = false;

        updated |= EnsureDefaultObjectAsset(settings);
        updated |= EnsureGroupSortSettings(settings);
        updated |= EnsurePackedAssetsTemplate(settings);
        updated |= EnsureDataBuilderAsset<BuildScriptFastMode>(settings);
        updated |= EnsureDataBuilderAsset<BuildScriptPackedMode>(settings);
        updated |= EnsureDataBuilderAsset<BuildScriptPackedPlayMode>(settings);

        if (!updated)
            return;

        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
    }

    private static bool EnsureDefaultObjectAsset(AddressableAssetSettings settings)
    {
        string defaultObjectPath = $"{settings.ConfigFolder}/DefaultObject.asset";
        Object defaultObject = AssetDatabase.LoadAssetAtPath<Object>(defaultObjectPath);
        if (defaultObject != null)
            return false;

        AddressableAssetSettingsDefaultObject.Settings = settings;
        return true;
    }

    private static bool EnsureGroupSortSettings(AddressableAssetSettings settings)
    {
        string sortSettingsPath = $"{settings.ConfigFolder}/{nameof(AddressableAssetGroupSortSettings)}.asset";
        bool needsCreation = AssetDatabase.LoadAssetAtPath<AddressableAssetGroupSortSettings>(sortSettingsPath) == null;

        AddressableAssetGroupSortSettings.GetSettings(settings);
        return needsCreation;
    }

    private static bool EnsurePackedAssetsTemplate(AddressableAssetSettings settings)
    {
        string templatePath = $"{settings.GroupTemplateFolder}/{PackedAssetsTemplateName}.asset";
        bool updated = false;

        AddressableAssetGroupTemplate templateAsset = AssetDatabase.LoadAssetAtPath<AddressableAssetGroupTemplate>(templatePath);
        if (templateAsset == null)
        {
            settings.CreateAndAddGroupTemplate(
                PackedAssetsTemplateName,
                "Pack assets into asset bundles.",
                typeof(BundledAssetGroupSchema),
                typeof(ContentUpdateGroupSchema));

            templateAsset = AssetDatabase.LoadAssetAtPath<AddressableAssetGroupTemplate>(templatePath);
            updated = true;
        }

        if (templateAsset != null && !settings.GroupTemplateObjects.Contains(templateAsset))
        {
            settings.AddGroupTemplateObject(templateAsset, false);
            updated = true;
        }

        return updated;
    }

    private static bool EnsureDataBuilderAsset<T>(AddressableAssetSettings settings)
        where T : ScriptableObject, IDataBuilder
    {
        string builderAssetPath = $"{settings.DataBuilderFolder}/{typeof(T).Name}.asset";
        bool updated = false;

        T builderAsset = AssetDatabase.LoadAssetAtPath<T>(builderAssetPath);
        if (builderAsset == null)
        {
            if (!AssetDatabase.IsValidFolder(settings.DataBuilderFolder))
                AssetDatabase.CreateFolder(settings.ConfigFolder, "DataBuilders");

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<T>(), builderAssetPath);
            builderAsset = AssetDatabase.LoadAssetAtPath<T>(builderAssetPath);
            updated = true;
        }

        if (builderAsset != null && !settings.DataBuilders.Contains(builderAsset))
        {
            settings.AddDataBuilder(builderAsset, false);
            updated = true;
        }

        return updated;
    }
}
#endif