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
    [SerializeField] private Rigidbody _rigidbodyToHit;

    private bool _isKnocked;

    private void Awake()
    {
        _ragdollObject.SetActive(false);
    }

    public void KnockDown(Vector2 direction, float punchStrenght)
    {
        _isKnocked = !_isKnocked;

        if (_isKnocked)
        {
            CopyTransformData(_animatedObject.transform, _ragdollObject.transform, _agentObject.velocity);
            _ragdollObject.SetActive(true);
            _animatedObject.SetActive(false);
            _agentObject.enabled = false;

            // Apply force in the player facing direction
            Vector3 forceDirection = new(direction.x, 0, direction.y);
            forceDirection *= punchStrenght;

            //_rigidbodyToHit.AddRelativeForce(forceDirection, ForceMode.Impulse);
            //_rigidbodyToHit.AddForceAtPosition(forceDirection, transform.position, ForceMode.Impulse);
            _rigidbodyToHit.velocity = forceDirection;
        }
    }

    [ContextMenu("Reset")]
    public void ResetNPC()
    {
        _isKnocked = false;
        CopyTransformData(_ragdollObject.transform, _animatedObject.transform, _agentObject.velocity);
        _ragdollObject.SetActive(false);
        _animatedObject.SetActive(true);
        _agentObject.enabled = true;
    }

    // Set the position of the ragdoll model the same of the animated model
    private void CopyTransformData(Transform sourceTransform, Transform destinationTransform, Vector3 direction)
    {
        // Check if both have the same amount of objects
        if (sourceTransform.childCount != destinationTransform.childCount) return;

        for (int i = 0; i < sourceTransform.childCount; i++)
        {
            Transform source = sourceTransform.GetChild(i);
            Transform destination = destinationTransform.GetChild(i);
            destination.position = source.position;
            destination.rotation = source.rotation;

            Rigidbody rigidbody = destination.GetComponent<Rigidbody>();
            if (rigidbody != null) rigidbody.velocity = direction;

            // Iterate to inner child objets
            CopyTransformData(source, destination, direction);
        }
    }
}
