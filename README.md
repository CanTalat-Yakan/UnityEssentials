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

- [Unity.Core.AudioMixer](https://github.com/CanTalat-Yakan/Unity.Core.AudioMixer)
- [Unity.Core.HDRP.Resources](https://www.github.com/cantalat-yakan/Unity.Core.HDRP.Resources)
- [Unity.Core.HDRP.Settings](https://www.github.com/cantalat-yakan/Unity.Core.HDRP.Settings)
- [Unity.Dependencies.GitFolderSynchronizer](https://www.github.com/cantalat-yakan/Unity.Dependencies.GitFolderSynchronizer)
- [Unity.Dependencies.GitHubRepositoryCloner](https://www.github.com/cantalat-yakan/Unity.Dependencies.GitHubRepositoryCloner)
- [Unity.Dependencies.NuGet](https://www.github.com/cantalat-yakan/Unity.Dependencies.NuGet)
- [Unity.Dependencies.PackageManifestEditor](https://www.github.com/cantalat-yakan/Unity.Dependencies.PackageManifestEditor)
- [Unity.Editor.Attributes.Button](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.Button)
- [Unity.Editor.Attributes.DateTime](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.DateTime)
- [Unity.Editor.Attributes.Directory](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.Directory)
- [Unity.Editor.Attributes.Foldout](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.Foldout)
- [Unity.Editor.Attributes.If](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.If)
- [Unity.Editor.Attributes.Info](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.Info)
- [Unity.Editor.Attributes.LabelOverride](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.LabelOverride)
- [Unity.Editor.Attributes.MinMaxSlider](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.MinMaxSlider)
- [Unity.Editor.Attributes.OnValueChanged](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.OnValueChanged)
- [Unity.Editor.Attributes.ReadOnly](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.ReadOnly)
- [Unity.Editor.Attributes.ToggleButton](https://www.github.com/cantalat-yakan/Unity.Editor.Attributes.ToggleButton)
- [Unity.Editor.Components.StickyNote](https://www.github.com/cantalat-yakan/Unity.Editor.Components.StickyNote)
- [Unity.Editor.Drawer.Enum](https://www.github.com/cantalat-yakan/Unity.Editor.Drawer.Enum)
- [Unity.Editor.Drawer.EnumFlags](https://www.github.com/cantalat-yakan/Unity.Editor.Drawer.EnumFlags)
- [Unity.Editor.Drawer.SceneReference](https://www.github.com/cantalat-yakan/Unity.Editor.Drawer.SceneReference)
- [Unity.Editor.Drawer.ScriptableObject](https://www.github.com/cantalat-yakan/Unity.Editor.Drawer.ScriptableObject)
- [Unity.Editor.Drawer.SerializedDictionary](https://www.github.com/cantalat-yakan/Unity.Editor.Drawer.SerializedDictionary)
- [Unity.Editor.Helper.EditorWindowDrawer](https://www.github.com/cantalat-yakan/Unity.Editor.Helper.EditorWindowDrawer)
- [Unity.Editor.Helper.MouseInputFetcher](https://www.github.com/cantalat-yakan/Unity.Editor.Helper.MouseInputFetcher)
- [Unity.Editor.Helper.SimpleTreeView](https://www.github.com/cantalat-yakan/Unity.Editor.Helper.SimpleTreeView)
- [Unity.Editor.Hooks.Hierarchy](https://www.github.com/cantalat-yakan/Unity.Editor.Hooks.Hierarchy)
- [Unity.Editor.Hooks.Inspector](https://www.github.com/cantalat-yakan/Unity.Editor.Hooks.Inspector)
- [Unity.Editor.Hooks.PlayerLoop](https://www.github.com/cantalat-yakan/Unity.Editor.Hooks.PlayerLoop)
- [Unity.Editor.Hooks.UIBuilder](https://www.github.com/cantalat-yakan/Unity.Editor.Hooks.UIBuilder)
- [Unity.Editor.Profiling.FrameTime](https://www.github.com/cantalat-yakan/Unity.Editor.Profiling.FrameTime)
- [Unity.Editor.Tools.EditorIcons](https://www.github.com/cantalat-yakan/Unity.Editor.Tools.EditorIcons)
- [Unity.Editor.Tools.MaskMapGenerator](https://www.github.com/cantalat-yakan/Unity.Editor.Tools.MaskMapGenerator)
- [Unity.Environment.CelestialBodiesCalculator](https://www.github.com/cantalat-yakan/Unity.Environment.CelestialBodiesCalculator)
- [Unity.Environment.Ocean](https://www.github.com/cantalat-yakan/Unity.Environment.Ocean)
- [Unity.Environment.TimeOfDay](https://www.github.com/cantalat-yakan/Unity.Environment.TimeOfDay)
- [Unity.Environment.Weather](https://www.github.com/cantalat-yakan/Unity.Environment.Weather)
- [Unity.Graphics.APVLightingBaker](https://www.github.com/cantalat-yakan/Unity.Graphics.APVLightingBaker)
- [Unity.Graphics.AdvancedSpotLight](https://www.github.com/cantalat-yakan/Unity.Graphics.AdvancedSpotLight)
- [Unity.Graphics.BlackoutReflectionProbe](https://www.github.com/cantalat-yakan/Unity.Graphics.BlackoutReflectionProbe)
- [Unity.Graphics.IES](https://www.github.com/cantalat-yakan/Unity.Graphics.IES)
- [Unity.Graphics.Tonemaps](https://www.github.com/cantalat-yakan/Unity.Graphics.Tonemaps)
- [Unity.Humanoid.ActiveRagdoll](https://www.github.com/cantalat-yakan/Unity.Humanoid.ActiveRagdoll)
- [Unity.Humanoid.AnimationRigging](https://www.github.com/cantalat-yakan/Unity.Humanoid.AnimationRigging)
- [Unity.Humanoid.Daz3DGenesis9](https://www.github.com/cantalat-yakan/Unity.Humanoid.Daz3DGenesis9)
- [Unity.Humanoid.PoseController](https://www.github.com/cantalat-yakan/Unity.Humanoid.PoseController)
- [Unity.Humanoid.Ragdoll](https://www.github.com/cantalat-yakan/Unity.Humanoid.Ragdoll)
- [Unity.Movement.CameraSpectatorController](https://www.github.com/cantalat-yakan/Unity.Movement.CameraSpectatorController)
- [Unity.Rendering.Camera.AutoExposureController](https://www.github.com/cantalat-yakan/Unity.Rendering.Camera.AutoExposureController)
- [Unity.Rendering.Camera.FocusPointRayCaster](https://www.github.com/cantalat-yakan/Unity.Rendering.Camera.FocusPointRayCaster)
- [Unity.Rendering.Camera.FrameRateLimiter](https://www.github.com/cantalat-yakan/Unity.Rendering.Camera.FrameRateLimiter)
- [Unity.Rendering.Camera.LuminanceCalculator](https://www.github.com/cantalat-yakan/Unity.Rendering.Camera.LuminanceCalculator)
- [Unity.Rendering.Camera.PhysicalPropertiesController](https://www.github.com/cantalat-yakan/Unity.Rendering.Camera.PhysicalPropertiesController)
- [Unity.Rendering.Camera.RenderTextureHandler](https://www.github.com/cantalat-yakan/Unity.Rendering.Camera.RenderTextureHandler)
- [Unity.Rendering.GlobalRefreshrateLimiter](https://www.github.com/cantalat-yakan/Unity.Rendering.GlobalRefreshrateLimiter)
- [Unity.Systems.DependencyInjection](https://www.github.com/cantalat-yakan/Unity.Systems.DependencyInjection)
- [Unity.Systems.EventBus](https://www.github.com/cantalat-yakan/Unity.Systems.EventBus)
- [Unity.Systems.PredefinedAssemblyUtilities](https://www.github.com/cantalat-yakan/Unity.Systems.PredefinedAssemblyUtilities)
- [Unity.Systems.SceneLoader](https://www.github.com/cantalat-yakan/Unity.Systems.SceneLoader)
- [Unity.Systems.Singleton](https://www.github.com/cantalat-yakan/Unity.Systems.Singleton)
- [Unity.Systems.Tasks](https://www.github.com/cantalat-yakan/Unity.Systems.Tasks)
- [Unity.Systems.TickUpdate](https://www.github.com/cantalat-yakan/Unity.Systems.TickUpdate)
- [Unity.Systems.Timing](https://www.github.com/cantalat-yakan/Unity.Systems.Timing)
- [Unity.Templates.Sponza](https://www.github.com/cantalat-yakan/Unity.Templates.Sponza)
- [Unity.Tools.FieldBinder](https://www.github.com/cantalat-yakan/Unity.Tools.FieldBinder)
- [Unity.Tools.RuntimeGizmo](https://www.github.com/cantalat-yakan/Unity.Tools.RuntimeGizmo)
- [Unity.Tools.RuntimeMonitor](https://www.github.com/cantalat-yakan/Unity.Tools.RuntimeMonitor)
- [Unity.Tools.SkinnedMeshTransfer](https://www.github.com/cantalat-yakan/Unity.Tools.SkinnedMeshTransfer)
- [Unity.Types.ManagedArray](https://www.github.com/cantalat-yakan/Unity.Types.ManagedArray)
- [Unity.Types.ObjectPool](https://www.github.com/cantalat-yakan/Unity.Types.ObjectPool)
- [Unity.UI.Fonts](https://www.github.com/cantalat-yakan/Unity.UI.Fonts)
- [Unity.UI.MenuGenerator](https://www.github.com/cantalat-yakan/Unity.UI.MenuGenerator)
- [Unity.UI.SplashScreen](https://www.github.com/cantalat-yakan/Unity.UI.SplashScreen)
- [Unity.UI.Toolkit.ElementLinker](https://www.github.com/cantalat-yakan/Unity.UI.Toolkit.ElementLinker)
- [Unity.UI.Toolkit.Extensions](https://www.github.com/cantalat-yakan/Unity.UI.Toolkit.Extensions)
- [Unity.UI.Toolkit.MarqueeLabel](https://www.github.com/cantalat-yakan/Unity.UI.Toolkit.MarqueeLabel)
- [Unity.UI.Toolkit.ScriptComponents](https://www.github.com/cantalat-yakan/Unity.UI.Toolkit.ScriptComponents)
- [Unity.UI.Toolkit.Transitions](https://www.github.com/cantalat-yakan/Unity.UI.Toolkit.Transitions)
- [Unity.UI.Tooltip](https://www.github.com/cantalat-yakan/Unity.UI.Tooltip)
- [Unity.Utilities.CameraProvider](https://www.github.com/cantalat-yakan/Unity.Utilities.CameraProvider)
- [Unity.Utilities.Extensions](https://www.github.com/cantalat-yakan/Unity.Utilities.Extensions)
- [Unity.Utilities.ResourceLoader](https://www.github.com/cantalat-yakan/Unity.Utilities.ResourceLoader)
