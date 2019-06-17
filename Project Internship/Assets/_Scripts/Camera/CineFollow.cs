using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineFollow : MonoBehaviour
{
    public GameObject Player_Follow;

    // Update is called once per frame
    void Update()
    {
        if(Player_Follow == null)
        {
            Player_Follow = GameObject.FindWithTag("Player");
        }

        this.gameObject.GetComponent<CinemachineVirtualCamera>().Follow = Player_Follow.transform;
    }
}
