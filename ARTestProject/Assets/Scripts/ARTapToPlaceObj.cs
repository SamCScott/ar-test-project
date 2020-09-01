using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObj : MonoBehaviour
{

    public GameObject placementIndicator;

    [SerializeField]
    //private ARSessionOrigin arOrigin;
    private ARRaycastManager raycastManager;
    private Pose placeObj;
    private bool validPlacement = false;
    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePosePlacement();
        UpdatePlaceIndicator();
    }

    private void UpdatePlaceIndicator()
    {
        if (validPlacement)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placeObj.position, placeObj.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePosePlacement()
    {
        var screenCentre = Camera.current.ViewportToScreenPoint(new Vector3(.5f,.5f));

        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCentre, hits, TrackableType.Planes);

        validPlacement = hits.Count > 0;
        if (validPlacement)
        {
            placeObj = hits[0].pose;
        }
    }
}
