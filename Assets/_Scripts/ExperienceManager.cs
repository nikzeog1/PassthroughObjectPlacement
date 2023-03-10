using System;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour {

    [SerializeField] private OVRSceneManager ovrSceneManager;
    [SerializeField] private LayerMask roomLayer;
    public OVRSceneAnchor[] sceneAnchors;
    private List<OVRSemanticClassification> initialClassifications = new List<OVRSemanticClassification>();
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

        sceneAnchors = FindObjectsOfType<OVRSceneAnchor>();
        if (sceneAnchors != null) {
            foreach (var anchor in sceneAnchors) {
                var semClass = anchor.GetComponent<OVRSemanticClassification>();
                initialClassifications.Add(semClass);
            }
        }
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

            if (OVRInput.GetDown(actionButton)) {
                if (hitInfo.transform.parent.TryGetComponent<OVRSemanticClassification>(out OVRSemanticClassification hitClassification)) {
                    if (initialClassifications.Contains(hitClassification)) {
                        var label = hitClassification.Labels[0];
                        switch (label) {
                            case OVRSceneManager.Classification.WallFace:
                                Debug.Log(OVRSceneManager.Classification.WallFace);
                                DrawDebugTools.DrawString3D(hitInfo.point, Quaternion.LookRotation(hitInfo.normal), label, TextAnchor.MiddleCenter, Color.green, 0f, 5f);
                                GameObject wallContent = Instantiate(wallObject);
                                wallContent.transform.position = hitInfo.point;
                                wallContent.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                                break;
                            case OVRSceneManager.Classification.Floor:
                                Debug.Log(OVRSceneManager.Classification.Floor);
                                DrawDebugTools.DrawString3D(hitInfo.point, Quaternion.LookRotation(hitInfo.normal), label, TextAnchor.MiddleCenter, Color.green, 0f, 5f);
                                GameObject floorContent = Instantiate(floorObject);
                                floorContent.transform.position = hitInfo.point;
                                floorContent.transform.up = hitInfo.normal;
                                break;
                            case OVRSceneManager.Classification.Ceiling:
                                Debug.Log(OVRSceneManager.Classification.Ceiling);
                                DrawDebugTools.DrawString3D(hitInfo.point, Quaternion.LookRotation(hitInfo.normal), label, TextAnchor.MiddleCenter, Color.green, 0f, 5f);
                                GameObject ceilingContent = Instantiate(ceilingObject);
                                ceilingContent.transform.position = hitInfo.point;
                                ceilingContent.transform.rotation = Quaternion.FromToRotation(ceilingContent.transform.up, -hitInfo.normal);
                                break;
                        }
                    }
                }
            }
            
            /*if (OVRInput.GetDown(actionButton)) {
                
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
            }*/

            

            DrawDebugTools.DrawRaycastHit(rayOrigin, rayDirection, raycastDistance, hitInfo, 0f);

            

            
            

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
