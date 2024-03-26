using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFenceManager : MonoBehaviour,Damagable
{
    public float maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
