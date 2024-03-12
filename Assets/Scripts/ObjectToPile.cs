using UnityEngine;

public class ObjectToPile : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject;

    public GameObject ParentObject => _parentObject;
}
