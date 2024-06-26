using UnityEditor;

public class BuildAssetBundles
{
    [MenuItem("Editor Utils/Build BYOJ Asset Bundle")]
    static void BuildBYOJAssetBundle()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }

        var bundleBuild = new AssetBundleBuild
        {
            assetBundleName = "byoj",
            assetNames      = AssetDatabase.GetAssetPathsFromAssetBundle("byoj")
        };
        
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, new[] { bundleBuild }, BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
    }
}