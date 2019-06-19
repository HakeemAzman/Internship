using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] PlayerSwitch ps;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ps.gameObject.GetComponent<PlayerSwitch>().canSwitch = true;
            Destroy(this.gameObject);
        }
    }
}
