using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet;

public class SniperController : NetworkBehaviour, Damagable, Electronic
{
    //Prefabs and tranforms
    [SerializeField] Transform bulletSpawn;
    [SerializeField] Transform bodyRotation;
    [SerializeField] Transform gun;
    [SerializeField] GameObject bulletTracer;

    //Shoot info
    float lastShootTime;
    public int burstLimit;
    public int shotCount = 0;
    public float shootDelay;
    public float burstRate;
    public float burstDelay;
    public float chargeTime;
    public float chargeCounter=0f;
    public float bulletSpeed = 100f;
    public float bulletDamage;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0f, 0f, 0f);

    //Self Info
    public float maxHealth;
    public float currentHealth;

    //Targeting
    public Transform target;
    float targetDis;
    public float attackRange;

    public bool disabled = false;
    public float empTimer = 0;
    public float empDuration = 0;

    void Start()
    {
        //Get Player as target
        currentHealth = maxHealth;
    }

    void Update()
    {
        //if (!IsOwner)
        //    return;

        if (disabled) { Deactivate(empDuration); }  
        GameObject targetObject = GameObject.FindGameObjectWithTag("Target");
        target = targetObject.transform;
        CheckTargetDistance();
    }

    void CheckTargetDistance()
    {
        targetDis = Vector3.Distance(target.position, transform.position);
        if (targetDis <= attackRange)
        {
            bodyRotation.LookAt(target);
            Shoot();
        }
        else
        {
            chargeCounter = 0f;
        }
    }

    public void Shoot()
    {

        if (lastShootTime + shootDelay < Time.time && !disabled)
        {
            shootDelay = burstRate;
            Vector3 direction = GetDirection();
            //Create shoot effect (muzzle flash)
            chargeCounter += Time.deltaTime;
            if (chargeCounter > chargeTime)
            {
                shotCount++;
                if (Physics.Raycast(bulletSpawn.position, direction, out RaycastHit hit, float.MaxValue))
                {
                    if (hit.collider.tag == "Player")
                    {
                        hit.collider.gameObject.transform.parent.GetComponent<Damagable>()?.TakeDamage(bulletDamage);
                    }

                    TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);
                    InstanceFinder.ServerManager.Spawn(trail.gameObject, null);

                    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                    lastShootTime = Time.time;
                    if (shotCount >= burstLimit)
                    {
                        shotCount = 0;
                        shootDelay = burstDelay;
                        chargeCounter = 0;
                    }
                }
                else
                {
                    TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);
                    InstanceFinder.ServerManager.Spawn(trail.gameObject, null);

                    StartCoroutine(SpawnTrail(trail, bulletSpawn.position + direction * 100, Vector3.zero, false));

                    lastShootTime = Time.time;
                    if (shotCount >= burstLimit)
                    {
                        shotCount = 0;
                        shootDelay = burstDelay;
                        chargeCounter = 0;
                    }
                }
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = gun.transform.forward;

        direction += new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
        );

        direction.Normalize();

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, bool madeImpact)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= bulletSpeed * Time.deltaTime;

            yield return null;
        }

        trail.transform.position = hitPoint;
        if (madeImpact)
        {
            //create on hit effect
        }

        Destroy(trail.gameObject, trail.time);
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
        InstanceFinder.ServerManager.Despawn(gameObject);
        Destroy(gameObject);
    }
}