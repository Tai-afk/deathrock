using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int multiplier = 2;
    private int streak = 0;
    public int messUps;
    public GameObject winObject, killInstructions;
    // Start is called before the first frame update
    void Start()
    {
        winObject.SetActive(false);
        killInstructions.SetActive(false);
        messUps = 0;

        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("RockMeter", 25);
        //PlayerPrefs.SetInt("HighStreak", 0);
        PlayerPrefs.SetInt("Streak", 0);
        PlayerPrefs.SetInt("Mult", 1);
        PlayerPrefs.SetInt("Start", 1);
    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("Score") >= PlayerPrefs.GetInt("HighStreak"))
        {
            PlayerPrefs.SetInt("HighStreak", PlayerPrefs.GetInt("Score"));
        }
    }
    public void AddStreak()
    {
        if (PlayerPrefs.GetInt("RockMeter") + 1 <= 50)
        {
            PlayerPrefs.SetInt("RockMeter", PlayerPrefs.GetInt("RockMeter") + 1);
        }
        streak++;
        //Debug.Log(streak);
        if (streak >= 24)
            multiplier = 4;
        else if (streak >= 16)
            multiplier = 3;
        else if (streak >= 8)
            multiplier = 2;
        else
            multiplier = 1;
        
        UpdateGUI();
    }
    public void ResetStreak()
    {
        PlayerPrefs.SetInt("RockMeter", PlayerPrefs.GetInt("RockMeter") - 2);
        if (messUps >= 30)
        {
            Lose();
        }
        streak = 0;
        multiplier = 1;
        PlayerPrefs.SetInt("Streak", streak);
        UpdateGUI();
    }
    public void Lose()
    {
        //Any time we leave the scene, we have to play the song at the right time
        PlayerPrefs.SetInt("Start", 0);
        SceneManager.LoadScene("LoseScene");
    }
    public void Win()
    {
        PlayerPrefs.SetInt("Start", 0);
        winObject.SetActive(true);
    }
    void UpdateGUI()
    {
        PlayerPrefs.SetInt("Streak", streak);
        PlayerPrefs.SetInt("Mult", multiplier);
    }
    public int GetScore()
    {
        return 100 * multiplier;
    }
    public int GetMessUps()
    {
        return messUps;
    }
    public void SetMessUps(int i)
    {
        messUps = i;
    }
    public void KillThatZombie()
    {
        StartCoroutine(Instructions());
    }
    IEnumerator Instructions()
    {
        killInstructions.SetActive(true);
        yield return new WaitForSeconds(5f);
        killInstructions.SetActive(false);
    }
}
