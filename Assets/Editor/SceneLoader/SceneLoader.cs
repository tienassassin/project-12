#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static partial class SceneLoader
{
    private static string PATH_TO_SCENES_FOLDER = "/_root/Scenes/";
    private static string PATH_TO_OUTPUT_SCRIPT_FILE = "/Editor/SceneLoaderDropdowns.cs";

    [MenuItem("Tools/Generate Scene Loader")]
    public static void GenerateSceneLoadMenuCode()
    {
        StringBuilder result = new StringBuilder();
        string basePath = Application.dataPath + PATH_TO_SCENES_FOLDER;
        AddClassHeader(result);
        AddCodeForDirectory(new DirectoryInfo(basePath), result);
        AddClassFooter(result);

        string scriptPath = Application.dataPath + PATH_TO_OUTPUT_SCRIPT_FILE;
        File.WriteAllText(scriptPath, result.ToString());

        void AddCodeForDirectory(DirectoryInfo directoryInfo, StringBuilder result)
        {
            FileInfo[] fileInfoList = directoryInfo.GetFiles();
            for (int i = 0; i < fileInfoList.Length; i++)
            {
                FileInfo fileInfo = fileInfoList[i];
                if (fileInfo.Extension == ".unity")
                {
                    AddCodeForFile(fileInfo, result);
                }
            }

            DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
            for (int i = 0; i < subDirectories.Length; i++)
            {
                AddCodeForDirectory(subDirectories[i], result);
            }

            void AddCodeForFile(FileInfo fileInfo, StringBuilder result)
            {
                string subPath = fileInfo.FullName.Replace('\\', '/').Replace(basePath, "");
                string assetPath = ASSETS_SCENE_PATH + subPath;

                string functionName = fileInfo.Name.Replace(".unity", "").Replace(" ", "").Replace("-", "");

                result.Append("        [MenuItem(\"Scenes/").Append(subPath.Replace(".unity", "")).Append("\")]")
                    .Append(Environment.NewLine);
                result.Append("        public static void Load").Append(functionName).Append("() { OpenScene(\"")
                    .Append(assetPath).Append("\"); }").Append(Environment.NewLine);
                ;
            }
        }
    }

    private static void AddClassHeader(StringBuilder result)
    {
        result.Append(@"using UnityEditor;

public partial class SceneLoader
{
");
        result.Append(@"#if UNITY_EDITOR
");
    }

    private static void AddClassFooter(StringBuilder result)
    {
        result.Append(@"#endif
}");
    }

    private static string ASSETS_SCENE_PATH = "Assets/_root/Scenes/";

    private static void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
    }
}
#endif