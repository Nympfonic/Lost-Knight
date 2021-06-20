using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private Transform target;
    public float speed = 4f; // movement speed

    public float maxRange = 12f; // max target detection range
    public float minRange = 2f; // min target detection range

    private bool collidingWithCollider;

    public bool hasHealth = true; // toggle damageable NPC
    public float health; // NPC health

    public bool isEnemy = false; // toggle aggression towards the player
    public int playerDmgVal = 1; // adjust how many lives the NPC will take from the player

    public bool hasAnAttackAnim = false; // boolean check for attack animation
    public float attackRange = 2f; // range for the enemy to do an attack animation
    private bool attackCDState = false; // stop the AI from attacking relentlessly
    private float timeSinceLastAttack = 0;
    public float attackCDTime = 0.7f; // how long the AI will stop attacking the player, in seconds

    public float knockbackForce = 1000f; // adjustable knockback force for various types of enemies

    private Animator animator;

    private bool deathState = false;
    public GameObject itemDrop;

    public bool hasSound = false;
    public AudioClip[] audioClips;



    // Side note: I want the enemy to patrol until player is in line of sight then continously follow the player.
    // Side note: This didn't happen so we're left with dumb Ai.

    public bool isRanged = false;
    public float fireRate = 1f;
    public float nextFire;
    public GameObject bullet;

    public bool canMove = true;

    public float PlayerBelowAI_range = 1f;

    private void Start()
    {
        nextFire = Time.time;
    }

    void CheckIfTimeToFire()
    {
        if (Time.time > nextFire)
        {
            // Calculate Rotation
            Vector3 directionVector = transform.position - target.position;
            directionVector = directionVector.normalized;


            //Quaternion.LookRotation(directionVector, Vector3.up)
            //Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 90));
            //Instantiate(bullet, transform.position, Quaternion.LookRotation(new Vector3(0,0,directionVector.z), Vector3.back));
            Instantiate(bullet, transform.position, Quaternion.LookRotation(Vector3.forward, directionVector));

            nextFire = Time.time + fireRate;

            //Debug.Log("Fired!");
        }
        else
        {
            //Debug.Log("Checked to fire, but did not.");
        }
    }

    private void CheckAttackTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > attackCDTime)
        {
            attackCDState = false;
            timeSinceLastAttack = 0;
        }
    }

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z));

        // Else if the enemy is a ranged type, set attack range to the minimum detection range
        if (isRanged)
        {
            attackRange = minRange;

            if (dist <= attackRange && hasAnAttackAnim && isRanged && !deathState)
            {
                //animator.SetTrigger("Attack");
                CheckIfTimeToFire();
                //Debug.Log("Checked if time to fire!");
            }
        }

        if (!deathState && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().deathState)
        {
            if (!collidingWithCollider)
            {
                if (dist > minRange && dist < maxRange && !attackCDState && canMove) // If within range...
                {
                    animator.SetBool("PlayerInbound", true);
                    

                    // Do not move ai if player is below ai between range given by the variable PlayerBelowAI_width.
                    if (transform.position.y > target.position.y && (target.position.x > transform.position.x-PlayerBelowAI_range && target.position.x < transform.position.x+PlayerBelowAI_range))
                    {
                        // Do Nothing
                    }
                    else
                    {
                        // Move towards player.
                        transform.position = Vector3.MoveTowards(transform.position,
                            new Vector3(target.position.x, transform.position.y, transform.position.z),
                            speed * Time.deltaTime);

                        // Play an attack animation when in range
                        if (dist <= attackRange && hasAnAttackAnim && !isRanged)
                        {
                            animator.SetTrigger("Attack");
                        }
                    }

                    
                }
                else
                {
                    animator.SetBool("PlayerInbound", false);
                }
            }
        }

        if (attackCDState)
        {
            CheckAttackTimer();
        }

        if (health <= 0 && !deathState)
        {
            Death();
        }

        if (target.position.x > transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    // colliding with the player will damage the player
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidingWithCollider = true;

            if (isEnemy)
            {
                PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
                //Vector3 hitDirection = collision.transform.position - transform.position;
                Vector3 hitDirection = new Vector3(collision.transform.position.x - transform.position.x, 0, 0);
                hitDirection = hitDirection.normalized;

                pc.TakeDamage(playerDmgVal, hitDirection);

                attackCDState = true;
            }

            if (deathState)
            {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidingWithCollider = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            Death();
        }
    }

    public void TakeDamage(float damageValue, Vector3 direction)
    {
        health -= damageValue;
        attackCDState = true;

        animator.SetTrigger("Damaged");
        
        StartCoroutine(Knockback(0.02f, knockbackForce, direction));
    }

    public void Death()
    {
        health = 0;
        deathState = true;
        animator.SetTrigger("Death");
    }

    public void DeleteSelf() // Used by animation event system.
    {
        if (itemDrop != null)
        {
            PlayerController.numOfConsecutiveKills++;
            if (PlayerController.numOfConsecutiveKills >= PlayerController.consecutiveKillsCount)
            {
                Instantiate(itemDrop, transform.position, transform.rotation);
            }
        }
        Destroy(gameObject);
    }

    private void PlaySound()
    {
        if (hasSound)
        {
            AudioSource audioSource = GetComponent<AudioSource>();

            int index = Random.Range(0, audioClips.Length);
            AudioClip audioClip = audioClips[index];
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    private IEnumerator Knockback(float knockDur, float knockbackForce, Vector3 knockbackDir)
    {
        float timer = 0;

        while (timer < knockDur)
        {
            timer += Time.deltaTime;

            rb2d.velocity = new Vector2(0, 0);
            rb2d.AddForce(new Vector3(knockbackDir.x * knockbackForce * 0.8f, knockbackDir.y * knockbackForce, transform.position.z));
        }

        yield return 0;
    }
}