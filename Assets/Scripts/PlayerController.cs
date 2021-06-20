using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static int playerHealth = 3;
    public static int numOfHearts = 3;
    public static int numOfConsecutiveKills = 0;
    public static int consecutiveKillsCount;
    public static int numOfHealthPotions = 0;
    public static bool itemDropped = false;

    private Image[] hearts = { null, null, null, null, null, null };
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private bool canMove = true;
    public float speed;
    private Vector2 movement;
    public float jumpForce = 25f;
    private Rigidbody2D rb2d;
    private Animator animator;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    public float attackDamage = 10f;
    public float attackRange = 0.15f;
    public float attackCd = 0.5f;
    private bool canAttack = true;
    [HideInInspector] public bool hasAttacked = false;

    public float fallMult = 2.5f;
    public float lowJumpMult = 2f;

    public LayerMask groundLayer;
    public float rayDist = 2f;

    [SerializeField] private bool isVulnerable = true;
    public float invulnerableTimer = 0.5f;

    private Collider2D c2D;
    private CameraController cm;

    private TextMeshProUGUI healthPotionCounter;
    private Image healthPotionUIMask;

    private bool healCDState = false;
    public float healCDTime = 2f; // The cooldown time before you can heal again
    private float healCDTimer = 0;

    private Animator deathAnim;
    public bool deathState = false;
    private bool canPlayDamagedAnim = true;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        c2D = GetComponent<Collider2D>();
        cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        // Initialise health potion UI objects
        GameObject healthPotionUIObject = GameObject.Find("HealthPotions");
        healthPotionCounter = healthPotionUIObject.transform.Find("HealthPotionCounter").GetComponent<TextMeshProUGUI>();
        healthPotionUIMask = healthPotionUIObject.transform.Find("HealthPotionMask").GetComponent<Image>();

        deathAnim = GameObject.Find("DeathScreen").GetComponent<Animator>();

        for (int i = 0; i < 6; i++)
        {
            hearts[i] = GameObject.Find("Heart" + i).GetComponent<Image>();
        }

        consecutiveKillsCount = Random.Range(2, 5);

        transform.Find("Sword").GetComponent<BoxCollider2D>().enabled = false;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            //Store the current horizontal input in the float moveHorizontal.
            float moveHorizontal = Input.GetAxisRaw("Horizontal");

            //Connects Animator variable speed to float moveHorizontal, to animate player.
            animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

            //Use the store floats to create a new Vector2 variable movement.
            movement = new Vector2(moveHorizontal, 0);

            //Animates Player Direction
            AnimatePlayerDirection(movement);

            //Add movement force to player.
            rb2d.AddForce(movement * speed);

            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.fixedDeltaTime;
            }
            else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMult - 1) * Time.fixedDeltaTime;
            }
        }
    }

    void Update()
    {
        ManageHeartSprites(); // Manage the player hearts on UI

        ManageHealthPotionUI(); // Manage the number of potions the player has on UI

        ManageHealthPotionDrop(); // Manage the counter before another health potion drops for the player

        if (playerHealth > numOfHearts)
        {
            playerHealth = numOfHearts;
        }

        if (playerHealth <= 0 && !deathState)
        {
            Death();
        }

        if (deathState)
        {
            canMove = false;
            canAttack = false;
            canPlayDamagedAnim = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // If Right Mouse Button or L is pressed
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.L))
        {
            Heal();
        }
        HealCooldown();

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
        }
        if (canAttack)
        {
            // If Left Mouse Button or M is pressed.
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.M)) && !isAttacking && PauseMenuScript.GameIsPaused == false)
            {
                attackTimer = attackCd;
                isAttacking = true;

                if (IsGrounded())
                {
                    canMove = false;
                }
                //Play Animation
                animator.SetTrigger("Attack");
            }
        }
        //Actually Attack on Specific Sprite Animation;
        if (GetComponent<SpriteRenderer>().sprite.name == "knight_attack_5" && hasAttacked == false)
        {
            transform.Find("Sword").GetComponent<BoxCollider2D>().enabled = true;

            // The rest of the attack is now handled by the Sword Trigger Script.

        }
        if (attackTimer <= 0)
        {
            attackTimer = 0;
            isAttacking = false;
            hasAttacked = false;

            if (!deathState)
            {
                canMove = true;
            }
        }
        //if (!isAttacking)
        if (GetComponent<SpriteRenderer>().sprite.name == "knight_attack_6")
        {
            transform.Find("Sword").GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void AnimatePlayerDirection(Vector2 movementVector)
    {
        //Stores the value of movement if player is moving to the right.
        Vector2 ToTheRight = new Vector2(1, 0);

        //Stores the value of movement if player is moving to the left.
        Vector2 ToTheLeft = new Vector2(-1, 0);

        //Animates the player depending on whether the player is moving left or right.
        if (movementVector == ToTheRight)
        {
            GetComponent<SpriteRenderer>().flipX = false; // Do not flip image on the X axis.
        }
        else if (movementVector == ToTheLeft)
        {
            GetComponent<SpriteRenderer>().flipX = true; // Flip Image on the X axis.
        }
    }

    private void ManageHeartSprites()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void ManageHealthPotionUI()
    {
        healthPotionCounter.SetText(""+numOfHealthPotions); // Set the counter to update the number of health potions the player has

        healthPotionUIMask.fillAmount = healCDTimer / healCDTime; // Set the cooldown mask fill value
    }

    private void ManageHealthPotionDrop()
    {
        if (itemDropped)
        {
            consecutiveKillsCount = Random.Range(2, 5); // Between 2 to 4 kills
            itemDropped = false;
        }

        if (numOfConsecutiveKills >= consecutiveKillsCount)
        {
            numOfConsecutiveKills = 0; // Reset number of kills until next potion drop
            itemDropped = true;
        }
    }

    /* This heal function runs once on RMB click */
    private void Heal()
    {
        // If the health potion is not on cooldown and the player has 1 or more health potions
        if (!healCDState && playerHealth < numOfHearts && numOfHealthPotions > 0)
        {
            playerHealth = numOfHearts; // Max out player health
            numOfHealthPotions--; // Decrement the number of health potions the player has
            healCDState = true; // Health potion cooldown begun
            healCDTimer = healCDTime; // Set timer to the healCDTime variable
        }
    }

    /* This updates every frame to get the healing cooldown timer to tick down */
    private void HealCooldown()
    {
        // If the health potion is on cooldown
        if (healCDState)
        {
            healCDTimer -= Time.deltaTime; // Start counting down the timer

            // If health potion timer is at 0 or below
            if (healCDTimer <= 0)
            {
                healCDState = false; // Health potion cooldown over
            }
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, rayDist, groundLayer.value) ||
            Physics2D.Raycast(this.transform.GetChild(1).GetChild(0).position, Vector2.down, rayDist, groundLayer.value) ||
            Physics2D.Raycast(this.transform.GetChild(1).GetChild(1).position, Vector2.down, rayDist, groundLayer.value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SoulAcquired()
    {
        numOfHearts += 1;
        playerHealth += 1;
    }

    public void TakeDamage(int damageValue, Vector3 direction)
    {
        if (isVulnerable)
        {
            playerHealth -= damageValue; // Subtract player health by damage value

            if (canPlayDamagedAnim)
            {
                animator.SetTrigger("Damaged");
            }
            
            StartCoroutine(Knockback(0.02f, 1000, direction));
            StartCoroutine(Invulnerable());       
        }
    }

    public void Death()
    {
        playerHealth = 0;
        deathState = true;
        animator.SetTrigger("Death");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && deathState)
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ColliderDistance2D colliderDist = collision.Distance(c2D);

        if (collision.CompareTag("Underground") && colliderDist.isOverlapped)
        {
            cm.zPos = -1.5f;
        }

        if (collision.CompareTag("Death") && !deathState)
        {
            Death();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ColliderDistance2D colliderDist = collision.Distance(c2D);

        if (collision.CompareTag("Underground") && !colliderDist.isOverlapped)
        {
            cm.zPos = -2.5f;
        }
    }

    // Brief invulnerability phase after taking damage
    private IEnumerator Invulnerable()
    {
        isVulnerable = false;
        yield return new WaitForSeconds(invulnerableTimer);
        isVulnerable = true;
    }

    private IEnumerator Knockback(float knockDur, float knockbackForce, Vector3 knockbackDir)
    {
        float timer = 0;

        while (knockDur > timer)
        {
            timer += Time.deltaTime;

            rb2d.velocity = new Vector2(0, 0);
            rb2d.AddForce(new Vector3(knockbackDir.x * knockbackForce, knockbackDir.y * knockbackForce, transform.position.z));
        }

        yield return 0;
    }

    private void DeathAnimFinished()
    {
        deathAnim.SetTrigger("DeathScreen");
        DeathCount.deaths++;
    }

    private void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
}
