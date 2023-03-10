
using System;
using UnityEngine;
using DG.Tweening;

public class ObjectTweenScale : MonoBehaviour {
    
    private Vector3 startingScale;
    private Vector3 endingScale;
    [SerializeField] private float popDuration = 0.35f;
    
    
    private void Awake() {

        endingScale = transform.localScale;
        startingScale = endingScale * 0.25f;

        transform.localScale = startingScale;

    }

    private void Start() {

        transform.DOScale(endingScale, popDuration);

    }
}