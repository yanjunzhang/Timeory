// ========================================================
// UcardStore
// 作 者：zhangyanjun 
// 创建时间：2017/11/27 15:41:35
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System;

public class MobileFunction : MonoBehaviour
{
	/// <summary>
	/// 本地识别成功
	/// </summary>
	/// <param name="uid">originalId.</param>
	public static void OnLocalIdentifySuccess(string originalId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("onLocalIdentifySuccess", originalId);
		}
		else
			Debug.Log("本地识别成功" + originalId);
	}

	/// <summary>
	/// 云识别成功
	/// </summary>
	/// <param name="uid">originalId.</param>
	public static void OnCloudIdentifySuccess(string targetId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("onCloudIdentifySuccess", targetId);
		}
		else
			Debug.Log("云识别成功" + targetId);
	}

	/// <summary>
	/// 添加本地识别视频
	/// </summary>
	/// <param name="uid">originalId.</param>
	public static void AddVideoIntoLocalSpace(string targetId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("addVideoIntoLocalSpace()", targetId);
		}
		else
			Debug.Log("添加本地识别视频" + targetId);
	}

	/// <summary>
	/// 添加云识别视频
	/// </summary>
	/// <param name="uid">originalId.</param>
	public static void AddVideoIntoCloudSpace(string targetId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("addVideoIntoCloudSpace()", targetId);
		}
		else
			Debug.Log("添加云识别视频" + targetId);
	}

	/// <summary>
	/// 本地识别输入密码弹窗
	/// </summary>
	/// <param name="uid">originalId.</param>
	public static void PopUpPasswordDialog(string targetId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("popUpPasswordDialog()", targetId);
		}
		else
			Debug.Log("本地识别输入密码弹窗" + targetId);
	}

	/// <summary>
	/// 重新识别
	/// </summary>
	/// <param name="uid">originalId.</param>
	public static void RefreshIdentification(string targetId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("refreshIdentification()", targetId);
		}
		else
			Debug.Log("重新识别" + targetId);
	}

	/// <summary>
	/// 好友添加请求
	/// </summary>
	/// <param name="uid">originalId.</param>
	public static void SendFriendRequest(string userId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("refreshIdentification()", userId);
		}
		else
			Debug.Log("好友添加请求" + userId);
	}


	/// <summary>
	/// 退出 unity.
	/// </summary>
	public static void QuitUnity()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("onBackPressed");
			Debug.Log("Quit Unity");
		}
		else
			Debug.Log("Quit Unity");
	}




	/// <summary>
	/// 安卓端debug输出信息
	/// </summary>
	/// <param name="log">Log.</param>
	public static void DebugByAndroid(string log)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("printUnityLogInfo", log);
		}
		else
		{
			Debug.Log("Android Error Lod:" + log);
		}
	}

	/// <summary>
	/// 后台无返回数据
	/// </summary>
	public static void NoDataReceived()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call("onNoDataReceived");
		}else
			Debug.Log("NoDataReceived");
	}
		




}
