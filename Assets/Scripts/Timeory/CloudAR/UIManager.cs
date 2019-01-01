using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Transform passwordPanel;
    public Transform passwordBtn;
    public Transform cantAddVideoPanel;
    public Transform errorPassword;
    public Transform addFriendSuccess;
    public CloudARManager m_manager;
    public InputField inputField;
    public Text debugPanel;
    bool isFlashOpen;
	// Use this for initialization
	void Start () {
		
	}
    private void Update()
    {

    }
    public void DebugToUI(string str)
    {
        debugPanel.text = str;
    }
    //打开密码
    public void OnPasswordBtnClick()
    {
        MobileFunction.PopUpPasswordDialog("btn Click");
        //passwordBtn.gameObject.SetActive(false);
        //passwordPanel.gameObject.SetActive(true);
        //取消识别 删除所有target

    }
    //打开闪光灯
    public void OnFlashBtnClick()
    {
        isFlashOpen = !isFlashOpen;
        ARBuilder.Instance.CameraDeviceBehaviours[0].SetFlashTorchMode(isFlashOpen);
    }
    //退出
    public void OnQuitBtnClick()
    {
        Application.Quit();
    }
    //输入密码后的操作
    public void OnPasswordReturn()
    {
        OnPasswordReturn(inputField.text);
    }
    public void OnPasswordReturn(string pwd)
    {
        App.MgrPost.LoadLocalTarget(pwd, InitLocalVideoCardTarget, ErrorPassword);
        passwordPanel.gameObject.SetActive(false);
        passwordBtn.gameObject.SetActive(true);
        inputField.text = "";
    }
    //输入密码关闭按钮的点击
    public void OnPasswordCloseBtnClick()
    {
        passwordBtn.gameObject.SetActive(true);
        passwordPanel.gameObject.SetActive(false);
        inputField.text = "";
    }

    //显示今日添加视频上限
    public void ShowCantAddVideoUI()
    {
        cantAddVideoPanel.gameObject.SetActive(true);
    }
    //添加上限提示按钮的点击
    public void OnCantAddVideUICloseBtnClick()
    {
        cantAddVideoPanel.gameObject.SetActive(false);
    }
    //显示添加好友成功
    public void ShowAddFriendSuccess()
    {
        addFriendSuccess.DOScale(Vector3.one, 1f);
        addFriendSuccess.DOScale(Vector3.zero, 1f).SetDelay(3f);
    }



    void ErrorPassword()
    {
        //密码错误
        errorPassword.DOScale(Vector3.one, 1f);
        errorPassword.DOScale(Vector3.zero, 1f).SetDelay(3f);
        Debug.Log("密码错误");
    }

    //初始化本地视频
    void InitLocalVideoCardTarget(VideoTargetDate data)
    {
        //var gameObj = new GameObject(imageTarget.Name);
        //gameObj.transform.SetParent(manager.transform);
        ImageTrackerBehaviour tracker = FindObjectOfType<ImageTrackerBehaviour>();
        GameObject _gameObj = new GameObject(data.targetUid);
        _gameObj.transform.localPosition = Vector3.zero;
        var targetBehaviour = _gameObj.AddComponent<CloudARVideoTargetBehaviour>();
        targetBehaviour.Bind(tracker);
        targetBehaviour.SetupWithImage(data.timeImgSrc, StorageType.Absolute, data.targetUid, new Vector2());
        App.MgrPrefab.Create(_gameObj, "VideoTarget", Vector3.zero, (gameObj) => {
            gameObj.GetComponentInChildren<VideoTargetController>().Init(data);
        });


    }
}
