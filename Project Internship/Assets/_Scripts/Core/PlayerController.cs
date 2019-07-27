using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    public float doubleTapWindow = 0.5f;
    public bool disableInput = false;
    float lastMoveDirection; // Records the last horizontal movement direction for double taps.
    bool firstHorizontalPress = false;

    void Start() {
        Init();
    }

    void Reset() {
        Autofill();
    }

    void Update() {

        if(disableInput) return;

        Platformer c = controlled as Platformer;
        Protagonist pScript = GetComponent<Protagonist>();

        if (Input.GetButtonDown("Switch")) {
            Protagonist p = controlled as Protagonist;
            if(p) p.Switch();
        }

        if(Input.GetButtonDown("Jump")) {
            c.Jump();
        } else if (Input.GetButtonUp("Jump")) {
            //If the player hasn't reached the peak of their jump yet
            Rigidbody2D rb = controlled.GetComponent<Rigidbody2D>();
            if (rb.velocity.y > 0) {
                //Cut vertical velocity by half
                rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        float moveDir = Input.GetAxis("Horizontal");
        if(!Mathf.Approximately(moveDir,0))
        {
            if(!firstHorizontalPress) {
                firstHorizontalPress = true;
                if (lastMoveDirection == 0)
                {
                    lastMoveDirection = doubleTapWindow * Mathf.Sign(moveDir);
                }
                else if (Mathf.Sign(lastMoveDirection) == Mathf.Sign(moveDir))
                { // Double tap detected.
                    moveDir = Mathf.Sign(moveDir) * 2;
                }

            }
        } else firstHorizontalPress = false;
        c.Move(moveDir);

        // Reduce lastMoveDirection by deltaTime.
        if(lastMoveDirection > 0) lastMoveDirection = Mathf.Max(0,lastMoveDirection - Time.deltaTime);
        else if(lastMoveDirection < 0) lastMoveDirection = Mathf.Min(0,lastMoveDirection + Time.deltaTime);

        HandleFireInput();
    }

    // Call this in Update in a child class to enable attacks.
    protected virtual void HandleFireInput() {
        
        Protagonist c = controlled as Protagonist;

        // Loop through all attacks to see if we are pressing a button that activates any one of them.
        for(int i=0;i<input.Length;i++) {
            
            //if(atk.Value.inAir != inAir) continue; // Check if attack type matches current in air condition.

            bool buttonsPressed = true; // Does this attack meet all the conditions to be fired?

            // Check if all the necessary buttons are pressed.
            foreach(string btn in input[i].buttons) {
                if(!Input.GetButtonDown(btn)) {
                    buttonsPressed = false;
                    break;
                }
            }
            
            // If all necessary buttons are pressed, check if move input conditions are met.
            if(buttonsPressed) {
                
                // If the movement input values are less than our specified x and y values, continue the loop.
                // We are not bothering to set activated here again because it's not necessary.
                if(!CheckFireMovementInput(input[i].movementInput)) continue;

            } else continue;

            // If the code manages to get here, the conditions have all been met.
            c.ReceiveAttackInput(input[i].name);
            
        }
    }

    // Checks if the current input for movement is greater than a given input vector.
    // Used to check whether attack conditions are met.
    protected virtual bool CheckFireMovementInput(Vector2 input) {
        Vector2 v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Check if the input is keyed if this input has directional requirements.
        if(input.x != 0) {
            Platformer c = controlled as Platformer;
            if(!c) return false;

            if(Mathf.Sign(c.GetMovementFacing() * input.x) != Mathf.Sign(v.x))
                return false;
        }
        if(input.y != 0) {
            if(Mathf.Sign(input.y) != Mathf.Sign(v.y))
                return false;
        }

        return true;
    }

}
