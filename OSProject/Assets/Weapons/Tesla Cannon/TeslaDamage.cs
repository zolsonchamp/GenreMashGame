using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaDamage : MonoBehaviour
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
            Debug.Log("Tesla Round hit " + other.gameObject);
            other.gameObject.GetComponent<Damagable>()?.TakeDamage(damage);
            //create behavior Electronic
            //other.gameObject.GetComponent<Electronic>()?.Overload();
            //overload should deactivate firing scripts and damaging scripts for the affected targets
            //Objects to be given Electronic behavior:turrets and electric fence
            Destroy(gameObject);
        }
    }
}
