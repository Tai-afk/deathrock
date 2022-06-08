using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject InstOne, InstTwo;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            InstOne.SetActive(false);
            InstTwo.SetActive(false);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("DeathRock");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ShowInstuctions()
    {
        InstOne.SetActive(true);
        InstTwo.SetActive(true);
    }
}
