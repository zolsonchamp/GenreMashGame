using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
    public float cameraSpeedVert = 1;
    public float cameraSpeedHoriz = 1;
    public float cameraZoom = 5;
    public float cameraBoundsVert = 235;
    public float cameraBoundsHoriz = 215;
    public float cameraBoundsZoomIn = 50;
    public float cameraBoundsZoomOut = 400;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position= new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,Camera.main.transform.position.z+cameraSpeedVert);
            if (Camera.main.transform.position.z > cameraBoundsVert)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, cameraBoundsVert);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - cameraSpeedVert);
            if (Camera.main.transform.position.z < cameraBoundsVert*-1)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, cameraBoundsVert*-1);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-cameraSpeedHoriz, Camera.main.transform.position.y, Camera.main.transform.position.z);
            if (Camera.main.transform.position.x < cameraBoundsHoriz*-1)
            {
                Camera.main.transform.position = new Vector3(cameraBoundsHoriz*-1, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+cameraSpeedHoriz, Camera.main.transform.position.y, Camera.main.transform.position.z);
            if (Camera.main.transform.position.x > cameraBoundsHoriz)
            {
                Camera.main.transform.position = new Vector3( cameraBoundsHoriz, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y-cameraZoom, Camera.main.transform.position.z);
            if (Camera.main.transform.position.y < cameraBoundsZoomIn)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraBoundsZoomIn, Camera.main.transform.position.z);
            }
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + cameraZoom, Camera.main.transform.position.z);
            if (Camera.main.transform.position.y > cameraBoundsZoomOut)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraBoundsZoomOut, Camera.main.transform.position.z);
            }
        }
    }
}
