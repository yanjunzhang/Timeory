using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;

public class CloudARImageTargetBehaviour : ImageTargetBaseBehaviour
{

	//是否时光轴

	protected override void Awake()
	{
		base.Awake();
		TargetFound += OnTargetFound;
		TargetLost += OnTargetLost;
	}



	void OnTargetFound(TargetAbstractBehaviour behaviour)
	{
        MobileFunction.OnCloudIdentifySuccess(gameObject.name);

		
	}


	void OnTargetLost(TargetAbstractBehaviour behaviour)
	{
		//脱卡(设置父物体)
		

	}


}
