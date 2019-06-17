using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gladiator : Platformer
{
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }

    void FixedUpdate()
    {
        base.UpdateInAir();   
    }
}
