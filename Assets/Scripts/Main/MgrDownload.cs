using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MgrDownload : MonoBehaviour {
    public bool useMinimumPic=false;

    List<string> downloadList;
    string path;
	void Awake()
	{
        //path = Application.persistentDataPath;
        path = "sdcard/timeory/unity/video";
    }
	// Use this for initialization
	void Start () {
        downloadList = new List<string>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DownloadLocalARImg("http://oss.timeory.com/Business/App/Scan/Img/ar1.jpg");
        }
    }

    public void DownloadLocalARImg(string url)
    {
        StartCoroutine((IE_DownloadLocalARImg(url)));
    }
    IEnumerator IE_DownloadLocalARImg(string url)
    {
        string[] splits = url.Split('/');
        string fileName = splits[splits.Length - 1];
            //url.Remove(0, url.Length - 40);
        path = Application.streamingAssetsPath;
        if (File.Exists(path + "/" + fileName))
        {
            yield break;
        }
        else
        {
            if (downloadList.Contains(fileName))
            {
                yield break;
            }
            downloadList.Add(fileName);
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
                Debug.Log(path + "/" + fileName);
            }
            catch (IOException e)
            {
                print(e);
            }

            downloadList.Remove(fileName);
        }
    }

    public void LoadImageWithUrl(Image image, string url)
    {
        StartCoroutine(IE_LoadImageWithUrl(image, url));
    }
    IEnumerator IE_LoadImageWithUrl(Image image, string url)
    {
        string fileName = url.Remove(0, url.Length - 40);
        if (useMinimumPic)
            fileName += "!unity";
        
        if (File.Exists(path + "/" + fileName))
        {
            WWW www = new WWW(path + "/" + fileName);
            Debug.Log("file://" + path + "/" + fileName);
            yield return www;

            if (www.error != null)
            {
                Debug.Log("error" + www.error);
            }

            if (www.isDone && www.error == null)
            {
                Texture2D tex2d = www.texture;
                Sprite s = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f));
                image.sprite = s;
            }
            yield break;
        }
        else {
            if (downloadList.Contains(fileName))
            {
                yield break;
            }
            downloadList.Add(fileName);
            WWW www2 = new WWW(url);
            //定义www为WWW类型并且等于所下载下来的WWW中内容。  
            yield return www2;
            //返回所下载的www的值   
            if (www2.error != null)
            {
                Debug.Log("error:" + www2.error);
            }
            Texture2D newTexture = www2.texture;
            image.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
            byte[] pngData = newTexture.EncodeToPNG();
            try
            {
                File.WriteAllBytes(path + "/" + fileName, pngData);
                Debug.Log(path + "/" + fileName);
            }
            catch (IOException e)
            {
                print(e);
            }

            downloadList.Remove(fileName);
        }
    }
    

	//判断是否已经下载到本地,并返回路径 
	public string GetVideoPath(string videoUrl)
	{
		string fileName = videoUrl.Remove (0, videoUrl.Length - 40);
		string videoPath = path+"/"+fileName;
		if (File.Exists(videoPath)) {
			if(Application.platform==RuntimePlatform.Android)
			{
                GameObject.FindObjectOfType<UIManager>().DebugToUI("file Exists"+videoPath);
                Debug.Log(videoPath);
                return videoPath;
			}else if(Application.platform==RuntimePlatform.IPhonePlayer)
			{
				return "file://"+videoPath;
            }else{
                return "file://" + videoPath;
            }
		}else
		{
            StartCoroutine(SaveMp4(videoPath));
			return videoUrl;
		}
	}

	IEnumerator SaveMp4(string url)
	{
		string fileName = url.Remove (0, url.Length - 40);
        if (downloadList.Contains(fileName))
        {
            yield break;
        }
        
		if (File.Exists(path + "/"+fileName)) {
            GameObject.FindObjectOfType<UIManager>().DebugToUI("file Exists");
			yield break;
		}
        downloadList.Add(fileName);
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
            GameObject.FindObjectOfType<UIManager>().DebugToUI("file downloaded"+path + "/" + fileName);
			Debug.Log(path + "/"+fileName);
            downloadList.Remove(fileName);
        }  
		catch(IOException e)  
		{  

			print(e);  
		}  
	}
}
