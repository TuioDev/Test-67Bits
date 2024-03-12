using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InertiaPile : MonoBehaviour
{
    [Header("Inertial stats")]
    [SerializeField] private float _heightBetweenObjects;
    [SerializeField] private float _delayOfFirstObject;
    [SerializeField] private float _delayBetweenObjects;

    private PlayerController _playerReference;
    private List<PileObject> _allObjects;
    private int _pileCount;

    public void SetPlayerReference(PlayerController playerReference) => _playerReference = playerReference;
    public int GetPileCount() => _pileCount;

    private void Awake()
    {
        _allObjects = new();
        _pileCount = 0;
    }

    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        Vector3 newPosition = _playerReference.transform.position;
        newPosition.y = this.transform.position.y;
        this.transform.position = newPosition;
    }

    public void AddToThePile(GameObject parent)
    {
        // Get the npc object and set as child of this
        parent.transform.SetParent(this.transform, false);

        // Get its height
        Vector3 heightPosition = this.transform.position;
        heightPosition.y += _heightBetweenObjects * _pileCount;

        // Set object height
        parent.transform.position = heightPosition;

        // Calculate the delay based on amount of objects in the list
        float newDelay = _delayOfFirstObject + (_delayBetweenObjects * _pileCount);

        // Add new object, the parent the pile component and set to this
        ObjectToFollow newObject = 
            new(_pileCount == 0 ? _playerReference.transform : _allObjects.Last().transform, newDelay);
        PileObject nextToPile = parent.AddComponent<PileObject>();
        nextToPile.SetObjectToFollow(newObject);

        _allObjects.Add(nextToPile);

        _pileCount++;

        GameManager.Instance.UpdateCarrying(_pileCount);
    }

    public void RemovePile()
    {
        foreach (PileObject obj in _allObjects)
        {
            Destroy(obj.gameObject);
        }

        _allObjects.Clear();
        _pileCount = 0;
    }
}