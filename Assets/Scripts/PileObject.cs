using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileObject : MonoBehaviour
{
    private ObjectToFollow _object;
    private int _frameCounter;
    private Vector3 _velocity = Vector3.zero;

    public void SetObjectToFollow(ObjectToFollow objectToFollow) => _object = objectToFollow;

    private void Update()
    {
        // Add objects to the list until the count is bigger than the buffer size
        if (_object.BufferSize > _object.AllFramesInfo.Count)
        {
            _object.AllFramesInfo.Add(new(_object.ObjectTransform.position, _object.ObjectTransform.rotation));
            return;
        }

        // Set frame info to the list at the counter position, modulus is here to loop the count
        _object.AllFramesInfo[_frameCounter % _object.BufferSize] = new (_object.ObjectTransform.position, _object.ObjectTransform.rotation);
        int followIndex = (_frameCounter % _object.BufferSize) + 1;

        // If at the end of the buffer size
        if (followIndex == _object.BufferSize) followIndex = 0;

        // Set the position and rotation of this object
        Vector3 delayedPosition = _object.AllFramesInfo[followIndex].Position;
        delayedPosition.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, delayedPosition, Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, delayedPosition, ref _velocity, 0.05f);

        //transform.rotation = Quaternion.Lerp(transform.rotation, _object.AllFramesInfo[followIndex].Rotation, Time.deltaTime);
        transform.rotation = _object.AllFramesInfo[followIndex].Rotation;

        _frameCounter++;
    }
}
