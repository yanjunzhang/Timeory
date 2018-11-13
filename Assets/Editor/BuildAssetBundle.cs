using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/**
 * @author:魏东方
 * @des:把场景、预制打包AssetBundle.使用代码进行动态加载。
 * @注意: unity5.x版本
 *    
 *     操作步骤:
 *     a.点击1.Clear All AssetBundleName （在创建资源包名
 *     之前先清除已存在的资源名，避免没必要的文件被打包）。
 *     b.点击2.Creat All AssetBundleName (创建需要打包的资源包名)
 *     c.点击3.Builde All AssetBundle (开始打包资源)
 *     d.clear All AssetBundle (清除所有资源，从新打包所需资源)
 * 
 * @date:2016-11-11
*/


public enum BundleTarget
{
    Windows,
    Android,
    MAC,
    IOS
}

public class BuildAssetBundle : EditorWindow{



    private BundleTarget m_BundleType = BundleTarget.IOS;
    private bool m_Prefabs = true;
    private bool m_Scenes = true;

    private string m_Variant = "unity3d";
    private string m_OutPath = "";

    [MenuItem("Quibos/AssetBundle", false, 23)]
    public static void OpenWindows()
    {
        EditorWindow window = GetWindow(typeof(BuildAssetBundle));
        window.titleContent.text = "Assets Bundle Editor";
        window.minSize = new Vector2(400, 250);
        window.maxSize = new Vector2(400, 250);
        window.Show();
    }

    void OnGUI()
    {

        GUILayout.BeginHorizontal("Box");
        GUILayout.Space(5);
        m_BundleType = (BundleTarget)EditorGUILayout.EnumPopup("Bundle Type", (System.Enum)m_BundleType);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("Box");
        GUILayout.Space(5);
        m_Prefabs = EditorGUILayout.Toggle("Prefabs", m_Prefabs);
        GUILayout.Space(30);
        m_Scenes = EditorGUILayout.Toggle("Scenes", m_Scenes);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);
        if (GUILayout.Button("Auto Creat AssetBundelName", GUILayout.Height(25))) {
            CreatAssetBundleName();
        }

        GUILayout.Space(5);
        if (GUILayout.Button(" Begin Bundle", GUILayout.Height(25))) {
            buildAssetBundle(m_BundleType);
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Remove All AssetBundle", GUILayout.Height(25))) {
            ClearAssetBundle();
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Remove All AssetBundleName", GUILayout.Height(25))) {
            ClearAssetBundlesName();
        }
    }

    /// <summary>  
    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包  
    /// 只要设置了AssetBundleName的，都会进行打包，不论在什么目录下  
    /// </summary>  
    /// 
    private void ClearAssetBundlesName() {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
       
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++) {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];           
        }

        for(int j = 0;j < oldAssetBundleNames.Length; j++){
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
        Debug.Log("AssetBundleNames is number : " + length);
       
    } 

    /// <summary>
    /// 创建需要打包的资源包名和场景包名
    /// </summary>

    private void CreatAssetBundleName()
    {
        if (m_Scenes) {
            //检查打包文件路径是否正确
            string scenepath = Application.dataPath + "/Scenes/";

            if (!System.IO.Directory.Exists(scenepath)) {
                this.ShowNotification(new GUIContent("场景文件夹不存在！"));
                return;
            }

            // 创建场景资源包名
            string[] filesSce = Directory.GetFiles(scenepath, "*.unity", SearchOption.AllDirectories);
            for (int i = 0; i < filesSce.Length; i++) {
                string currentFile = filesSce[i].Replace("\\", "/");
                int startIndex = currentFile.IndexOf("Assets");
                string assetFile = currentFile.Substring(startIndex, currentFile.Length - startIndex);
                string sceneName = Path.GetFileNameWithoutExtension(assetFile);
                AssetImporter ai = AssetImporter.GetAtPath(assetFile);
                ai.assetBundleName = sceneName;
                ai.assetBundleVariant = m_Variant;
            }
            Debug.Log(" Creat AssetBundleName Finish ! Scenes Total :" + filesSce.Length );
        }

        if (m_Prefabs) {
            string prefabspath = Application.dataPath + "/Resources/Prefabs/";

            if (!System.IO.Directory.Exists(prefabspath)) {
                this.ShowNotification(new GUIContent("预制文件夹不存在！"));
                return;
            }

            //创建预制资源包名
            string[] filesPre = Directory.GetFiles(prefabspath, "*.prefab", SearchOption.AllDirectories);
            for (int i = 0; i < filesPre.Length; i++) {
                string currentFile = filesPre[i].Replace("\\", "/");
                int startIndex = currentFile.IndexOf("Assets");
                string assetFile = currentFile.Substring(startIndex, currentFile.Length - startIndex);
                string assetName = Path.GetFileNameWithoutExtension(assetFile);
                string name = WWW.EscapeURL(assetName);
                name = name.Replace("%", "");
                AssetImporter ai = AssetImporter.GetAtPath(assetFile);
                ai.assetBundleName = name;
                ai.assetBundleVariant = m_Variant;
            }

            Debug.Log(" Creat AssetBundleName Finish !  Prefabs Total :" + filesPre.Length);
        }
       
       
       
	}

    /// <summary>
    /// 开始打包资源
    /// </summary>

	private void buildAssetBundle( BundleTarget op) {
        m_OutPath = Application.streamingAssetsPath + "/";
		
		if (!Directory.Exists (m_OutPath)) {
			Directory.CreateDirectory (m_OutPath);
		}

         BuildTarget builderTarget = BuildTarget.StandaloneWindows;
        switch ( op ) {
            case BundleTarget.Windows:
                builderTarget = BuildTarget.StandaloneWindows;
                break;
            case BundleTarget.Android:
                builderTarget = BuildTarget.Android;
                break;
            case BundleTarget.IOS:
                builderTarget = BuildTarget.iOS;
                break;
            case BundleTarget.MAC:
                builderTarget = BuildTarget.StandaloneOSXUniversal;
                break;
        }

        BuildPipeline.BuildAssetBundles(m_OutPath,BuildAssetBundleOptions.ChunkBasedCompression,builderTarget);
      //  AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log(" Creat AssetBundle Finish !");
	}

    /// <summary>
    /// 清除所有打包的资源
    /// </summary>

	private void ClearAssetBundle() {
		string outPath=Application.streamingAssetsPath;
		if (!Directory.Exists (outPath)) {
			return;
		}

		deleteFileOrFolder (outPath);
		AssetDatabase.Refresh ();

        Debug.Log(" Clear All AssetBundle Finish !");
	}

  
	private void deleteFileOrFolder(string fileOrFolder) {
		if (Directory.Exists (fileOrFolder)) {
			string[] allFiles=Directory.GetFiles (fileOrFolder);
			if (allFiles != null) {
				for (int i = 0; i < allFiles.Length; i++) {
					deleteFileOrFolder (allFiles [i]);
				}				
			}

			string[] allFolders=Directory.GetDirectories (fileOrFolder);
			if (allFolders != null) {
				for (int i = 0; i < allFolders.Length; i++) {
					deleteFileOrFolder (allFolders [i]);
				}				
			}
			Directory.Delete (fileOrFolder);
		}
		else {
			if (File.Exists (fileOrFolder)) {
				File.Delete (fileOrFolder);
			}
		}
	}
}
