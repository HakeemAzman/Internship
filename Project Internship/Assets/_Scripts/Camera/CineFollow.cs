using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineFollow : MonoBehaviour
{
    [SerializeField] GameObject Player_Follow;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<CinemachineVirtualCamera>().Follow = Player_Follow.transform;

        Player_Follow = GameObject.FindWithTag("Player");

    }
}
