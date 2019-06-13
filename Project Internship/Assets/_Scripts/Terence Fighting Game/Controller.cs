using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {

    [Header("Controller Settings")]
    public Actor controlled;

    [System.Serializable]
    public struct InputAction {
        public string name;
        public string[] buttons;
        public Vector2 movementInput;
    }
    public InputAction[] input;

    protected virtual void Init() {
        if(!controlled) controlled = GetComponent<Actor>();
    }

    protected virtual void Autofill() {
        if(!controlled) controlled = GetComponent<Actor>();

        input = new InputAction[4] {
            new InputAction {
                name = "Light", buttons = new string[]{ "LightAttack" }
            },

            new InputAction {
                name = "ForwardHeavy", buttons = new string[]{ "HeavyAttack" },
                movementInput = new Vector2(1,0)
            },

            new InputAction {
                name = "Heavy", buttons = new string[]{ "HeavyAttack" }
            },

            new InputAction {
                name = "Kick", buttons = new string[]{ "Kick" }
            }
        };
    }

}
