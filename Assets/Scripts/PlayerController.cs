using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    /*Player Controls are as follows
     * Player movement - All Directions
     * Player Attack
     * Player Die
     * Player Health
     */

    [Header("Health Settings")]
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;

    [Header("Simulated Physics Settings")]
    [SerializeField] private Rigidbody2D player_rb;
    [SerializeField] private float player_speed = 5;

    [Header("Visual Component Settings")]
    [SerializeField] private SpriteRenderer player_sr;
    [SerializeField] private Animator player_animator;

    private Vector2 currentMovement;

    // Awake is called before the first frame
    void Awake()
    {
        player_rb = gameObject.GetComponent<Rigidbody2D>();
        player_sr = gameObject.GetComponent<SpriteRenderer>();
        player_animator = gameObject.GetComponent<Animator>();
    }

    // Start is called on the first frame
    private void Start() {
        
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
        Move();
    }

    void Move() {
        //player_rb.MovePosition(player_rb.position + currentMovement * player_speed * Time.fixedDeltaTime);
        player_rb.velocity = currentMovement * player_speed;
    }

    void Attack() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            // Do attack ...
        }
    }

    void CheckDeath() {

        if(currentHealth <= 0) {
            // Die ...

            // Call Respawn Coroutine
            Respawn();
        }

    }

    private void OnCollisionStay2D(Collision2D collision) {
        // if colliding with the enemy - lose Health

        if (collision.gameObject.CompareTag("Enemy")) {
            // lose 5 health
            currentHealth -= 5;
        }
    }

    IEnumerator Respawn(float delay = 3) {

        yield return new WaitForSeconds(delay);

        // teleport to respawn position
        // set health back to max
        currentHealth = maxHealth;
    }

}
