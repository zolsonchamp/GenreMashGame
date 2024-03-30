using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWallManager : MonoBehaviour, Damagable, Flammable
{
    public bool ignited = false;
    public float burnTimer = 0f;
    float burnDuration = 4f;
    public float maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ignited)
        {
            Burn();
        }
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
    public void Burn()
    {
        burnTimer += Time.deltaTime;
        if (burnTimer > burnDuration)
        {
            ignited = false;
            burnTimer = 0f;
        }
        currentHealth -= .03f;
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
