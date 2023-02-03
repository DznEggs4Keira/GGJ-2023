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
    [SerializeField] private float currentHealth = 100;
    [SerializeField] private int maxHealth = 100;

    //Healthbar
    public Healthbar _Healthbar;

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
    }

    // Update is called once per frame
    void Update()
    {
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
        Attack();

        // handle death
        CheckDeath();
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
        AudioManager.instance.HandleFootsteps("Tate Footsteps", currentMovement.sqrMagnitude <= 0.01 ? false : true);
    }

    void Attack() {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) {
            // Do attack ...

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPoint.position, hitRange, enemyLayers);

            // you collide with nothing, then play miss sound
            if(hitEnemies != null) {

                foreach (var enemy in hitEnemies) {
                    // can only attack the trap if rooted
                    if (enemy.gameObject.layer == 7) {
                        if (!isRooted) {
                            continue;
                        }
                    }

                    var hit = enemy.transform.GetComponent<EnemyController>();

                    if (hit != null) {
                        hit.EnemyHealth -= 10;

                        //Play Attack Sound
                        AudioManager.instance.Play("Player Attack", true);
                    }

                    //if we destroyed a spawner, track that so that the player can finish game
                    if(enemy.gameObject.layer == 8) {
                        GameManager.instance.CurrentBossEnemiesKilled++;
                    }
                }
            } else {
                //Play Missing Sound
                AudioManager.instance.Play("Player Miss", true);
            }

            
        }
    }

    void CheckDeath() {

        if(currentHealth <= 0) {
            // Die ...
            dead = true;

            //Update Dead Count
            GameManager.instance.CurrentTries++;

            //Play Dying Sound
            AudioManager.instance.Play("Player Death", true);

            // Call Respawn Coroutine
            StartCoroutine(Respawn());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 7) {
            // TRAP - rooted
            isRooted = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        // if colliding with the enemy - lose Health

        delayTimer += Time.deltaTime;

        if(delayTimer >= recieveHitDelay) {
            if (collision.gameObject.layer == 6) {
                // MYCELIUM - lose 5 health
                currentHealth -= 0.1f;
            }
        }
        
        _Healthbar.Sethealth((int)currentHealth);
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

    IEnumerator Respawn(float delay = 1) {

        yield return new WaitForSeconds(delay);

        dead = false;
        // teleport to respawn position
        transform.position = respawnPoint;
        // set health back to max
        currentHealth = maxHealth;
    }

}
