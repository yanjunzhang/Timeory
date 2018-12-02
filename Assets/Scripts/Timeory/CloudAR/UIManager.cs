using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using DG.Tweening;

public class UIManager : MonoBehaviour {
    public Transform passwordPanel;
    public Transform passwordBtn;
    public Transform cantAddVideoPanel;
    public Transform addFriendSuccess;
    public CloudARManager m_manager;
    public UnityEngine.UI.Text debugPanel;
    bool isFlashOpen;
	// Use this for initialization
	void Start () {
		
	}
    private void Update()
    {

    }
    public void DebugToUI(string str)
    {
        debugPanel.text += str;
        debugPanel.text += " | ";
    }
    //打开密码
    public void OnPasswordBtnClick()
    {
        passwordBtn.gameObject.SetActive(false);
        passwordPanel.gameObject.SetActive(true);
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

    }
    //输入密码关闭按钮的点击
    public void OnPasswordCloseBtnClick()
    {
        passwordBtn.gameObject.SetActive(true);
        passwordPanel.gameObject.SetActive(false);
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

}
