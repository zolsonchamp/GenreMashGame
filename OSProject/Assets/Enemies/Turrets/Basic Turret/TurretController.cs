using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    //Prefabs and tranforms
    [SerializeField] Transform bulletSpawn;
    [SerializeField] Transform bodyRotation;
    [SerializeField] Transform gun;
    [SerializeField] GameObject bulletTracer;

    //Shoot info
    float lastShootTime;
    public float shootDelay;
    public float bulletSpeed = 100f;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

    //Self Info
    public float currentHealth;

    //Targeting
    public Transform target;
    float targetDis;
    public float attackRange;
 

    void Start()
    {
        //Get Player as target
    }

    void Update()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("Target");
        target = targetObject.transform;
        CheckTargetDistance();
    }

    void CheckTargetDistance()
    {
        targetDis = Vector3.Distance(target.position, transform.position);
        if(targetDis <= attackRange)
        {
            bodyRotation.LookAt(target);
            Shoot();
        }
    }

    public void Shoot()
    {

        if (lastShootTime + shootDelay < Time.time)
        {

            Vector3 direction = GetDirection();
            //Create shoot effect (muzzle flash)

            if (Physics.Raycast(bulletSpawn.position, direction, out RaycastHit hit, float.MaxValue))
            {
                //Add damage

                TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                lastShootTime = Time.time;
            }
            else
            {
                TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, bulletSpawn.position + direction * 100, Vector3.zero, false));

                lastShootTime = Time.time;
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
}
