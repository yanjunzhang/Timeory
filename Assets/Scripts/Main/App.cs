using UnityEngine;
using System.Collections;


public enum AssetType
{
	Resource ,
	Bundle ,
}

public class App : MonoBehaviour
{

	#region Config

	public AssetType _AssetType;

	public string _key;
	public string _Server;
	public string _token;
    public string _id;
    public string _phone;


	#endregion

	public static bool FirstRun;

	private static App m_App;
	public static App MgrConfig {
		get {
			if (m_App) {
				return m_App;
			} else {
				GameObject go = Instantiate (Resources.Load ("App")) as GameObject;
				go.name = "App";
				m_App = go.GetComponent<App> ();
				return m_App;
			}
		}
	}

	private static MgrTool m_MgrTool;
	private static MgrData m_MgrData;
	private static MgrResource m_MgrResource;
	private static MgrBundle m_MgrBundle;
	private static MgrAudio m_MgrAudio;
	private static MgrPrefab m_MgrPrefab;
	private static MgrLevel m_MgrLevel;
	private static MgrPost m_MgrPost;
    private static MgrDownload m_MgrDownload;

	public static MgrPost MgrPost {
		get {
			CheckPreExist ();
			return m_MgrPost;
		}
	}
	public static MgrTool MgrTool {
		get {
			CheckPreExist ();
			return m_MgrTool;
		}
	}
	public static MgrData MgrData {
		get {
			CheckPreExist ();
			return m_MgrData;
		}
	}
	public static MgrResource MgrResource {
		get {
			CheckPreExist ();
			return m_MgrResource;
		}
	}
	public static MgrBundle MgrBundle {
		get {
			CheckPreExist ();
			return m_MgrBundle;
		}
	}
	public static MgrAudio MgrAudio {
		get {
			CheckPreExist ();
			return m_MgrAudio;
		}
	}
	public static MgrPrefab MgrPrefab {
		get {
			CheckPreExist ();
			return m_MgrPrefab;
		}
	}
	public static MgrLevel MgrLevel {
		get {
			CheckPreExist ();
			return m_MgrLevel;
		}
	}
    public static MgrDownload MgrDownload
    {
        get
        {
            CheckPreExist();
            return m_MgrDownload;
        }
    }


    #region Mono
    void Awake ()
	{
		if (!m_App) {
			m_App = this;
		}
		DontDestroyOnLoad (gameObject);

		InitMgr ();
		InitFPS ();
		CheckFirstRun ();
	}

	void Start ()
	{

	}

	#endregion


	private void InitFPS ()
	{
		switch (Application.platform) {
		case RuntimePlatform.Android:
		case RuntimePlatform.IPhonePlayer:
			Application.targetFrameRate = 60;
			break;
		}
	}

	private void CheckFirstRun ()
	{
		if (PlayerPrefs.HasKey ("FirstRun")) {
			FirstRun = false;
		} else {
			FirstRun = true;
			PlayerPrefs.SetString ("FirstRun", "false");
		}
	}

	private void InitMgr ()
	{
		m_MgrData = transform.Find ("MgrData").GetComponent<MgrData> ();
		m_MgrTool = CreateMgr<MgrTool> (gameObject);
		m_MgrResource = CreateMgr<MgrResource> (gameObject);
		m_MgrBundle = CreateMgr<MgrBundle> (gameObject);
		m_MgrAudio = CreateMgr<MgrAudio> (gameObject);
		m_MgrPrefab = CreateMgr<MgrPrefab> (gameObject);
		m_MgrLevel = CreateMgr<MgrLevel> (gameObject);
		m_MgrPost = CreateMgr<MgrPost> (gameObject);
        m_MgrDownload= CreateMgr<MgrDownload>(gameObject);
    }

	private T CreateMgr<T> (GameObject parent) where T : Component
	{
		T component = this.GetComponentInChildren<T> ();
		if (component == null) {
			GameObject go = new GameObject (typeof(T).Name);
			component = go.AddComponent<T> ();
			go.transform.parent = parent.transform;
		}
		return component;
	}

	public static bool CheckPreExist ()
	{
		if (MgrConfig)
			return true;
		else
			return false;
	}

	public static void Log (object ob)
	{
		Debug.Log (ob);
	}

	public static void LogError (object ob)
	{
		Debug.LogError (ob);
	}

	public static void LogWarning (object ob)
	{
		Debug.LogWarning (ob);
	}
		

}
