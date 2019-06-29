using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Pawn
 * ==================================
 * Is a class that represents all sentient characters in our game.
 * Can be the base class for all characters (protagonist, allies, enemies, etc.) in the game.
*/
public class Pawn : Platformer {

    [Header("Pawn")]
    public float turnSpeed = 720f; // Time it takes for player to flip. If 0, will be instant.
    protected Quaternion desiredRotation;

    protected override void Init() {
        base.Init();
        desiredRotation = transform.rotation;
    }

    public override float Move(float dir = 0) {
        
        if(!IsAlive() || !CanMove()) return 0;
        
        HandleMovementFacing(dir);

        // If we are not facing where we want to move towards, rotate and terminate.
        if(Quaternion.Angle(transform.rotation,desiredRotation) > 0f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,desiredRotation,turnSpeed * Time.deltaTime);
            return dir;
        }

        Vector2 localMoveVector = transform.TransformDirection(rigidbody.velocity);
        
        // When button is unpressed, slow the character down.
        if(Mathf.Approximately(dir,0)) {

            // If we are moving horizontally, then slow ourselves down.
            if(Mathf.Abs(localMoveVector.x) > 0) {
                
                localMoveVector += new Vector2(-1,0) * GetMoveAcceleration() * GetDecelerationFactor() * Time.deltaTime;

                // If the sign of the X-axis changes, it means that we have accelerated the character
                // in the opposite direction. In which case we zero out the velocity.
                if(localMoveVector.x < 0) localMoveVector.x = 0;

            } else {
                localMoveVector.x = 0;
            }

        } else { // Otherwise move him in the direction we are pressing.
           
            // Apply a force in the direction we are moving.
            localMoveVector += new Vector2(1,0) * GetMoveAcceleration() * Time.deltaTime;
            
            // Get velocity in local axis, so we can limit it in the X axis.
            float maxLand = GetMaxLandSpeed();
            if(localMoveVector.x > maxLand) {
                localMoveVector = new Vector2(maxLand, localMoveVector.y);
            }

        }

        rigidbody.velocity = transform.InverseTransformDirection(localMoveVector);
        
        return dir;
    }

    protected override void HandleMovementFacing(float dir) {
        if(dir == 0) return;

        if(Mathf.Sign(dir) != Mathf.Sign(currentMovementFacing)) {
            desiredRotation = transform.rotation * Quaternion.Euler(0,180,0);
            currentMovementFacing = Mathf.Sign(dir);
        }
    }

}
