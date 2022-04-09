using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform followTransform;
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float panBorderThickness;
    public float rotationAmount;
    public Vector3 zoomAmount;
    public float minZoom;
    public float maxZoom;

    public Quaternion newRotation;
    public Vector3 newPosition;
    public Vector3 newZoom;
    public Vector2 panLimit;

    // Start is called before the first frame update
    void Start()
    {
        instance = this; 

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else
        {
            HandleMouseInput();
            HandleMovementInput();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            followTransform = null;
        }
    }

    public void FreezeCamera()
    {
        panLimit.x = 0;
        panLimit.y = 0;
    }

    public void UnFreezeCamera()
    {
        panLimit.x = 30;
        panLimit.y = 30;
    }

    public void UnFreezeLab()
    {
        panLimit.x = 25;
        panLimit.y = 25;
    }


    void HandleMouseInput()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

    }

    void HandleMovementInput()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness)
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness)
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if(Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if(Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, -panLimit.x, panLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, -panLimit.y, panLimit.y);

        newZoom.y = Mathf.Clamp(newZoom.y, -minZoom, maxZoom);
        newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, minZoom);


        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
