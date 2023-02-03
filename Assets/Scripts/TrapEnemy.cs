using System;
using UnityEngine;

public class TrapEnemy : MonoBehaviour {
    [Header("Player Information")]
    [SerializeField] private Transform player;

    [Header("Health Settings")]
    [SerializeField] private float currentHealth = 10;
    [SerializeField] private int maxHealth = 10;

    public float EnemyHealth{
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // get player position
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {

        CheckDeath();
    }  

    private void CheckDeath() {
        if (currentHealth <= 0) {
            Debug.Log("Enemy Died");
        }
    }


}
