using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float runSpeed, jumpForce;
    [Range(0, 1)]
    public float jumpHeight;
    private float moveInput;


    private Rigidbody2D myBody;
    private Animator anim;

    private bool facingRight = true;
    

    public Vector3 range, rangeHide, rangeLight, rangeInteract;
    public Transform groundCheck, hidingCheck, lightCheck, interactCheck;
    public LayerMask groundLayer, hidingLayer, lightLayer, interactLayer;
    public string interactTag;
    

    public AudioClip[] footStepClips;
    public AudioClip jumpClip;

   
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
       
    }

    private void Update()
    {
        CheckCollisionForJump();
        CheckCollisionForHiding();
        CheckCollisionWithLight();
       

    }

    void FixedUpdate()
    {
        Movement();
        CheckCollisionWithInteractableObject();

    }

    void Movement()
    {
        moveInput = Input.GetAxisRaw("Horizontal") * runSpeed;
        
        if (anim.GetBool("SwordAttack")) moveInput = 0;
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        myBody.velocity = new Vector2(moveInput, myBody.velocity.y);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (myBody.velocity.y > 0)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y * jumpHeight);
            }
        }
        if (moveInput > 0 && !facingRight || moveInput < 0 && facingRight)
        {
            Flip();
        }

        if (myBody.velocity.y < 0)
            anim.SetBool("Fall", true);
        else anim.SetBool("Fall", false);
    }

    void CheckCollisionForJump()
    {
        Collider2D bottomHit = Physics2D.OverlapBox(groundCheck.position, range, 0, groundLayer);

        if(bottomHit != null)
        {
            if(bottomHit.gameObject.tag == "Ground"  && Input.GetKeyDown(KeyCode.Space))
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpForce);
                SoundManager.instance.PlaySoundFx(jumpClip, Random.Range(.1f, .2f));
                anim.SetBool("Jump", true);
            }
            else
            {
                anim.SetBool("Jump", false);
            }
        }
    }

    void CheckCollisionForHiding()
    {
        Collider2D hitPlaceToHide = Physics2D.OverlapBox(hidingCheck.position, rangeHide, 0, hidingLayer);

        if(hitPlaceToHide != null)
        {
            if(hitPlaceToHide.gameObject.tag == "CanHideIn" && Input.GetKey(KeyCode.Q))
            {
              this.gameObject.tag = "PlayerHidden";
                               
            }
            else
            {
               this.gameObject.tag = "Player";
                
            }
        }
       
    }


    void CheckCollisionWithLight()
    {
        Collider2D hitLight = Physics2D.OverlapBox(lightCheck.position, rangeLight, 0, lightLayer);

        if (hitLight != null)
        {
            if (hitLight.gameObject.tag == "Light")
            {
                this.gameObject.tag = "PlayerOnLight";

            }
            else
            {
                this.gameObject.tag = "Player";
            }
        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector2 transformScale = transform.localScale;
        transformScale.x *= -1;
        transform.localScale = transformScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(groundCheck.position, range);
        Gizmos.DrawWireCube(hidingCheck.position, rangeHide);
        Gizmos.DrawWireCube(lightCheck.position, rangeLight);
    }

    void RunningSound()
    {
        SoundManager.instance.PlayRandomSOundFx(footStepClips);
    }

    void CheckCollisionWithInteractableObject()
    {
        Collider2D hitInteract = Physics2D.OverlapBox(interactCheck.position, rangeInteract, 0, interactLayer);
        if (hitInteract != null)
        {
            if (hitInteract.gameObject.tag == interactTag && Input.GetKeyDown(KeyCode.F))
            {
                GameObject.FindGameObjectWithTag(interactTag).tag = "WasInteracted";
               
            }
                       
        }
        
        
    }
}
