using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectToFollow
{
    public Transform ObjectTransform;
    public float DelayToFollow;
    public List<ObjectFrameInfo> AllFramesInfo = new();
    public int BufferSize;

    private float _fps;

    public ObjectToFollow(Transform objectTransform, float delayToFollow)
    {
        ObjectTransform = objectTransform;
        DelayToFollow = delayToFollow;
        _fps = 1.0f / Time.deltaTime;
        BufferSize = Mathf.CeilToInt(_fps * DelayToFollow);
        AllFramesInfo.Add(new ObjectFrameInfo(ObjectTransform.position, ObjectTransform.rotation));
    }
}
