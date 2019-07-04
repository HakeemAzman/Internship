using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatforms : MonoBehaviour
{
    [SerializeField] Vector3 yAxis_Min, yAxis_Max;
    [SerializeField] float Platform_Speed;
    bool isGoingUp = true, startMoving = false;

    private void Start()
    {
        yAxis_Min = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float step = Platform_Speed * Time.deltaTime;

        if(startMoving)
        {
            if (transform.position == yAxis_Min) isGoingUp = true;
            else if (transform.position == yAxis_Max) isGoingUp = false;

            if (isGoingUp) transform.position = Vector3.MoveTowards(transform.position, yAxis_Max, step);
            else
                transform.position = Vector3.MoveTowards(transform.position, yAxis_Min, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D enter)
    {
        if (enter.gameObject.tag == "Player")
        {
            startMoving = true;
            enter.gameObject.transform.SetParent(transform);
            enter.gameObject.transform.localScale = new Vector3(1, 1, 1);
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
