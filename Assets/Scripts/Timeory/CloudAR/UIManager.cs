using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager _instance;
    public Transform passwordPanel;
    public Transform passwordBtn;
    public Transform cantAddVideoPanel;
    public Transform errorPassword;
    public Transform addFriendSuccess;
    public CloudARManager m_manager;
    public InputField inputField;
    public Text debugPanel;
    public Transform scaner;
    public UnityEngine.UI.Image image;
    public Sprite[] flash;
    public GameObject rescan;
    bool isFlashOpen;
    bool isScanerStoped;
	// Use this for initialization
	void Start () {
        _instance = this;
		//扫描UI旋转
        scaner.GetChild(0).DORotate(new Vector3(0, 0, 360f), 1.4f).SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart).SetRelative();
        scaner.GetChild(1).DORotate(new Vector3(0, 0, 360f), 1.6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetRelative();
        scaner.GetChild(2).DORotate(new Vector3(0, 0, 360f), 1.8f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetRelative();
        scaner.GetChild(3).DORotate(new Vector3(0, 0, -360f), 2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetRelative();
    }
    private void Update()
    {

    }

    /// <summary>
    /// 停止扫描中，并且显示重新识别按钮
    /// </summary>
    public void StopScanerRotate()
    {
        if (!isScanerStoped)
        {
            isScanerStoped = true;
            //停止扫描UI的旋转
            scaner.gameObject.SetActive(false);
            //显示重新识别
            //rescan.SetActive(true);
        }
    }

    /// <summary>
    /// 重新开始扫描中  隐藏重新识别
    /// </summary>
    public void StartScanerRotate()
    {
        if (isScanerStoped)
        {
            isScanerStoped = false;
            //停止扫描UI的旋转
            scaner.gameObject.SetActive(true);
            //隐藏重新识别
            rescan.SetActive(false);
        }
    }

    public void DebugToUI(string str)
    {
        debugPanel.text = str;
    }
    //打开密码
    public void OnPasswordBtnClick()
    {
        try
        {
            MobileFunction.PopUpPasswordDialog("btn Click");
        }
        catch (System.Exception ex)
        {

        }
        //passwordBtn.gameObject.SetActive(false);
        //passwordPanel.gameObject.SetActive(true);
        //取消识别 删除所有target

    }
    //打开闪光灯
    public void OnFlashBtnClick()
    {
        isFlashOpen = !isFlashOpen;
        ARBuilder.Instance.CameraDeviceBehaviours[0].SetFlashTorchMode(isFlashOpen);
        if (isFlashOpen)
        {
            //闪光灯打开状态时
            image.sprite = flash[1];
            image.SetNativeSize();
        }else{
            //闪光灯关闭时
            image.sprite = flash[0];
            image.SetNativeSize();
        }
    }
    //退出
    public void OnQuitBtnClick()
    {
        //Application.Quit();
        MobileFunction.QuitUnity();
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
        targetBehaviour.TargetFound += OnTargetFound;
        targetBehaviour.Bind(tracker);
        targetBehaviour.SetupWithImage(data.timeImgSrc, StorageType.Absolute, data.targetUid, Vector2.one*12f);
        App.MgrPrefab.Create(_gameObj, "VideoTarget", Vector3.zero, (gameObj) => {
            gameObj.GetComponentInChildren<VideoTargetController>().Init(data,false);
        });


    }
    void OnTargetFound(TargetAbstractBehaviour behaviour)
    {
        //停止旋转
        StopScanerRotate();
    }
}
