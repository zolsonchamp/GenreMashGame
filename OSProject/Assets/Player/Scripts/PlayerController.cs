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

    private Vector3 jumpMomentum;
    private float jumpSpeed;

    [Header("Weapon Prefabs")]
    public GameObject flameCone;
    public GameObject nade;
    public GameObject emp;
    public GameObject teslaRound;
    
    [Header("Weapon FireRates")]
    public float rifleShootDelay;
    public float nadeShootDelay;
    public float revolverShootDelay;
    public float shotgunShootDelay;
    public float teslaShootDelay;
    public float teslaChargeCounter = 0f;
    public float teslaChargeTime;
    public float altShootDelay;

    [Header("Weapon Damage")]
    public float rifleDamage;
    public float revolverDamage;
    public float shotgunDamage;
    public float shotgunKnockback;

    [Header("Weapon Ammo")]
    public int rifleAmmoCapacity;
    public int nadeAmmoCapacity;
    public int revolverAmmoCapacity;
    public int shotgunAmmoCapacity;
    public int teslaAmmoCapacity;
    public float flamethrowerFuelCapacity;
    public int rifleAmmoCount;
    public int nadeAmmoCount;
    public int revolverAmmoCount;
    public int shotgunAmmoCount;
    public int teslaAmmoCount;
    public float flamethrowerFuelCount;
    public float rifleReloadTime;
    public float nadeReloadTime;
    public float revolverReloadTime;
    public float shotgunReloadTime;
    public float teslaReloadTime;
    public float flamethrowerReloadTime;
    public float reloadTimer = 0f;
    private IEnumerator reload;

    int weaponSlot = 0;
    bool reloadCancelled = false;

    KeyCode sprintKey;
    KeyCode jumpKey;
    KeyCode shootButton;
    KeyCode altShootButton;

    public Text ammoIndicator;

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
    float lastShotgunShootTime;
    float lastTeslaShootTime;
    float lastAltShootTime;
    public float nadeSpeed;
    public float teslaSpeed;
    public float currentHealth;
    [SerializeField] Image healthFill;

    [SerializeField]
    private Camera playerCam;

    private void Awake()
    {
       
        flameCone.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload = Reload(weaponSlot);
            StartCoroutine(reload);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            weaponSlot = 0;
            StopCoroutine(reload);
            reloadTimer = 0f;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            weaponSlot = 1;
            StopCoroutine(reload);
            reloadTimer = 0f;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            weaponSlot = 2;
            StopCoroutine(reload);
            reloadTimer = 0f;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            weaponSlot = 3;
            StopCoroutine(reload);
            reloadTimer = 0f;
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            weaponSlot = 4;
            StopCoroutine(reload);
            reloadTimer = 0f;
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            weaponSlot = 5;
            StopCoroutine(reload);
            reloadTimer = 0f;
        }
        
        UpdateAmmoIndicator(weaponSlot);
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
            //rifle slot
            if (Input.GetKey(shootButton) && lastRifleShootTime + rifleShootDelay < Time.time && rifleAmmoCount > 0)
            {
                rifleAmmoCount--;

                Vector3 direction = GetDirection();
                //Create shoot effect (muzzle flash)

                if (Physics.Raycast(bulletSpawn.position, direction, out RaycastHit hit, float.MaxValue))
                {
                    if (hit.collider.tag != tag)
                    {
                        hit.collider.gameObject.GetComponent<Damagable>()?.TakeDamage(rifleDamage);
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
        if(weaponSlot == 1) //grenade launcher, left click for explosive grenades, right click for emp grenades
        {
            if (Input.GetKey(shootButton) && lastNadeShootTime + nadeShootDelay < Time.time && nadeAmmoCount > 0)
            {
                nadeAmmoCount--;
                Vector3 direction = GetAccurateDirection();
                GameObject projectile = Instantiate(nade);
                projectile.transform.position = nadeSpawn.position;
                projectile.GetComponent<Rigidbody>().AddForce(direction * nadeSpeed, ForceMode.Impulse);

                lastNadeShootTime = Time.time;
               

            }
            if (Input.GetKey(altShootButton) && lastNadeShootTime + nadeShootDelay < Time.time && nadeAmmoCount > 0)
            {
                nadeAmmoCount--;
                Vector3 direction = GetAccurateDirection();
                GameObject projectile = Instantiate(emp);
                projectile.transform.position = nadeSpawn.position;
                projectile.GetComponent<Rigidbody>().AddForce(direction * nadeSpeed, ForceMode.Impulse);

                lastNadeShootTime = Time.time;
                //read EmpNade script for more info on what needs to be added

            }
        }
        if (weaponSlot == 2) //revolver
        {
            if (Input.GetKeyDown(shootButton) && lastRevovlerShootTime + revolverShootDelay < Time.time && revolverAmmoCount > 0)
            {
                revolverAmmoCount--;
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
        if (weaponSlot == 3) //shotgun, gives backwards knockback effect when fired
        {
            if (Input.GetKeyDown(shootButton) && lastShotgunShootTime + shotgunShootDelay < Time.time && shotgunAmmoCount > 0)
            {
                Vector3 kickback = playerCam.transform.forward;
               
                shotgunAmmoCount--;

                jumpMomentum = new Vector3(kickback.x * shotgunKnockback*-1, Mathf.Abs(kickback.y), kickback.z * shotgunKnockback*-1);
                rb.AddForce(rb.velocity  * shotgunKnockback, ForceMode.Impulse);
                for (int buckshot = 0; buckshot < 10; buckshot++)
                {
                    Vector3 direction = GetDirection();
                    //Create shoot effect (muzzle flash)
                
                    if (Physics.Raycast(bulletSpawn.position, direction, out RaycastHit hit, float.MaxValue))
                    {
                        if (hit.collider.tag != tag)
                        {
                            hit.collider.gameObject.GetComponent<Damagable>()?.TakeDamage(shotgunDamage);
                        }

                        TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                        StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                        lastShotgunShootTime = Time.time;
                    }
                    else
                    {
                        TrailRenderer trail = Instantiate(bulletTracer.GetComponent<TrailRenderer>(), bulletSpawn.position, Quaternion.identity);

                        StartCoroutine(SpawnTrail(trail, bulletSpawn.position + direction * 100, Vector3.zero, false));

                        lastShotgunShootTime = Time.time;
                    }
                }
            }
        }
        if (weaponSlot == 4)
        {
           //tesla cannon, hold leftclick to charge, after .7 seconds TeslaRound fires, release leftclick to cancel
            if (Input.GetKey(shootButton) && lastTeslaShootTime + teslaShootDelay < Time.time && teslaAmmoCount > 0)
            {
               
                teslaChargeCounter += Time.deltaTime;
                if (teslaChargeCounter > teslaChargeTime)
                {
                    teslaAmmoCount--;
                    Vector3 direction = GetAccurateDirection();
                    GameObject projectile = Instantiate(teslaRound);
                    projectile.transform.position = nadeSpawn.position;
                    projectile.GetComponent<Rigidbody>().AddForce(direction * teslaSpeed, ForceMode.Impulse);
                    teslaChargeCounter = 0f;
                    lastTeslaShootTime = Time.time;
                }
                
            }
            else {
                teslaChargeCounter = 0f;
            }
        }
        if (weaponSlot == 5)
        {
            //Flamethrower
               if (Input.GetKey(shootButton) && flamethrowerFuelCount>0)
            {
                flamethrowerFuelCount -= .03f;
                if(flamethrowerFuelCount<0)flamethrowerFuelCount = 0;
                flameCone.SetActive(true);
            }
            else
            {
                flameCone.SetActive(false);
            }
        }
    }
    IEnumerator Reload(int weaponSlot)
    {
        bool reloadComplete = false;
        
        switch (weaponSlot)
        {
            case 0:
                while (!reloadComplete)
                {
                    reloadTimer += Time.deltaTime;
                    ammoIndicator.text = string.Format("Reloading...\n{0}", rifleReloadTime-reloadTimer);
                    if (reloadTimer >= rifleReloadTime)
                    {
                        rifleAmmoCount = rifleAmmoCapacity;
                        reloadComplete = true;
                        reloadTimer = 0f;
                    }
                    yield return null;
                }
                StopCoroutine(reload);
                break;
            case 1:
                while (!reloadComplete)
                {
                    reloadTimer += Time.deltaTime;
                    ammoIndicator.text = string.Format("Reloading...\n{0}", nadeReloadTime-reloadTimer);
                    if (reloadTimer >= nadeReloadTime)
                    {
                        nadeAmmoCount = nadeAmmoCapacity;
                        reloadComplete = true;
                        reloadTimer = 0f;
                    }
                    yield return null;
                }
                StopCoroutine(reload);
                break;
            case 2:
                while (!reloadComplete)
                {
                    reloadTimer += Time.deltaTime;
                    ammoIndicator.text = string.Format("Reloading...\n{0}", revolverReloadTime-reloadTimer);
                    if (reloadTimer >= revolverReloadTime)
                    {
                        revolverAmmoCount = revolverAmmoCapacity;
                        reloadComplete = true;
                        reloadTimer = 0f;
                    }
                    yield return null;
                }
                StopCoroutine (reload);
                break;
            case 3:
                while (!reloadComplete)
                {
                    reloadTimer += Time.deltaTime;
                    ammoIndicator.text = string.Format("Reloading...\n{0}", shotgunReloadTime-reloadTimer);
                    if (reloadTimer >= shotgunReloadTime)
                    {
                        shotgunAmmoCount = shotgunAmmoCapacity;
                        reloadComplete = true;
                        reloadTimer= 0f;
                    }
                    yield return null;
                }
                StopCoroutine (reload);
                break;
            case 4:
                while (!reloadComplete)
                {
                    reloadTimer += Time.deltaTime;
                    ammoIndicator.text = string.Format("Reloading...\n{0}", teslaReloadTime-reloadTimer);
                    if (reloadTimer >= teslaReloadTime)
                    {
                        teslaAmmoCount = teslaAmmoCapacity;
                        reloadComplete = true;
                        reloadTimer = 0f;
                    }
                    yield return null;
                }
                StopCoroutine (reload);
                break;
            case 5:
                while (!reloadComplete)
                {
                    reloadTimer += Time.deltaTime;
                    ammoIndicator.text = string.Format("Reloading...\n{0}", flamethrowerReloadTime-reloadTimer);
                    if (reloadTimer >= flamethrowerReloadTime)
                    {
                        flamethrowerFuelCount = flamethrowerFuelCapacity;
                        reloadComplete = true;
                        reloadTimer = 0f;
                    }
                    yield return null;
                }
                StopCoroutine(reload);
                break;
        }
    }
    public void UpdateAmmoIndicator(int weaponSlot)
    {
        if (weaponSlot == 0) ammoIndicator.text = string.Format("Rifle\n{0}/{1}", rifleAmmoCount, rifleAmmoCapacity);
        if (weaponSlot == 1) ammoIndicator.text = string.Format("Grenade Launcher\n{0}/{1}", nadeAmmoCount, nadeAmmoCapacity);
        if (weaponSlot == 2) ammoIndicator.text = string.Format("Revolver\n{0}/{1}", revolverAmmoCount, revolverAmmoCapacity);
        if (weaponSlot == 3) ammoIndicator.text = string.Format("Shotgun\n{0}/{1}", shotgunAmmoCount, shotgunAmmoCapacity);
        if (weaponSlot == 4) ammoIndicator.text = string.Format("Tesla Cannon\n{0}/{1}\nCharge: {2}%", teslaAmmoCount, teslaAmmoCapacity,((teslaChargeCounter/teslaChargeTime)*100).ToString("0"));
        if (weaponSlot == 5) ammoIndicator.text = string.Format("Flamethrower\n{0}/{1}", flamethrowerFuelCount, flamethrowerFuelCapacity);
        
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
