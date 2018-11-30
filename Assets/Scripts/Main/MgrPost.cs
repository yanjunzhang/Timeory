// * ============================================================= *
// * 描 述：
// * 作 者：
// * 版 本：v 1.0
// * 创建时间：2018/11/10 15:07:16
// * ============================================================= *
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using EasyAR;
using System;

public class MgrPost : MonoBehaviour {
	public void Load(ImageTarget target,System.Action<ImageTarget,VideoTargetDate> handle )
	{
		StartCoroutine (LoadImageTarget (target,handle));
	}
    #region 后台交互   
	//获取该target下的所有用户及视频信息
	//POST /videos/cloud/targetId/{targetId}/memberCode/{memberCode} 2018-01-22-用户查看云识别时光轴
	IEnumerator LoadImageTarget(ImageTarget target,System.Action<ImageTarget,VideoTargetDate> handle)
	{
		/*自动获取手机的信息
		System.Collections.Generic.Dictionary<string, string> headers = new System.Collections.Generic.Dictionary<string, string>();

		headers.Add("Content-Type", "application/json");


		JsonData data = new JsonData();
		data["phone"] = "13816848999";
		data["targetId"] = "920df90c-dbb4-41bf-ab70-b2bc14c189c1";
		data["token"] = "";
		byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data.ToJson());
		*/
		//WWW www = new WWW("http://106.14.60.213:8080/business/AR/cloud", bs, headers);

		WWWForm wwwForm = new WWWForm ();
		wwwForm.AddField ("phone", "13816848999");
		wwwForm.AddField ("targetId", target.Uid);
		wwwForm.AddField ("token", "");
		WWW www = new WWW("http://106.14.60.213:8080/business/AR/cloud",wwwForm);
		yield return www;
		string m_info = string.Empty;

		if (www.error != null)
		{
			m_info = www.error;
			yield return null;
		}
		if (www.isDone && www.error == null)
		{

			//m_info = www.text.ToString();
			m_info = "{\n    \"code\": 200,\n    \"data\": {\n        \"createDate\": \"2018-11-09 00:00:00\",\n        \"id\": 10063001,\n        \"status\": 1,\n        \"timeLineVideoList\": [\n            {\n                \"createDate\": \"2018-11-09 00:00:00\",\n                \"id\": 10064002,\n                \"timeLineId\": 10063001,\n                \"timeVideoSrc\": \"http://oss.timeory.com/video//ead78d39-dbf7-11e8-8192-00163e00a099.mp4\"\n            },\n            {\n                \"createDate\": \"2018-11-09 00:00:00\",\n                \"id\": 10064002,\n                \"timeLineId\": 10063001,\n                \"timeVideoSrc\": \"http://oss.timeory.com/video//ead78d39-dbf7-11e8-8192-00163e00a099.mp4\"\n            },\n            {\n                \"createDate\": \"2018-11-09 00:00:00\",\n                \"id\": 10064002,\n                \"timeLineId\": 10063001,\n                \"timeVideoSrc\": \"http://oss.timeory.com/video//ead78d39-dbf7-11e8-8192-00163e00a099.mp4\"\n            }\n        ],\n        \"timesImgSrc\": \"http://oss.timeory.com/Business/App/Scan/Img/ar1.jpg\",\n        \"userId\": 710600163\n    },\n    \"msg\": \"识别成功\",\n    \"time\": \"2018-11-25T20:17:39.640\"\n}";
			yield return m_info;
			Debug.Log(m_info);
			JsonData jd = JsonMapper.ToObject(m_info);
			jd = jd["data"]["timeLineVideoList"];
			//Debug.Log (jd.ToString ());
			byte[] bytes = Convert.FromBase64String(target.MetaData);
			string s = System.Text.Encoding.GetEncoding("utf-8").GetString(bytes);
			VideoTargetDate data = new VideoTargetDate (target.Uid,s);


			for (int i = 0; i < jd.Count; i++) {
				VideoTargetCell cell = new VideoTargetCell (
					                       jd [i] ["createDate"].ToString (),
					                       jd [i] ["user"]["nickName"].ToString (),
					                       jd [i] ["timeVideoSrc"].ToString (),
                                           jd [i] ["user"]["userLogo"].ToString());
				data.videoList.Add (cell);
			}

			Debug.Log (data.videoList.Count);

			handle (target, data);
		}

	}

    //添加好友
	#endregion


}
