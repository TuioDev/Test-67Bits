using UnityEngine;

public class ObjectFrameInfo
{
    public Vector3 Position;
    public Quaternion Rotation;

    public ObjectFrameInfo(Vector3 position, Quaternion rotation)
    {
        this.Position = position;
        this.Rotation = rotation;
    }
}