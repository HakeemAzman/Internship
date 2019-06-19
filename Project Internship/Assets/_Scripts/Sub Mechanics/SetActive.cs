using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    [SerializeField]
    GameObject[] GO_SetActive;
    [SerializeField]
    [Space]
    bool isTrue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            for (int i = 0; i < GO_SetActive.Length; i++)
            {
                GO_SetActive[i].SetActive(isTrue);
            }
        }
    }
}
