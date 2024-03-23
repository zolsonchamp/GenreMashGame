using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using FishNet.Connection;
using FishNet.Object;
using FishNet;

public class placeWall : NetworkBehaviour
{
    public GameObject[] util;
    public GameObject selectedUtil;
    public GameObject wall;
    public GameObject snake;
    public GameObject turret;
    public GameObject mud;
    public GameObject acid;
    public GameObject mine;
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public Vector3 correctedWorldPosition;
    public LayerMask layersToHit;
    // Start is called before the first frame update
    void Start()
    {
        //sets wall placement rotation at start
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        selectedUtil.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (!base.IsOwner)
            return;

        //converts mouse position to a world position through camera
        screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if(Physics.Raycast(ray, out RaycastHit hitData, 500, layersToHit))
        {
            worldPosition= hitData.point;
            
        }
        transform.position= worldPosition;
        selectedUtil.transform.position = worldPosition;
        //object selection
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            selectedUtil= util[0];
            wall.SetActive(true);
            snake.SetActive(false);
            turret.SetActive(false);
            mud.SetActive(false);
            acid.SetActive(false);
            mine.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            selectedUtil = util[1];
            wall.SetActive(false);
            snake.SetActive(true);
            turret.SetActive(false);
            mud.SetActive(false);
            acid.SetActive(false);
            mine.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            selectedUtil = util[2];
            wall.SetActive(false);
            snake.SetActive(false);
            turret.SetActive(true);
            mud.SetActive(false);
            acid.SetActive(false);
            mine.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            selectedUtil = util[3];
            wall.SetActive(false);
            snake.SetActive(false);
            turret.SetActive(false);
            mud.SetActive(true);
            acid.SetActive(false);
            mine.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            selectedUtil = util[4];
            wall.SetActive(false);
            snake.SetActive(false);
            turret.SetActive(false);
            mud.SetActive(false);
            acid.SetActive(true);
            mine.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            selectedUtil = util[5];
            wall.SetActive(false);
            snake.SetActive(false);
            turret.SetActive(false);
            mud.SetActive(false);
            acid.SetActive(false);
            mine.SetActive(true);
        }
        //on left click place a wall
        if (Input.GetMouseButtonUp(0))
        {
            selectedUtil.transform.position = worldPosition;
            GameObject go = Instantiate(selectedUtil);
            InstanceFinder.ServerManager.Spawn(go, null);

        }
        //rotate wall clockwise
        if (Input.GetKeyUp(KeyCode.R))
        {
            transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);
            selectedUtil.transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);
        }
        //rotate wall counterclockwise
        if (Input.GetKeyUp(KeyCode.Q))
        {
            transform.Rotate(0.0f, -15.0f, 0.0f, Space.World);
            selectedUtil.transform.Rotate(0.0f, -15.0f, 0.0f, Space.World);
        }
    }

}
