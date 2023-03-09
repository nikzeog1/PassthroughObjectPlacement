using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour {

    [SerializeField] private OVRSceneManager ovrSceneManager;
    [SerializeField] private LayerMask roomLayer;
    private bool _sceneReady;
    
    [SerializeField] private Transform placementIndicator;
    [SerializeField] private float raycastDistance;

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
            placementIndicator.position = hitInfo.point + hitInfo.normal * .001f; //no z-fighting
            
            // all these rotation functions do the same thing...

            var objectsBlueLine = placementIndicator.transform.forward;
            var inverseNormal = -hitInfo.normal;
            
            //placementIndicator.rotation = Quaternion.FromToRotation(Vector3.forward, -hitInfo.normal);
            placementIndicator.rotation = Quaternion.LookRotation(-hitInfo.normal);
            //placementIndicator.transform.forward = -hitInfo.normal;
            //objectsBlueLine = inverseNormal;
        }
    }
    
}
