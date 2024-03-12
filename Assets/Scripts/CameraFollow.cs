using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetToFollow;
    [SerializeField] private float _cameraHeight;

    private Vector3 _initialPosition;

    private void Awake()
    {
        _initialPosition = Camera.main.transform.position;
    }
    void LateUpdate()
    {
        Vector3 newPosition = _targetToFollow.position + _initialPosition;
        newPosition.y = _cameraHeight;
        this.transform.position = newPosition;
    }
}
