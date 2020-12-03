using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONTROLLER : MonoBehaviour
{

	public float speed;
	public float jumpForce;
	private float moveInput;

	private Rigidbody2D rb;

	private bool facingRight = true;


	private bool isGrounded;
	public Transform groundCheck;
	public float checkRadius;
	public LayerMask whatIsGround;

	private int extraJumps;
	public int extraJumpsValue;

    private bool crouch = false;
    public Collider2D standingCollider;

    public float hangTime = .2f;
    private float hangCounter = 0f;
    private float airTimeAnimation = 0.5f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        extraJumps = extraJumpsValue;
    }

    // Update is called once per frame
    void Update()
    {

    	if(isGrounded)
    	{
    		extraJumps = extraJumpsValue;
            hangCounter = hangTime;
            airTimeAnimation = 0.5f;

    	} else
        {
            hangCounter -= Time.deltaTime;
            airTimeAnimation -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump"))
        {
            airTimeAnimation = 0.5f;

            animator.SetBool("isJumping", true);
            animator.SetBool("isLanding", false);

        	if(hangCounter > 0)
        	{
        		rb.velocity = Vector2.up * jumpForce;
        		
        	} else if(extraJumps > 0)
        	{
        		rb.velocity = Vector2.up * jumpForce;
                extraJumps --;
        	}
        } else if(Input.GetButtonUp("Jump") && rb.velocity.y > 0)
    	{
    		rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

    	}

        if(airTimeAnimation == 0.5f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isLanding", false);

        } else if(airTimeAnimation <= 0.5f && airTimeAnimation >= 0.0f && !isGrounded)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isLanding", false);
        } else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isLanding", true);
        }



        if(Input.GetButtonDown("Crouch"))
        {
            crouch = true;

        } else if(Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
    }

    void FixedUpdate()
    {
    	isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(!facingRight && moveInput > 0)
        {
        	Flip();
        } else if(facingRight && moveInput < 0)
        {
        	Flip();
        }

        if(isGrounded)
        {
            if(crouch)
            {
                standingCollider.enabled = false;
            } else if(!crouch)
            {
                standingCollider.enabled = true;
            }
        } else
        {
            standingCollider.enabled = true;
        }
    }

    void Flip()
    {
    	facingRight = !facingRight;
    	Vector3 Scaler = transform.localScale;
    	Scaler.x *= -1;
    	transform.localScale = Scaler;
    }
}
