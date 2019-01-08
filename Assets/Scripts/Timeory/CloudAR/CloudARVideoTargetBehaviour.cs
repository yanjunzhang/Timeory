using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using DG.Tweening;


public class CloudARVideoTargetBehaviour : ImageTargetBaseBehaviour {
    protected override void Awake()
    {
        base.Awake();
        TargetFound += OnTargetFound;
    }



    void OnTargetFound(TargetAbstractBehaviour behaviour)
    {
        /*
try
        {
            MobileFunction.OnCloudIdentifySuccess(gameObject.name);
        }
        catch (System.Exception ex)
        {
           //GameObject.FindObjectOfType<UIManager>().DebugToUI(ex.ToString());
        }

        */

    }

}
