using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    private bool canBePressed;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Start") == 1)
        {
            rb.velocity = new Vector2(0, -speed);
        }
    }


}
