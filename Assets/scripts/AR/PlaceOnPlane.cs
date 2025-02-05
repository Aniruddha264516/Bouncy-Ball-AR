using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    [SerializeField]
    GameObject visualObject; // Placement indicator

    UnityEvent placementUpdate;

    public bool alreadyPlaced = false;

    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    public GameObject spawnedObject { get; private set; }

    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();

        if (placementUpdate == null)
            placementUpdate = new UnityEvent();

        placementUpdate.AddListener(DiableVisual);
    }

    void Update()
    {
        if (!alreadyPlaced)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && placementPoseIsValid)
            {
                PlaceObject();
                alreadyPlaced = true;
                placementUpdate.Invoke();
            }
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            placementPoseIsValid = true;
            placementPose = s_Hits[0].pose;
        }
        else
        {
            placementPoseIsValid = false;
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            visualObject.SetActive(true);
            visualObject.transform.SetLocalPositionAndRotation(placementPose.position, placementPose.rotation);
        }

        else
        {
            visualObject.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        spawnedObject = Instantiate(m_PlacedPrefab, placementPose.position , placementPose.rotation);
    }

    public void DiableVisual()
    {
        visualObject.SetActive(false);
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARRaycastManager m_RaycastManager;
}
