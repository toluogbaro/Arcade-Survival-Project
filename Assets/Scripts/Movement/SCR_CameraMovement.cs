using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CameraMovement : MonoBehaviour
{
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;

    public GameObject characterBody;

    [SerializeField]
    private Vector2 clampInDegrees = new Vector2(360, 180);
    [SerializeField]
    public static Vector2 sensitivity = new Vector2(2, 2);
    [SerializeField]
    private Vector2 smoothing = new Vector2(3, 3);
    [SerializeField]
    private Vector2 targetDirection;
    [SerializeField]
    private Vector2 targetCharacterDirection;


    public static bool yInvert = false;
    public static bool xInvert = false;

    public static Camera playerCam;

    public static bool cantMoveCamera;


    void Update()
    {
        if(!cantMoveCamera)//if cant move camera is true then the camera doesnt move in update
        {
            var targetOrientation = Quaternion.Euler(targetDirection);
            var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

            var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

            // Interpolate mouse movement over time to apply smoothing delta.
            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

            //_mouseAbsolute += _smoothMouse;

            if (yInvert)
            {
                _mouseAbsolute.y -= _smoothMouse.y;
            }
            else
            {
                _mouseAbsolute.y += _smoothMouse.y;
            }

            if (xInvert)
            {
                _mouseAbsolute.x -= _smoothMouse.x;
            }
            else
            {
                _mouseAbsolute.x += _smoothMouse.x;
            }

            //if the camera is inverted then it moves in the opposite way


            if (clampInDegrees.x < 360)
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

            if (clampInDegrees.y < 360)
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

            transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

            // If there's a character body that acts as a parent to the camera
            if (characterBody)
            {
                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
                characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
                transform.LookAt(characterBody.transform);
            }
            else
            {
                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
                transform.localRotation *= yRotation;
            }

            //moves the camera in a circular rotation and adjusts the player characters local rotation based on the camera
            //position and rotation
        }
        
    }
}
