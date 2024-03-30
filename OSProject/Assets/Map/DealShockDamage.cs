using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealShockDamage : MonoBehaviour, Electronic
{
    public float Damage;
    public bool disabled = false;
    public float empTimer = 0;
    public float empDuration = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (disabled) { Deactivate(empDuration); }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Target"&&!disabled)
        {
            Debug.Log("Collided with" + other.gameObject);
            other.gameObject.GetComponent<Damagable>()?.TakeDamage(Damage);
            other.gameObject.GetComponent<PlayerController>().isSlowed = true;
            other.gameObject.GetComponent<PlayerController>().slowTimer = 0;
        }
    }
    public void Deactivate(float duration)
    {
        empDuration = duration;
        disabled = true;
        empTimer += Time.deltaTime;
        if (empTimer > duration)
        {
            disabled = false;
            empTimer = 0;
            empDuration = 0;
        }
    }
    public void ResetDuration()
    {
        empTimer = 0;
    }
}
