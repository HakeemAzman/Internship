using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Platformer {
    void Start() {
        base.Init();
    }

    void FixedUpdate()
    {
        base.UpdateInAir();
    }
}
