using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fighter : Pawn {

    // Struct for the Attack datatype.
    [System.Serializable]
    public struct Attack
    {
        public string name;
        
        [System.Serializable]
        public struct AnimatorStateTransitionData
        {
            public string stateName, layerName;
            public float transitionDuration, timeOffset, transitionTime;
        }

        public AnimatorStateTransitionData[] animatorStateTransitionData;
        
        [System.Serializable]
        public struct OffsetForces { public Vector2 startup, active, recovery; }
        public enum AttackType { ground, air, run, dash }

        [Header("Properties")]
        public string[] inputName;
		public int priority; // Higher priority attacks will be executed if one input executes multiple attacks.
        public bool allowsMovement;
        public AttackType attackType;
        public float startup, active, recovery;
        public OffsetForces offsetForces; // If this attack moves the character, key in the forces here.
        public string[] cancelsFrom; // A list of attack names that this attack is cancellable from.
        public bool cancelOnly; // If checked, this attack can only be accessed when cancelled from another move.

        [System.Serializable]
        public struct Hitbox
        {
            public Collider2D collider; // Collider of the hitbox.
            public int damage, instance; // Damage that the hitbox deals.
            public bool knockdown;
            public Vector2 attackerKnockback, targetKnockback;
            public float hitStun;
        }

        [Header("Damage")]
        public Hitbox[] hitboxes;
        public LayerMask targets;
    }

    // Protected variables for managing the attack state.
    [System.Serializable]
    public enum AttackPhase { none, startup, active, recovery } // <none> means there is no attack at the moment.
    protected AttackPhase attackPhase; // Records whether an attack is in startup, recovery or if there is no attack.
    protected Attack currentAttack; // Contains currently executing attack, only relevant if <attackPhase> is not <none>.
    Coroutine attackPhaseCoroutine; // Contains the current coroutine used to manage the <attackPhase>.
    
    [Header("Combat")]
    public Attack[] attacks;
    readonly Dictionary<string,Attack> attackData = new Dictionary<string, Attack>(); // Convert into Dictionary during runtime for easier access.
    [Range(0.01f,3f)] public float attackSpeed = 1f;
    public string attackSpeedAnimatorParameter = "AttackSpeed";
    List<Attack> attacksSelected = new List<Attack>(); // List of attacks that are possible from input received this frame.

    // Get the attached Animator component and duplicate the attack data array.
    protected override void Init() {
        base.Init();

        // Duplicate data in <attacks> into <attackData>.
        for(int i=0;i<attacks.Length;i++)
            attackData.Add(attacks[i].name, attacks[i]);
        
    }

	// ReceiveFireInput() will call this method to queue all possible attacks from input.
    public virtual bool QueueFire(string attackName) {
        Attack a = attackData[attackName];
        if(attacksSelected.Contains(a)) return false;
        attacksSelected.Add(a);
        return true;
    }
	
	// Selects a single attack from <attackSelected> to Fire().
	// Will clear the List once execution is done.
	public virtual void HandleFireQueue() {
		List<Attack> al = attacksSelected.OrderByDescending(o => o.priority).ToList();
        foreach(Attack atk in al) {
            if(Fire(atk.name)) {
                break;
            }
        }
        attacksSelected.Clear();
	}

    // "Fires" an attack by the name of <attackName>. Will return false if attack cannot be started.
    // This function does not contain functionality to damage enemies. Subclasses will have to code in this
    // functionality themselves by overriding Fire().
    public virtual bool Fire(string attackName) {
        
        // Set Animation parameter.
        Attack atk = attackData[attackName];
        
        // Ready up the player if he is not in a ready state.
        if(!isReady) {
            switch(atk.attackType) {

                // If we are dashing or running, ready will not have any stun.
                case Attack.AttackType.run:
                case Attack.AttackType.dash:
                    Ready(true,0f);
                    break;

                default:
                    Ready();
                    return false;

            }
        }

        // If we cannot fire this attack at this time, abort.
        if(!CanFire(attackName)) return false;

        // Cross fade to all animation states in the provided animatorStateTransitionData array.
        foreach(Attack.AnimatorStateTransitionData dat in atk.animatorStateTransitionData) {
            animator.CrossFade(dat.stateName,dat.transitionDuration,animator.GetLayerIndex(dat.layerName),dat.timeOffset,dat.transitionTime);
        }
        
        if(!atk.allowsMovement) DisableMovement("fire_" + attackName, atk.startup + atk.recovery);

        // Disables the previous coroutine first before we start this new attack phase.
        if(attackPhaseCoroutine != null) {
            StopCoroutine(attackPhaseCoroutine);
            attackPhaseCoroutine = null;
        }
        attackPhaseCoroutine = StartCoroutine(AttackPhaseControl(atk));
        currentAttack = atk;

        return true;
    }

    // Checks whether we can fire an attack by the name of <attackName>.
    // If we cannot fire an attack, it is either because we are in attack recovery,
    // or because it cannot be cancelled from another attack. When we implement hitstun,
    // the hitstun check will be worked into this function too.
    public virtual bool CanFire(string attackName = "") {

        if(stunned > 0f) return false;

        // If no attack name is specified, just check if we are in the middle of an attack.
        if(attackName == "") {
            if(attackPhase == AttackPhase.none) return true;
            else return false;
        } 

        Attack atk = attackData[attackName];

        if(attackPhase == AttackPhase.none) {
            
            // If we are not attacking and the attack is cancel only, the attack cannot fire.
            if(atk.cancelOnly) return false;

        } else if(attackPhase == AttackPhase.recovery) {

            // If we are recovering from an attack, the attack is ready to be cancelled.
            // Check whether the current attack can be cancelled into the new <atk>.
            bool cancellable = false;
            foreach(string n in atk.cancelsFrom) {
                if(n == currentAttack.name) {
                    cancellable = true;
                    break;
                }
            }

            if(!cancellable) return false;

        } else { // If the phase is startup or active, abort as we cannot fire the attack again.
            return false;
        }

        return true;
    }

    IEnumerator AttackPhaseControl(Attack attack) {
        float invAtkSpeed = 1/attackSpeed;

        animator.SetFloat(attackSpeedAnimatorParameter,attackSpeed);
        currentAttack = attack;
        attackPhase = AttackPhase.startup;
        if(attack.offsetForces.startup.sqrMagnitude > 0f)
            rigidbody.velocity += attack.offsetForces.startup * Time.fixedDeltaTime * GetMovementFacing();
        yield return new WaitForSeconds(attack.startup * invAtkSpeed);

        attackPhase = AttackPhase.active;
        if(attack.offsetForces.active.sqrMagnitude > 0f)
            rigidbody.velocity += attack.offsetForces.active * Time.fixedDeltaTime * GetMovementFacing();
        StartCoroutine(ExecuteAttack(attack));
        yield return new WaitForSeconds(attack.active * invAtkSpeed);

        attackPhase = AttackPhase.recovery;
        if(attack.offsetForces.recovery.sqrMagnitude > 0f)
            rigidbody.velocity += attack.offsetForces.recovery * Time.fixedDeltaTime * GetMovementFacing();
        yield return new WaitForSeconds(attack.recovery * invAtkSpeed);
        attackPhase = AttackPhase.none;
    }

    // Damage system for Fighter.cs.
    protected virtual IEnumerator ExecuteAttack(Attack attack) {

        // Get arguments to pass into OverlapCollider.
        ContactFilter2D c = new ContactFilter2D();
        Collider2D[] r = new Collider2D[10];

        // Set contact filters.
        c.layerMask = attack.targets;
        c.useLayerMask = true;
        c.useTriggers = true;

        // Track who has been damaged.
        Dictionary<int,List<Actor>> damaged = new Dictionary<int,List<Actor>>();

        // Keep looping through the hitboxes to damage potential targets.
        while(attackPhase == AttackPhase.active) {
            
            foreach(Attack.Hitbox h in attack.hitboxes) {
                
                // Log an error message if a hitbox is not assigned.
                if(h.collider == null) {
                    Debug.LogError(string.Format("No hitbox assigned to {0} for {1}.", attack.name, gameObject.name));
                    continue;
                }
                if(!h.collider.enabled) continue; // If collider is disabled, skip.

                if(h.collider.OverlapCollider(c,r) > 0) {
                    
                    foreach(Collider2D col in r) {

                        // Don't hit ourselves.
                        if(!col) continue;
                        if(col.gameObject == gameObject) continue;

                        Actor a = col.GetComponent<Actor>();
                        if(!a) a = col.GetComponentInParent<Actor>();

                        if(a) {

                            ColliderDistance2D dist = h.collider.Distance(col);
                            Vector3 damagePoint = dist.pointB;

                            bool containsKey = damaged.ContainsKey(h.instance);
                            if(!containsKey || !damaged[h.instance].Contains(a)) {

                                a.Damage(h.damage,gameObject, damagePoint);

                                // Handle knockback on the attacker.
                                if(h.attackerKnockback.sqrMagnitude > 0 || h.targetKnockback.sqrMagnitude > 0) {
                                    float atkDir = Mathf.Sign(col.transform.position.x - transform.position.x);
                                    Vector2 force = Vector2.zero;

                                    // Apply knockback to attacker.
                                    if(h.attackerKnockback.sqrMagnitude > 0) {
                                        force.Set(h.attackerKnockback.x * -atkDir, h.attackerKnockback.y);
                                        Knockback(force * Time.fixedDeltaTime, 0);
                                    }

                                    // Apply knockback to damaged target.
                                    if(h.targetKnockback.sqrMagnitude > 0) {
                                        Pawn p = a as Pawn;
                                        force.Set(h.targetKnockback.x * atkDir, h.targetKnockback.y);
                                        if(p) {
                                            float diff = p.transform.position.x - transform.position.x;
                                            if(h.knockdown) {
                                                if(!p.IsBlocked(diff))
                                                    p.Knockdown(force * Time.fixedDeltaTime);
                                            } else {
                                                p.Knockback(force * Time.fixedDeltaTime, h.hitStun);
                                            }
                                        } else
                                            a.AddForce(force);
                                    }
                                }

                                // Record that this Actor has been damaged by this instance already.
                                if(!containsKey) {
                                    damaged.Add(h.instance,new List<Actor>());
                                }
                                damaged[h.instance].Add(a);
                            }

                        }
                    }
                }

            }

            yield return new WaitForEndOfFrame();

        }
    }

    // Get the current attack phase, in string.
    public virtual string GetAttackPhase() { return nameof(attackPhase); }

    // Called by the controller script of this script. 
    // Takes a string and determines the attack name to be fired.
    public virtual void ReceiveFireInput(string inputName) {
        
        // Loop through all attacks to see if we are pressing a button that activates any one of them.
        foreach(KeyValuePair<string, Attack> atk in attackData) {
            
            switch(atk.Value.attackType) {
                case Attack.AttackType.air:
                    if(!inAir) continue;
                    break;
                case Attack.AttackType.dash:
                    if(!HasMovementModifier("dash_movement_disable")) continue;
                    break;
                case Attack.AttackType.run:
                    if(!HasMovementModifier("run_modifier")) continue;
                    break;
            }

            // If the input is not for this particular attack, then continue on.
            bool isRightAttack = false;
            for(int i = 0;i < atk.Value.inputName.Length;i++) {
                if(atk.Value.inputName[i] == inputName) {
                    isRightAttack = true;
                    break;
                } else isRightAttack = false;
            }
            if(!isRightAttack) continue;
            
            // If the code manages to get here, the conditions have all been met.
            //Fire(atk.Value.name);
            QueueFire(atk.Value.name);
        }
    }

    // Adds an additional condition to CanBlock(). When attack is on-going we can't block!
    public override bool CanBlock() {
        if(attackPhase != AttackPhase.none) return false;
        return base.CanBlock();
    }
}