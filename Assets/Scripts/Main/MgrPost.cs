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

    public void LoadLocalTarget(string pwd,System.Action<VideoTargetDate> handle,System.Action errorHandle)
    {
        StartCoroutine(LoadLocalImageTarget(pwd,handle,errorHandle));
    }
    #region 后台交互   

    IEnumerator LoadLocalImageTarget(string pwd,System.Action<VideoTargetDate> handle,System.Action errorHandle)
    {
        Debug.Log(pwd);
        Dictionary<string, string> headers = new Dictionary<string, string>();

        headers.Add("Content-Type", "application/json");


        JsonData data = new JsonData();
        data["password"] = pwd;
        byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data.ToJson());

        WWW www = new WWW("http://106.14.60.213:8080/business/AR/local", bs, headers);
        yield return www;
        Debug.Log(www.text);
        string m_info = string.Empty;

        if (www.error != null)
        {
            m_info = www.error;
            Debug.Log(m_info);
            yield return null;
        }
        if (www.isDone && www.error == null)
        {
            Debug.Log(www.text);
            m_info = www.text.ToString();
            yield return m_info;
            Debug.Log(m_info);

            JsonData jd = JsonMapper.ToObject(m_info);
            if (!jd["msg"].ToString().Equals("识别成功"))
            {
                Debug.Log("密码错误");
                errorHandle();
                yield break;
            }

            //Debug.Log (jd.ToString ());
            string s = jd["data"]["timesImgSrc"].ToString();
            s = App.MgrDownload.DownloadLocalARImg(s);
            VideoTargetDate _data = new VideoTargetDate(pwd,"", s);

            jd = jd["data"]["timeLineVideoList"];
            for (int i = 0; i < jd.Count; i++)
            {
                VideoTargetCell cell = new VideoTargetCell(
                                           jd[i]["createDate"].ToString(),
                                           jd[i]["user"]["nickName"].ToString(),
                                           jd[i]["timeVideoSrc"].ToString(),
                                           jd[i]["user"]["userLogo"].ToString(),
                                           jd[i]["id"].ToString(),
                                           jd[i]["user"]["isFriend"].ValueAsBoolean());
                _data.videoList.Add(cell);
            }

            Debug.Log(_data.videoList.Count);

            handle(_data);
        }

    }

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

			m_info = www.text.ToString();
            yield return m_info;
			Debug.Log(m_info);
			JsonData jd = JsonMapper.ToObject(m_info);
			jd = jd["data"]["timeLineVideoList"];
            if (jd == null)
            {
                Destroy(GameObject.Find(target.Uid));
                yield break;
            } 
			//Debug.Log (jd.ToString ());
			byte[] bytes = Convert.FromBase64String(target.MetaData);
			string s = System.Text.Encoding.GetEncoding("utf-8").GetString(bytes);
			VideoTargetDate data = new VideoTargetDate (target.Uid,s,"");


			for (int i = 0; i < jd.Count; i++) {
				VideoTargetCell cell = new VideoTargetCell (
					                       jd [i] ["createDate"].ToString (),
					                       jd [i] ["user"]["nickName"].ToString (),
					                       jd [i] ["timeVideoSrc"].ToString (),
                                           jd [i] ["user"]["userLogo"].ToString(),
                                           jd [i] ["id"].ToString(),
                                           jd [i] ["user"]["isFriend"].ValueAsBoolean());
				data.videoList.Add (cell);
			}

			Debug.Log (data.videoList.Count);

			handle (target, data);
		}

	}

    //添加好友
	#endregion


}
