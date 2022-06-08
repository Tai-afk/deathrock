using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieManager : MonoBehaviour
{
    [Tooltip("Set zombies from the scene onto this array")]
    public List<GameObject> zombies;
    //Choose from an array of humans randomly for the zombie to choose from
    public GameObject[] humans;
    [SerializeField]
    GameObject zombiePlaceHolder, humanPlaceHolder;
    private int countZombie;
    private int countHuman;
    //Makes sure that the we can control the number of zombies converted
    private readonly HashSet<int> zombieTracker = new HashSet<int>();
    private readonly HashSet<int> humanTracker = new HashSet<int>();
    
    //Keep check of how many zombies are killed
    private int killZombieCount = 0;
    //index fix for possible bug
    int indexConvert = 0;
    public AudioSource explodeZombie;

    bool timeForZombiesToDie = false;
    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space) && timeForZombiesToDie)
        {
            explodeZombie.Play();
            KillZombie();
            timeForZombiesToDie = false;
        }
        //print(count);
        countZombie = Random.Range(0, zombies.Count);
        countHuman = Random.Range(0, humans.Length);
    }
    public void SetZombieToChase()
    {
        //int indHuman = HumanPicker();
        //int indZombie = ZombiePicker();
        zombies[indexConvert].GetComponent<ZombieBehavior>().SetChase(true, humans[indexConvert]);
        humans[indexConvert].GetComponent<HumanBehavior>().SetAnimActive();
        indexConvert++;
    }
    public bool GetZombieToChase()
    {
        return zombies[countZombie].GetComponent<ZombieBehavior>().GetChase();
    }
    public void KillZombie()
    {
        print(killZombieCount);
        if (killZombieCount != zombies.Count)
        {
            zombies[killZombieCount].GetComponent<ZombieBehavior>().Kill();
            killZombieCount++;
            //allZombiesDead = true;
        }   
        else
            print("you have no more zombies to kill!");
        
    }
    //Possible reconversion mechanic
    public void ConvertZombieBackToHuman()
    {
        //TODO fix the zombie picker logic because after zombie is reconverted back to humans, it becomes another target for the zombies
        zombies[indexConvert].GetComponent<ZombieBehavior>().ReConversion(zombiePlaceHolder, humanPlaceHolder);
    }

    private int ZombiePicker()
    {
        //Random number count, this while loop will stop when found a zombie to chsae;
        //If the zombie is already converted
        if (zombieTracker.Contains(countZombie))
        {
            while (zombieTracker.Contains(countZombie))
            {
                countZombie = Random.Range(0, zombies.Count-1);
            }
            zombieTracker.Add(countZombie);
        }
        else
        {
            zombieTracker.Add(countZombie);
        }
        return countZombie;
    }
    
    private int HumanPicker()
    {
        //If human is already converted, then don't chase that convertedhuman
        //Instead chase a new random human
        if (humanTracker.Contains(countHuman))
        {
            while (humanTracker.Contains(countHuman))
            {
                countHuman = Random.Range(0, humans.Length - 1);
            }
            humanTracker.Add(countHuman);
        }
        else
        {
            humanTracker.Add(countHuman);
        }
        return countHuman;
    }

    public void AddZombie(GameObject zombie)
    {
        zombies.Add(zombie);
    }
    public void SetZombiesToDie(bool b)
    {
        timeForZombiesToDie = b;
    }
}
