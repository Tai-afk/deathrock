using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    GameObject gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Note"))
        {
            Destroy(col.gameObject);
        }
        gm.GetComponent<GameManager>().ResetStreak();
    }
}
