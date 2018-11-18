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

public class MgrPost : MonoBehaviour {
	#region 后台交互   判断是否识别图
	//判断是时光轴  还是 其他识别图
	//POST /videos/cloud/targetId/{targetId}/memberCode/{memberCode} 2018-01-22-用户查看云识别时光轴
	IEnumerator LoadImageTarget(ImageTarget imageTarget)
	{
		System.Collections.Generic.Dictionary<string, string> headers = new System.Collections.Generic.Dictionary<string, string>();

		headers.Add("Content-Type", "application/json");
		JsonData data = new JsonData();
		/*自动获取手机的信息*/

		byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data.ToJson());

		WWW www = new WWW("/videos/cloud/targetId/" + imageTarget.Uid + "/memberCode/", bs, headers);
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
			string detect = imageTarget.Name.Substring(0, 12);
			bool flag = false;


			jd = jd["data"]["arAddressList"];
			if (jd != null)
			{
				string videoUrl = jd.Count > 1 ? jd[1]["videoUrl"].ToString() : jd[0]["videoUrl"].ToString();
				if (videoUrl != null)
				{
					

				}
			}


		}

	}
	#endregion


}
