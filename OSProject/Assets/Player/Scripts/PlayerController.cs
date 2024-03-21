using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, Damagable
{
    [Header("Stats")]
    public float maxHealth;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpHeight;
    public float gravity;
    public float mouseSensitivity;
    public float rifleShootDelay;
    public float nadeShootDelay;
    public float revolverShootDelay;
    public float altShootDelay;
    public float gunDamage;
    public float nadeDamage;
    public float revolverDamage;
    private Vector3 jumpMomentum;
    private float jumpSpeed;

    KeyCode sprintKey;
    KeyCode jumpKey;
    KeyCode shootButton;
    KeyCode altShootButton;

    [Header("Animation")]
    public Animator animator;

    Rigidbody rb;

    float rotationX = 0;
    public bool isGrounded;
    public bool isMud;
    public bool isAcid;
    public LayerMask ground;
    public LayerMask mud;
    public LayerMask acid;
    
    [SerializeField]
    Transform bulletSpawn;
    float lastRifleShootTime;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject bulletTracer;
    public float bulletSpeed = 100f;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField]
    Transform nadeSpawn;
    float lastNadeShootTime;
    float lastRevovlerShootTime;
    float lastAltShootTime;
    public float nadeSpeed;

    public float currentHealth;
    [SerializeField] Image healthFill;

    [SerializeField]
    private Camera playerCam;

    public GameObject nade;

    int weaponSlot = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        sprintKey = KeyCode.LeftShift;
        jumpKey = KeyCode.Space;
        shootButton = KeyCode.Mouse0;
        altShootButton = KeyCode.Mouse1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
    }

    void Update()
    {

        GroundCheck();
        MudCheck();
        AcidCheck();
        if (isAcid)
            gameObject.GetComponent<Damagable>()?.TakeDamage(.01f);
        HandleLook();
        HandleMovement();
        Shoot();
        altShoot();
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            weaponSlot = 0;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            weaponSlot = 1;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            weaponSlot = 2;
        }

    }

    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, ground))
            isGrounded = true;
        else
            isGrounded = false;
    }
    void MudCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, mud))
        {
            isMud = true;
            isGrounded = true;
        }
        else
            isMud = false;
    }
    void AcidCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, acid))
        {
            isGrounded = true;
        }
        if (Physics.Raycast(transform.position, Vector3.down, 100f, acid))
        {
            isAcid = true;
        }
        else
            isAcid = false;
        
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
        float moveSpeed = (Input.GetKey(sprintKey) ? sprintSpeed : walkSpeed);
        // Handle player movement
        if (isGrounded)
        {
            if(isMud) 
            rb.velocity = new Vector3(moveDirection.x * moveSpeed/3, rb.velocity.y, moveDirection.z * moveSpeed/3);
            else
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

            jumpMomentum = moveDirection;
        }
        else
        {   
            
            rb.velocity = new Vector3((jumpMomentum.x+(moveDirection.x)/2) * jumpSpeed, rb.velocity.y, (jumpMomentum.z+(moveDirection.z/2)) * jumpSpeed);
        }
        
        //Jump
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            if (isMud)
            {
                jumpSpeed = moveSpeed / 3;
                rb.AddForce(Vector3.up * jumpHeight / 2, ForceMode.Impulse);
            }
            else
            {
                jumpSpeed = moveSpeed;
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Force);
        }

        //Animation
        //animator.SetFloat("PlayerMoveSpeed", new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
    }

    void HandleLook()
    {
        // Player Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void Shoot()
    {
        if (weaponSlot == 0)
        {
            if (Input.GetKey(shootButton) && lastRifleShootTime + rifleShootDelay < Time.time)
            {

                Vector3 direction = GetDirection();
                //Create shoot effect (muzzle flash)

                if (Physics.Raycast(bulletSpawn.position, direction, out RaycastHit hit, float.MaxValue))
                {
                    if (hit.collider.tag != tag)
                    {
                        hit.collider.gameObject.GetComponent<Damagable>()?.TakeDamage(gunDamage);
                    }

                    TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                    lastRifleShootTime = Time.time;
                }
                else
                {
                    TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                    StartCoroutine(SpawnTrail(trail, bulletSpawn.position + direction * 100, Vector3.zero, false));

                    lastRifleShootTime = Time.time;
                }
            }
        }
        if(weaponSlot == 1)
        {
            if (Input.GetKey(shootButton) && lastNadeShootTime + nadeShootDelay < Time.time)
            {

                Vector3 direction = GetAccurateDirection();
                GameObject projectile = Instantiate(nade);
                projectile.transform.position = nadeSpawn.position;
                projectile.GetComponent<Rigidbody>().AddForce(direction * nadeSpeed, ForceMode.Impulse);

                lastNadeShootTime = Time.time;

            }
        }
        if (weaponSlot == 2)
        {
            if (Input.GetKey(shootButton) && lastRevovlerShootTime + revolverShootDelay < Time.time)
            {

                Vector3 direction = GetAccurateDirection();
                //Create shoot effect (muzzle flash)

                if (Physics.Raycast(bulletSpawn.position, direction, out RaycastHit hit, float.MaxValue))
                {
                    if (hit.collider.tag != tag)
                    {
                        hit.collider.gameObject.GetComponent<Damagable>()?.TakeDamage(revolverDamage);
                    }

                    TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                    lastRevovlerShootTime = Time.time;
                }
                else
                {
                    TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                    StartCoroutine(SpawnTrail(trail, bulletSpawn.position + direction * 500, Vector3.zero, false));

                    lastRevovlerShootTime = Time.time;
                }
            }
        }
    }
    public void altShoot()
    {
        /*  possible jetpack???
        if (Input.GetKey(altShootButton) && lastAltShootTime + altShootDelay < Time.time)
        {
            Vector3 direction = GetDirection();
            GameObject projectile=Instantiate(nade);
            projectile.transform.position = this.transform.position;
            projectile.GetComponent<Rigidbody>().AddForce(direction,ForceMode.Impulse);
            lastRifleShootTime = Time.time;

        }
        */
         //grenade
       /* if (Input.GetKey(altShootButton) && lastAltShootTime + altShootDelay < Time.time)
        {
            
            Vector3 direction = GetDirection();
            GameObject projectile = Instantiate(nade);
            projectile.transform.position = nadeSpawn.position;
            projectile.GetComponent<Rigidbody>().AddForce(direction*nadeSpeed, ForceMode.Impulse);
           
            lastAltShootTime = Time.time;

        }*/
       /* if (Input.GetKey(altShootButton) && lastAltShootTime + altShootDelay < Time.time)
        {

            Vector3 direction = GetAltDirection();
            //Create shoot effect (muzzle flash)

            if (Physics.Raycast(bulletSpawn.position, direction, out RaycastHit hit, float.MaxValue))
            {
                if (hit.collider.tag != tag)
                {
                    hit.collider.gameObject.GetComponent<Damagable>()?.TakeDamage(revolverDamage);
                }

                TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                lastAltShootTime = Time.time;
            }
            else
            {
                TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, bulletSpawn.position + direction * 1000, Vector3.zero, false));

                lastAltShootTime = Time.time;
            }
        }*/
    }
    private Vector3 GetDirection()
    {
        Vector3 direction = playerCam.transform.forward;

        direction += new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
        );

        direction.Normalize();

        return direction;
    }
    private Vector3 GetAccurateDirection()
    {
        Vector3 direction = playerCam.transform.forward;

       
        direction.Normalize();

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= bulletSpeed * Time.deltaTime;

            yield return null;
        }

        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            //create on hit effect
        }

        Destroy(Trail.gameObject, Trail.time);
    }

    private void UpdateHealthUI(float newHealth)
    {
        float fillAmount = newHealth / maxHealth;
        healthFill.fillAmount = fillAmount;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        UpdateHealthUI(currentHealth);
    }

    public void ReceiveHealth(float health)
    {
        currentHealth += health;
        UpdateHealthUI(currentHealth);
    }

    private void Die()
    {

    }
}
