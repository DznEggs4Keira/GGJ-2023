using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D player_rb;
    [SerializeField]
    private SpriteRenderer player_sr;
    [SerializeField]
    private float player_speed = 5;
    [SerializeField]
    private float player_force = 10;


    // Start is called before the first frame update
    void Start()
    {
        player_rb = gameObject.GetComponent<Rigidbody2D>();
        player_sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        CheckPlayerJump();
    }

    void MovePlayer() {
        var horizontalAxis = Input.GetAxis("Horizontal");

        player_sr.flipX = horizontalAxis > 0 ? false : true;

        player_rb.velocity = new Vector2(horizontalAxis * player_speed, player_rb.velocity.y);
    }

    void CheckPlayerJump() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            player_rb.AddForce(Vector2.up * player_force, ForceMode2D.Impulse);
        }
    }
}
