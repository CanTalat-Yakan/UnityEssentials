# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials package via Unity's Package Manager, then install modules from the Tools menu.

- Add the package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

> Quick overview: Add the package via Package Manager, then use Tools → Install & Update UnityEssentials to select and update the modules you need. Modules are lightweight, editor‑first, and live under the `UnityEssentials` namespace.

## Available packages in this project

- [Unity.Core.HDRP.ImGui](https://www.github.com/CanTalat-Yakan/Unity.Core.HDRP.ImGui)
- [Unity.Core.HDRP.Resources](https://www.github.com/CanTalat-Yakan/Unity.Core.HDRP.Resources)
- [Unity.Core.HDRP.Settings](https://www.github.com/CanTalat-Yakan/Unity.Core.HDRP.Settings)
- [Unity.Core.Templates.Sponza](https://www.github.com/CanTalat-Yakan/Unity.Core.Templates.Sponza)
- [Unity.Dependencies.GitApi](https://www.github.com/CanTalat-Yakan/Unity.Dependencies.GitApi)
- [Unity.Dependencies.GitHub](https://www.github.com/CanTalat-Yakan/Unity.Dependencies.GitHub)
- [Unity.Dependencies.NuGet](https://www.github.com/CanTalat-Yakan/Unity.Dependencies.NuGet)
- [Unity.Diagnostics.Gizmo](https://www.github.com/CanTalat-Yakan/Unity.Diagnostics.Gizmo)
- [Unity.Diagnostics.Modules](https://www.github.com/CanTalat-Yakan/Unity.Diagnostics.Modules)
- [Unity.Diagnostics.Monitoring](https://www.github.com/CanTalat-Yakan/Unity.Diagnostics.Monitoring)
- [Unity.Editor.Attributes.Button](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.Button)
- [Unity.Editor.Attributes.DateTime](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.DateTime)
- [Unity.Editor.Attributes.Directory](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.Directory)
- [Unity.Editor.Attributes.Foldout](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.Foldout)
- [Unity.Editor.Attributes.Info](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.Info)
- [Unity.Editor.Attributes.LabelOverride](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.LabelOverride)
- [Unity.Editor.Attributes.MinMaxSlider](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.MinMaxSlider)
- [Unity.Editor.Attributes.OnValueChanged](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.OnValueChanged)
- [Unity.Editor.Attributes.ReadOnly](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.ReadOnly)
- [Unity.Editor.Attributes.ShowIf](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.ShowIf)
- [Unity.Editor.Attributes.ToggleButton](https://www.github.com/CanTalat-Yakan/Unity.Editor.Attributes.ToggleButton)
- [Unity.Editor.Components.StickyNote](https://www.github.com/CanTalat-Yakan/Unity.Editor.Components.StickyNote)
- [Unity.Editor.Drawer.Enum](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.Enum)
- [Unity.Editor.Drawer.EnumFlags](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.EnumFlags)
- [Unity.Editor.Drawer.MarkdownViewer](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.MarkdownViewer)
- [Unity.Editor.Drawer.PackageManifest](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.PackageManifest)
- [Unity.Editor.Drawer.ReorderableList](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.ReorderableList)
- [Unity.Editor.Drawer.SceneReference](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.SceneReference)
- [Unity.Editor.Drawer.ScriptableObject](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.ScriptableObject)
- [Unity.Editor.Drawer.SerializedDictionary](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.SerializedDictionary)
- [Unity.Editor.Drawer.SimpleTreeView](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.SimpleTreeView)
- [Unity.Editor.Drawer.WindowBuilder](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.WindowBuilder)
- [Unity.Editor.Fetcher.MouseInput](https://www.github.com/CanTalat-Yakan/Unity.Editor.Fetcher.MouseInput)
- [Unity.Editor.Hooks.Hierarchy](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.Hierarchy)
- [Unity.Editor.Hooks.Inspector](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.Inspector)
- [Unity.Editor.Hooks.PlayerLoop](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.PlayerLoop)
- [Unity.Editor.Hooks.TextAsset](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.TextAsset)
- [Unity.Editor.Hooks.UIBuilder](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.UIBuilder)
- [Unity.Editor.Tools.EditorIcons](https://www.github.com/CanTalat-Yakan/Unity.Editor.Tools.EditorIcons)
- [Unity.Editor.Tools.MaskMapGenerator](https://www.github.com/CanTalat-Yakan/Unity.Editor.Tools.MaskMapGenerator)
- [Unity.Environment.CelestialBodiesCalculator](https://www.github.com/CanTalat-Yakan/Unity.Environment.CelestialBodiesCalculator)
- [Unity.Environment.Ocean](https://www.github.com/CanTalat-Yakan/Unity.Environment.Ocean)
- [Unity.Environment.TimeOfDay](https://www.github.com/CanTalat-Yakan/Unity.Environment.TimeOfDay)
- [Unity.Environment.Weather](https://www.github.com/CanTalat-Yakan/Unity.Environment.Weather)
- [Unity.Graphics.AdvancedSpotLight](https://www.github.com/CanTalat-Yakan/Unity.Graphics.AdvancedSpotLight)
- [Unity.Graphics.AdvancedTerrain](https://www.github.com/CanTalat-Yakan/Unity.Graphics.AdvancedTerrain)
- [Unity.Graphics.ApvLightingBaker](https://www.github.com/CanTalat-Yakan/Unity.Graphics.ApvLightingBaker)
- [Unity.Graphics.BlackoutReflectionProbe](https://www.github.com/CanTalat-Yakan/Unity.Graphics.BlackoutReflectionProbe)
- [Unity.Graphics.LightCookies](https://www.github.com/CanTalat-Yakan/Unity.Graphics.LightCookies)
- [Unity.Graphics.Tonemaps](https://www.github.com/CanTalat-Yakan/Unity.Graphics.Tonemaps)
- [Unity.Humanoid.ActiveRagdoll](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.ActiveRagdoll)
- [Unity.Humanoid.AnimationRigging](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.AnimationRigging)
- [Unity.Humanoid.Genesis9](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.Genesis9)
- [Unity.Humanoid.PoseController](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.PoseController)
- [Unity.Humanoid.Ragdoll](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.Ragdoll)
- [Unity.Movement.Camera.SpectatorController](https://www.github.com/CanTalat-Yakan/Unity.Movement.Camera.SpectatorController)
- [Unity.Rendering.Camera.AutoExposureController](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.AutoExposureController)
- [Unity.Rendering.Camera.FocusPointRayCaster](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.FocusPointRayCaster)
- [Unity.Rendering.Camera.LuminanceCalculator](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.LuminanceCalculator)
- [Unity.Rendering.Camera.PhysicalPropertiesController](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.PhysicalPropertiesController)
- [Unity.Rendering.CameraRefreshRate](https://www.github.com/CanTalat-Yakan/Unity.Rendering.CameraRefreshRate)
- [Unity.Rendering.Camera.RenderTextureHandler](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.RenderTextureHandler)
- [Unity.Rendering.GlobalRefreshRate](https://www.github.com/CanTalat-Yakan/Unity.Rendering.GlobalRefreshRate)
- [Unity.Sound.AudioMixer](https://www.github.com/CanTalat-Yakan/Unity.Sound.AudioMixer)
- [Unity.Systems.AssetResolver](https://www.github.com/CanTalat-Yakan/Unity.Systems.AssetResolver)
- [Unity.Systems.CameraProvider](https://www.github.com/CanTalat-Yakan/Unity.Systems.CameraProvider)
- [Unity.Systems.DependencyInjection](https://www.github.com/CanTalat-Yakan/Unity.Systems.DependencyInjection)
- [Unity.Systems.EventBus](https://www.github.com/CanTalat-Yakan/Unity.Systems.EventBus)
- [Unity.Systems.FieldBinder](https://www.github.com/CanTalat-Yakan/Unity.Systems.FieldBinder)
- [Unity.Systems.PredefinedAssembly](https://www.github.com/CanTalat-Yakan/Unity.Systems.PredefinedAssembly)
- [Unity.Systems.RuntimeDiscovery](https://www.github.com/CanTalat-Yakan/Unity.Systems.RuntimeDiscovery)
- [Unity.Systems.SceneLoader](https://www.github.com/CanTalat-Yakan/Unity.Systems.SceneLoader)
- [Unity.Systems.Serializer](https://www.github.com/CanTalat-Yakan/Unity.Systems.Serializer)
- [Unity.Systems.SettingsDefinition](https://www.github.com/CanTalat-Yakan/Unity.Systems.SettingsDefinition)
- [Unity.Systems.SettingsProfile](https://www.github.com/CanTalat-Yakan/Unity.Systems.SettingsProfile)
- [Unity.Systems.Singleton](https://www.github.com/CanTalat-Yakan/Unity.Systems.Singleton)
- [Unity.Systems.SkinnedMeshTransfer](https://www.github.com/CanTalat-Yakan/Unity.Systems.SkinnedMeshTransfer)
- [Unity.Systems.Task](https://www.github.com/CanTalat-Yakan/Unity.Systems.Task)
- [Unity.Systems.Tick](https://www.github.com/CanTalat-Yakan/Unity.Systems.Tick)
- [Unity.Systems.Timing](https://www.github.com/CanTalat-Yakan/Unity.Systems.Timing)
- [Unity.Systems.UtilityExtensions](https://www.github.com/CanTalat-Yakan/Unity.Systems.UtilityExtensions)
- [Unity.Types.ManagedArray](https://www.github.com/CanTalat-Yakan/Unity.Types.ManagedArray)
- [Unity.Types.ObjectPool](https://www.github.com/CanTalat-Yakan/Unity.Types.ObjectPool)
- [Unity.UI.Fonts](https://www.github.com/CanTalat-Yakan/Unity.UI.Fonts)
- [Unity.UI.Toolkit.Animation](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.Animation)
- [Unity.UI.Toolkit.ElementLinker](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.ElementLinker)
- [Unity.UI.Toolkit.Extensions](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.Extensions)
- [Unity.UI.Toolkit.MarqueeLabel](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.MarqueeLabel)
- [Unity.UI.Toolkit.MenuGenerator](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.MenuGenerator)
- [Unity.UI.Toolkit.ScriptComponents](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.ScriptComponents)
- [Unity.UI.Toolkit.SplashScreen](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.SplashScreen)
- [Unity.UI.Toolkit.Tooltip](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.Tooltip)