using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void OnTriggerEnter2D(Collider2D enter)
    {
        var player = enter.gameObject.GetComponent<Protagonist>();
       
        if (enter.gameObject.tag == "Player")
        {
            if (enter.transform.position.x < transform.position.x)
            {
                enter.gameObject.GetComponent<Protagonist>().isKnockbackRight = true;
            }
            else
                enter.gameObject.GetComponent<Protagonist>().isKnockbackRight = false;
        }
    }
}
