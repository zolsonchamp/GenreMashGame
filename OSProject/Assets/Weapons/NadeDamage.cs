using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadeDamage : MonoBehaviour
{
    public float damage;
    public bool isWorking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        isWorking = true;
        other.gameObject.GetComponent<Damagable>()?.TakeDamage(damage);
    }
}
