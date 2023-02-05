using System;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [Header("Player Information")]
    [SerializeField] private Transform player;

    [Header("Speed Settings")]
    [SerializeField] private float enemy_speed = 0.8f;

    [Header("Health Settings")]
    [SerializeField] private float currentHealth = 10;
    [SerializeField] private int maxHealth = 10;
    private Healthbar healthBar;

    [Header("Set Enemy Type")]
    public bool isMushroom;

    public float EnemyHealth{
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    private Action<EnemyController> killAction;

    // Start is called before the first frame update
    void Start()
    {
        // get player position
        player = GameObject.FindGameObjectWithTag("Player").transform;

        EnemyHealth = maxHealth;

        healthBar = gameObject.GetComponentInChildren<Healthbar>();
    }

    public void Init(Action<EnemyController> enemyToKill) {
        killAction = enemyToKill;
    }

    private void Update() {

        if(isMushroom) {
            // move the enemy towards the player every frame
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemy_speed * Time.deltaTime);

            // play footsteps of shrooms
            AudioManager.instance.ReccuringPlay("Shroom Move", true);
        }

        CheckDeath();
    }

    private void CheckDeath() {
        if (currentHealth <= 0) {

            // Enemy must die
            if(isMushroom) {
                killAction(this);
            } else {
                // play myce dies sound
                AudioManager.instance.Play("Myce Dies", true);
                Destroy(gameObject);
            }
            
        }
    }

    private void OnEnable() {
        EnemyHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        // if this enemy is a trap
        if(gameObject.layer == 7 || gameObject.tag == "TrapCenter") {
            if (collision.gameObject.tag == "PlayerFeet") {
                // TRAP - rooted
                player.GetComponent<PlayerController>().Rooted = true;

                // Play stuck sound
                AudioManager.instance.Play("Tate Stuck", true);
            }
        }
    }


}
