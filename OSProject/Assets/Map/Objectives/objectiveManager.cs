using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;

public class objectiveManager : NetworkBehaviour, Damagable
{
    public float maxHealth;
    public float currentHealth;

    public Transform FPSPos;
    public float viableRange;
    public float playerDistance;

    [SerializeField] Transform circle;

    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        circle.localScale = new Vector3(viableRange, 1, viableRange);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject FPSObject = GameObject.FindGameObjectWithTag("Target");
        FPSPos = FPSObject.transform;
        
    }
    public void TakeDamage(float damage)
    {
        playerDistance = Vector3.Distance(FPSPos.position, transform.position);
        if (playerDistance <= viableRange)
        {
            currentHealth -= damage;
        }
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

