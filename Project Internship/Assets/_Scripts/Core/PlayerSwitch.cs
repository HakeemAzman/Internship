using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    [SerializeField] GameObject Player_Thief, Player_Gladiator;

    public bool isThief = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        {
            if (isThief)
            {
                Player_Gladiator.transform.position = Player_Thief.transform.position;
                isThief = false;
            }
            else
            {
                Player_Thief.transform.position = Player_Gladiator.transform.position;
                isThief = true;
            }          
        }

        Characters();
    }

    void Characters()
    {
        if (isThief) //Thief
        {
            Player_Gladiator.SetActive(false);
            Player_Thief.SetActive(true);
        }
        else //Gladiator
        {
            Player_Thief.SetActive(false);
            Player_Gladiator.SetActive(true);

        }
    }
}
