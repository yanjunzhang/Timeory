using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using DG.Tweening;

public struct VideoTargetDate{
	public string target;
	public string userName;
	

}
public class CloudARVideoTargetBehaviour : ImageTargetBaseBehaviour {
//只展开一次
	public bool isFirst = true;
	public Transform ARCardTarget;
    public EasyAR.VideoPlayerBehaviour vPlayer;
	public GameObject playImage;
	public RectTransform background;
	bool isPlaying = true;
	//双击事件
	private float t1;
	private float t2;

	public string targetName;

    void OnEnable()
	{
		if (isFirst)
		{
			transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.0028f, 0, 0.5f);
			transform.GetChild(0).GetComponent<RectTransform>().DOScaleY(0.0028f, 0.5f);
			isFirst = false;
		}
	}
	//执行一次
	new void Awake()
	{
        base.Awake();
		ARCardTarget = transform.parent;

	}

	public void SetVideoPath(string path)
	{
		//path = GetComponent<VideoDownloader>().GetVideoPath(path);
		//player.Path = path;
		//player.Open();
	}

	// Update is called once per frame
	new void Update()
	{
        base.Update();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
		{
			//单击
			OnPlayBtnClick();
			//双击
			t2 = Time.realtimeSinceStartup;
			if (t2 - t1 < 0.4)
			{
				PlayOnPhonePlayer();
			}
			t1 = t2;
		}


	}

	

	public void SetBackgroundSize(float width, float height)
	{
		background.gameObject.SetActive(true);
		background.localScale = new Vector3(1, height / width, 1);
		Debug.Log(width.ToString() + height.ToString());
	}
	public void PlayOnPhonePlayer()
	{
		//string path =downloader.GetVideoPath(transform.GetComponentInChildren<EasyAR.VideoPlayerBehaviour>().Path);
		//PlayOnPhonePlayer(path);
	}
	void PlayOnPhonePlayer(string moviePath)
	{
        Handheld.PlayFullScreenMovie(moviePath, new Color(0, 0, 0, 0), FullScreenMovieControlMode.Full);
	}


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

	public void TurnPlaneEffect()
	{
		transform.GetChild(0).GetComponent<RectTransform>().DOLocalRotate(new Vector3(90f, 0, 0), 1f);
		transform.GetChild(1).transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
		transform.localPosition = new Vector3(0, 0, 0);
	}

	public void TurnAREffect()
	{

		transform.GetChild(0).GetComponent<RectTransform>().DOLocalRotate(new Vector3(19f, 0, 0), 1f);
		transform.GetChild(1).transform.DOLocalRotate(new Vector3(-71f, 0, 0), 1f);

	}
	//隐藏ar名片
	public void HideARCard()
	{
		transform.SetParent(ARCardTarget);
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		isFirst = true;
	}

}
