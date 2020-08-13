using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config params
    [SerializeField] float playerRunSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] int numExtraJumps = 1;
    [SerializeField] int deathKick = 8; 
    int numCurrentJumps; 
    float velocityX, velocityY;

    //State
    bool isAlive = true; 

    //Cached References
    Rigidbody2D rigidBody;
    Animator animator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeet; 

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>(); 
        numCurrentJumps = numExtraJumps; 
    }

    void Update()
    {
        if (!isAlive)
        {
            return; 
        }
        Run();
        Jump(); 
        FlipSprite();
    }

    private void Run()
    {
        velocityX = Input.GetAxisRaw("Horizontal");
        velocityY = rigidBody.velocity.y;
        Vector2 playerVelocity = new Vector2(velocityX * playerRunSpeed, velocityY);
        rigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetBool("Jumping", true); 
            if (numCurrentJumps == 0)
            {
                animator.SetBool("DoubleJumping", true); 
            }
            if (rigidBody.velocity.y < 0)
            {
                animator.SetBool("Falling", true); 
            }
        }
        if (Input.GetButtonDown("Jump") && numCurrentJumps > 0)
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            rigidBody.velocity += jumpVelocityToAdd;
            numCurrentJumps--;
        }

        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            numCurrentJumps = numExtraJumps;
            animator.SetBool("Jumping", false);
            animator.SetBool("DoubleJumping", false);
            animator.SetBool("Falling", false); 
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon; 
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x), 1f); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            if (!isAlive) { return; }
            isAlive = false;
            animator.SetTrigger("Die");
            Vector2 directionOfLaunch = new Vector2(transform.position.x, transform.position.y) - new Vector2(other.transform.position.x, other.transform.position.y);
            rigidBody.AddForce(directionOfLaunch * deathKick, ForceMode2D.Impulse);
            FindObjectOfType<GameSession>().ProccessPlayerDeath(); 
        }
    }

}
