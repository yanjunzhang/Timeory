using UnityEngine;
using System.Collections;

public class QuitApp : MonoBehaviour {

    public enum BackType
    {
        Defult,
        Quit,
        Title
    }

    [SerializeField]
    private BackType _BackType = BackType.Title;
#if UNITY_ANDROID && !UNITY_EDITOR
	void Update () {

        if (Input.GetKey(KeyCode.Escape)) {
            if (_BackType == BackType.Quit) {
                Application.Quit();
            } else {
                App.MgrLevel.Load("title",0, LoadType.LoadingCacheStreamFadeAsync);
            }
        }
	}
#endif
}
