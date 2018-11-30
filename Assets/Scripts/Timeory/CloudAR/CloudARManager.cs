using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;

public class CloudARManager : MonoBehaviour {
    public enum States
    {
        CloudMode,
        LocalMove
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //手动加载videoTarget
    public void LoadLocalTarget()
    {
        //关闭云识别

        CloudARVideoTargetBehaviour targetBehaviour;
        ImageTrackerBehaviour tracker = FindObjectOfType<ImageTrackerBehaviour>();

        // dynamically load from image (*.jpg, *.png)
        CreateTarget("argame01", out targetBehaviour);
        targetBehaviour.Bind(tracker);
        targetBehaviour.SetupWithImage("sightplus/argame01.jpg", StorageType.Assets, "argame01", new Vector2());
        GameObject duck02_1 = Instantiate(Resources.Load("duck02")) as GameObject;
        duck02_1.transform.parent = targetBehaviour.gameObject.transform;
    }

    void CreateTarget(string targetName, out CloudARVideoTargetBehaviour targetBehaviour)
        {
            GameObject Target = new GameObject(targetName);
            Target.transform.localPosition = Vector3.zero;
            targetBehaviour = Target.AddComponent<CloudARVideoTargetBehaviour>();
        }
}
