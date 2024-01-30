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
        //moves camera up
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position= new Vector3(Camera.main.transform.position.x - cameraSpeedVert, Camera.main.transform.position.y,Camera.main.transform.position.z);
            //restricts camera to map
            if (Camera.main.transform.position.x < cameraBoundsVert*-1)
            {
                Camera.main.transform.position = new Vector3(cameraBoundsVert*-1, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
        }
        //moves camera down
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + cameraSpeedVert, Camera.main.transform.position.y, Camera.main.transform.position.z);
            //restricts camera to map
            if (Camera.main.transform.position.x > cameraBoundsVert)
            {
                Camera.main.transform.position = new Vector3(cameraBoundsVert, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
        }
        //moves camera left
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z-cameraSpeedHoriz);
            //restricts camera to map
            if (Camera.main.transform.position.z < cameraBoundsHoriz*-1)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, cameraBoundsHoriz*-1);
            }
        }
        //moves camera right
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z+cameraSpeedHoriz);
            //restricts camera to map
            if (Camera.main.transform.position.z > cameraBoundsHoriz)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, cameraBoundsHoriz);
            }
        }
        //zooms camera in
        if (Input.mouseScrollDelta.y > 0)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y-cameraZoom, Camera.main.transform.position.z);
            //sets limit to zoom in
            if (Camera.main.transform.position.y < cameraBoundsZoomIn)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraBoundsZoomIn, Camera.main.transform.position.z);
            }
        }
        //zooms camera out
        if (Input.mouseScrollDelta.y < 0)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + cameraZoom, Camera.main.transform.position.z);
            //sets limit to zoom out
            if (Camera.main.transform.position.y > cameraBoundsZoomOut)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraBoundsZoomOut, Camera.main.transform.position.z);
            }
        }
    }
}
