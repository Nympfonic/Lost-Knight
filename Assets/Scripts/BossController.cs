using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public Slider bossHealthSlider; // Link to the Boss health bar slider object
    private Animator animator; // Boss animator
    private Quaternion rotation;
    private Transform playerTarget;
    private CameraController cameraController;
    private AudioSource hurtSound;

    public float health = 125f; // Boss health value
    private float totalHealth;
    public int laserDamage = 1; // Boss laser attack damage value
    private float anglePerSecond = 10f; // Degrees per second that the Boss rotates towards the player
    public bool deathState = false; // A check for if the Boss is dead
    private bool hasPlayedIntroAnim = false, hasPlayedIntroPhaseTwoAnim = false, hasPlayedDeathAnim = false;
    private bool isAttacking = false, isVulnerable = true;
    private bool phaseOne = false, phaseTwo = false;
    private float targetLastLocationX, lastLocationX, lastLocationY;
    public float trackingTime = 1f;
    private float trackingTimer = 0;
    private bool beginTracking = false, storedTargetLoc = false;
    private bool stopChildRotation = false;
    private bool hasDoneAttackOnce = false;
    private bool laserAttack = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        hurtSound = GetComponent<AudioSource>();

        rotation = transform.Find("BossSupport").rotation; // Storing the rotation value of the child object in the rotation variable

        totalHealth = health;
        bossHealthSlider.maxValue = health; // Ensure that the max Boss health bar slider value is the same as the Boss' health
    }
    
    void Update()
    {
        bossHealthSlider.value = health; // Equate the Boss health bar slider value to the Boss' health

        ManagePhaseOne();

        ManagePhaseTwo();

        ManageDeath();
    }

    private void LateUpdate()
    {
        /* Stop the Boss Support arm game object from rotating along with the Boss itself.
         * This is for when the Boss rotates to face the player which would normally
         * affect child objects too. */
        if (!stopChildRotation)
        {
            transform.GetChild(0).rotation = rotation;
        }

        if (phaseTwo && !deathState)
        {
            JumpAttackTracking(); // Begin running the Jump attack tracking function during phase 2
        }

        if (deathState)
        {
            transform.position = new Vector3(lastLocationX, lastLocationY, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (deathState)
            {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            }
        }
    }

    /* On collision with the player we want the player to take damage */
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            Vector3 hitDirection = new Vector3(collision.transform.position.x - transform.position.x, 0, 0); // Calculate knockback direction vector
            hitDirection = hitDirection.normalized; // Normalise it to only get the direction without magnitude

            pc.TakeDamage(1, hitDirection); // Apply damage and knockback to the player
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }
    }

    /* This function manages the damage being dealt to the Boss */
    public void TakeDamage(float damageValue)
    {
        // Check if the Boss is vulnerable to attack
        if (isVulnerable)
        {
            float damageVariance = Random.Range(0.8f, 1f); // Randomise player damage between 80% to 100% of PlayerController damage value lol

            health -= damageValue * damageVariance;

            hurtSound.Play();
            animator.SetTrigger("Damaged"); // Play Boss damaged animation
        }
    }

    private void ManagePhaseOne()
    {
        if (!hasPlayedIntroAnim && !phaseOne)
        {
            hasPlayedIntroAnim = animator.GetBool("HasPlayedIntroAnim"); // Get a boolean from the animator and store in hasPlayedIntroAnim variable
        }

        if (hasPlayedIntroAnim)
        {
            phaseOne = true; // Activate phase 1 mechanics once the intro animation finishes playing
        }

        if (phaseOne && !phaseTwo)
        {
            TargetPlayer(); // Begin targeting the player during phase 1
        }
    }

    private void ManagePhaseTwo()
    {
        // If the Boss has less than 40% of its hp left, play the phase 2 intro animation
        if (health <= 0.6f * totalHealth && !hasPlayedIntroPhaseTwoAnim && !isAttacking)
        {
            phaseOne = false; // Deactivate phase 1 mechanics
            ResetRotation(25f); // Reset the Eye rotation to the centre from its current rotation
            animator.SetTrigger("Intro_Phase2"); // Play the phase 2 intro animation
        }

        if (!hasPlayedIntroPhaseTwoAnim && !phaseTwo)
        {
            hasPlayedIntroPhaseTwoAnim = animator.GetBool("HasPlayedIntroPhase2Anim");
        }

        if (hasPlayedIntroPhaseTwoAnim)
        {
            phaseTwo = true; // Activate phase 2 mechanics once the intro animation finishes playing
        }

        if (phaseTwo && laserAttack)
        {
            TargetPlayer();
        }

        if (phaseTwo && !laserAttack)
        {
            ResetRotation(45f);
        }
    }

    private void ManageDeath()
    {
        if (health <= 0 && !deathState)
        {
            health = 0;
            lastLocationX = transform.position.x;
            lastLocationY = transform.position.y;
            deathState = true;
        }

        if (deathState && !hasPlayedDeathAnim)
        {
            animator.SetTrigger("Death"); // Play Boss death animation
            hasPlayedDeathAnim = true;

            BossCameraTrigger camTrigger = GameObject.Find("BossTrigger").GetComponent<BossCameraTrigger>();
            camTrigger.bossFightOver = true;
        }
    }

    /* This manages the Boss targeting system */
    private void TargetPlayer()
    {
        Vector3 targetVector = playerTarget.position - transform.position; // Find direction vector to the player
        Quaternion lookAtRotation = Quaternion.LookRotation(Vector3.forward, targetVector); // Correctly face the Boss towards the player
        //float distanceFromTarget = Vector2.Distance(transform.position, playerTarget.position); // Find distance to player

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, 
            lookAtRotation, 
            Time.deltaTime * RotationSpeed()); // Set the Boss' rotation every frame update
    }

    private float RotationSpeed()
    {
        if (animator.GetBool("IsAttacking"))
        {
            anglePerSecond = 12f; // Slow down Boss rotation speed when attacking
        }
        else
        {
            anglePerSecond = 14f; // Normal rotation speed
        }
        return anglePerSecond;
    }

    private void ResetRotation(float angleSpeed)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * angleSpeed);
    }

    private void JumpAttackTracking()
    {
        if (!storedTargetLoc && beginTracking)
        {
            targetLastLocationX = playerTarget.position.x; // Store last player position before tracking
            storedTargetLoc = true; // Make the condition run once
        }

        if (hasDoneAttackOnce)
        {
            transform.position = new Vector3(lastLocationX, transform.position.y, transform.position.z);
        }
        
        // Timer
        if (beginTracking)
        {
            trackingTimer += Time.deltaTime / trackingTime;

            //Vector2 targetLocationX = new Vector2(playerTarget.position.x - transform.position.x, transform.position.y);

            //Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            //rb2d.MovePosition(Vector2.Lerp(
                //transform.position,
                //new Vector2(targetLastLocationX, transform.position.y),
                //trackingTimer));
            if (!hasDoneAttackOnce)
            {
                transform.position = new Vector3(
                    Mathf.SmoothStep(transform.position.x, targetLastLocationX, trackingTimer),
                    transform.position.y,
                    transform.position.z);
            }
            else
            {
                transform.position = new Vector3(
                    Mathf.SmoothStep(lastLocationX, targetLastLocationX, trackingTimer),
                    transform.position.y,
                    transform.position.z);
            }

            if (trackingTimer >= 1)
            {
                beginTracking = false; // Disable timer
                lastLocationX = transform.position.x; // Store last position before tracking
                hasDoneAttackOnce = true;
                transform.position = new Vector3(lastLocationX, transform.position.y, transform.position.z);
            }
        }
    }
    
    #region Animation Event functions
    private void AttackingState()
    {
        animator.SetBool("IsAttacking", !isAttacking);
        isAttacking = !isAttacking;
    }

    public void PhaseOneInit()
    {
        // Camera shake
        float storedShakeTime = cameraController.shakeTime; // Store original shakeTime value
        float storedShakeMagnitude = cameraController.shakeMagnitude;
        cameraController.shakeTime = 5f; // Set new shakeTime value
        cameraController.shakeMagnitude = 0.1f;
        cameraController.CameraShake();

        // Reset original values
        cameraController.shakeTime = storedShakeTime;
        cameraController.shakeMagnitude = storedShakeMagnitude;
    }

    public void PhaseTwoInit()
    {
        stopChildRotation = true;

        // Camera shake
        // shake should last 2 or 4 seconds
        float storedShakeTime = cameraController.shakeTime; // Store original shakeTime value
        float storedShakeMagnitude = cameraController.shakeMagnitude;
        cameraController.shakeTime = 4f; // Set new shakeTime value
        cameraController.shakeMagnitude = 0.1f;
        cameraController.CameraShake();

        // Reset original values
        cameraController.shakeTime = storedShakeTime;
        cameraController.shakeMagnitude = storedShakeMagnitude;
    }

    private void GroundSlamCamShake()
    {
        float storedShakeTime = cameraController.shakeTime; // Store original shakeTime value
        float storedShakeMagnitude = cameraController.shakeMagnitude;
        cameraController.shakeTime = 0.3f; // Set new shakeTime value
        cameraController.shakeMagnitude = 0.08f;
        cameraController.CameraShake();

        // Reset original values
        cameraController.shakeTime = storedShakeTime;
        cameraController.shakeMagnitude = storedShakeMagnitude;
    }

    private void StartTracking()
    {
        beginTracking = true;
    }

    private void ResetTracking()
    {
        trackingTimer = 0;
        storedTargetLoc = false;
    }

    private void LaserAttackStart()
    {
        laserAttack = true;
    }

    private void LaserAttackEnd()
    {
        laserAttack = false;
    }

    private void DeathCall()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.isKinematic = false;
        rb2d.velocity = new Vector2(0, 0);
        rb2d.gravityScale = 6;
        rb2d.mass = 20;
    }
    #endregion
}
