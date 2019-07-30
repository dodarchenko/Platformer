using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{

    private Rigidbody2D myBody;
    [Header("Movement")]
    public float moveSpeed;
    private float minX, maxX;
    public float distance;
    public int direction;

    private bool patrol, detect;

    private Transform playerPos;
    private Animator anim;

    [Header("Attack")]
    public Transform attackPos;
    public float attackRange;
    public LayerMask playerLayer, rayCastLayer;
    public int damage;
    public float rayDistance;
    RaycastHit2D hitInfo;

    public string linkedTag;
    bool wasSpawned;
    int spawned;
    
   
    

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerPos = GameObject.Find("Assassin").transform;
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        maxX = transform.position.x + (distance / 2);
        minX = maxX - distance;
        spawned = 0;
        

        // if (Random.value > 0.5) direction = 1;
        //else direction = -1; (Vector2.Distance(transform.position, playerPos.position) <= .2f)||
    }

    void Update()
    {
                 
        if (transform.localScale.x < 0)
        {
            hitInfo = Physics2D.Raycast(transform.position, -transform.right, rayDistance, rayCastLayer);

           if ((Vector2.Distance(transform.position, playerPos.position) <= .2f) || hitInfo.collider.CompareTag("PlayerOnLight"))
            {
                patrol = false;
            }
            else
            {
                patrol = true;
            }
            
        }
        else
        {
            hitInfo = Physics2D.Raycast(transform.position, transform.right, rayDistance, rayCastLayer);
            if ((Vector2.Distance(transform.position, playerPos.position) <= .2f) || hitInfo.collider.CompareTag("PlayerOnLight"))
            {
                patrol = false;
                
            }
            else
            {
                patrol = true;
            }
        }
         
            
       
    }

    private void FixedUpdate()
    {
        if (anim.GetBool("Death"))
        {
            myBody.velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            myBody.isKinematic = true;
            anim.SetBool("Attack", false);
            return;
        }

       
        if (myBody.velocity.x > 0)
        {
            transform.localScale = new Vector2(1.3f, transform.localScale.y);
            anim.SetBool("Attack", false);
        }
        else if
            (myBody.velocity.x < 0) transform.localScale = new Vector2(-1.3f, transform.localScale.y);

        if (patrol)
        {
            detect = false;

            switch (direction)
            {
                case -1:
                    if (transform.position.x > minX)
                        myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
                    else
                        direction = 1;
                    break;
                case 1:
                    if (transform.position.x < maxX)
                    {
                        myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
                    }
                    else
                        direction = -1;
                    break;

            }
        }
        else
        {
            if (Vector2.Distance(playerPos.position, transform.position) >= 0.25f)
            {
                if (!detect)
                {
                    detect = true;
                    anim.SetTrigger("Detect");
                    myBody.velocity = new Vector2(0, myBody.velocity.y);
                }

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("SkeletonDetect")) return;

                Vector3 playerDir = (playerPos.position - transform.position).normalized;
                if (playerDir.x > 0)
                    myBody.velocity = new Vector2(moveSpeed + 0.4f, myBody.velocity.y);
                else
                    myBody.velocity = new Vector2(-(moveSpeed + 0.4f), myBody.velocity.y);
            }
            else if ((Vector2.Distance(playerPos.position, transform.position) <= 0.20f))
            {
                myBody.velocity = new Vector2(0, myBody.velocity.y);
                anim.SetBool("Attack", true);
            }
        }
    }

    public void Attack()
    {
        myBody.velocity = new Vector2(0, myBody.velocity.y);

        Collider2D attackPlayer = Physics2D.OverlapCircle(attackPos.position, attackRange, playerLayer);
        if (attackPlayer != null)
        {
            if (attackPlayer.tag == "Player")
            {
                attackPlayer.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawLine(transform.position, transform.position + -transform.right * rayDistance);
    }

    public void SpawnInAnotherPlace()
    {        
        Instantiate(gameObject, new Vector3(0.5f, -0.45f, 0), transform.rotation);
        Destroy(gameObject);
    }
}

