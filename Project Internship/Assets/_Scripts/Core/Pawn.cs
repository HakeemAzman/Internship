using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Pawn
 * ==================================
 * Is a class that represents all sentient characters in our game.
 * Can be the base class for all characters (protagonist, allies, enemies, etc.) in the game.
*/
public abstract class Pawn : Platformer {

    [Header("Pawn")]
    public float turnSpeed = 720f; // Time it takes for player to flip. If 0, will be instant.
    protected Quaternion desiredRotation;

    public Attack2D[] attacks = new Attack2D[1];
    public Collider2D[] hitboxes = new Collider2D[1];
    protected Attack2D currentAttack;

    // Container for <attacks>. Easier to access since its a name to attack pairing.
    readonly Dictionary<string,Attack2D> attackData = new Dictionary<string, Attack2D>();

    protected override void Init() {
        base.Init();
        desiredRotation = transform.rotation;
        
        // Populate the <attackData> variable, and arm its events.
        for(int i = 0; i < attacks.Length; i++) {
            if(!attacks[i]) continue;
            attackData.Add(attacks[i].name, attacks[i]);

            attacks[i].onStartup = OnAttackStartup;
            attacks[i].onActive = OnAttackActive;
            attacks[i].onRecovery = OnAttackRecovery;
            attacks[i].onEnd = OnAttackEnded;
            attacks[i].onDamage = OnAttackDamage;
        }
    }

    #region Attack Functionality
    // Executes the attack.
    public virtual void Attack(Attack2D atk) {
        // Cannot attack if in air or is already attacking.
        if(currentAttack != null) return;
        if(inAir) return;

        if(currentMovementFacing == 0)
            Debug.LogError("Can't attack because there is no facing direction.");

        currentAttack = atk;
        Quaternion direction = currentMovementFacing > 0 ? Quaternion.LookRotation(Vector3.right) : Quaternion.LookRotation(-Vector3.right);
        atk.Execute(this, hitboxes, direction);
    }

    public abstract void OnAttackStartup(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed);
    public abstract void OnAttackActive(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed);
    public abstract void OnAttackRecovery(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed);
    public virtual void OnAttackEnded(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        currentAttack = null;
    }

    public virtual void OnAttackDamage(int amount, MonoBehaviour target, MonoBehaviour instigator, Vector3? damageLocation) {
        Actor a = target.GetComponent<Actor>();
        if(a) {
            a.Damage(amount,instigator.gameObject);
        }
    }

    // Picks out the most suitable attack from a given <inputName>.
    // For PlayerController to use.
    public virtual void ReceiveAttackInput(string inputName) {
        Attack2D result = null;
        int highestPriority = int.MinValue;
        foreach(KeyValuePair<string,Attack2D> data in attackData) {
            if(data.Value.inputName.Contains(inputName) && data.Value.priority > highestPriority) {
                result = data.Value;
                highestPriority = data.Value.priority;
            }
        }
        Attack(result);
    }

    public virtual Attack2D GetCurrentAttack() {
        return currentAttack;
    }
    #endregion

    public override float Move(float dir = 0) {
        
        if(!IsAlive() || !CanMove()) return 0;
        if(currentAttack != null) return 0; // Cannot move when attacking.
        
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
