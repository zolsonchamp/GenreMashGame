using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMine : MonoBehaviour, Damagable, Electronic
{
    public Transform target;
    float targetDis;
    public float triggerRange;
    public float damageRange;
    public float mineDamage;
    public bool disabled = false;
    public float empTimer = 0;
    public float empDuration = 0;
    public float currentHealth = 1;
    public float maxHealth = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (disabled) { Deactivate(empDuration); }
        GameObject targetObject = GameObject.FindGameObjectWithTag("Target");
        target = targetObject.transform;
        CheckTargetDistance();
    }

    void CheckTargetDistance()
    {
        targetDis = Vector3.Distance(target.position, transform.position);
        if (targetDis <= triggerRange && !disabled)
        {
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("Target").GetComponent<Damagable>()?.TakeDamage(mineDamage);
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
    private void Die()
    {
        targetDis = Vector3.Distance(target.position, transform.position);
        if(targetDis <= damageRange)
        {
            GameObject.FindGameObjectWithTag("Target").GetComponent<Damagable>()?.TakeDamage(mineDamage);
        }
        Destroy(gameObject);
    }
}
