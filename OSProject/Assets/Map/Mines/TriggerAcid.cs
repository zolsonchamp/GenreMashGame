using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAcid : MonoBehaviour, Damagable, Electronic
{
    public Transform target;
    public GameObject acidPool;
    float targetDis;
    public float triggerRange;
    public float damageRange;
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
            GameObject pool=Instantiate(acidPool);
            pool.transform.position=gameObject.transform.position;
            Destroy(gameObject);
          
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
        GameObject pool = Instantiate(acidPool);
        pool.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}
