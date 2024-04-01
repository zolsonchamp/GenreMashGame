using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;

public class objectiveManager : NetworkBehaviour, Damagable
{
    public float maxHealth;
    public float currentHealth;
    
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
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
        GameManager.Instance.objectiveCount--;
        InstanceFinder.ServerManager.Despawn(gameObject);
        gameObject.SetActive(false);
    }
}

