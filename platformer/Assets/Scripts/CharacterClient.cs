using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterClient : MonoBehaviour
{
    public DataScenes.CharacterInfomation chrctrInfomation;


    //параметры
    public static bool isLife=true;
    public static int life = 3;
	public float groundRadius;
    public float distance;
    
    //компоненты
	public Rigidbody2D playerRigidbody;
	public Animator charAnimator;
	private SpriteRenderer sprite;
	private Transform groundCheck;
    public BoxCollider2D characterCollider;

    //анимация
	private bool idle = false;
	private bool run = false;
 	private bool doubleJump = false;
    private bool wallStick = false;
    private bool wallJump = false;
    public bool hit = false;
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
        characterCollider = GetComponent<BoxCollider2D>();
		Transform[] tempTransform = GetComponentsInChildren<Transform>();
		groundCheck= tempTransform[2];
        Physics2D.queriesStartInColliders = false;
        playerMask = LayerMask.NameToLayer("Player");
        colliderMask = LayerMask.NameToLayer("Collider");
        colliderLayerMask = LayerMask.GetMask("Collider");
        groundLayerMask = LayerMask.GetMask("Ground");

        isLife = true;
    }
	
    void ReadStruct()
    {
        transform.position = chrctrInfomation.position;
        playerRigidbody.velocity = chrctrInfomation.velocity;
        doubleJump = chrctrInfomation.doubleJump;
        run = chrctrInfomation.run;
        wallStick = chrctrInfomation.wallStick;
        wallJump = chrctrInfomation.wallJump;
        sprite.flipX = chrctrInfomation.flipX;
    }
 
	// Update is called once per frame
    void Update()
    {
        ReadStruct();

        if (!wallStick && !wallJump && doubleJump)
	    {
            charAnimator.SetTrigger("DoubleJump");
        }

        if (onGround)
        {
            doubleJump = false;
        }

        hit = charAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "hit";
        if (!run && onGround && !hit && !wallStick && !wallJump)
	    {
		    idle=true;							
	    }else
	    {
		    idle=false;
	    }
        if (wallStick || doubleJump || hit || onGround || run)
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
    }		
  
    void FixedUpdate()
    {

        if (sprite.flipX == true)
        {
            wallRay = Physics2D.Raycast(transform.position, -transform.right, distance, groundLayerMask);
            Debug.DrawRay(transform.position, -transform.right * distance, Color.red);
        }
        else
        {
            wallRay = Physics2D.Raycast(transform.position, transform.right, distance, groundLayerMask);
            Debug.DrawRay(transform.position, transform.right * distance, Color.yellow);
        }

        
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, colliderLayerMask) || Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayerMask);
    }

}
