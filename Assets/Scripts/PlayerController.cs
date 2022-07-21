using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    /*[SerializeField] private int extraJumps;

    */[Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Death Event")]
    public UnityEvent deathEvent;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
 
    private SpriteRenderer spriteRenderer;

    public ProjectilePlayer ProjectilePrefab;
    public Transform LaunchOffset;
    
    public int playerHealth = 3;
    public bool HasKey = false;
    public bool HasBlitz = false;
    public bool facingRight = true;/*
    private int coinCount = 0;
    private int jumpCount = 0;

   */ private bool IsGrounded => Physics2D.OverlapCircle(groundCheck.position,
        checkRadius, whatIsGround);

    // Start is called before the first frame update
     void Start()
    {
        //jumpCount = extraJumps;#
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var moveInput = Input.GetAxis("Horizontal");
        transform.position += new Vector3(moveInput, 0, 0) * speed * Time.deltaTime;
        //Debug.Log(moveInput);

        var rb = GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        //if(!Mathf.Approximately(0, moveInput))
          //  transform.rotation = moveInput > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        /*if(Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }*/

        // play run animation
        
        if(moveInput != 0)
            _animator.SetFloat("Speed", speed);
        else
            _animator.SetFloat("Speed", 0);


        // player flip
        if (!facingRight && moveInput > 0) // move to right
        {
            Flip();
        }
        else if (facingRight && moveInput < 0) // move to left
        {
            Flip();
        }

        if(Input.GetButtonDown("Fire1"))
        {
            if(HasBlitz)
            {
                Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            } 
        }

        /* if (IsGrounded)
          {
              jumpCount = extraJumps;
          }*/


        // player jump
        
        var jump = Input.GetButtonDown("Jump");
        if (jump && IsGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
        if (!IsGrounded)
        {
            _animator.SetBool("Grounded", IsGrounded);
        }
        if (IsGrounded)
        {
            _animator.SetBool("Grounded", IsGrounded);
        }
    }
    private void Flip()
    {
        facingRight = !facingRight; // !true = false; !false = true
        var scale = transform.localScale;
        scale.x *= -1; // scale.x = scale.x * -1;

        transform.localScale = scale;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("is triggered");
        if(other.tag == "deadly")
        {
            Debug.Log("Dead!");
            Application.LoadLevel(Application.loadedLevel);
        }
        else if (other.tag == "nextlevel")
        {
            Debug.Log("Next Level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (other.tag == "Key")
        {
            HasKey = true;
            other.gameObject.SetActive(false);
            Debug.Log("gone");
        }
        else if (other.tag == "Blitz")
        {
            HasBlitz = true;
            other.gameObject.SetActive(false);
            Debug.Log("gone");
        }
        else if (other.tag == "Enemy")
        {
            playerHealth--;
            Debug.Log("-1 Life");
        }
        deathEvent.Invoke();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            playerHealth--;
            Debug.Log("-1 Life");
        }
    }

        /* private void OnTriggerEnter2D(Collider2D other)
         {
             if (other.CompareTag("Coin") == false)
                 return;

             coinCount++; // coinCount = coinCount + 1;
             Debug.Log($"Coin Count = {coinCount}");

             other.gameObject.SetActive(false);
         }*/
    }