## Unity WebGL Platform Utils

Utilities for Unity WebGL to detect mobile browsers and request fullscreen/landscape mode. 

Includes a C# runtime bridge and a post-build processor to inject helper JS into the build.

### 1️⃣ Installation
Option 1: Install via Git URL (recommended)

`https://github.com/IlyaBlokh/UnityWebGL-PlatformUtils.git`


Option 2: Add to manifest.json

You can also edit your project’s Packages/manifest.json manually:

```
{
  "dependencies": {
    "com.ilyablokh.webglplatformutils": "https://github.com/IlyaBlokh/UnityWebGL-PlatformUtils.git",
    ...
  }
}
```

### 2️⃣ Using Platform Utils

The `Platform` is a static utility class to interact with the WebGL environment:

Example:

```
public class GameStarter : MonoBehaviour
{
    void Start()
    {
        if (Platform.IsMobileBrowser())
        {
            ShowMobileInputUI();
        }
    }

    public void OnPlayButtonPressed()
    {
        Platform.RequestLandscape();
        Platform.RequestFullscreen();
    }
}
```

Tip: Call **RequestLandscape()** and **RequestFullscreen()** in response to a user interaction (button click or tap), as most mobile browsers block programmatic fullscreen or orientation changes on page load.

These methods use the **bridge.jslib** and optionally inject platform-utils.js to allow reading debug logs in the browser.

### 3️⃣ How Post-Build Processing Works

The package includes a WebGL build postprocessor (**WebGLBuildPostprocessor**) that:

1. Automatically copies **platform-utils.js** into the WebGL build folder.
2. Injects a `<script src="platform-utils.js"></script>` before the Unity loader script in **index.html**.
3. Ensures runtime JS utilities are available without manual edits.
