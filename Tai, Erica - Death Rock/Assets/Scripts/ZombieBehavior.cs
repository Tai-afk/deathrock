using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour
{
    
    public float speed;
    private float waitTime;
    public float startWaitTime;

    public Transform[] moveSpots;
    private int randomSpot;

    public float range;
    public float maxDistance;

    private Transform target;
    Vector2 wayPoint;

    public GameObject human;
    //Make this prefab have already instantiated bool converted to true so converted zombie 
    //can not pursue human but just aimlessly wander
    public GameObject convertedHuman;

    //If the zombie was a converted human
    public bool converted = false;
    //If the player messes up set this chase to true
    public bool chase = false;
    public bool aggro = false;
    public bool debug;
    //Zombie Manager object reference
    GameObject zm;

    //FIX BUG TO DELETE NEWLY CONVERTED PREFAB
    private GameObject newInstance;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(convertedHuman, new Vector3(0, 0, 0), Quaternion.identity);
        //zm = GameObject.Find("ZombieManager");
        SetNewDestination();

        //For random movement
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);

        zm = GameObject.Find("ZombieManager");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //TODO fix zombie behavior tree
        //Debug purposes
        if (debug)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(ExplodeAnimation());
            }
            /*chase = true;
            ChaseHuman();
            Wander();*/
        }
        else if (converted)
        {
            Wander();
        }
        else
        {
            //Zombie is in a calm state 
            if(!aggro && !chase)
            {
                Idle();
            }
            //First logic, if zombie is aggro then just wander "aimlessly"
            else if(!chase && aggro)
            {
                BetterWander();
            }
            //If player messes up a certain amount of time, zombies gets aggro and chases
            else
            {
                ChaseHuman();
                BetterWander();
            }
        }
    }
    void Idle()
    {
        anim.SetBool("isWalking", false);
    }
    //Wander aimlessly within range
    void Wander()
    {
        anim.SetBool("isWalking", true);
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }
    }
    void BetterWander()
    {
        anim.SetBool("isWalking", true);
        //anim.SetBool("isAggro", false);
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, moveSpots[randomSpot].position) < .2f)
        {
            if(waitTime <= 0)
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
    //Chase a human and then convert them.
    void ChaseHuman()
    {
        //Place animation triggers here where Zombie is transitioning to state to attack,
        //then attack the npcs
        anim.SetBool("isAggro", true);
        transform.position = Vector2.MoveTowards(transform.position, target.position, (5 * speed * Time.deltaTime));
    }
    void Convert(GameObject human, GameObject convertedHuman)
    {
        try
        {
            newInstance = Instantiate(convertedHuman, human.transform.position, Quaternion.identity);
            converted = false;
            chase = false;
            Destroy(human);
        }
        catch(MissingReferenceException e)
        {
            print(e.StackTrace);
        }
        zm.GetComponent<ZombieManager>().AddZombie(newInstance);
        anim.SetBool("isAggro", false);
    }
    public void ReConversion(GameObject zombie, GameObject human)
    {
        Instantiate(human, zombie.transform.position, Quaternion.identity);
        Destroy(zombie);
    }
    public void Kill()
    {
        StartCoroutine(ExplodeAnimation());
    }
    public bool GetChase()
    {
        return chase;
    }
    //Zombie is chasing a random human
    public void SetChase(bool b, GameObject human)
    {
        target = human.GetComponent<Transform>();
        this.human = human;
        chase = b;
    }
    public void SetAggro(bool b)
    {
        aggro = b;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        //Change human into zombie, once they've converted finish
        if (coll.gameObject == human && chase)
        {
            anim.SetTrigger("attack");
            StartCoroutine(Conversion());
        }
    }
    //Animation of human to be played out until finished
    IEnumerator Conversion()
    {
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(1.5f);
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        //my code here after 3 seconds
        Convert(human, convertedHuman);
    }
    IEnumerator ExplodeAnimation()
    {
        anim.SetTrigger("explode");
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(.4f);
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        //zm.GetComponent<ZombieManager>().KillZombie();
        if (gameObject)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
            
        //my code here after 3 seconds
    }

}
