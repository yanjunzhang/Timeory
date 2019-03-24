using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using EasyAR;
using UnityEngine.Video;

public struct VideoTargetDate
{
    public VideoTargetDate(string targetUid,string timeVideoScr,string id,string timeImgSrc){
        this.targetUid = targetUid;
        this.timeVideoSrc = timeVideoScr;
        this.userName="";
        this.createDate = "";
        this.status = "";
        this.timeImgSrc=timeImgSrc;
        this.id = id;
        this.videoList = new List<VideoTargetCell> ();

    }
    public string targetUid;
    public string userName;
    public string createDate;
    public string status;
    public string timeImgSrc;
    public string timeVideoSrc;
    public string id;
    public List<VideoTargetCell> videoList;

}

public struct VideoTargetCell
{
    public VideoTargetCell(string createDate,string nickName,string timeVideoSrc,string userlogo,string userId,bool isFriend,string isVertical)
    {
        this.isVertical = isVertical;
        this.createDate = createDate;
        this.timeVideoSrc = timeVideoSrc;
        this.nickName = nickName;
        this.userlogo = userlogo;
        this.userId = userId;
        this.timeLineId = "";
        this.isFriend = isFriend;
    }
    public string isVertical;
    public string createDate;
    public string timeLineId;
    public string timeVideoSrc;
    public string nickName;
    public string userlogo;
    public string userId;
    public bool isFriend;
}


public class VideoTargetController : MonoBehaviour {
    public enum States
    {
        PlaneMode,
        ARMode,
        WithoutCard
    }

    private States _state;
    public States State{
        get{ 
            return _state;
        }
        set{ 
            if (value==_state) {
                return;
            }
            switch (value) {
            case States.ARMode:
                break;
            case States.PlaneMode:
                break;
            case States.WithoutCard:
                break;
            default:
                break;
            }
        }
    }
    public int selectedNumber;
    public Transform BGWide, BGTall;
    public Transform backBtn, nextBtn,playBtn;
    public Transform bg;
    public Transform[] btns;
    //[HideInInspector]
    public EasyAR.VideoPlayerBehaviour vPlayer;
    //public GameObject playImage;
    //public RectTransform background;
    public UnityEngine.UI.Image ui_userlogo;
    public Text ui_nickName;
    public Text ui_date;

    Transform cam;
    Collider videoCollider;
    bool isPlaying = true;
    bool isPlaneMode=true;
    CloudARVideoTargetBehaviour videoTargetBehaviour;
    public VideoTargetDate m_data;
    ImageTargetBaseBehaviour ARCardTarget;//target对象   处理脱卡
    CloudARManager cloudArManager;
    public ImageTarget imageTarget;
    string targetName;
    bool isFirst = true;
    bool isCloud;

    /*void OnEnable()
    {
        if (isFirst)
        {
            transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.0028f, 0, 0.5f);
            transform.GetChild(0).GetComponent<RectTransform>().DOScaleY(0.0028f, 0.5f);
            isFirst = false;
        }
    }*/
    //执行一次
    void Awake()
    {
        //ARCardTarget = transform.parent;
        videoCollider = vPlayer.GetComponent<Collider>();
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            //单击
            PauseVideo();
        }
    }
    public void SetVideoPath(string path)
    {
        //path = GetComponent<VideoDownloader>().GetVideoPath(path);
        path = App.MgrDownload.GetVideoPath(path);
        //vPlayer = GetComponentInChildren<EasyAR.VideoPlayerBehaviour>();、
        vPlayer.Stop();
        vPlayer.Path = path;
        //GameObject.Find("UI").GetComponent<UIManager>().DebugToUI("video Path : "+path);
        vPlayer.Open();
        PlayVideo();
        
    }
    public void ResetPlaneRotation()
    {

        vPlayer.transform.rotation=Quaternion.identity;
    }

    //初始化
    public void Init(VideoTargetDate data,bool isCloud = true)
    {
        cam = GameObject.Find("ARCamera").transform;
        this.isCloud = isCloud;
        targetName = data.targetUid;
        ARCardTarget = transform.GetComponentInParent<ImageTargetBaseBehaviour>();
        ARCardTarget.TargetFound+= OnTargetFound;
        ARCardTarget.TargetLost += OnTargetLost;

        cloudArManager = GameObject.FindObjectOfType<CloudARManager>();
        cloudArManager.videoTargetControllers.Add(this);
        m_data = data;
        this.selectedNumber = 0;
        videoTargetBehaviour = GetComponentInParent<CloudARVideoTargetBehaviour> ();
        videoTargetBehaviour.TargetFound += OnTargetFound_First;
        transform.localScale = Vector3.one;
        //vPlayer.transform.rotation=Quaternion.Euler(new Vector3(180f,0,0));
        
        //准备完后自动播放
        vPlayer.VideoReadyEvent += (sender, e) => {
            //GameObject.FindObjectOfType<UIManager>().DebugToUI("ready");
            //激活视频collider（允许点击暂停）
            videoCollider.enabled = true;
            vPlayer.Play();
        };
        //播放结束后暂停
        vPlayer.VideoReachEndEvent += (sender, e) => {
            //GameObject.FindObjectOfType<UIManager>().DebugToUI("reachEnd");
            //停止播放
            //PauseVideo();
            //如果是第一次就展开动画
            if (isFirst)
            {
                StopCoroutine("IE_Spreadout");
                StartCoroutine("IE_Spreadout");
            }
        };
        RefreshUI();

        UIManager._instance.DebugToUI("videoPlayer path: "+ARCardTarget.Path);
    }

    void OnTargetFound_First(TargetAbstractBehaviour behaviour)
    {
        if (isFirst)
        { 
            bg.GetComponent<UnityEngine.UI.Image>().DOFillAmount(1f, 1f).ChangeStartValue(0.1f);
            App.MgrAudio.Play("open");
        }

    }

    IEnumerator IE_Spreadout()
    {
        
        ui_userlogo.transform.DOScale(0.5f,0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero);
        yield return new WaitForSeconds(0.3f);
        btns[5].DOScale(1f, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero);
        yield return new WaitForSeconds(0.3f);
        btns[0].DOScale(1f, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero);
        btns[1].DOScale(1f, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero);
        yield return new WaitForSeconds(0.3f);
        btns[2].DOScale(1f, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero);
        yield return new WaitForSeconds(0.3f);
        btns[3].DOScale(1f, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero);
        yield return new WaitForSeconds(0.3f);
        btns[4].DOScale(1f, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero);
        yield return new WaitForSeconds(0.3f);
        /*
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].DOScale(1f, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero);
            yield return new WaitForSeconds(0.3f);
        }
        */
        isFirst = false;
    }

    public void UpdateData(VideoTargetDate data)
    {
        m_data = data;
        this.selectedNumber = 0;
        RefreshUI();
    }

    //下一个视频
    public void OnNextBtnClick()
    {
        App.MgrAudio.MultPlay("Open3");
        if (selectedNumber<m_data.videoList.Count-1) {
            //下一个
            selectedNumber++;
        }
        RefreshUI ();
    }
    //上一个视频
    public void OnBackBtnClick()
    {
        App.MgrAudio.MultPlay("Open3");
        if (selectedNumber>0) {
            selectedNumber--;
        }
        RefreshUI ();
    }
    //更新UI
    void RefreshUI()
    {
        //更新界面
        if (m_data.videoList[selectedNumber].isVertical == "0")
        {
            TurnToWideMode();
        }
        else
            TurnToTallMode();
        //更新按钮
        if (selectedNumber==0) {
            if (m_data.videoList.Count>1) {
                SetArrowBtn(false,true);
            }else if (m_data.videoList.Count==1) {
                SetArrowBtn(false,false);
            }
        }else if (selectedNumber>0&&selectedNumber<m_data.videoList.Count-1) {
            SetArrowBtn(true,true);
        }else if (selectedNumber==m_data.videoList.Count-1) {
            SetArrowBtn(true,false);
        }

        if (m_data.videoList[selectedNumber].isFriend)
        {
            btns[2].gameObject.SetActive(true);
        }else
            btns[2].gameObject.SetActive(false);
        //更新用户信息
        Debug.Log("当前选择： "+selectedNumber);
        VideoTargetCell currCell = m_data.videoList[selectedNumber];
        ui_date.text = currCell.createDate;
        ui_nickName.text = currCell.nickName;
        App.MgrDownload.LoadImageWithUrl(ui_userlogo, currCell.userlogo);
        //切换视频
        SetVideoPath (currCell.timeVideoSrc);
    }
    void SetArrowBtn(bool back,bool next)
    {
        backBtn.gameObject.SetActive (back);
        nextBtn.gameObject.SetActive (next);
    }
    //播放视频
    public void PlayVideo()
    {
        //隐藏播放icon
        playBtn.gameObject.SetActive(false);
        //激活视频collider
        videoCollider.enabled = true;
        //继续视频
        vPlayer.Play();
    }
    //暂停视频
    void PauseVideo()
    {
        //显示播放icon
        playBtn.gameObject.SetActive(true);
        //关闭视频collider
        videoCollider.enabled = false;
        //暂停视频
        vPlayer.Pause();
    }
    //全屏播放
    public void PlayOnPhonePlayer()
    {
        //string path =downloader.GetVideoPath(transform.GetComponentInChildren<EasyAR.VideoPlayerBehaviour>().Path);
        //PlayOnPhonePlayer(path);
    }
    void PlayOnPhonePlayer(string moviePath)
    {
        Handheld.PlayFullScreenMovie(moviePath, new Color(0, 0, 0, 0), FullScreenMovieControlMode.Full);
    }
    //添加好友
    public void OnAddFriendBtnClick()
    {
        App.MgrAudio.MultPlay("Open3");
        string userId = m_data.videoList[selectedNumber].userId;
        GameObject.FindObjectOfType<UIManager>().DebugToUI("Add Friend: "+userId);
        try
        {
            MobileFunction.SendFriendRequest(userId);
        }
        catch (System.Exception ex)
        {
            //GameObject.FindObjectOfType<UIManager>().DebugToUI(ex.ToString());
        }
    }
    //添加视频
    public void OnAddVideBtnClick()
    {
        App.MgrAudio.MultPlay("Open3");
        //string targetId = m_data.targetUid;
        string userId = m_data.id;
        GameObject.FindObjectOfType<UIManager>().DebugToUI("Add Video: " + userId);
        try
        {
            if (isCloud)
            {
                MobileFunction.AddVideoIntoCloudSpace(userId);
            }else{
                MobileFunction.AddVideoIntoLocalSpace(userId);
            }

        }
        catch (System.Exception ex)
        {
            //GameObject.FindObjectOfType<UIManager>().DebugToUI(ex.ToString());
        }

    }
    /*
    public void SetBackgroundSize(float width, float height)
    {
        background.gameObject.SetActive(true);
        background.localScale = new Vector3(1, height / width, 1);
        Debug.Log(width.ToString() + height.ToString());
    }
    

    //播放按钮
    void OnPlayBtnClick()
    {
        Debug.Log("OnPlayBtnClick");
        if (isPlaying)
        {
            vPlayer.Pause();
            playImage.SetActive(true);
        }
        else
        {
            vPlayer.Play();
            playImage.SetActive(false);
        }

        isPlaying = !isPlaying;
    }
    */

    //切换模式
    public void OnModeBtnClick()
    {
        App.MgrAudio.MultPlay("Open3");
        if (isPlaneMode) {
            TurnARMode ();
        } else
            TurnPlaneMode();
        isPlaneMode = !isPlaneMode;

    }

    //切换到平面模式
    void TurnPlaneMode()
    {
        //transform.GetChild(0).GetComponent<RectTransform>().DOLocalRotate(new Vector3(90f, 0, 0), 1f);
        //vPlayer.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
        transform.localPosition = new Vector3(0, 0, 0);

    }
    //切换到立体模式
    void TurnARMode()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.DOLocalRotate(new Vector3(-71f, 0, 0), 1f);
        //transform.GetChild(0).GetComponent<RectTransform>().DOLocalRotate(new Vector3(19f, 0, 0), 1f);
        //vPlayer.transform.DOLocalRotate(new Vector3(-71f, 0, 0), 1f);

    }
    //脱卡模式
    public void SetToLoseCardMode()
    { 
        //脱卡(设置父物体)
        cam.position = Vector3.zero;
        cam.rotation = Quaternion.identity;
        transform.SetParent(cloudArManager.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        //btns[4].DOScale(0, 1f);//切换模式按钮
        btns[4].GetComponent<UnityEngine.UI.Image>().DOFade(0, 1f);
        //显示重新识别按钮
        FindObjectOfType<UIManager>().rescan.SetActive(true);
    }
    //恢复AR模式 
    public void SetToCardMode()
    {
        transform.SetParent(ARCardTarget.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;  
        //btns[4].DOScale(1, 1f);//切换模式按钮
        btns[4].GetComponent<UnityEngine.UI.Image>().DOFade(1f, 1f);
    }
    void OnTargetFound(TargetAbstractBehaviour behaviour)
    {
        //取消脱卡
        cloudArManager.SetToCardMode();
        try
        {
            if (isCloud)
            {
                MobileFunction.OnCloudIdentifySuccess(m_data.videoList[selectedNumber].userId,m_data.videoList[selectedNumber].isVertical);
            }else
                MobileFunction.OnLocalIdentifySuccess(m_data.videoList[selectedNumber].userId,m_data.videoList[selectedNumber].isVertical);
        }
        catch (System.Exception ex)
        {
            //GameObject.FindObjectOfType<UIManager>().DebugToUI(ex.ToString());
        }
    }


    void OnTargetLost(TargetAbstractBehaviour behaviour)
    {
        SetToLoseCardMode();
    }

    void TurnToTallMode()
    {
        btns[0].DOLocalMove(new Vector3(-70f,200f,0),1f);//返回
        btns[1].DOLocalMove(new Vector3(470f,200f,0),1f);//下一个
        btns[2].DOLocalMove(new Vector3(171f, 510f, 0), 1f);//添加好友
        btns[3].DOLocalMove(new Vector3(255f, 510f, 0), 1f);//3添加视频
        btns[4].DOLocalMove(new Vector3(339f, 510f, 0), 1f);//4切换模式
        btns[5].DOLocalMove(new Vector3(255f, -132f, 0), 1f);//5日期
        //头像 昵称
        ui_userlogo.transform.DOLocalMove(new Vector3(12f, -139f, 0), 1f);
        //ui_nickName.transform.DOLocalMove(new Vector3(157f,440f,0),1f);

        BGWide.gameObject.SetActive(false);
        BGTall.gameObject.SetActive(true);
        bg = BGTall;
    }
    void TurnToWideMode()
    {
        btns[0].DOLocalMove(new Vector3(-150f, 200f, 0), 1f);
        btns[1].DOLocalMove(new Vector3(550f, 200f, 0), 1f);
        btns[2].DOLocalMove(new Vector3(161f, 418f, 0), 1f);//添加好友
        btns[3].DOLocalMove(new Vector3(252f, 418f, 0), 1f);//3添加视频
        btns[4].DOLocalMove(new Vector3(339f, 418f, 0), 1f);//4切换模式
        btns[5].DOLocalMove(new Vector3(255f, -35f, 0), 1f);//5日期
        //头像 昵称
        ui_userlogo.transform.DOLocalMove(new Vector3(12f, -34f, 0), 1f);
        //ui_nickName.transform.DOLocalMove(new Vector3(69.9f, 353.2f, 0), 1f);

        BGWide.gameObject.SetActive(true);
        BGTall.gameObject.SetActive(false);
        bg = BGWide;
    }
}
