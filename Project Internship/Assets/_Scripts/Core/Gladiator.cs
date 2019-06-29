using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gladiator : Protagonist {
    // Start is called before the first frame update
    void Awake() {
        base.Init();
    }

    void FixedUpdate() {
        base.UpdateInAir();
    }
}
