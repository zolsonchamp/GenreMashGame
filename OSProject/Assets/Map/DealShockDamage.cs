using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealShockDamage : MonoBehaviour
{
    public float Damage;
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
        if (other.gameObject.tag == "Target")
        {
            Debug.Log("Collided with" + other.gameObject);
            other.gameObject.GetComponent<Damagable>()?.TakeDamage(Damage);
            other.gameObject.GetComponent<PlayerController>().isSlowed = true;
        }
    }
}
