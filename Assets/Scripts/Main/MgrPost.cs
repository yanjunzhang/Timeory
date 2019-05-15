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
        
        Dictionary<string, string> headers = new Dictionary<string, string>();

        headers.Add("Content-Type", "application/json");
        //headers.Add("token", App.MgrConfig._token);

        JsonData data = new JsonData();
        data["password"] = pwd;
        data["id"] = App.MgrConfig._id;
        //byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data.ToJson());
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(data.ToJson());
        Debug.Log(data.ToJson());
        WWW www = new WWW(App.MgrConfig._Server+"AR/local", bs, headers);
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
                MobileFunction.DebugByAndroid("密码错误");
                errorHandle();
                yield break;
            }

            //Debug.Log (jd.ToString ());
            string s = jd["data"]["timesImgSrc"].ToString();
            string id = jd["data"]["id"].ToString();
            s = App.MgrDownload.DownloadLocalARImg(s);

            VideoTargetDate _data = new VideoTargetDate(pwd,"",id, s);

            jd = jd["data"]["timeLineVideoList"];
            for (int i = 0; i < jd.Count; i++)
            {
                VideoTargetCell cell = new VideoTargetCell(
                                           jd[i]["createDate"].ToString(),
                                           jd[i]["user"]["nickName"].ToString(),
                                           jd[i]["timeVideoSrc"].ToString(),
                                           jd[i]["user"]["userLogo"].ToString(),
                                           jd[i]["user"]["id"].ToString(),
                                           jd[i]["user"]["isFriend"].ValueAsBoolean(),
                                           jd[i]["isVertical"].ToString()
                );
                _data.videoList.Add(cell);
            }

            Debug.Log(_data.videoList.Count);
            try
            {
                MobileFunction.DebugByAndroid("视频数量: " + _data.videoList.Count);
            }
            catch (Exception ex)
            {

            }

            yield return new WaitForSeconds(1f);
            if(handle!=null)
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

		//WWW www = new WWW("http://106.14.60.213:8080/business/AR/cloud", bs, headers);
        
        //headers.Add("token", App.MgrConfig._token);
*/
		WWWForm wwwForm = new WWWForm ();
        wwwForm.AddField ("phone", App.MgrConfig._phone);
		wwwForm.AddField ("targetId", target.Uid);
		wwwForm.AddField ("token", "");
        byte[] rawData = wwwForm.data;
        WWW www = new WWW(App.MgrConfig._Server + "AR/cloud",wwwForm);
/*
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        JsonData jsonData = new JsonData();
        jsonData["phone"] = App.MgrConfig._phone;
        jsonData["targetId"] = target.Uid;
        Debug.Log(jsonData.ToJson());
        //byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data.ToJson());
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(jsonData.ToJson());

        WWW www = new WWW(App.MgrConfig._Server +"AR/cloud",bs,headers);
        */
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
            string id = jd["data"]["id"].ToString();
			jd = jd["data"]["timeLineVideoList"];
            if (jd == null)
            {
                Destroy(GameObject.Find(target.Uid));
                yield break;
            } 
			//Debug.Log (jd.ToString ());
			byte[] bytes = Convert.FromBase64String(target.MetaData);
			string s = System.Text.Encoding.GetEncoding("utf-8").GetString(bytes);

            VideoTargetDate data = new VideoTargetDate (target.Uid,s,id,"");


			for (int i = 0; i < jd.Count; i++) {
				VideoTargetCell cell = new VideoTargetCell (
					                       jd [i] ["createDate"].ToString (),
					                       jd [i] ["user"]["nickName"].ToString (),
					                       jd [i] ["timeVideoSrc"].ToString (),
                                           jd [i] ["user"]["userLogo"].ToString(),
                                           jd [i] ["user"]["id"].ToString(),
                                           jd [i] ["user"]["isFriend"].ValueAsBoolean(),
                                           jd[i]["isVertical"].ToString());
				data.videoList.Add (cell);
			}

			handle (target, data);
		}

	}

    //添加好友
	#endregion


}
