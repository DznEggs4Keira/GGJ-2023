using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    /*Player Controls are as follows
     * Player movement - All Directions
     * Player Attack
     * Player Die
     * Player Health
     */

    [Header("Health Settings")]
    [SerializeField] private float currentHealth;
    [SerializeField] private int maxHealth = 100;
    public Healthbar healthBar;

    public float PlayerHealth {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    [Header("Hit Settings")]
    [SerializeField] private Transform hitPoint;
    [SerializeField] private float hitRange = 1.2f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float recieveHitDelay = 1.5f;
    

    [Header("Simulated Physics Settings")]
    [SerializeField] private Rigidbody2D player_rb;
    [SerializeField] private float player_speed = 5;

    [Header("Visual Component Settings")]
    [SerializeField] private SpriteRenderer player_sr;
    [SerializeField] private Animator player_animator;

    [Header("Respawn Point")]
    [SerializeField] private Vector2 respawnPoint;

    private Vector2 currentMovement;
    private float delayTimer = 0;
    private bool dead = false;
    private bool isRooted = false;

    public bool Rooted {
        get { return isRooted; }
        set { isRooted = value; }
    }

    // Awake is called before the first frame
    void Awake()
    {
        player_rb = gameObject.GetComponent<Rigidbody2D>();
        player_sr = gameObject.GetComponent<SpriteRenderer>();
        player_animator = gameObject.GetComponent<Animator>();
    }

    // Start is called on the first frame
    private void Start() {
        respawnPoint = transform.position;

        PlayerHealth = maxHealth;
        healthBar.SetMaxHealth(PlayerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0) {
            return;
        }

        // check movement input
        currentMovement.x = Input.GetAxisRaw("Horizontal");
        currentMovement.y = Input.GetAxisRaw("Vertical");

        // update the animation controller with current input
        // direction
        player_animator.SetFloat("Horizontal", currentMovement.x);
        player_animator.SetFloat("Vertical", currentMovement.y);
        // speed in order to switch between idle and moving
        player_animator.SetFloat("Speed", currentMovement.SqrMagnitude());

        // handle player attacking
        if (Input.GetButtonDown("Fire1")) {
            Attack();
        }

        // handle death
        CheckDeath();

        //update health
        healthBar.SetHealth(PlayerHealth);
    }

    private void FixedUpdate() {

        // handle player movement
        if (dead) {
            return;
        }

        Move();
    }

    void Move() {

        if (isRooted) {
            // stop movement
            player_rb.velocity = Vector2.zero;
            return;
        }
        //player_rb.MovePosition(player_rb.position + currentMovement * player_speed * Time.fixedDeltaTime);
        player_rb.velocity = currentMovement * player_speed;

        //Play Moving Sound
        AudioManager.instance.ReccuringPlay("Tate Footsteps", currentMovement.sqrMagnitude <= 0.01 ? false : true);
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPoint.position, hitRange);
    }

    void Attack() {

        // Do attack ...
        player_animator.SetTrigger("Attack");

        foreach (var enemy in Physics2D.OverlapCircleAll(hitPoint.position, hitRange, enemyLayers)) {

            // can only attack the trap if rooted
            if (enemy.gameObject.layer == 7) {
                if (!isRooted) {
                    //Play Missing Sound
                    AudioManager.instance.Play("Tate Misses", true);
                    continue;
                }
            }

            var hit = enemy.transform.GetComponent<EnemyController>();

            // make sure it is indeed the enemy with a script for health on it
            if (hit != null) {
                hit.EnemyHealth -= 10;

                //Play Attack Sound
                if (hit.gameObject.layer == 6) {
                    AudioManager.instance.Play("Tate Hits Shrooms", true);
                } else {
                    AudioManager.instance.Play("Tate Hits Myce", true);
                }

            }
        }
            
    }

    void CheckDeath() {

        if(currentHealth <= 0) {
            // Die ...
            dead = true;

            //Update Dead Count
            GameManager.instance.CurrentTries++;

            // Call Respawn Coroutine
            StartCoroutine(Respawn());
        }

    }

    private void OnTriggerStay2D(Collider2D collision) {
        // if colliding with the enemy - lose Health

        delayTimer += Time.deltaTime;

        if(delayTimer >= recieveHitDelay) {
            if (collision.gameObject.layer == 6) {
                // Mushrooms - lose 5 health
                currentHealth -= 0.1f;

                AudioManager.instance.ReccuringPlay("Shroom Attack", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // mushroom hits delay reset
        if (collision.gameObject.layer == 6) {
            delayTimer = 0;
        }

        // mycelium patch trap reset
        if (collision.gameObject.layer == 7) {
            isRooted = false;
        }
    }

    IEnumerator Respawn(float delay = 3f) {

        //Play Dying Sound
        //AudioManager.instance.Play("Tate Dies", true);

        yield return new WaitForSeconds(delay);

        dead = false;
        // teleport to respawn position
        transform.position = respawnPoint;
        // set health back to max
        currentHealth = maxHealth;
    }

}
