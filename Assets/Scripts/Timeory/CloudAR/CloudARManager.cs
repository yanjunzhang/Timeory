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
    public UIManager m_uiManager;
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
        //关闭重新识别按钮
        FindObjectOfType<UIManager>().rescan.SetActive(false);
        for (int i = 0; i < videoTargetControllers.Count; i++)
        {
            videoTargetControllers[i].SetToCardMode();
            videoTargetControllers[i].ResetVideo();
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
        m_uiManager.StartScanerRotate();
    }

    public void ReloadVideoTargetData(string id)
    {
        for (int i = 0; i < videoTargetControllers.Count; i++)
        {
            if (videoTargetControllers[i].m_data.id == id)
            {
                App.MgrPost.Load(videoTargetControllers[i].imageTarget,ReloadDataHandle);
            }
        }
    }



    void ReloadDataHandle(ImageTarget imageTarget,VideoTargetDate data)
    {
        for (int i = 0; i < videoTargetControllers.Count; i++)
        {
            if (videoTargetControllers[i].m_data.id == data.id)
            {
                videoTargetControllers[i].UpdateData(data);
            }
        }

    }

    //本地对象 的uid 就是pwd
    public void ReloadLocalVideoTargetData(string id)
    {
        MobileFunction.DebugByAndroid("重新加载本地对象: "+id);
        for (int i = 0; i < videoTargetControllers.Count; i++)
        {
            if (videoTargetControllers[i].m_data.id == id)
            {
                App.MgrPost.LoadLocalTarget(videoTargetControllers[i].m_data.targetUid,ReloadLocalDataHandle,()=>{
                    MobileFunction.DebugByAndroid("密码错误");
                });
                MobileFunction.DebugByAndroid("获取到该对象");
            }
        }
        MobileFunction.DebugByAndroid("没有获取到该对象");
    }
    void ReloadLocalDataHandle(VideoTargetDate data)
    {
        for (int i = 0; i < videoTargetControllers.Count; i++)
        {
            if (videoTargetControllers[i].m_data.id == data.id)
            {
                videoTargetControllers[i].UpdateData(data);
            }
        }
    }
}
