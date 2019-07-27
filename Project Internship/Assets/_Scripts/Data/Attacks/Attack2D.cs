using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Game/Attack 2D",order = 10000)]
public class Attack2D : ScriptableObject {
    
    [System.Serializable]
    public struct Movement { public Vector2 startup, active, recovery; }

    [System.Serializable]
    public struct HitboxData {
        public int index, damage, instance; // Damage that the hitbox deals.
        public Vector2 attackerKnockback, targetKnockback;
        public float hitStun;
    }

    public enum Phase { none, startup, active, recovery };
    
    // All events related to attacks.
    public delegate void Event(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed);
    public delegate void DamageEvent(int amount, MonoBehaviour instigator, Vector3 damageLocation);
    public Event onStartup, onActive, onRecovery;
    public DamageEvent onDamage;

    [Header("Input")]
    public new string name;
    public string[] inputName = new string[1];

    [Header("Properties")]
    public HitboxData[] hitboxData = new HitboxData[1];
    public float startup = 0.2f, active = 0.2f, recovery = 0.2f;
    public Movement movement; // If this attack moves the character, key in the forces here.
    public LayerMask targets; // Valid targets in layers to be hit by this attack.

    protected Phase phase = Phase.none;

    public virtual bool Execute(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed = 1f) {
        if(hitboxes.Length <= 0) return false;

        instigator.StartCoroutine(PhaseControl(instigator, hitboxes, direction, attackSpeed));
        return true;
    }

    protected virtual IEnumerator PhaseControl(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        float f = 1 / attackSpeed;
        Rigidbody2D rb = instigator.GetComponent<Rigidbody2D>();

        // Do startup phase actions.
        phase = Phase.startup;
        onStartup(instigator, hitboxes, direction, attackSpeed);
        if(movement.startup.sqrMagnitude > 0 && rb) {
            rb.velocity += (Vector2)(direction * movement.startup) * Time.fixedDeltaTime;
        }
        yield return new WaitForSeconds(startup * f);

        // Do active phase actions.
        phase = Phase.active;
        onActive(instigator, hitboxes, direction, attackSpeed);
        if(movement.active.sqrMagnitude > 0 && rb) {
            rb.velocity += (Vector2)(direction * movement.active) * Time.fixedDeltaTime;
        }
        instigator.StartCoroutine(Hit(instigator,hitboxes));
        yield return new WaitForSeconds(active * f);

        // Do recovery phase actions.
        phase = Phase.recovery;
        onRecovery(instigator, hitboxes, direction, attackSpeed);
        if(movement.recovery.sqrMagnitude > 0 && rb) {
            rb.velocity += (Vector2)(direction * movement.recovery) * Time.fixedDeltaTime;
        }
        yield return new WaitForSeconds(recovery * f);

        phase = Phase.none;
    }

    protected virtual IEnumerator Hit(MonoBehaviour instigator, Collider2D[] hitboxes) {
        // Get arguments to pass into OverlapCollider.
        ContactFilter2D c = new ContactFilter2D();
        Collider2D[] r = new Collider2D[10];

        // Set contact filters.
        c.layerMask = targets;
        c.useLayerMask = true;
        c.useTriggers = true;

        // Track who has been damaged.
        Dictionary<int,List<MonoBehaviour>> damaged = new Dictionary<int,List<MonoBehaviour>>();

        // Keep looping through the hitboxes to damage potential targets.
        while(phase == Phase.active) {
            
            foreach(HitboxData h in hitboxData) {
                
                // Log an error message if a hitbox is not assigned.
                if(hitboxes[h.index] == null) {
                    Debug.LogError(string.Format("No hitbox assigned to {0} for {1}.", name, instigator.gameObject.name));
                    continue;
                }
                if(!hitboxes[h.index].enabled) continue; // If collider is disabled, skip.

                if(hitboxes[h.index].OverlapCollider(c,r) > 0) {
                    
                    foreach(Collider2D col in r) {

                        // Don't hit ourselves.
                        if(!col) continue;
                        if(col.gameObject == instigator.gameObject) continue;

                        MonoBehaviour a = col.GetComponent<MonoBehaviour>();
                        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                        if(!a) a = col.GetComponentInParent<Actor>();

                        if(a) {

                            ColliderDistance2D dist = hitboxes[h.index].Distance(col);
                            Vector3 damagePoint = dist.pointB;

                            bool containsKey = damaged.ContainsKey(h.instance);
                            if(!containsKey || !damaged[h.instance].Contains(a)) {

                                onDamage(h.damage, instigator, damagePoint);

                                // Handle knockback on the attacker.
                                if(h.attackerKnockback.sqrMagnitude > 0 || h.targetKnockback.sqrMagnitude > 0) {
                                    float atkDir = Mathf.Sign(col.transform.position.x - instigator.transform.position.x);
                                    Vector2 force = Vector2.zero;

                                    // Apply knockback to attacker.
                                    if(h.attackerKnockback.sqrMagnitude > 0) {
                                        force.Set(h.attackerKnockback.x * -atkDir, h.attackerKnockback.y);
                                        if(rb) rb.AddForce(force * Time.fixedDeltaTime);
                                    }

                                    // Apply knockback to damaged target.
                                    if(h.targetKnockback.sqrMagnitude > 0) {
                                        Pawn p = a as Pawn;
                                        force.Set(h.targetKnockback.x * atkDir, h.targetKnockback.y);
                                        if(p) {
                                            float diff = p.transform.position.x - instigator.transform.position.x;
                                            if(rb) rb.AddForce(force * Time.fixedDeltaTime);
                                        } else
                                            if(rb) rb.AddForce(force * Time.fixedDeltaTime);
                                    }
                                }

                                // Record that this Actor has been damaged by this instance already.
                                if(!containsKey) {
                                    damaged.Add(h.instance,new List<MonoBehaviour>());
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

}
