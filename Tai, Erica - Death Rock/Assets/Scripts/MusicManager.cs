using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 3f;
    bool called = false;

    void Start()
    {
        currentTime = startingTime;
    }
    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        if (currentTime <= 0) currentTime = 0;
        if (currentTime == 0 && (PlayerPrefs.GetInt("Start") == 1 && !called))
        {
            GetComponent<AudioSource>().Play();
            called = true;
        }
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
    public void PlayMusic()
    {
        GetComponent<AudioSource>().Play();
    }
}
