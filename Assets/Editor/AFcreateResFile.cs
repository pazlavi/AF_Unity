using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class MyBuildPostprocessor {
    public static string devKey = "replece_with_dev_key";
    public static string appID = "replece_with_app_id";
    public static bool isDebug = false;
    public static bool appsFlyerShouldSwizzle = false;
    private static string dirPath = "Assets/StreamingAssets";
    private static string propFileName = "AppsFlyer.properties";
    
    [PostProcessBuildAttribute]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        
        if (target == BuildTarget.iOS)
        {
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            
            PlistElementDict rootDict = plist.root;
            rootDict.SetBoolean("AppsFlyerShouldSwizzle", appsFlyerShouldSwizzle);
            rootDict.SetBoolean("isDebug", isDebug);
            rootDict.SetString("devKey", devKey);
            rootDict.SetString("appID", appID);
            
            File.WriteAllText(plistPath, plist.WriteToString());
            
            Debug.Log("Info.plist updated with AppsFlyerShouldSwizzle");

        }else if (target == BuildTarget.Android){
              if (!Directory.Exists(dirPath))
             {
                 Directory.CreateDirectory(dirPath);
             }

            string path = dirPath + "/" + propFileName;
            string text =
            "AF_DEV_KEY=" + devKey + "\nAF_DEBUG="+isDebug;
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(text);
            writer.Close();

        }
        
    }

}