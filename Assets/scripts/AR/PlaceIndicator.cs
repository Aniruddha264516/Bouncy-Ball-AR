using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class PlaceIndicator : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject indicator;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        indicator = transform.GetChild(0).gameObject;
        indicator.SetActive(false);
    }

    private void Update()
    {
        var ray = new Vector2(Screen.width / 2, Screen.height / 2);

        if(raycastManager.Raycast(ray , hits , TrackableType.Planes))
        {
            Pose hitpos = hits[0].pose;

            transform.position = hitpos.position;
            transform.rotation = hitpos.rotation;

            if(!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
            }
        }
    }
}
