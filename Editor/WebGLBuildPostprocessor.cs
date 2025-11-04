using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace WebGLUtils.Editor
{
    public static class WebGLBuildPostprocessor
    {
        private const string PlatformUtilsClientName = "platform-utils.js";

        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.WebGL)
                return;

            string indexPath = Path.Combine(pathToBuiltProject, "index.html");
            if (!File.Exists(indexPath))
            {
                Debug.LogWarning("WebGL postprocessor: index.html not found");
                return;
            }

            string html = File.ReadAllText(indexPath);

            html = InjectScriptBeforeLoader(html, PlatformUtilsClientName);

            File.WriteAllText(indexPath, html);

            string srcPath = Path.Combine(Application.dataPath, "WebGL", PlatformUtilsClientName);
            string dstPath = Path.Combine(pathToBuiltProject, PlatformUtilsClientName);

            if (File.Exists(srcPath))
            {
                File.Copy(srcPath, dstPath, true);
                Debug.Log("Copied platform-utils.js to WebGL build folder.");
            }
            else
            {
                Debug.LogWarning($"platform-utils.js not found at {srcPath}");
            }
        }

        private static string InjectScriptBeforeLoader(string htmlContent, string scriptName)
        {
            if (htmlContent.Contains(scriptName))
            {
                Debug.Log("WebGL postprocess: " + scriptName + " already included.");
                return htmlContent;
            }

            int loaderIndex = htmlContent.IndexOf("loader.js", StringComparison.Ordinal);
            if (loaderIndex != -1)
            {
                int lineStart = htmlContent.LastIndexOf("<script", loaderIndex, StringComparison.Ordinal);
                if (lineStart != -1)
                {
                    string injection = $"<script src=\"{scriptName}\"></script>\n";
                    htmlContent = htmlContent.Insert(lineStart, injection);
                    Debug.Log($"Injected {scriptName} before Unity loader script.");
                }
                else
                    Debug.LogWarning("WebGL postprocess: could not find <script> tag for loader.js.");
            }
            else
                Debug.LogWarning("WebGL postprocess: loader.js script not found in index.html.");

            return htmlContent;
        }
    }
}
