using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public struct VideoTargetDate
{
	public VideoTargetDate(string targetId,string timeVideoScr){
		this.targetId = targetId;
		this.timeVideoScr = timeVideoScr;
		this.userName="";
		this.createDate = "";
		this.status = "";
		this.timeImgScr="";
		this.videoList = new List<VideoTargetCell> ();

	}
    public string targetId;
    public string userName;
	public string createDate;
	public string status;
	public string timeImgScr;
	public string timeVideoScr;
    public List<VideoTargetCell> videoList;

}
public struct VideoTargetCell
{
	public VideoTargetCell(string createDate,string timeLineId,string timeVideoSrc)
	{
		this.createDate = createDate;
		this.timeLineId = timeLineId;
		this.timeVideoSrc = timeVideoSrc;
	}
    public string createDate;
    public string timeLineId;
    public string timeVideoSrc;
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
	public Transform backBtn, nextBtn;
    //public Transform ARCardTarget;
    [HideInInspector]
    public EasyAR.VideoPlayerBehaviour vPlayer;
    //public GameObject playImage;
    //public RectTransform background;
    public Image head;
    public Text nickName;
    public Text date;
    bool isPlaying = true;
	bool isPlaneMode=true;
	CloudARVideoTargetBehaviour videoTargetBehaviour;
	VideoTargetDate m_data;

    public string targetName;


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
    }

    public void SetVideoPath(string path)
    {
        //path = GetComponent<VideoDownloader>().GetVideoPath(path);
        vPlayer = GetComponentInChildren<EasyAR.VideoPlayerBehaviour>();

        vPlayer.Path = path;
		vPlayer.Open();
		vPlayer.VideoReadyEvent += (sender,e) => {
			Debug.Log("ready");

			vPlayer.Play();
		};
    }
	public void ResetPlaneRotation()
	{

		vPlayer.transform.rotation=Quaternion.identity;
	}

    //初始化
    public void Init(VideoTargetDate data)
    {
		m_data = data;
		this.selectedNumber = 0;
		videoTargetBehaviour = GetComponentInParent<CloudARVideoTargetBehaviour> ();
		transform.localScale = Vector3.one;
		vPlayer.transform.rotation=Quaternion.Euler(new Vector3(180f,0,0));
		RefreshUI ();
    }
		

    //下一个视频
    public void OnNextBtnClick()
    {
		if (selectedNumber<m_data.videoList.Count-1) {
			//下一个
			selectedNumber++;
		}
		RefreshUI ();
    }
    //上一个视频
    public void OnBackBtnClick()
    {
		if (selectedNumber>0) {
			selectedNumber--;
		}
		RefreshUI ();
    }
    //更新UI
    void RefreshUI()
    {
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
		//更新用户信息
		Debug.Log("当前选择： "+selectedNumber);
		//切换视频
		SetVideoPath (m_data.videoList[selectedNumber].timeVideoSrc);
    }
	void SetArrowBtn(bool back,bool next)
	{
		backBtn.gameObject.SetActive (back);
		nextBtn.gameObject.SetActive (next);
	}
    //播放视频
    void PlayVideo()
    {
		
    }
    //暂停视频
    void PauseVideo()
    {

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
    void OnAddFriendBtnClick()
    {
		
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
		if (isPlaneMode) {
			TurnARMode ();
		} else
			TurnPlaneMode();
		isPlaneMode = !isPlaneMode;

	}

    //切换到平面模式
    void TurnPlaneMode()
    {
        transform.GetChild(0).GetComponent<RectTransform>().DOLocalRotate(new Vector3(90f, 0, 0), 1f);
        transform.GetChild(1).transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
        transform.localPosition = new Vector3(0, 0, 0);
    }
    //切换到立体模式
    void TurnARMode()
    {
		transform.localPosition = new Vector3(0, 0, 0);
        transform.GetChild(0).GetComponent<RectTransform>().DOLocalRotate(new Vector3(19f, 0, 0), 1f);
        transform.GetChild(1).transform.DOLocalRotate(new Vector3(-71f, 0, 0), 1f);

    }

}
