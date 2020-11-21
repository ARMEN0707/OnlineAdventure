using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Character : MonoBehaviour
{
    public DataScenes.CharacterInfomation chrctrInfomation;

    //параметры
    public static bool isLife=true;
    public static int life;
    public float speed;
	public float jumpForce;
	public float groundRadius;
    public float distance;

    private float dir = 0;

    //компоненты
    public Rigidbody2D playerRigidbody;
	public Animator charAnimator;
	private SpriteRenderer sprite;
	private Transform groundCheck;
	public BoxCollider2D characterCollider;
    private ParticleSystem smoke;

    //анимация
    private bool idle = false;
	private bool run = false;
 	private bool doubleJump = false;
	private bool wallStick = false;
    private bool wallJump = false;
    private bool hit = false;
	public bool onGround = false;


    private RaycastHit2D wallRay;

	private int colliderLayerMask, groundLayerMask;
    private int playerMask, colliderMask;

    // Start is called before the first frame update
    
	void Start()    
	{ 
		playerRigidbody = GetComponent<Rigidbody2D>();
		charAnimator = GetComponentInChildren<Animator>();
		sprite = GetComponentInChildren<SpriteRenderer>();
        Transform[] tempTransform = GetComponentsInChildren<Transform>();
        characterCollider = GetComponent<BoxCollider2D>();
		groundCheck= tempTransform[2];
        playerMask = LayerMask.NameToLayer("Player");
        colliderMask = LayerMask.NameToLayer("Collider");
        colliderLayerMask = LayerMask.GetMask("Collider");
        groundLayerMask = LayerMask.GetMask("Ground");
        smoke = gameObject.GetComponentInChildren<ParticleSystem>();
        smoke.Stop();

        isLife = true;
        life = 3;
        Physics2D.IgnoreLayerCollision(playerMask, playerMask, true);
    }
      

	void Move()
	{
        Vector3 tempVector = Vector3.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position+tempVector, speed*Time.deltaTime);
		if(tempVector.x<0)
		{
			sprite.flipX = true;
		}
		else
		{
			sprite.flipX = false; 
		}
	}
	void Jump(float x, float y)
	{
       playerRigidbody.velocity = new Vector2(x,y);
	}	
	
    void WriteStruct()
    {
        chrctrInfomation.position = transform.position;
        chrctrInfomation.velocity = playerRigidbody.velocity;
        chrctrInfomation.doubleJump = doubleJump;
        chrctrInfomation.run = run;
        chrctrInfomation.wallStick = wallStick;
        chrctrInfomation.wallJump = wallJump;
        chrctrInfomation.flipX = sprite.flipX;
    }
 
	// Update is called once per frame
    void Update()
    {
        
        if (wallRay.collider != null && Input.GetButton("Horizontal"))
        {            
            if (Input.GetButtonDown("Jump"))
            {
                wallJump = true;
                if(sprite.flipX == true)
                {
                    dir = 1.0f;
                }else
                {
                    dir = -1.0f;
                }
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x + dir, playerRigidbody.velocity.y + 3.0f);
            }
            if(!wallJump)
            {
                playerRigidbody.velocity =new Vector2(playerRigidbody.velocity.x, -0.25f);
                wallStick = true;
                doubleJump = false;
            }

        }
        else
        {            
            wallStick = false;
        }
        if (playerRigidbody.velocity.y <= 0.0f && wallJump)
        {
            wallJump = false;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x - dir, playerRigidbody.velocity.y);
        }
               

        if (!onGround && !wallJump && !wallStick && !doubleJump && Input.GetButtonDown("Jump") && !DataScenes.finish)
        {
            Jump(0, jumpForce);
            doubleJump = true;
            charAnimator.SetTrigger("DoubleJump");
        }

        if (onGround)
        {
            doubleJump = false;
        }

        if (onGround && !wallJump && !wallStick &&(Input.GetButtonDown("Jump")) && !DataScenes.finish)
        {
            Jump(0, jumpForce);
        }

        hit = charAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "hit";
        if (!run && onGround && !wallStick && !hit)
	    {
		    idle=true;							
	    }else
	    {
		    idle=false;
	    }
        if (doubleJump || hit || onGround || run)
        {
            charAnimator.SetFloat("vSpeed", 0.0f);
        }
        else
        {
            charAnimator.SetFloat("vSpeed", playerRigidbody.velocity.y);
        }
        charAnimator.SetBool("Idle", idle);
        charAnimator.SetBool("Run", run);
        charAnimator.SetBool("WallStick", wallStick);      

        
        if (!isLife && !hit && !characterCollider.isTrigger)
        {
            characterCollider.isTrigger = true;
            charAnimator.SetTrigger("Hit");
        }

        WriteStruct();
    }		
  
    void FixedUpdate()
    {
        //Луч для столкновения со стеной
        if (sprite.flipX == true)
        {
            wallRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.01f), -transform.right, distance, groundLayerMask);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.01f), -transform.right * distance, Color.red);
        }
        else
        {
            wallRay = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.01f), transform.right, distance, groundLayerMask);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.01f), transform.right * distance, Color.yellow);
        }


        onGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayerMask) || Physics2D.OverlapCircle(groundCheck.position, groundRadius, colliderLayerMask);

        //Игнорирование слоёв
        if (!onGround)
        {
            Physics2D.IgnoreLayerCollision(playerMask, colliderMask, true);
        }
        else if (onGround)
        {
            Physics2D.IgnoreLayerCollision(playerMask, colliderMask, false);
        }


        //бег
        if (Input.GetButton("Horizontal") && !DataScenes.finish)
        {
            Move();
            if (onGround  && wallRay.collider == null)
            {
                run = true;
                smoke.Play();
            }
            else
            {
                run = false;
                smoke.Stop();
            }
        }
        else
        {
            run = false;
            smoke.Stop();
        }
    }
}
