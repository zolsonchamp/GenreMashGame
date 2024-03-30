using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Target")
        {
            Debug.Log("FlameCone hit " + other.gameObject);
            other.gameObject.GetComponent<Damagable>()?.TakeDamage(damage);
            if (other.gameObject.tag == "Wood")
            {
                other.gameObject.GetComponent<WoodWallManager>().ignited = true;
                other.gameObject.GetComponent<WoodWallManager>().burnTimer = 0f;
                other.gameObject.GetComponent<Flammable>()?.Burn();
            }
            //implement an flamable behavior for certain targets
            //other.gameObject.GetComponent<Flammable>()?.Ignited();
            //ignited: while true take damage over time,
            //being hit with the flamethower refreshed the timer
            //set to fale when timer expires
        }
    }
}
