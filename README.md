# Unity Essentials

A modular package ecosystem for Unity 6 and HDRP, providing production-ready building blocks for game and simulation development. From core architecture and environment systems to graphics, networking, and editor tooling, each module is independently installable with automatic dependency resolution.

> All scripts are contained within the `UnityEssentials` namespace.

## Installation

1. **Add the package** via Unity's Package Manager
    - Window → Package Manager → "+" → "Add package from git URL…"
    - Paste the following:
    ```
    https://github.com/CanTalat-Yakan/UnityEssentials.git
    ```

2. **Install modules** from the Tools menu
    - Tools → Install & Update UnityEssentials
    - Select individual modules or install all; run again anytime to update

Dependencies between modules are resolved automatically.

## Module Overview

**Core Systems**: 
Foundational architecture patterns and runtime infrastructure for scalable Unity projects. Includes dependency injection, event buses, singletons, scene loading, serialization, asset resolution, and timing utilities.

**Diagnostics & Profiling**: 
In-game console, performance monitoring overlays, and runtime debugging tools powered by ImGui. Provides gizmo rendering, toggle shortcuts, and modular diagnostic panels for rapid iteration during development.

**Environment & World Simulation**: 
Dynamic world systems including day/night cycles, weather, ocean, terrain, atmosphere, and ecosystem management. Covers wind, precipitation, clouds, celestial bodies, flora, fauna, and vegetation for rich, living environments.

**Graphics & Rendering**: 
Extended HDRP rendering features for lighting, volumetrics, sky, water, camera control, and post-processing. Adds advanced atmosphere, terrain shading, spotlight effects, APV lighting baking, tonemaps, and light cookies.

**Humanoid & Character**: 
Character animation, ragdoll physics, pose control, rigging, and avatar systems. Supports active ragdolls, animation rigging, ragdoll builders, and advanced avatar customization for humanoid characters.

**Editor Tooling**: 
Custom inspector attributes, property drawers, editor hooks, and asset pipeline tools. Extends the Unity Editor with hierarchy hooks, toolbar customization, reorderable lists, serialized dictionaries, and window builders.

**UI Toolkit**: 
Reusable UIToolkit components and extensions for runtime and editor interfaces. Includes animations, tooltips, marquee labels, splash screens, menu generators, and element linking for streamlined UI development.

**Sound & Audio**: 
Spatial audio, mixing, synthesis, acoustics, and voice systems. Provides configurable audio playback, room acoustics simulation, spatial positioning, and voice communication support.

**Networking & Multiplayer**: 
P2P and session-based multiplayer with matchmaking, replication, social features, and backend infrastructure. Covers transport, encryption, NAT traversal, relay servers, identity management, persistence, and inventory systems.

**Dependencies & Infrastructure**: 
Third-party integrations and HDRP core services required by other modules. Manages Addressables, Git API access, GitHub integration, and NuGet package support.

## Available Packages

### Core & HDRP
- [Unity.Core.HDRP.ImGui](https://www.github.com/CanTalat-Yakan/Unity.Core.HDRP.ImGui)
- [Unity.Core.HDRP.Resources](https://www.github.com/CanTalat-Yakan/Unity.Core.HDRP.Resources)
- [Unity.Core.HDRP.Settings](https://www.github.com/CanTalat-Yakan/Unity.Core.HDRP.Settings)

### Dependencies
- [Unity.Dependencies.Addressables](https://www.github.com/CanTalat-Yakan/Unity.Dependencies.Addressables)
- [Unity.Dependencies.GitApi](https://www.github.com/CanTalat-Yakan/Unity.Dependencies.GitApi)
- [Unity.Dependencies.GitHub](https://www.github.com/CanTalat-Yakan/Unity.Dependencies.GitHub)
- [Unity.Dependencies.NuGet](https://www.github.com/CanTalat-Yakan/Unity.Dependencies.NuGet)

### Diagnostics
- [Unity.Diagnostics.Console](https://www.github.com/CanTalat-Yakan/Unity.Diagnostics.Console)
- [Unity.Diagnostics.Gizmo](https://www.github.com/CanTalat-Yakan/Unity.Diagnostics.Gizmo)
- [Unity.Diagnostics.Modules](https://www.github.com/CanTalat-Yakan/Unity.Diagnostics.Modules)
- [Unity.Diagnostics.Monitoring](https://www.github.com/CanTalat-Yakan/Unity.Diagnostics.Monitoring)
- [Unity.Diagnostics.ToggleShortcut](https://www.github.com/CanTalat-Yakan/Unity.Diagnostics.ToggleShortcut)

### Editor - Attributes
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

### Editor - Components & Drawers
- [Unity.Editor.Components.StickyNote](https://www.github.com/CanTalat-Yakan/Unity.Editor.Components.StickyNote)
- [Unity.Editor.Drawer.Console](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.Console)
- [Unity.Editor.Drawer.DomainReload](https://www.github.com/CanTalat-Yakan/Unity.Editor.Drawer.DomainReload)
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

### Editor - Hooks, Fetchers & Tools
- [Unity.Editor.Fetcher.MouseInput](https://www.github.com/CanTalat-Yakan/Unity.Editor.Fetcher.MouseInput)
- [Unity.Editor.Hooks.Hierarchy](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.Hierarchy)
- [Unity.Editor.Hooks.Inspector](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.Inspector)
- [Unity.Editor.Hooks.PlayerLoop](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.PlayerLoop)
- [Unity.Editor.Hooks.Statusbar](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.Statusbar)
- [Unity.Editor.Hooks.TextAsset](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.TextAsset)
- [Unity.Editor.Hooks.Toolbar](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.Toolbar)
- [Unity.Editor.Hooks.UIBuilder](https://www.github.com/CanTalat-Yakan/Unity.Editor.Hooks.UIBuilder)
- [Unity.Editor.Tools.EditorIcons](https://www.github.com/CanTalat-Yakan/Unity.Editor.Tools.EditorIcons)
- [Unity.Editor.Tools.MaskMapGenerator](https://www.github.com/CanTalat-Yakan/Unity.Editor.Tools.MaskMapGenerator)

### Environment
- [Unity.Environment.Atmosphere](https://www.github.com/CanTalat-Yakan/Unity.Environment.Atmosphere)
- [Unity.Environment.CelestialBodies](https://www.github.com/CanTalat-Yakan/Unity.Environment.CelestialBodies)
- [Unity.Environment.Clouds](https://www.github.com/CanTalat-Yakan/Unity.Environment.Clouds)
- [Unity.Environment.Ecosystem](https://www.github.com/CanTalat-Yakan/Unity.Environment.Ecosystem)
- [Unity.Environment.Fauna](https://www.github.com/CanTalat-Yakan/Unity.Environment.Fauna)
- [Unity.Environment.Flora](https://www.github.com/CanTalat-Yakan/Unity.Environment.Flora)
- [Unity.Environment.Ocean](https://www.github.com/CanTalat-Yakan/Unity.Environment.Ocean)
- [Unity.Environment.Precipitation](https://www.github.com/CanTalat-Yakan/Unity.Environment.Precipitation)
- [Unity.Environment.Terrain](https://www.github.com/CanTalat-Yakan/Unity.Environment.Terrain)
- [Unity.Environment.TimeOfDay](https://www.github.com/CanTalat-Yakan/Unity.Environment.TimeOfDay)
- [Unity.Environment.Vegetation](https://www.github.com/CanTalat-Yakan/Unity.Environment.Vegetation)
- [Unity.Environment.Weather](https://www.github.com/CanTalat-Yakan/Unity.Environment.Weather)
- [Unity.Environment.Wind](https://www.github.com/CanTalat-Yakan/Unity.Environment.Wind)

### Graphics & Rendering
- [Unity.Graphics.AdvancedAtmosphere](https://www.github.com/CanTalat-Yakan/Unity.Graphics.AdvancedAtmosphere)
- [Unity.Graphics.AdvancedSky](https://www.github.com/CanTalat-Yakan/Unity.Graphics.AdvancedSky)
- [Unity.Graphics.AdvancedSpotLight](https://www.github.com/CanTalat-Yakan/Unity.Graphics.AdvancedSpotLight)
- [Unity.Graphics.AdvancedTerrain](https://www.github.com/CanTalat-Yakan/Unity.Graphics.AdvancedTerrain)
- [Unity.Graphics.AdvancedVolumetrics](https://www.github.com/CanTalat-Yakan/Unity.Graphics.AdvancedVolumetrics)
- [Unity.Graphics.AdvancedWater](https://www.github.com/CanTalat-Yakan/Unity.Graphics.AdvancedWater)
- [Unity.Graphics.ApvLightingBaker](https://www.github.com/CanTalat-Yakan/Unity.Graphics.ApvLightingBaker)
- [Unity.Graphics.BlackoutReflectionProbe](https://www.github.com/CanTalat-Yakan/Unity.Graphics.BlackoutReflectionProbe)
- [Unity.Graphics.LightCookies](https://www.github.com/CanTalat-Yakan/Unity.Graphics.LightCookies)
- [Unity.Graphics.Tonemaps](https://www.github.com/CanTalat-Yakan/Unity.Graphics.Tonemaps)

### Humanoid
- [Unity.Humanoid.ActiveRagdoll](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.ActiveRagdoll)
- [Unity.Humanoid.AdvancedAvatar](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.AdvancedAvatar)
- [Unity.Humanoid.AnimationRigging](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.AnimationRigging)
- [Unity.Humanoid.PoseController](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.PoseController)
- [Unity.Humanoid.RagdollBuilder](https://www.github.com/CanTalat-Yakan/Unity.Humanoid.RagdollBuilder)

### Movement
- [Unity.Movement.AdvancedCharacterController](https://www.github.com/CanTalat-Yakan/Unity.Movement.AdvancedCharacterController)
- [Unity.Movement.SpectatorController](https://www.github.com/CanTalat-Yakan/Unity.Movement.SpectatorController)

### Multiplayer
- [Unity.Multiplayer.Backend.InterestManagement](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Backend.InterestManagement)
- [Unity.Multiplayer.Backend.WorldPartition](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Backend.WorldPartition)
- [Unity.Multiplayer.Communication.Chat](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Communication.Chat)
- [Unity.Multiplayer.Communication.Voice](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Communication.Voice)
- [Unity.Multiplayer.Lobbies](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Lobbies)
- [Unity.Multiplayer.Matchmaking](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Matchmaking)
- [Unity.Multiplayer.P2P](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.P2P)
- [Unity.Multiplayer.P2P.Replication](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.P2P.Replication)
- [Unity.Multiplayer.P2P.Sessions](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.P2P.Sessions)
- [Unity.Multiplayer.Sessions](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Sessions)
- [Unity.Multiplayer.Social.Friends](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Social.Friends)
- [Unity.Multiplayer.Social.Groups](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Social.Groups)
- [Unity.Multiplayer.Social.Invites](https://www.github.com/CanTalat-Yakan/Unity.Multiplayer.Social.Invites)

### Networking
- [Unity.Networking.Backend.Encryption](https://www.github.com/CanTalat-Yakan/Unity.Networking.Backend.Encryption)
- [Unity.Networking.Backend.NatTraversal](https://www.github.com/CanTalat-Yakan/Unity.Networking.Backend.NatTraversal)
- [Unity.Networking.Backend.PeerDiscovery](https://www.github.com/CanTalat-Yakan/Unity.Networking.Backend.PeerDiscovery)
- [Unity.Networking.Backend.Relay](https://www.github.com/CanTalat-Yakan/Unity.Networking.Backend.Relay)
- [Unity.Networking.Backend.Serialization](https://www.github.com/CanTalat-Yakan/Unity.Networking.Backend.Serialization)
- [Unity.Networking.Backend.Transport](https://www.github.com/CanTalat-Yakan/Unity.Networking.Backend.Transport)
- [Unity.Networking.Database](https://www.github.com/CanTalat-Yakan/Unity.Networking.Database)
- [Unity.Networking.Identity](https://www.github.com/CanTalat-Yakan/Unity.Networking.Identity)
- [Unity.Networking.Inventory](https://www.github.com/CanTalat-Yakan/Unity.Networking.Inventory)
- [Unity.Networking.Persistence](https://www.github.com/CanTalat-Yakan/Unity.Networking.Persistence)
- [Unity.Networking.Profiles](https://www.github.com/CanTalat-Yakan/Unity.Networking.Profiles)

### Rendering & Camera
- [Unity.Rendering.AdvancedCamera](https://www.github.com/CanTalat-Yakan/Unity.Rendering.AdvancedCamera)
- [Unity.Rendering.Camera.AutoExposure](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.AutoExposure)
- [Unity.Rendering.Camera.FocusPoint](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.FocusPoint)
- [Unity.Rendering.Camera.Luminance](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.Luminance)
- [Unity.Rendering.Camera.PhysicalProperties](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.PhysicalProperties)
- [Unity.Rendering.Camera.RenderTexture](https://www.github.com/CanTalat-Yakan/Unity.Rendering.Camera.RenderTexture)
- [Unity.Rendering.CameraRefreshRate](https://www.github.com/CanTalat-Yakan/Unity.Rendering.CameraRefreshRate)
- [Unity.Rendering.GlobalRefreshRate](https://www.github.com/CanTalat-Yakan/Unity.Rendering.GlobalRefreshRate)

### Sound & Audio
- [Unity.Sound.Acoustics](https://www.github.com/CanTalat-Yakan/Unity.Sound.Acoustics)
- [Unity.Sound.AudioMixer](https://www.github.com/CanTalat-Yakan/Unity.Sound.AudioMixer)
- [Unity.Sound.Playback](https://www.github.com/CanTalat-Yakan/Unity.Sound.Playback)
- [Unity.Sound.SpatialAudio](https://www.github.com/CanTalat-Yakan/Unity.Sound.SpatialAudio)
- [Unity.Sound.Synthesis](https://www.github.com/CanTalat-Yakan/Unity.Sound.Synthesis)
- [Unity.Sound.Voice](https://www.github.com/CanTalat-Yakan/Unity.Sound.Voice)

### Systems
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

### Types
- [Unity.Types.ManagedArray](https://www.github.com/CanTalat-Yakan/Unity.Types.ManagedArray)
- [Unity.Types.ObjectPool](https://www.github.com/CanTalat-Yakan/Unity.Types.ObjectPool)

### UI
- [Unity.UI.Fonts](https://www.github.com/CanTalat-Yakan/Unity.UI.Fonts)
- [Unity.UI.Toolkit.Animation](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.Animation)
- [Unity.UI.Toolkit.ElementLinker](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.ElementLinker)
- [Unity.UI.Toolkit.Extensions](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.Extensions)
- [Unity.UI.Toolkit.MarqueeLabel](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.MarqueeLabel)
- [Unity.UI.Toolkit.MenuGenerator](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.MenuGenerator)
- [Unity.UI.Toolkit.ScriptComponents](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.ScriptComponents)
- [Unity.UI.Toolkit.SplashScreen](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.SplashScreen)
- [Unity.UI.Toolkit.Tooltip](https://www.github.com/CanTalat-Yakan/Unity.UI.Toolkit.Tooltip)