using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpHeight;
    public float mouseSensitivity;
    public float shootDelay;
    public float gunDamage;

    KeyCode sprintKey;
    KeyCode jumpKey;
    KeyCode shootButton;

    [Header("Animation")]
    public Animator animator;

    Rigidbody rb;

    float rotationX = 0;
    public bool isGrounded;
    public LayerMask ground;

    [SerializeField]
    Transform bulletSpawn;
    float lastShootTime;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject bulletTracer;
    public float bulletSpeed = 100f;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    public float currentHealth;

    [SerializeField]
    private Camera playerCam;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        sprintKey = KeyCode.LeftShift;
        jumpKey = KeyCode.Space;
        shootButton = KeyCode.Mouse0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
    }

    void Update()
    {

        GroundCheck();
        HandleLook();
        HandleMovement();
        Shoot();
    }

    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, ground))
            isGrounded = true;
        else
            isGrounded = false;
    }

    void HandleMovement()
    {

        // Handle player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float moveSpeed = (Input.GetKey(sprintKey) ? sprintSpeed : walkSpeed);

        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        //Jump
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
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

        if (Input.GetKey(shootButton) && lastShootTime + shootDelay < Time.time)
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
        Vector3 direction = playerCam.transform.forward;

        direction += new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
        );

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