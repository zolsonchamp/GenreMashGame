using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadeDamage : MonoBehaviour
{
    public float count = 0;
    public float damage;
    public bool isWorking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        
        count++;
        if(count > 20)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("collided with " + other.gameObject);
        isWorking = true;
        other.gameObject.GetComponent<Damagable>()?.TakeDamage(damage);
    }
}
