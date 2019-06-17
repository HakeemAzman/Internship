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
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (isThief)
                isThief = false;
            else
                isThief = true;
        }

        Characters();
    }

    void Characters()
    {
        if (isThief)
        {
            Player_Gladiator.SetActive(false);
            Player_Thief.SetActive(true);
        }
        else
        {
            Player_Thief.SetActive(false);
            Player_Gladiator.SetActive(true);

        }
    }
}
