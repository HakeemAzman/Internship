using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatforms : MonoBehaviour
{
    [SerializeField] Vector3 yAxis_Min, yAxis_Max;
    [SerializeField] float Platform_Speed;
    bool isGoingUp = true, startMoving = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        float step = Platform_Speed * Time.deltaTime;

        if(startMoving)
        {
            if (transform.position == yAxis_Min) isGoingUp = true; //If current position equals minimum height it will go up
            else if (transform.position == yAxis_Max) isGoingUp = false; //If current position equals maximum height it will go down

            if (isGoingUp) transform.position = Vector3.MoveTowards(transform.position, yAxis_Max, step); //Where to stop
            else
                transform.position = Vector3.MoveTowards(transform.position, yAxis_Min, step); //Where to stop
        }
    }

    private void OnTriggerEnter2D(Collider2D enter)
    {
        if (enter.gameObject.tag == "Player")
        {
            startMoving = true;
            enter.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D exit)
    {
        if(exit.gameObject.tag == "Player")
        {
            exit.gameObject.transform.SetParent(null);
        }
    }
}
