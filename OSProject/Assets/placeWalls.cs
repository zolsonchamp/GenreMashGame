using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class placeWalls : MonoBehaviour
{
    public GameObject wall;
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public Vector3 correctedWorldPosition;
    public LayerMask layersToHit;
    // Start is called before the first frame update
    void Start()
    {
        
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
        correctedWorldPosition = new Vector3(worldPosition.x, 5, worldPosition.z);
        transform.position = correctedWorldPosition;
        if (Input.GetMouseButtonUp(0))
        {
            Instantiate(wall);
            wall.transform.position = correctedWorldPosition; 
            Debug.Log(wall.transform.position);
        }
    }

}
