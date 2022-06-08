using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMusic : MonoBehaviour
{
    public KeyCode key1;
    public KeyCode key2;
    public GameObject guitar;
    public GameObject bass;
    public GameObject drum;
    public GameObject vocal;


    bool g = true;
    bool b = false;
    int count = 0;
    // Update is called once per frame
    private void Start()
    {
        guitar.gameObject.SetActive(true);

        bass.gameObject.SetActive(false);

        drum.gameObject.SetActive(false);

        vocal.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(key2))
        {
            count++;
        }
        if (Input.GetKeyDown(key1))
        {
            count--;
        }
        SwitchInstruments();
    }
    void Switch(GameObject a, GameObject b)
    {
        a.SetActive(false);
        b.SetActive(true);
    }
    void SwitchInstruments()
    {
        int check = Mathf.Abs(count % 4);
        switch(check)
        {
            case 0:
                guitar.gameObject.SetActive(true);

                bass.gameObject.SetActive(false);

                drum.gameObject.SetActive(false);

                vocal.gameObject.SetActive(false);
                break;

            case 1:
                guitar.gameObject.SetActive(false);

                bass.gameObject.SetActive(true);

                drum.gameObject.SetActive(false);

                vocal.gameObject.SetActive(false);
                break;

            case 2:
                guitar.gameObject.SetActive(false);

                bass.gameObject.SetActive(false);

                drum.gameObject.SetActive(true);

                vocal.gameObject.SetActive(false);
                break;
            case 3:
                guitar.gameObject.SetActive(false);

                bass.gameObject.SetActive(false);

                drum.gameObject.SetActive(false);

                vocal.gameObject.SetActive(true);
                break;
        }
    }
}
    
