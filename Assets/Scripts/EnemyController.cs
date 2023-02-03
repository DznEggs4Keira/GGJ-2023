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
                Destroy(gameObject);
            }
            
        }
    }

    private void OnEnable() {
        EnemyHealth = maxHealth;
    }


}
