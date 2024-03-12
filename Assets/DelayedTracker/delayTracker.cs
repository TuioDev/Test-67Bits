using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class delayTracker : MonoBehaviour
{
    public Transform tracker;//
    public float delayTime = 0.5f;//
    private List<trackerPos> trackerPoses;//
    private int bufferSize = 100;//
    private float fps;//
    private int counter = 0;//
 
    void Awake()
    {
        resetPosesBuffer(delayTime);
    }

    void Update()
    {
        if(bufferSize > trackerPoses.Count) //grow the array until it reached the buffer size & do nothing until it is full
        {
            trackerPoses.Add(new trackerPos(tracker.position, tracker.rotation));
            return;
        }

        trackerPoses[counter % bufferSize] = new trackerPos(tracker.position, tracker.rotation);
        int followIndex = (counter % bufferSize) + 1; //point to the index furthest from just set

        if (followIndex == bufferSize) //handle out of bounds, set back to 0
            followIndex = 0;

        Vector3 delayedPosition = trackerPoses[followIndex].position;
        delayedPosition.y = transform.position.y;

        //transform.position = delayedPosition;

        transform.rotation = trackerPoses[followIndex].rotation;
                 
        counter++;
    }

    public void resetPositionBufferSize(InputField trackerDelayInputField)
    {
        delayTime = float.Parse(trackerDelayInputField.text);
        resetPosesBuffer(delayTime);
    }

    void resetPosesBuffer(float _delayTime)
    {
        counter = 0;
        fps = 1.0f / Time.deltaTime;
        bufferSize = Mathf.CeilToInt(fps * _delayTime);
        trackerPoses = new List<trackerPos>();
        trackerPoses.Add(new trackerPos(tracker.position, tracker.rotation)); //fill first index
    }
}

class trackerPos
{
    public Vector3 position;
    public Quaternion rotation;

    public trackerPos(Vector3 _pos, Quaternion _rot)
    {
        position = _pos;
        rotation = _rot;
    }
}
