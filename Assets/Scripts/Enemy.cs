using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Animator _animator;
    private Rigidbody2D rb;

    [SerializeField] private float speed = 1;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float checkBigRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float checkAggRadius;

    private int direction = 1;
    private bool facingRight = true;
    private int timer = 0;
    private bool destroyed = false;

    private bool IsGrounded => Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    private bool IsVeryGrounded => Physics2D.OverlapCircle(groundCheck.position, checkBigRadius, whatIsGround);
    private bool IsPlayer => Physics2D.OverlapCircle(groundCheck.position, checkAggRadius, whatIsPlayer);

    private void Start()
    {
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyed) return;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        _animator.SetFloat("Speed", speed);

        if(IsGrounded != IsVeryGrounded)
        {
            
            Flip();
            checkBigRadius = checkRadius;
            timer = 0;
        }
        if (IsPlayer)
        {
            //speed = 4;
            var player = GameObject.FindGameObjectWithTag("Player");
            var enemyPosition = transform.position;
            var playerPosition = player.transform.position;
            if (IsGrounded)
            {
                if (enemyPosition.x < playerPosition.x) // player on the right side
                {
                    if (!facingRight)
                    {
                        Flip();
                    }
                }
                else
                {
                    if (facingRight)
                    {
                        Flip();
                    }
                }
            }
        }
        else speed = 2;
        timer++;
        if(timer >= 10)
        {
            checkBigRadius = 1;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight; // !true = false; !false = true
        var scale = transform.localScale;
        scale.x *= -1; // scale.x = scale.x * -1;
        direction *= -1;
        transform.localScale = scale;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            destroyed = true;
            rb.velocity = Vector2.zero;
            var collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
            _animator.SetFloat("Speed", 0);
            Destroy(gameObject, 3);
            //gameObject.SetActive(false);
        }
    }
}