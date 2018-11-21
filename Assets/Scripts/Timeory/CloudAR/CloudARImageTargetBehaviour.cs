using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;

public class ARCardImageTargetBehaviour : ImageTargetBaseBehaviour
{
	GameObject ARCard;
	//是否时光轴
	public bool isTimeline;
	public bool isLocalAR;

	protected override void Awake()
	{
		base.Awake();
		TargetFound += OnTargetFound;
		TargetLost += OnTargetLost;
		// TargetUnload += OnTargetUnload;
	}



	void OnTargetFound(TargetAbstractBehaviour behaviour)
	{


		//ARCardGyroDriver.flag = false;
		if (ARCard == null)
		{
			ARCard = transform.GetChild(0).gameObject;
		}
		ARCard.transform.SetParent(transform);
		ARCard.transform.localScale = Vector3.one;
		ARCard.transform.localRotation = Quaternion.identity;
		ARCard.transform.localPosition = Vector3.zero;
		if (ARCard.GetComponent<ARCardControllerBase>().isFirst == false)
			ARCard.GetComponent<ARCardControllerBase>().TurnAREffect();
		if (ARCard.GetComponent<ARCardFingerController>() != null)
		{
			ARCard.GetComponent<ARCardFingerController>().enabled = false;
		}
		//ARCardController 如果targetname不同则隐藏，否则不隐藏
		transform.parent.BroadcastMessage("HideARCard2", ARCard.GetComponent<ARCardControllerBase>().targetName, SendMessageOptions.DontRequireReceiver);
		if (isTimeline)
		{
			string targetUid = Target.Uid;
			if (isLocalAR)
			{
				targetUid = transform.name;
			}
			if (CloudARUIManager._instance.currentTargetId != targetUid)
			{
				//object name：targetId  Target.Uid==targetId
				CloudARUIManager._instance.InitTimeLineVideos(targetUid);
				CloudARUIManager._instance.currentTargetId = targetUid;
				//将视频播放器添加到UI模块 =>更换时光轴视频时使用
				CloudARUIManager._instance.currentVideoPlayer = transform.GetComponentInChildren<VideoPlayerBehaviour>();
			}
			else
				CloudARUIManager._instance.ShowTimeLineContent();


			MobileFunction.GetTargetNameUID(targetUid);
		}
	}


	void OnTargetLost(TargetAbstractBehaviour behaviour)
	{
		//脱卡
		if (isLocalAR)
		{
			CloudARUIManager._instance.ShowFullScreenHint();
		}
		if (ARCard.GetComponent<ARCardControllerBase>().isFirst == false)
		{
			ARCard.transform.SetParent(transform.parent);
			//ARCard.transform.position=new Vector3(0,-10,20);
			ARCard.transform.localRotation = Quaternion.identity;
			ARCard.transform.localScale = Vector3.one * 6f;
			ARCard.GetComponent<ARCardControllerBase>().TurnPlaneEffect();
			if (ARCard.GetComponent<ARCardController>() != null)
			{
				ARCard.GetComponent<ARCardController>().LoadPic();
			}
			if (ARCard.GetComponent<ARCardFingerController>() != null)
			{
				ARCard.GetComponent<ARCardFingerController>().enabled = true;
			}

			//			ARCardGyroDriver.flag = true;
			Camera.main.transform.parent.localPosition = new Vector3(0, 20, 0);
			Camera.main.transform.parent.localRotation = Quaternion.Euler(90, 0, 0);
		}

	}


}
