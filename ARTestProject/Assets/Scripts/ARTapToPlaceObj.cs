using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObj : MonoBehaviour
{
    public GameObject gameObj;
    public GameObject placementIndicator;

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

        if(validPlacement && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        Instantiate(gameObj, placeObj.position, placeObj.rotation);
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

            var camForward = Camera.current.transform.forward;
            var camBearing = new Vector3(camForward.x, 0, camForward.z).normalized;

            placeObj.rotation = Quaternion.LookRotation(camBearing);
        }
    }
}
