// ========================================================
// UcardStore
// 作 者：zhangyanjun 
// 创建时间：2018/03/27 13:59:00
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using EasyAR;

public class VideoDownloader : MonoBehaviour {

	//下载视频
	public void SaveMp4WithUrl(string url)
	{
		StartCoroutine (SaveMp4 (url));
	}
	public void SetVideoUrl(string url)
	{
		GetComponentInChildren<VideoPlayerBehaviour>().Path=GetVideoPath(url);
	}

    public void SavePicWithUrl(string url)
    {
        StartCoroutine(SavePic(url));
    }

    IEnumerator SavePic(string url)
    {
        string path = Application.persistentDataPath;
        string fileName = url.Remove(0, url.Length - 40) + "!unity";

        if (File.Exists(path + "/" + fileName))
        {

            yield break;
        }
        WWW www2 = new WWW(url);
        //定义www为WWW类型并且等于所下载下来的WWW中内容。  
        yield return www2;
        //返回所下载的www的值   
        if (www2.error != null)
        {
            Debug.Log("error:" + www2.error);
        }
        Texture2D newTexture = www2.texture;
        byte[] pngData = newTexture.EncodeToPNG();
        try
        {
            File.WriteAllBytes(path + "/" + fileName, pngData);

        }
        catch (IOException e)
        {

            print(e);
        }

    }

    public string GetFilePath(string fileUrl)
    {
        string path = Application.persistentDataPath;
        string fileName = fileUrl.Remove(0, fileUrl.Length - 40);
        string videoPath = path + "/" + fileName;
        if (File.Exists(videoPath))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return videoPath;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return "file://" + videoPath;
            }
            else
                return "file://" + videoPath;
        }
        else
        {
            // SaveMp4WithUrl(videoUrl);
            StartCoroutine(SaveMp4(fileUrl));
            Debug.Log(fileUrl);
            return fileUrl;
        }
    }

    //判断是否已经下载到本地,并返回路径 
    public string GetVideoPath(string videoUrl)
	{
		string path = Application.persistentDataPath;
		string fileName = videoUrl.Remove (0, videoUrl.Length - 40);
		string videoPath = path+"/"+fileName;
		if (File.Exists(videoPath)) {
			if(Application.platform==RuntimePlatform.Android)
			{
				return videoPath;
			}else if(Application.platform==RuntimePlatform.IPhonePlayer)
			{
				return "file://"+videoPath;
			}else
				return "file://"+videoPath;
		}else
		{
			// SaveMp4WithUrl(videoUrl);
            StartCoroutine(SaveMp4(videoUrl));
            Debug.Log(videoUrl);
			return videoUrl;
		}
	}
	

	IEnumerator SaveMp4(string url)
	{
		string path = Application.persistentDataPath;
		string fileName = url.Remove (0, url.Length - 40);


		if (File.Exists(path + "/"+fileName)) {
			yield break;
		}
		//        string str = path.Replace(@"/", @"\");
		WWW www2 = new WWW(url);  
		//定义www为WWW类型并且等于所下载下来的WWW中内容。  
		yield return www2;  
		//Debug.Log (www2.isDone);
		//Debug.Log (www2.bytes.Length);

		//返回所下载的www的值   
		if (www2.error != null) {
			Debug.Log ("error:" + www2.error);
		}

		try  
		{  
			File.WriteAllBytes(path + "/"+fileName,www2.bytes);  
			Debug.Log(path + "/"+fileName);

		}  
		catch(IOException e)  
		{  

			print(e);  
		}  
	}
}
