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

    public List<Transform> targets;
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

        //App.MgrPost.LoadLocalTarget("pwd", InitLocalVideoCardTarget, ErrorPassword);
    }




}
