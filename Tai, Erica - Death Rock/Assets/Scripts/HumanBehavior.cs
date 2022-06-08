using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBehavior : MonoBehaviour
{
    public float speed;
    public float range;
    public float maxDistance;

    private float waitTime;
    public float startWaitTime;

    //Movespots for humans to move
    public Transform[] moveSpots;
    private int randomSpot;
    Vector2 wayPoint;

    //Debug test
    public bool debug;
    //boolean to activate the animation
    private bool animActive = false;

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        SetNewDestination();
    }

    void Update()
    {
        //Wander is dumb wandering 
        if (debug)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetBool("isDeath", true);
                
            }
            else
            {
                anim.SetBool("isIdle", true);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                anim.SetTrigger("death");
                anim.SetBool("isZombie", true);
            }
                
            
        }
        else
            BetterWander();
    }

    void Wander()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }
    }
    void BetterWander()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < .2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void SetNewDestination()
    {
        wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie") && animActive)
        {
            anim.SetTrigger("hurt");
            anim.SetBool("isDeath", true);
            anim.SetBool("isZombie", true);
        }
    }
    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(10f);
        //my code here after 3 seconds
    }
    public void SetAnimActive()
    {
        animActive = true;
    }
}
