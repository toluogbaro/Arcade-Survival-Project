using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DoorPivot : MonoBehaviour, IPuzzleInterface
{
    [SerializeField] private float speed = 0.01f;
    float timeCount = 0f;
    bool trigger = false;
    [SerializeField] Vector3 targetAngle;
    Vector3 currentAngle;
    public void PuzzleComplete()
    {
        trigger = true;
    }

    void Update()
    {
        if(trigger)
            currentAngle = new Vector3(
                Mathf.LerpAngle(currentAngle.x, currentAngle.x, speed),
                Mathf.LerpAngle(currentAngle.y, targetAngle.y, speed),
                Mathf.LerpAngle(currentAngle.x, currentAngle.z, speed));
            transform.eulerAngles = currentAngle;
            timeCount = timeCount + Time.deltaTime;
    }   
}
