using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace WebGLUtils.Runtime
{
  public static class Platform
  {
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern bool IsMobile();
    [DllImport("__Internal")] private static extern void RequestLandscape();
    [DllImport("__Internal")] private static extern void RequiresFullscreen();
#endif

    /// <summary>
    /// Returns true if running in a mobile browser (WebGL). False otherwise.
    /// </summary>
    public static bool IsMobileBrowser()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsMobile();
#else
      return false;
#endif
    }

    /// <summary>
    /// Requests locking the screen to landscape orientation (WebGL).
    /// </summary>
    public static void RequestLandscapeMode()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RequestLandscape();
#else
      Debug.Log("[Platform] RequestLandscapeMode - not supported on this platform");
#endif
    }

    /// <summary>
    /// Requests fullscreen mode (WebGL).
    /// Note: most browsers require this to be triggered by a user gesture.
    /// </summary>
    public static void RequireFullscreenMode()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RequiresFullscreen();
#else
      Debug.Log("[Platform] RequireFullscreenMode - not supported on this platform");
#endif
    }
  }
}