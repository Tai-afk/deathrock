using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Activator : MonoBehaviour
{
    SpriteRenderer sr;
    public KeyCode key;
    bool active = false;
    bool win = false;
    //Gamemanager and zombie manager
    GameObject note, gm, zm;

    //Notes for creation
    public GameObject n;

    //Particle effects
    public GameObject effectOk, effectGood, effectPerfect;
    
    //Text effects
    [SerializeField] private GameObject floatingTextPrefab;

    //Audio effects
    public AudioSource bad, okay, good, perf, zombie;
    
    Color old;

    //Modes for debug (and tutorial) and creation
    public bool createMode;
    public bool debugMode;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        old = sr.color;
        gm = GameObject.Find("GameManager");
        zm = GameObject.Find("ZombieManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (win)
        {
            gm.GetComponent<GameManager>().Win();
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        if (createMode)
        {
            if (Input.GetKeyDown(key))
            {
                Instantiate(n, transform.position, Quaternion.identity);
            }
        }
        else if (debugMode)
        {
            if (Input.GetKeyDown(key))
            {
                StartCoroutine(Pressed());
            }
            if (Input.GetKeyDown(key) && active)
            {
                //Add and play in particle effects here
                GameObject effectOk_ = Instantiate(effectOk, transform.position, Quaternion.identity);
                GameObject effectGood_ = Instantiate(effectGood, transform.position, Quaternion.identity);
                GameObject effectPerfect_ = Instantiate(effectPerfect, transform.position, Quaternion.identity);
                //Play Sound effect here

                //Handles scoring system
                if (Mathf.Abs(note.gameObject.transform.position.y - transform.position.y) > 0.3f)
                {
                    effectOk_.GetComponent<ParticleSystem>().Play();
                    ShowText("Okay", 0);
                    Debug.Log("Okay");
                }
                else if (Mathf.Abs(note.gameObject.transform.position.y - transform.position.y) > 0.25f)
                {
                    effectGood_.GetComponent<ParticleSystem>().Play();
                    ShowText("Good", 1);
                    Debug.Log("Good");
                }
                else
                {
                    effectPerfect_.GetComponent<ParticleSystem>().Play();
                    ShowText("Perfect", 2);
                    Debug.Log("Perfect");
                }
                Destroy(note);
                //Streak system and add score
                gm.GetComponent<GameManager>().AddStreak();
                AddScore();
                active = false;
            }
            else if (Input.GetKeyDown(key) && !active)
            {
                ShowText("BAD!", 3);
                gm.GetComponent<GameManager>().ResetStreak();
            }
        }
        else
        {
            if(PlayerPrefs.GetInt("Streak") % 10 == 0 && PlayerPrefs.GetInt("Streak") != 0)
            {
                zm.GetComponent<ZombieManager>().SetZombiesToDie(true);
                gm.GetComponent<GameManager>().KillThatZombie();
            }
            if (Input.GetKeyDown(key))
            {
                StartCoroutine(Pressed());
            }
            if (Input.GetKeyDown(key) && active)
            {
                //Add and play in particle effects here
                GameObject effectOk_ = Instantiate(effectOk, transform.position, Quaternion.identity);
                GameObject effectGood_ = Instantiate(effectGood, transform.position, Quaternion.identity);
                GameObject effectPerfect_ = Instantiate(effectPerfect, transform.position, Quaternion.identity);
                //Play Sound effect here

                //Handles scoring system
                if (Mathf.Abs(note.gameObject.transform.position.y - transform.position.y) > 0.3f)
                {
                    if (okay) okay.Play();
                    effectOk_.GetComponent<ParticleSystem>().Play();
                    ShowText("Okay", 0);
                    Debug.Log("Okay");
                }
                else if (Mathf.Abs(note.gameObject.transform.position.y - transform.position.y) > 0.25f)
                {
                    if (good) good.Play();
                    effectGood_.GetComponent<ParticleSystem>().Play();
                    ShowText("Good", 1);
                    Debug.Log("Good");
                }
                else
                {
                    if (perf) perf.Play();
                    effectPerfect_.GetComponent<ParticleSystem>().Play();
                    ShowText("Perfect", 2);
                    Debug.Log("Perfect");
                }
                Destroy(note);
                //Streak system and add score
                gm.GetComponent<GameManager>().AddStreak();
                AddScore();
                active = false;
            }
            else if (Input.GetKeyDown(key) && !active)
            {
                if(bad) bad.Play();
                ShowText("BAD!", 3);
                gm.GetComponent<GameManager>().SetMessUps(gm.GetComponent<GameManager>().GetMessUps() + 1);
                //If Player messes up 3 times, the zombie will chase a human
                //zm.GetComponent<ZombieManager>().setNumFuckUps(zm.GetComponent<ZombieManager>().getNumFuckUps()+1);
                if (gm.GetComponent<GameManager>().GetMessUps() % 5 == 0)
                {
                    if (zombie) zombie.Play();
                    zm.GetComponent<ZombieManager>().SetZombieToChase();
                }
                gm.GetComponent<GameManager>().ResetStreak();
            }
        }

    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("WinNote"))
        {
            Debug.Log("here");
            win = true;
        }
        if (coll.gameObject.CompareTag("Note"))
        {
            note = coll.gameObject;
            active = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        active = false;
    }
    void AddScore()
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + gm.GetComponent<GameManager>().GetScore());
    }

    void ShowText(string text, int i)
    {
        //i = 2 means perfect
        //i = 1 means good
        //i = 0 means ok
        //i == 3 means bad
        if (i == 2)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
            prefab.GetComponentInChildren<TextMesh>().color = Color.green;
            //Destroy(prefab.GetComponent<GameObject>().gameObject);
        }
        if (i == 1)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
            prefab.GetComponentInChildren<TextMesh>().color = Color.yellow;
            //Destroy(prefab.GetComponent<GameObject>().gameObject);
        }
        if (i == 0)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
            Color orange = new Color(255, 165, 0);
            prefab.GetComponentInChildren<TextMesh>().color = orange;
            //Destroy(prefab.GetComponent<GameObject>().gameObject);
        }
        if(i == 3)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
            prefab.GetComponentInChildren<TextMesh>().color = Color.red;
        }
    }
    
        IEnumerator Pressed()
    {
        sr.color = new Color(0, 0, 0);
        yield return new WaitForSeconds(0.05f);
        sr.color = old;
    }
    
}
