using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [Header("Moving Settings")]
    public bool isMovable;
    public float Movement_Speed, Ray_Distance;
    
    bool isRight = true;

    [SerializeField] Transform Player_Pos;
    [SerializeField] LayerMask Player_LayerMask;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Update()
    {
        if(isMovable)
        {
            transform.Translate(Vector3.forward * Movement_Speed * Time.deltaTime);

            bool isGroundDetected = Physics2D.OverlapCircle(Player_Pos.position, 0.1f, Player_LayerMask);

            if(!isGroundDetected)
            {
                if (isRight)
                {
                    transform.localRotation = Quaternion.Euler(0, 90, 0);
                    isRight = false;
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, -90, 0);
                    isRight = true;
                }
            }
        }
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
