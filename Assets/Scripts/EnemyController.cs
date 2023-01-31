using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Player Information")]
    [SerializeField] private Transform player;

    [Header("Simulated Physics Settings")]
    [SerializeField] private Rigidbody2D enemy_rb;
    [SerializeField] private float enemy_speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        // get player position
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // get enemy rigidbody
        enemy_rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        // move the enemy towards the player every frame
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemy_speed * Time.fixedDeltaTime);
    }
}
