using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMine : MonoBehaviour
{
    public Transform target;
    float targetDis;
    public float triggerRange;
    public float mineDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("Target");
        target = targetObject.transform;
        CheckTargetDistance();
    }

    void CheckTargetDistance()
    {
        targetDis = Vector3.Distance(target.position, transform.position);
        if (targetDis <= triggerRange)
        {
            gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Target").GetComponent<Damagable>()?.TakeDamage(mineDamage);
        }
    }
}
