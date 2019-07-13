﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] GameObject Axis_Min, Axis_Max;
    [SerializeField] float Platform_Speed;
    [SerializeField] bool startMoving = false; //Start Moving on Awake
    bool isMin = true;

    [Header("Death Collider")]
    
    [SerializeField] GameObject Death_Collider;
    [SerializeField] float Guage;
    [SerializeField] bool isSwitch;

    void FixedUpdate()
    {
        float step = Platform_Speed * Time.deltaTime;

        if(startMoving)
        {
            //Moving Up and Down
            if (transform.position == Axis_Min.transform.position) isMin = true; //If current position equals minimum height it will go up
            else if (transform.position == Axis_Max.transform.position) isMin = false; //If current position equals maximum height it will go down

            if (isMin) transform.position = Vector3.MoveTowards(transform.position, Axis_Max.transform.position, step); //Vector3.MoveTowards(transform.position, yAxis_Max, step); //Where to stop
            else
                transform.position = Vector3.MoveTowards(transform.position, Axis_Min.transform.position, step);
        }

        if (isSwitch) //Controls more or less than
        {
            if (transform.position.x <= Guage) Death_Collider.SetActive(true);
            else
                Death_Collider.SetActive(false);
        }
        else
        {
            if (transform.position.x >= Guage) Death_Collider.SetActive(true);
            else
                Death_Collider.SetActive(false);
        }
    }

    #region Set Parent
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
    #endregion
}
