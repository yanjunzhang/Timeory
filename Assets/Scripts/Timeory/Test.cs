using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnVideoClick();
        }
    }

    public void OnVideoClick()
    {
        StopCoroutine("ShowBarBg");
        StartCoroutine("ShowBarBg");
    }

    IEnumerator ShowBarBg()
    {
        
        yield return new WaitForSeconds(3);
        Debug.Log("123123213");
    }


}
