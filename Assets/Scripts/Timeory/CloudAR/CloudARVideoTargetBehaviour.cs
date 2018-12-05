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
        MobileFunction.OnCloudIdentifySuccess(gameObject.name);


    }

}
