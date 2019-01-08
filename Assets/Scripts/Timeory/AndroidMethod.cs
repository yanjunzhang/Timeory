using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndroidMethod : MonoBehaviour {
    public UIManager m_ui;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void LoadLocalTarget(string pwd)
    {
        m_ui.OnPasswordReturn(pwd);
        GameObject.FindObjectOfType<UIManager>().DebugToUI(pwd);
    }

    public void LoadScene(string sceneName)
    {
        App.MgrLevel.Load(sceneName);

    }
}
