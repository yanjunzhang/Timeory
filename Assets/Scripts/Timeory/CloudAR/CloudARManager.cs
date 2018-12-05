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
    public CloudARCloudBehaviour cloudReconginzer;
    //public List<Transform> targets;
    public List<VideoTargetController> videoTargetControllers;
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

        //App.MgrPost.LoadLocalTarget("pwd", InitLocalVideoCardTarget, ErrorPassword);
    }
    //所有target取消脱卡
    public void SetToCardMode()
    {
        for (int i = 0; i < videoTargetControllers.Count; i++)
        {
            videoTargetControllers[i].SetToCardMode();
        }
    }

    //打开/关闭 云识别
    public void SetCloudRecognize(bool canRec)
    {
        if (canRec)
        {
            cloudReconginzer.enabled = true;
        }
        else
            cloudReconginzer.enabled = false;
    }
    //清空所有target
    public void ClearAllTarget()
    {
        SetToCardMode();

    }
}
