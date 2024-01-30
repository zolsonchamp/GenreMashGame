using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class placeWall : MonoBehaviour
{
    public GameObject wall;
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public Vector3 correctedWorldPosition;
    public LayerMask layersToHit;
    // Start is called before the first frame update
    void Start()
    {
        //sets wall placement rotation at start
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //converts mouse position to a world position through camera
        screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if(Physics.Raycast(ray, out RaycastHit hitData, 500, layersToHit))
        {
            worldPosition= hitData.point;
        }
        //corrects height of mouse to world position and moves wall object to mouse
        correctedWorldPosition = new Vector3(worldPosition.x, 0, worldPosition.z);
        transform.position = correctedWorldPosition;
        //on left click place a wall
        if (Input.GetMouseButtonUp(0))
        {
            wall.transform.position = correctedWorldPosition;
            Instantiate(wall);
        }
        //rotate wall clockwise
        if (Input.GetKeyUp(KeyCode.R))
        {
            transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);
            wall.transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);
        }
        //rotate wall counterclockwise
        if (Input.GetKeyUp(KeyCode.Q))
        {
            transform.Rotate(0.0f, -15.0f, 0.0f, Space.World);
            wall.transform.Rotate(0.0f, -15.0f, 0.0f, Space.World);
        }
    }

}
