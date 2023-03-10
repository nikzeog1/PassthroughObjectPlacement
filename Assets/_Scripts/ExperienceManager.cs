using System;
using UnityEngine;

public class ExperienceManager : MonoBehaviour {

    [SerializeField] private OVRSceneManager ovrSceneManager;
    [SerializeField] private LayerMask roomLayer;
    private bool _sceneReady;
    
    [SerializeField] private Transform placementIndicator;
    [SerializeField] private float raycastDistance;

    [SerializeField] private OVRInput.RawButton actionButton;
    [SerializeField] private GameObject floorObject;
    [SerializeField] private GameObject ceilingObject;
    [SerializeField] private GameObject wallObject;

    private void Awake() {

        _sceneReady = false;
        ovrSceneManager.SceneModelLoadedSuccessfully += SceneInitialised;
        
    }
    

    private void SceneInitialised() {

        _sceneReady = true;

    }

    private void Update() {

        if (!_sceneReady) 
            return;
        
        Vector3 rayOrigin = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Vector3 rayDirection = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, raycastDistance, roomLayer)) {
            
            SetIndicatorPosition(hitInfo);
            SetIndicatorRotation(hitInfo);
            
            DrawDebugTools.DrawRaycastHit(rayOrigin, rayDirection, raycastDistance, hitInfo, 0f);

            if (OVRInput.GetDown(actionButton)) {
                float surfaceDot = Vector3.Dot(hitInfo.normal, Vector3.up);
                if (surfaceDot > 0.99f) {
                    Debug.Log("Floor");

                    GameObject newContent = Instantiate(floorObject);
                    newContent.transform.position = hitInfo.point;
                    newContent.transform.up = hitInfo.normal;

                }
                else if (surfaceDot < -0.99) {
                    Debug.Log("Ceiling");

                    GameObject newContent = Instantiate(ceilingObject);
                    newContent.transform.position = hitInfo.point;
                    newContent.transform.rotation = Quaternion.FromToRotation(newContent.transform.up, -hitInfo.normal);

                }
                else {
                    Debug.Log("Wall");
                    
                    GameObject newContent = Instantiate(wallObject);
                    newContent.transform.position = hitInfo.point;
                    newContent.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                }
                
            }
            

        }
    }

    private void SetIndicatorRotation(RaycastHit hitInfo) {
        placementIndicator.rotation = Quaternion.LookRotation(-hitInfo.normal);
        //placementIndicator.rotation = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
        //placementIndicator.transform.forward = -hitInfo.normal;
        //var objectsBlueLine = placementIndicator.transform.forward;
        //var inverseNormal = -hitInfo.normal;
        //objectsBlueLine = inverseNormal;
    }

    private void SetIndicatorPosition(RaycastHit hitInfo) {
        
        placementIndicator.position = hitInfo.point + hitInfo.normal * .001f; //no z-fighting
        
    }
    
    
}
