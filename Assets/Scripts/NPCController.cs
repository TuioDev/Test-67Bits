using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour, IKnockable
{
    [Header("References")]
    [SerializeField] private GameObject _ragdollObject;
    [SerializeField] private GameObject _animatedObject;
    [SerializeField] private NavMeshAgent _agentObject;

    private bool _isKnocked;

    private void Awake()
    {
        _ragdollObject.SetActive(false);
    }

    [ContextMenu("Knoch down")]
    public void KnockDown()
    {
        _isKnocked = !_isKnocked;

        if(_isKnocked)
        {
            // TODO: Change the direction from the players forward direction
            CopyTransformData(_animatedObject.transform, _ragdollObject.transform, _agentObject.velocity);
            _ragdollObject.SetActive(true);
            _animatedObject.SetActive(false);
            _agentObject.enabled = false;
        }
    }

    [ContextMenu("Reset")]
    public void ResetNPC()
    {
        _isKnocked = false;
        _ragdollObject.SetActive(false);
        _animatedObject.SetActive(true);
        _agentObject.enabled = true;
    }

    private void CopyTransformData(Transform sourceTransform, Transform destinationTransform, Vector3 direction)
    {
        // Check if both have the same amount of objects
        if (sourceTransform.childCount != destinationTransform.childCount)
        {
            return;
        }

        for (int i = 0; i < sourceTransform.childCount; i++)
        {
            Transform source = sourceTransform.GetChild(i);
            Transform destination = destinationTransform.GetChild(i);
            destination.position = source.position;
            destination.rotation = source.rotation;

            Rigidbody rigidbody = destination.GetComponent<Rigidbody>();
            if (rigidbody != null ) rigidbody.velocity = direction;

            // Iterate to inner child objets
            CopyTransformData(source, destination, direction);
        }
    }
}
