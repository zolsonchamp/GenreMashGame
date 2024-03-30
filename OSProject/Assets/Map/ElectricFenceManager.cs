using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFenceManager : MonoBehaviour,Damagable, Electronic
{
    public float maxHealth;
    public float currentHealth;
    public GameObject electricField;
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
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }
    public void Deactivate(float duration)
    {
        empDuration = duration;
        disabled = true;
        electricField.GetComponent<Electronic>()?.Deactivate(duration);
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
        electricField.GetComponent<Electronic>()?.ResetDuration();
        empTimer = 0;
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
