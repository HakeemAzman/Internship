using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour
{
    [Header("Death Collider")]

    [SerializeField] GameObject Death_Collider;
    [SerializeField] float Guage;
    [SerializeField] bool isSwitch;

    // Update is called once per frame
    void Update()
    {
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
}
