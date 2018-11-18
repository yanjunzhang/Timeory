﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using System;
using System.IO;

public class CloudARCloudBehaviour :CloudRecognizerBehaviour {

	private List<string> uids = new List<string>();
	private ImageTrackerBaseBehaviour trackerBehaviour;
	private string persistentDataPath;
	public bool isTimeory;
	public bool SaveNewTarget;
	public GameObject manager;


	private void Awake()
	{

		//新key
		Key = @"aa27352ae524cc07e94e3c37d6fc5751";
		Server = @"c4eb1fd24f3d65629083a01f6de81b10.cn1.crs.easyar.com:8080";
		Secret = @"A02uJay0YGgo71gn1NxvA0F7detu9qVadxr2zOIky4rQuyFszE29a2jMqyjNp9rfIOEEBM0JJFXE0qYYhcpsJA63T8JSTRvKo3zq8jZqZ5vBd9Sno4VM4JEvnRgJIeHG";



		FindObjectOfType<EasyARBehaviour>().Initialize();
		persistentDataPath = Application.persistentDataPath;
		CloudUpdate += OnCloudUpdate;
		WorkStart += OnWorkStart;
		if (ARBuilder.Instance.ImageTrackerBehaviours.Count > 0)
			trackerBehaviour = ARBuilder.Instance.ImageTrackerBehaviours[0];

	}


	private void OnWorkStart(DeviceUserAbstractBehaviour cloud, DeviceAbstractBehaviour camera)
	{
		Debug.Log("cloud start to work!");
	}


	private void OnCloudUpdate(CloudRecognizerBaseBehaviour cloud, Status status, List<ImageTarget> targets)
	{
		if (status != Status.Success && status != Status.Fail)
		{
			Debug.LogWarning("cloud: " + status);
		}
		if (!trackerBehaviour)
			return;
		foreach (var imageTarget in targets)
		{

			if (uids.Contains(imageTarget.Uid))
				continue;

			Debug.Log("New Cloud Target.uid: " + imageTarget.Uid + "(imageTarget.Name：" + imageTarget.Name + ")" + "(imageTarget.MetaData:" + imageTarget.MetaData + ")");
			byte[] bytes = Convert.FromBase64String(imageTarget.MetaData);
			string s = System.Text.Encoding.GetEncoding("utf-8").GetString(bytes);
			Debug.Log("metaUrl:" + s);
			//创建ImageTarget
			uids.Add(imageTarget.Uid);

		}
	}
	string MetaToString(string meta)
	{
		byte[] bytes = Convert.FromBase64String(meta);
		return System.Text.Encoding.GetEncoding("utf-8").GetString(bytes);

	}
		


	#region imageTarget事件
	void OnVideoTargetFound(TargetAbstractBehaviour behaviour)
	{

	}
	void OnVideoTargetLost(TargetAbstractBehaviour behaviour)
	{

	}

	#endregion


	//初始化带视频带宣传单页
	void InitVideoCardTarget(ImageTarget imageTarget)
	{
		var gameObj = new GameObject(imageTarget.Name);
		gameObj.transform.SetParent(manager.transform);

		var targetBehaviour = gameObj.AddComponent<CloudARVideoTargetBehaviour>();


	}
}
