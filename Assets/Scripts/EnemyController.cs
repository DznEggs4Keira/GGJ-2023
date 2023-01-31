using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [Header("Player Information")]
    [SerializeField] private Transform player;

    [Header("Speed Settings")]
    [SerializeField] private float enemy_speed = 0.5f;

    [Header("Health Settings")]
    [SerializeField] private float currentHealth = 10;
    [SerializeField] private int maxHealth = 10;

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
    }

    public void Init(Action<EnemyController> enemyToKill) {
        killAction = enemyToKill;
    }

    private void Update() {
        // move the enemy towards the player every frame
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemy_speed * Time.deltaTime);

        CheckDeath();
    }  

    private void CheckDeath() {
        if (currentHealth <= 0) {

            Debug.Log("Enemy Died");
            killAction(this);
        }
    }

    private void OnEnable() {
        currentHealth = maxHealth;
    }


}
