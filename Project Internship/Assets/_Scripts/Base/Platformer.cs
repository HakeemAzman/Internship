using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Platformer
 * ==================================
 * This class gives the object basic platform behaviourial features, and is designed to work
 * just by being plugged in to any platforming character:
 * - Moving left and right
 * - Jumping
 * - Tracking of whether the character is on the ground on in the air.
*/

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public abstract class Platformer : Actor {

    protected bool inAir = true; // Tracks whether this is touching the ground.
    public bool InAir() { return inAir; }

    protected Vector2 groundNormal; // If you are not in the air, this records the normal of the ground you are on.
    protected Collider2D platform; // Platform that one is standing on when grounded.

    // Character stats.
    [Header("Platformer")]
    public float jumpAcceleration = 6f; // Represents how high you can jump.
    public float moveAcceleration = 20f; // Represents how fast you accelerate when you press directional keys.
    public float decelerationFactor = 1f; // How fast do you want the main character to decelerate when we stop moving?
    public float maxLandSpeed = 5f; // Represents the maximum speed you can accelerate to with directional movement.
    public float airControlFactor = 0.5f; // How much movement control do you have when in the air?
    public BoxCollider2D movementCollider;
    public LayerMask whatIsGround;
    public float groundLineOffset = 0.02f; // How far down does the line to find the ground extend?
    public Vector3 flipDirection = new Vector3(1,0,0);
    protected float currentMovementFacing = 1f;

    // Values that are applied to Platformer to affect movement parameters.
    [System.Serializable]
    public class MovementModifier {
        public float maxLandSpeedMultiplier = 1f, moveAccelerationMultiplier = 1f, decelerationMultiplier = 1f;
        public float jumpAccelerationMultiplier = 1f, airControlFactorMultiplier = 1f, duration = 1f;
        public bool fullDisable = false; // Fully immobilizes the player as long as the modifier is active.
        [HideInInspector] public Coroutine coroutine;
    }
    public readonly Dictionary<string,MovementModifier> movementModifiers = new Dictionary<string,MovementModifier>();

    public virtual bool HasMovementModifier(string identifier) { return movementModifiers.ContainsKey(identifier); }

    public virtual void AddMovementModifier(string identifier, MovementModifier mod, float duration = Mathf.Infinity) {

        if(duration <= 0) return;

        if (HasMovementModifier(identifier)) {
            if(movementModifiers[identifier].duration < duration)
                RemoveMovementModifier(identifier); // Remove movement modifier by identifier, if any, to avoid repeats.
            else
                return;
        }

        // Applies the effect of the modifier.
        movementModifiers.Add(identifier, mod);
        if(duration != Mathf.Infinity)
            mod.coroutine = StartCoroutine(UpdateMovementModifier(identifier));
        
    }

    public virtual void RemoveMovementModifier(string identifier = "") {
        // If the modifier doesn't exist any more, return.
        if(!HasMovementModifier(identifier)) return;

        // Otherwise remove the modifier.
        MovementModifier m = movementModifiers[identifier];
        if(m.coroutine != null) StopCoroutine(m.coroutine);
        movementModifiers.Remove(identifier);
    }

    // Shortcut for adding a movement modifier that disables everything.
    // If <duration> is 0, will last until manually removed.
    public virtual void DisableMovement(string identifier, float duration = Mathf.Infinity) {
        if(duration <= 0) return;

        MovementModifier mod = new MovementModifier{
            fullDisable = true,
            duration = duration
        };
        rigidbody.velocity = Vector2.zero;
        AddMovementModifier(identifier,mod,duration);
    }

    // Alias for RemoveMovementModifier.
    public virtual void EnableMovement(string identifier) {
        RemoveMovementModifier(identifier);
    }

    // Checks whether this character has any full disable modifiers on him.
    public virtual bool CanMove() {
        foreach(KeyValuePair<string,MovementModifier> mod in movementModifiers) {
            if(mod.Value.fullDisable) return false;
        }
        return true;
    }

    IEnumerator UpdateMovementModifier(string identifier) {
        
        MovementModifier m = movementModifiers[identifier];
        while (m.duration > 0) {
            yield return new WaitForEndOfFrame();
            m.duration -= Time.deltaTime;
        }
        RemoveMovementModifier(identifier);

    }

    // Attributes that we have to apply movement modifiers to.
    public virtual float GetJumpAcceleration() {
        float r = jumpAcceleration;
        foreach(KeyValuePair<string, MovementModifier> entry in movementModifiers)
            r *= entry.Value.jumpAccelerationMultiplier;
        return r;
    }
    public virtual float GetMoveAcceleration() {
        float r = moveAcceleration;
        foreach (KeyValuePair<string, MovementModifier> entry in movementModifiers)
            r *= entry.Value.moveAccelerationMultiplier;
        return r;
    }
    public virtual float GetDecelerationFactor() {
        float r = decelerationFactor;
        foreach (KeyValuePair<string, MovementModifier> entry in movementModifiers)
            r *= entry.Value.decelerationMultiplier;
        return r;
    }
    public virtual float GetMaxLandSpeed() {
        float r = maxLandSpeed;
        foreach (KeyValuePair<string, MovementModifier> entry in movementModifiers) {
            r *= entry.Value.maxLandSpeedMultiplier;
        }
        return r;
    }
    public virtual float GetAirControlFactor() {
        float r = airControlFactor;
        foreach (KeyValuePair<string, MovementModifier> entry in movementModifiers)
            r *= entry.Value.airControlFactorMultiplier;
        return r;
    }
    
    // Use this for initialization
    protected override void Init () {
        base.Init();
        if(movementCollider == null) movementCollider = GetComponent<BoxCollider2D>();
	}

    protected override void Autofill() {
        base.Autofill();
        whatIsGround = 1 << LayerMask.NameToLayer("Ground");
    }

    // Takes a custom jump value if you want.
    public virtual bool Jump(float jumpStr = 0) {
        
        if(!IsAlive()) return false; // Can't jump if you are dead!
        if (inAir) return false; // Don't allow jump if in air.
        if (!CanMove()) return false; // Don't allow jump if movement is disabled.

        if (jumpStr == 0) jumpStr = GetJumpAcceleration();
        Vector2 jumpVector = Vector2.up * jumpStr;

        //PhysicsBody.AddForce(jumpVector); // Add the force at the offset value of the feet collider.
        rigidbody.velocity += jumpVector;
        
        return true;
    }

    // Moves the Platformer in a 2D platformer world. When passed a positive value, moves rightwards; negative value
    // moves rightwards. The function moves a walker along their own local xy axis instead of the global one.
    public virtual float Move(float dir = 0) {
        
        if(!IsAlive() || !CanMove()) return 0;
        
        Vector2 localMoveVector = transform.TransformDirection(rigidbody.velocity);
        
        // When button is unpressed, slow the character down.
        if(Mathf.Approximately(dir,0)) {

            // If we are moving horizontally, then slow ourselves down.
            if(Mathf.Abs(localMoveVector.x) > 0) {
                
                float sign = Mathf.Sign(localMoveVector.x);
                localMoveVector += new Vector2(-sign, 0) * GetMoveAcceleration() * GetDecelerationFactor() * Time.deltaTime;

                // If the sign of the X-axis changes, it means that we have accelerated the character
                // in the opposite direction. In which case we zero out the velocity.
                if(sign * Mathf.Sign(localMoveVector.x) < 0) localMoveVector.x = 0;

            } else {
                localMoveVector.x = 0;
            }

        } else { // Otherwise move him in the direction we are pressing.
           
            // Apply a force in the direction we are moving.
            localMoveVector += new Vector2(dir,0) * GetMoveAcceleration() * Time.deltaTime;
            
            // Get velocity in local axis, so we can limit it in the X axis.
            float maxLand = GetMaxLandSpeed();
            if(Mathf.Abs(localMoveVector.x) > maxLand) {
                localMoveVector = new Vector2(Mathf.Sign(localMoveVector.x) * maxLand, localMoveVector.y);
            }

        }

        rigidbody.velocity = transform.InverseTransformDirection(localMoveVector);
        
        HandleMovementFacing(dir);
        return dir;
    }

    // For other classes to get where this Platformer is facing at the moment.
    public virtual float GetMovementFacing() { return Mathf.Sign(currentMovementFacing); }

    // This function is called by Move() and is passed its <dir> value. It handles sprite flipping.
    protected virtual void HandleMovementFacing(float dir) {
        // Handle sprite reflection.
        if (dir != 0) {
            Vector3 scale = transform.localScale;

            if(!Mathf.Approximately(flipDirection.x,0)) {
                scale.x = Mathf.Abs(scale.x) * flipDirection.x;
                if (dir < 0) scale.x *= -1;
            }
            if(!Mathf.Approximately(flipDirection.y,0)) {
                scale.y = Mathf.Abs(scale.y) * flipDirection.y;
                if (dir < 0) scale.y *= -1;
            }
            if(!Mathf.Approximately(flipDirection.z,0)) {
                scale.z = Mathf.Abs(scale.z) * flipDirection.z;
                if (dir < 0) scale.z *= -1;
            }
            currentMovementFacing = dir; // Update facing direction if we are moving somewhere.
            transform.localScale = scale;
        }
    }

    // Checks whether the character is in the air.
    // In a class that implements Platformer, please call this method in FixedUpdate().
    protected virtual void UpdateInAir() {
        Vector2 v = new Vector2(0, -movementCollider.size.y * 0.5f - groundLineOffset),
                x = v + movementCollider.offset, y = v + movementCollider.offset;
        float hw = movementCollider.size.x * 0.5f;

        x.x += -hw;
        y.x += hw;
        
        RaycastHit2D h = Physics2D.Linecast(transform.localToWorldMatrix.MultiplyPoint3x4(x), transform.localToWorldMatrix.MultiplyPoint3x4(y), whatIsGround);
        if (h.collider == null || h.collider.gameObject == gameObject) {
            inAir = true;
            groundNormal = Vector2.zero;
            platform = null;
        } else {
            inAir = false;

            // Recalculate essential variables if character is not in air.
            if(platform != h.collider) { 
                RaycastHit2D g = Physics2D.Raycast(h.point, Physics2D.gravity, 1f, whatIsGround);
                groundNormal = g.normal;
                platform = h.collider;
            }
        }
    }

}
