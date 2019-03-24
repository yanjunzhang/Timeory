using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndroidMethod : MonoBehaviour {
    public UIManager m_ui;
    public CloudARManager arManager;
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
    /// <summary>
    /// 设置id
    /// </summary>
    /// <param name="id">Identifier.</param>
    public void SetId(string id)
    {
        App.MgrConfig._id = id;
        MobileFunction.DebugByAndroid("currentId=" + App.MgrConfig._id);
    }
    /// <summary>
    /// 设置token
    /// </summary>
    /// <param name="token">Token.</param>
    public void SetToken(string token)
    {
        App.MgrConfig._token = token;
        MobileFunction.DebugByAndroid("currentToken="+App.MgrConfig._token);
    }
    public void SetPhone(string phone)
    {
        App.MgrConfig._phone = phone;
        MobileFunction.DebugByAndroid("currentPhone=" + App.MgrConfig._phone);
    }
    public void SetServer(string server)
    {
        App.MgrConfig._Server = server;
        MobileFunction.DebugByAndroid("currentServer=" + App.MgrConfig._Server);
    }

    public void LoadScene(string sceneName)
    {
        App.MgrLevel.Load(sceneName);
    }

    //本地对象 的uid 就是pwd
    public void ReloadLocalVideoTargetData(string id)
    {
        arManager.ReloadLocalVideoTargetData(id);
    }

    public void ReloadVideoTargetData(string id)
    {
        arManager.ReloadVideoTargetData(id);
    }

}
