using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if(Physics.Raycast(ray, out RaycastHit hitData, 500, layersToHit))
        {
            worldPosition= hitData.point;
        }
        correctedWorldPosition = new Vector3(worldPosition.x, 0, worldPosition.z);
        transform.position = correctedWorldPosition;
        if (Input.GetMouseButtonUp(0))
        {
            wall.transform.position = correctedWorldPosition;
            Instantiate(wall);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);
            wall.transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            transform.Rotate(0.0f, -15.0f, 0.0f, Space.World);
            wall.transform.Rotate(0.0f, -15.0f, 0.0f, Space.World);
        }
    }

}
