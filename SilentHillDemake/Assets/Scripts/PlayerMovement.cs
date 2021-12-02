using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header ("Movimentação")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float climbSpeed = 10f;

    [Header("Circulo Imaginario - Escada")]
    [SerializeField] float checkRadius = 0.4f;
    [SerializeField] GameObject checkCirclePosition;

    
    //Player Input
    float inputXAxis;
    float inputYAxis;
    float inputCrouchButton;

    //other
    bool canMoveX;
    bool canAgachar;


    //cached reference
    Animator myAnimator;
    Rigidbody2D myRigidbody2D;
    int climbingLayerMask;
    int groundLayerMask;
    BoxCollider2D myFeetCollider;
    float initialGravity;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        climbingLayerMask = LayerMask.GetMask("Climbing");
        groundLayerMask = LayerMask.GetMask("Ground");
        myRigidbody2D = GetComponent<Rigidbody2D>();
        initialGravity = myRigidbody2D.gravityScale;

        canMoveX = true;
        canAgachar = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        HandleAgachar();
    }

    private void FixedUpdate()
    {
        Move();
        HandleClimb();
    }

    private void GetInput()
    {
        inputXAxis = Input.GetAxis("Horizontal");
        inputYAxis = Input.GetAxis("Vertical");
        inputCrouchButton = Input.GetAxis("Fire3");
    }

    private void Move()
    {
        if (!canMoveX) //não anda pros lado se estiver na escada
        {
            myAnimator.SetBool("isMoving", false);
            return; 
        } 

        Vector2 playerVelocity = new Vector2(inputXAxis * moveSpeed, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;
        HandleRunAnimation();
    }

    private void HandleRunAnimation()
    {

        if (Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon)
        {
            ResetClimbAnimation();
            myAnimator.SetBool("isMoving", true);
        }
        else if (myRigidbody2D.velocity.x == 0)
        {
            myAnimator.SetBool("isMoving", false);
        }



    }

    void HandleAgachar()
    {
        
        if (!canAgachar) //só agacha se estiver no chão e não estiver se movendo (definido no metodo de escalada)
        {
            myAnimator.SetBool("isAgachando", false);
            return; 
        } 
     
        
        if ((inputCrouchButton > 0))
        {
            ResetClimbAnimation();
            myAnimator.SetBool("isAgachando", true);
            canMoveX = false;
        }
        else
        {
            myAnimator.SetBool("isAgachando", false);
            canMoveX = true;
        }
    }

    void HandleClimb()
    {

        if (!myFeetCollider.IsTouchingLayers(climbingLayerMask))
        {
            myRigidbody2D.gravityScale = initialGravity;
            return;
        }
        else if (myAnimator.GetBool("isAgachando"))
        {
            return;
        }

        myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, inputYAxis * climbSpeed);
        myRigidbody2D.gravityScale = 0;

        bool onTheGround = myFeetCollider.IsTouchingLayers(groundLayerMask);
        bool isInLadder = Physics2D.OverlapCircle(checkCirclePosition.transform.position, checkRadius, climbingLayerMask);
        bool hasVerticalVelocity = Mathf.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;

        myAnimator.SetBool("isClimbing", !onTheGround);


        if (!isInLadder)
        {
            ResetClimbAnimation();
        }
        else if (!onTheGround && hasVerticalVelocity)
        {
            myRigidbody2D.velocity = new Vector2(0, inputYAxis * climbSpeed);
            myAnimator.speed = 1f;
            canMoveX = false;
            canAgachar = false;

        }
        else if (!onTheGround && !hasVerticalVelocity)
        {
            myAnimator.speed = 0f;
            canMoveX = false;
            canAgachar = false;
        }
        else
        {
            ResetClimbAnimation();
        }
    }

    private void ResetClimbAnimation()
    {
        canMoveX = true;
        canAgachar = true;
        myAnimator.SetBool("isClimbing", false);
        myAnimator.speed = 1f;
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (myRigidbody2D.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (myRigidbody2D.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkCirclePosition.transform.position, checkRadius);
    }


}
