using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace WebGLUtils.Editor
{
  public class WebGLBuildPostprocessor : IPostprocessBuildWithReport
  {
    public int callbackOrder => 0;
    private const string PlatformUtilsClientName = "platform-utils.js";

    public void OnPostprocessBuild(BuildReport report)
    {
      if (report.summary.platform != BuildTarget.WebGL)
        return;

      string buildPath = report.summary.outputPath;
      string indexPath = Path.Combine(buildPath, "index.html");

      string[] guids = AssetDatabase.FindAssets($"t:Script {nameof(WebGLBuildPostprocessor)}");
      if (guids.Length == 0)
      {
        Debug.LogError($"[WebGLPlatformUtils] Could not find {nameof(WebGLBuildPostprocessor)} script.");
        return;
      }

      string scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);
      string editorFolder = Path.GetDirectoryName(scriptPath);
      string packageRoot = Path.GetDirectoryName(editorFolder);
      
      System.Diagnostics.Debug.Assert(packageRoot != null, nameof(packageRoot) + " != null");
      
      string webGLFolder = Path.Combine(packageRoot, "WebGL");
      string srcPath = Path.Combine(webGLFolder, PlatformUtilsClientName);

      srcPath = Path.GetFullPath(srcPath);

      if (!File.Exists(srcPath))
      {
        Debug.LogError($"[WebGLPlatformUtils] Could not find {PlatformUtilsClientName} at: {srcPath}");
        return;
      }

      string dstPath = Path.Combine(buildPath, PlatformUtilsClientName);
      File.Copy(srcPath, dstPath, true);

      InjectScriptIntoIndex(indexPath, PlatformUtilsClientName);
      Debug.Log("[WebGLPlatformUtils] platform-utils.js injected successfully.");
    }

    private void InjectScriptIntoIndex(string indexPath, string scriptFile)
    {
      string html = File.ReadAllText(indexPath);
      string marker = "<script src=\"Build/";
      string injection = $"<script src=\"{scriptFile}\"></script>\n";

      int insertIndex = html.IndexOf(marker, StringComparison.Ordinal);
      if (insertIndex >= 0)
      {
        html = html.Insert(insertIndex, injection);
        File.WriteAllText(indexPath, html);
      }
      else
      {
        Debug.LogWarning("[WebGLPlatformUtils] Could not find injection point in index.html");
      }
    }
  }
}
