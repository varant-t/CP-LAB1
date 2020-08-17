using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


// The GameObject is made to bounce using the space key.
// Also the GameOject can be moved forward/backward and left/right.
// Add a Quad to the scene so this GameObject can collider with a floor.

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{

    public int maxHealth = 10;
    public int currentHealth;

    public HealthBar healthBar;


    public float speed;
    public float jumpSpeed;
    public float gravity;

    Vector3 moveDirection = Vector3.zero;
    public CharacterController controller;

    public Transform thingToLookFrom;
    public float lookDistance;

    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileForce;

    Animator animator;

    GameManager gm;


    void Start()
    {
     
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        gm = GetComponent<GameManager>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (speed <= 0)
        {
            speed = 6.0f;

            Debug.Log("Speed not set on " + name + ". Defaulting to " + speed);
        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 10.0f;

        }

        if (gravity <= 0)
        {
            gravity = 20.0f;
        }

        if (lookDistance <= 0)
        {
            lookDistance = 10.0f;
        }

        if (projectileForce <= 0)
        {
            projectileForce = 10.0f;
        }

        // let the gameObject fall down
        //gameObject.transform.position = new Vector3(0, 5, 0);
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);

        // Used for the Raycast to store information about what it collides with
        RaycastHit hit;

        // If there is no thingToLookFrom, default to Character
        if (!thingToLookFrom)
        {
            Debug.DrawRay(transform.position, transform.forward * lookDistance, Color.red);

            if (Physics.Raycast(transform.position, transform.forward, out hit, lookDistance))
            {
               // Debug.Log("Raycast: " + hit.collider.gameObject.name);
            }
        }
        else
        {
            Debug.DrawRay(thingToLookFrom.position, thingToLookFrom.forward * lookDistance, Color.red);

            if (Physics.Raycast(thingToLookFrom.position, thingToLookFrom.forward, out hit, lookDistance))
            {
               // Debug.Log("Raycast: " + hit.collider.gameObject.name);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //fire();

            // Use Animation Event to create projectile
            animator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Punching");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Kick");
        }


        if(currentHealth <= 0)
        {
            speed = 0;
            animator.SetTrigger("IsDead");
        }
        
        
       

        animator.SetFloat("Speed", transform.InverseTransformDirection(controller.velocity).z);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void fire()
    {
        Debug.Log("Pew Pew");

        Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);
    }

    public void TakeDamage(int damage)
    {
       
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    // Usage Rules:
    // - Both GameObjects must have Colliders
    // - One or both GameObjects must have a Rigidbody
    void OnCollisionEnter(Collision collision)
    {
       
    }

    void OnCollisionExit(Collision collision)
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
      
    }

    // Usage Rules:
    // - GameObject must have CharacterController
    // - Other GameObject must have a Collider
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
       
    }

    // Usage Rules:
    // - GameObject must have Collider marked as "IsTrigger"
    // - One or both GameObjects must have a Collider
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lava")
        {
            TakeDamage(2);
            Debug.Log("In lava Trigger");

        }

        if (other.gameObject.tag == "Snow")
        {
            speed = 3.0f;
            Debug.Log("In lava Trigger");

        }



    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Lava")
        {
            TakeDamage(2);
            Debug.Log("In lava Trigger");

        }

        if (other.gameObject.tag == "Snow")
        {
            speed = 6.0f;
            Debug.Log("In lava Trigger");

        }


    }

    void OnTriggerStay(Collider other)
    {
       
       
    }

    public void killPlayer()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");

    }
}

