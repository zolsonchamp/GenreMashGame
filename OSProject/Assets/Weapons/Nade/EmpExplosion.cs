using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpExplosion : MonoBehaviour
{
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        count++;
        if (count > 20)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("collided with " + other.gameObject);
        //create behavior Electronic
        //other.gameObject.GetComponent<Electronic>()?.Overload();
        //overload should deactivate firing scripts and damaging scripts for the affected targets
        //Objects to be given Electronic behavior:turrets and electric fence
        
    }
}
