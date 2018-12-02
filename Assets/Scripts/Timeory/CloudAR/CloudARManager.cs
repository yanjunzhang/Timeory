using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;

public class CloudARManager : MonoBehaviour {
    public enum States
    {
        CloudMode,
        LocalMove
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    //手动加载videoTarget
    public void LoadLocalTarget()
    {
        //关闭云识别

        App.MgrPost.LoadLocalTarget("pwd", InitLocalVideoCardTarget, ErrorPassword);
    }



    void ErrorPassword()
    {
        //密码错误
        Debug.Log("密码错误");
    }

    //初始化本地视频
    void InitLocalVideoCardTarget(VideoTargetDate data)
    {
        //var gameObj = new GameObject(imageTarget.Name);
        //gameObj.transform.SetParent(manager.transform);
        ImageTrackerBehaviour tracker = FindObjectOfType<ImageTrackerBehaviour>();
        GameObject _gameObj = new GameObject("ar1");
        _gameObj.transform.localPosition = Vector3.zero;
        var targetBehaviour = _gameObj.AddComponent<CloudARVideoTargetBehaviour>();
        targetBehaviour.Bind(tracker);
        targetBehaviour.SetupWithImage("ar1.jpg", StorageType.Assets, "ar1", new Vector2());
        App.MgrPrefab.Create(_gameObj, "VideoTarget", Vector3.zero, (gameObj) => {

            //gameObj.GetComponentInChildren<VideoTargetController>().Init(data);
        });


    }
}
