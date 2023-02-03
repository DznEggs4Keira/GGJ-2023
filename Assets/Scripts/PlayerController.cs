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
    private bool isAttacking = false;

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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) {
            isAttacking = true;
        } else {
            isAttacking = false;
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

        if(isAttacking) {
            Attack();
        }
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

    #region Debug Circle 2D
        public static void DrawCircle(Vector3 position, float radius, int segments, Color color) {
        // If either radius or number of segments are less or equal to 0, skip drawing
        if (radius <= 0.0f || segments <= 0) {
            return;
        }

        // Single segment of the circle covers (360 / number of segments) degrees
        float angleStep = (360.0f / segments);

        // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
        // which are required by Unity's Mathf class trigonometry methods

        angleStep *= Mathf.Deg2Rad;

        // lineStart and lineEnd variables are declared outside of the following for loop
        Vector3 lineStart = Vector3.zero;
        Vector3 lineEnd = Vector3.zero;

        for (int i = 0; i < segments; i++) {
            // Line start is defined as starting angle of the current segment (i)
            lineStart.x = Mathf.Cos(angleStep * i);
            lineStart.y = Mathf.Sin(angleStep * i);

            // Line end is defined by the angle of the next segment (i+1)
            lineEnd.x = Mathf.Cos(angleStep * (i + 1));
            lineEnd.y = Mathf.Sin(angleStep * (i + 1));

            // Results are multiplied so they match the desired radius
            lineStart *= radius;
            lineEnd *= radius;

            // Results are offset by the desired position/origin 
            lineStart += position;
            lineEnd += position;

            // Points are connected using DrawLine method and using the passed color
            Debug.DrawLine(lineStart, lineEnd, color);
        }
    }
    #endregion

    void Attack() {

        //DrawCircle(hitPoint.position, hitRange, 32, Color.red);

        // Do attack ...
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPoint.position, hitRange, enemyLayers);

        // you collide with nothing, then play miss sound
        if(hitEnemies != null) {
            foreach (var enemy in hitEnemies) {
                // can only attack the trap if rooted
                if (enemy.gameObject.layer == 7) {
                    if (!isRooted) {
                        //Play Missing Sound
                        AudioManager.instance.Play("Tate Misses", true);
                        continue;
                    }
                }

                var hit = enemy.transform.GetComponent<EnemyController>();

                if (hit != null) {
                    hit.EnemyHealth -= 10;

                    //Play Attack Sound
                    AudioManager.instance.Play("Tate Attack", true);
                }

                //if we destroyed a spawner, track that so that the player can finish game
                if(enemy.gameObject.layer == 8) {
                    GameManager.instance.CurrentBossEnemiesKilled++;
                }
            }
        } else {
            //Play Missing Sound
            AudioManager.instance.Play("Tate Misses", true);
        }
    }

    void CheckDeath() {

        if(currentHealth <= 0) {
            // Die ...
            dead = true;

            //Update Dead Count
            GameManager.instance.CurrentTries++;

            //Play Dying Sound
            //AudioManager.instance.Play("Tate Die", true);

            // Call Respawn Coroutine
            StartCoroutine(Respawn());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 7) {
            // TRAP - rooted
            isRooted = true;

            // Play stuck sound
            AudioManager.instance.Play("Tate Stuck", true);
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

    IEnumerator Respawn(float delay = 1) {

        yield return new WaitForSeconds(delay);

        dead = false;
        // teleport to respawn position
        transform.position = respawnPoint;
        // set health back to max
        currentHealth = maxHealth;
    }

}
