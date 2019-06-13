using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Pawn
 * ==================================
 * Is a class that represents all sentient characters in our game.
 * Contains functionality for knockdown, damage animations, and hit stun.
 * Separated from Platformer to create clearer differentiation of functionalities
 * between subclasses.
*/
public class Pawn : Platformer {

    // Properties to track and manage knockdowns.
    [System.Serializable]
    public struct KnockdownData {
        public string animationStateFront, animationStateBack, recoveryAnimatorTriggerName;
        public float recoveryTime, wakeupTime;
    }

    [System.Serializable]
    public struct DamageAnimationStates {
        public string frontMid, frontHigh, frontLow, backMid, backHigh, backLow;
    }

    [System.Serializable]
    public struct DashAction {
        public float speed, startup, active, recovery;
        public string animationStateName;
    }
 
    [Header("Pawn")]
    public DamageAnimationStates damageAnimationStates;
    public KnockdownData knockdownData;
    protected Coroutine knockdownCoroutine;

    public DashAction[] dashData;
    Coroutine dashCoroutine;
    
    protected bool isBlocking = false;
    public float blockFactor = 0.02f;
    public Color32 blockFlashColour = new Color32(255,255,0,255);

    [System.Serializable]
    public struct ReadyStateData {
        public string animatorBoolean;
        public MovementModifier movementData;
        public float transitionDuration;
    }
    public ReadyStateData readyStateData;
    protected bool isReady = false;

    public MovementModifier runState;

    [HideInInspector] public float stunned; // Stores the current stun duration.
    Coroutine stunCoroutine;

    protected override void Autofill() {
        base.Autofill();
        deathTime = 6f;
        knockdownData = new KnockdownData() {
            animationStateFront = "Knockdown_Front_Air",
            animationStateBack = "Knockdown_Back_Air",
            recoveryTime = 1f, wakeupTime = 1f,
            recoveryAnimatorTriggerName = "KnockdownWakeup"
        };
        damageAnimationStates = new DamageAnimationStates() {
            frontHigh = "Damaged_Front_High"
        };
        readyStateData = new ReadyStateData() {
            animatorBoolean = "IsReady",
            movementData = new MovementModifier() {
                maxLandSpeedMultiplier = 0.6f, moveAccelerationMultiplier = 3f, airControlFactorMultiplier = 1f,
                jumpAccelerationMultiplier = 1f, decelerationMultiplier = 1f, duration = 1f
            },
            transitionDuration = 0.25f
        };
        runState = new MovementModifier() {
            maxLandSpeedMultiplier = 2f, moveAccelerationMultiplier = 3f, airControlFactorMultiplier = 1f,
            jumpAccelerationMultiplier = 1f, decelerationMultiplier = 1f, duration = 1f
        };
        dashData = new DashAction[2]{
            new DashAction() { speed = 265, recovery = 0.4f, animationStateName = "Dash_Forward" },
            new DashAction() { speed = -200, recovery = 0.4f, animationStateName = "Dash_Backward" }
        };
    }

    // Make the animator play the specified animation states when damaged.
    // Also reduces the damage received if we are blocking.
    public override int Damage(int amount,GameObject instigator = null,Vector3? location = null) {
        int amt = amount;
        Vector2 diff = transform.position - instigator.transform.position;
        if(isBlocking && IsBlocked(diff.x)) amt = (int)Mathf.Max(1,amount * blockFactor);
        else animator.CrossFade(damageAnimationStates.frontHigh,0.25f,0);
        return base.Damage(amt,instigator,location);
    }

    // Overriding of movement methods to disable movement during block.
    public override float Move(float dir = 0) {

        if(!CanMove()) return 0;

        // If dir > 1, add a run modifier.
        if(Mathf.Abs(dir) > 1) {
            Ready(false);
            AddMovementModifier("run_modifier",runState);
        } else if(dir == 0)
            RemoveMovementModifier("run_modifier");

        return base.Move(dir);
    }
    public override bool Jump(float jumpStr = 0) {
        if(isBlocking) return false;
        return base.Jump(jumpStr);
    }
    protected override void UpdateInAir() {
        base.UpdateInAir();
        if(inAir) Unblock();
    }

    // Override the original method so that we can flash a different colour if an
    // attack is blocked.
    protected override IEnumerator DamageFlash(int amount, GameObject instigator = null, Vector3? location = null) {

        Vector2 diff = transform.position - instigator.transform.position;
        Color flash = damageFeedback.colour;

        // Flash a different colour if we are blocking in the right direction.
        if(isBlocking && IsBlocked(diff.x)) {
            flash = blockFlashColour;
        }

        foreach(SpriteRenderer sr in damageFeedback.renderers)
            sr.color = flash;
        yield return new WaitForSeconds(damageFeedback.duration);
        foreach(SpriteRenderer sr in damageFeedback.renderers)
            sr.color = damageFeedback.originalColour;
    }

    // For toggling between ready and unready.
    public void Ready(bool b = true, float transitionDuration = -1f) {
        // Don't apply this function if we are already in the same state.
        if(isReady == b) return;

        isReady = b;
        if(readyStateData.animatorBoolean != "") {
            animator.SetBool(readyStateData.animatorBoolean,isReady);
            if(transitionDuration < 0) Stun(readyStateData.transitionDuration);
            else if(transitionDuration > 0) Stun(transitionDuration);
        }

        if(isReady) AddMovementModifier("ready_modifier",readyStateData.movementData);
        else RemoveMovementModifier("ready_modifier");
    }

    // For toggling blocking and unblocking.
    public virtual bool Block() {

        // Stop the blocking if we cannot block.
        if(!CanBlock()) {
            isBlocking = false;
            return false;
        }

        // Otherwise enable blocking and disable character movement.
        isBlocking = true;
        DisableMovement("Blocking", 0);
        return isBlocking;
    }
    public virtual bool Unblock() {
        // Don't unblock if stunned.
        // So player knows no action can be taken yet.
        if(!CanUnblock()) return isBlocking;
        isBlocking = false;
        EnableMovement("Blocking");
        return isBlocking;
    }
    public virtual bool IsBlocking() { return isBlocking; }
    
    // Checks whether this character is currently capable of blocking.
    public virtual bool CanBlock() {
        // If not already blocking, stun will prevent blocking.
        if(stunned > 0 && !isBlocking) return false;

        // If in air or not ready, cannot block too.
        if(inAir || !isReady || HasMovementModifier("dash_movement_disable")) {
            if(!isReady) Ready();
            return false;
        }
        return true;
    }
    public virtual bool CanUnblock() {
        if(stunned > 0) return false;
        return true;
    }

    // Checks whether attacks from a certain <direction> are blocked.
    // < 0 = left side, > 0 = right side.
    public virtual bool IsBlocked(float direction) {
        if(!isBlocking) return false;
        float dir = GetMovementFacing();
        if(dir != Mathf.Sign(direction)) return true;
        return false;
    }

    public virtual bool Dash(float dir) {
        if(!CanMove()) return false;

        // Get facing and compare whether dash direction matches facing.
        float facing = GetMovementFacing();
        int index = Mathf.Sign(facing) == Mathf.Sign(dir) ? 0 : 1;

        // Dashes towards the direction we are heading and add a movement disable.
        rigidbody.velocity += (Vector2)transform.right * dashData[index].speed * Time.fixedDeltaTime * facing;
        AddMovementModifier("dash_movement_disable", new MovementModifier() { fullDisable = true });

        // Add the coroutine to manage this dash if it has startup, active and / or recovery frames.
        if(dashData[index].startup + dashData[index].active + dashData[index].recovery > 0) {
            StartCoroutine(DashUpdater(dir,index));
        }

        if(dashData[index].animationStateName != "")
            animator.CrossFade(dashData[index].animationStateName, 0.1f, 0);

        return true;
    }

    protected IEnumerator DashUpdater(float dir, int dashIndex) {
        yield return new WaitForSeconds(dashData[dashIndex].startup + dashData[dashIndex].active + dashData[dashIndex].recovery);
        RemoveMovementModifier("dash_movement_disable");
        rigidbody.velocity = Vector2.zero;
    }

    // Applies knockback to this Pawn.
    public virtual void Knockback(Vector2 velocity, float duration = 0) {
        // If the force is against where we are blocking, don't knockback upwards as we are
        // blocking the correct direction.
        if(isBlocking && GetMovementFacing() != Mathf.Sign(velocity.x))
            velocity.Set(velocity.x,0);

        if(duration > 0) Stun(duration);
        rigidbody.velocity += velocity; // Set this to the given velocity.
    }

    // Applies a knock down to this Pawn.
    public virtual void Knockdown(Vector2 velocity) {
        Stun();
        rigidbody.velocity += velocity;
        knockdownCoroutine = StartCoroutine(KnockdownUpdater());
        if(animator) {
            //animator.SetBool(knockdownAnimatorBoolName,true);
            if(Vector2.Dot(velocity,new Vector2(GetMovementFacing(),0)) > 0)
                animator.CrossFade(knockdownData.animationStateBack,0.1f,0);
            else animator.CrossFade(knockdownData.animationStateFront,0.1f,0);
        }
    }

    // Coroutine updater for Knockdown.
    protected virtual IEnumerator KnockdownUpdater() {
        WaitForEndOfFrame delta = new WaitForEndOfFrame();
        yield return delta;
        while(knockdownCoroutine != null) {
            if(!inAir) {
                if(!IsAlive()) yield break; // If we are not alive, don't get up.

                yield return new WaitForSeconds(knockdownData.recoveryTime);
                if(animator) animator.SetTrigger(knockdownData.recoveryAnimatorTriggerName);

                yield return new WaitForSeconds(knockdownData.wakeupTime);
                knockdownCoroutine = null;
                Unstun();
                break;
            }
            yield return delta;
        }
    }

    // Causes this character to be unable to take actions for <duration>.
    // Returns false if the new stun duration did not apply.
    public virtual bool Stun(float duration = Mathf.Infinity) {

        if(duration <= 0) return false;

        // Check if we are already stunned, and override old stun only if new stun is longer!
        if(stunned > 0) {
            if(duration > stunned) {
                // Clean up old stun coroutine before applying new stun
                Unstun();
            } else return false;
        }

        stunned = duration;
        if(duration != Mathf.Infinity) stunCoroutine = StartCoroutine(StunUpdater());
        DisableMovement("pawn_stun",duration); // An additional layer of movement disable.

        return true;
    }

    // Removes stun from the Fighter, if he is stunned.
    public virtual void Unstun() {
        if(stunCoroutine != null) {
            StopCoroutine(stunCoroutine);
            stunCoroutine = null;
        }
        stunned = 0;
        EnableMovement("pawn_stun");
    }

    IEnumerator StunUpdater() {
        while(stunned > 0f) {
            yield return new WaitForEndOfFrame();
            stunned = Mathf.Max(0f,stunned - Time.deltaTime);
        }
        Unstun();
    }

    public override void Death(GameObject instigator = null) {
        Knockdown(new Vector2(-150 * GetMovementFacing(),50) * Time.fixedDeltaTime);
        gameObject.layer = LayerMask.NameToLayer("Dead");
        base.Death(instigator);
    }

}
