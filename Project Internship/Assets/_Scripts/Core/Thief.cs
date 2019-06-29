using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Protagonist {

    void Awake() {
        base.Init();
    }

    void FixedUpdate() {
        base.UpdateInAir();
    }
    
}
