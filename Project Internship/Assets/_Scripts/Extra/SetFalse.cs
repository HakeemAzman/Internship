using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFalse : MonoBehaviour
{
    [SerializeField] float timeToWait;
    
    void Awake()
    {
        StartCoroutine(SetActiveFalse());
    }

    IEnumerator SetActiveFalse()
    {
        yield return new WaitForSeconds(timeToWait);
        this.gameObject.SetActive(false);
    }
}
