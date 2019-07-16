using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Actor
 * ==================================
 * This class declares the base properties and methods for all GameObjects that need to
 * be able to take damage (and hence, can die) in the game world.
*/
public class Actor : MonoBehaviour {

    // Defines the DamageFeedback struct for <damageFeedback>.
    [System.Serializable]
    public struct DamageFeedback {
        public bool enabled;
        public float duration;
        public Color32 colour, originalColour;
        public Renderer[] renderers;
    }

    [Header("Health")]
    public bool invulnerable = false;
    public int health = 100, maxHealth = 100;
    public float deathTime = 5f, deathFadeTime = 1.89f;
    public DamageFeedback damageFeedback;
    public GameObject hitEffectPrefab;

    // Only Actor is allowed to control isAlive's value. Children and subclasses cannot.
    bool isAlive = true;
    public bool IsAlive() { return isAlive; }

    protected new Rigidbody2D rigidbody; // For knockback.
    protected Animator animator;

    protected virtual void Init() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        deathFadeTime = Mathf.Min(deathFadeTime,deathTime);
    }

    // Fills up any variables that we need to fill.
    protected virtual void Autofill() {
        Renderer[] sr = GetComponents<Renderer>(), 
                         children = GetComponentsInChildren<Renderer>(true);
        List<Renderer> r = new List<Renderer>();
        r.AddRange(sr);
        r.AddRange(children);
        damageFeedback.enabled = true;
        damageFeedback.renderers = r.ToArray();
        damageFeedback.colour = new Color32(255,0,0,255);
        damageFeedback.originalColour = new Color32(255,255,255,255);
        damageFeedback.duration = 0.12f;
    }

    // Deal damage to this Actor. Takes an optional MonoBehaviour object to allow one to specify who dealt
    // the damage. Returns the amount of damage dealt (if there are reductions).
    public virtual int Damage(int amount, GameObject instigator = null, Vector3? location = null) {

        if (!invulnerable) {
            health = Mathf.Max(0, health - amount);
        }

        // Show damage feedback.
        if (damageFeedback.renderers.Length > 0 && damageFeedback.enabled) {
            StartCoroutine(DamageFlash(amount,instigator,location));
        }

        // If a hit effect is specified, play it.
        if(hitEffectPrefab) {
            Vector3 targLoc;
            if(location != null) targLoc = (Vector3)location;
            else targLoc = transform.position;
            Instantiate(hitEffectPrefab, targLoc, transform.rotation);
        }

        // Handle player dying.
        if (health <= 0) {
            if (isAlive)
            {
                //Death(instigator); Disabled this so I can spawn my player at the last checkpoint
                Protagonist p = GetComponent<Protagonist>();
                transform.position = p.Current_SpawnPoint;
            }
        } else if(health > maxHealth) health = maxHealth;

        return amount;
    }

    // Damage() calls this function when the Actor dies. You can also manually call this function to 
    // kill off the Actor.
    public virtual void Death(GameObject instigator = null) {
        isAlive = false;
        health = 0;
        Destroy(gameObject,deathTime);
        if (deathFadeTime > 0)
            StartCoroutine(DeathFade(damageFeedback.renderers,deathTime - deathFadeTime,deathFadeTime));
    }

    // Wrapper method to add force to an Actor with Rigidbody.
    public virtual void AddForce(Vector2 force, Vector2? position = null, ForceMode2D mode = ForceMode2D.Force) {
        if(!rigidbody) return;
        if(position != null)
            rigidbody.AddForceAtPosition(force, (Vector2)position, mode);
        else
            rigidbody.AddForce(force,mode);
    }

    // For handling the hit flash when a character receives damage.
    protected virtual IEnumerator DamageFlash(int amount, GameObject instigator = null, Vector3? location = null) {
        foreach(Renderer sr in damageFeedback.renderers) {
            if(sr is SpriteRenderer) (sr as SpriteRenderer).color = damageFeedback.colour;
            else sr.material.color = damageFeedback.colour;
        }
        yield return new WaitForSeconds(damageFeedback.duration);
        foreach(Renderer sr in damageFeedback.renderers) {
            if(sr is SpriteRenderer) (sr as SpriteRenderer).color = damageFeedback.originalColour;
            else sr.material.color = damageFeedback.originalColour;
        }
    }

    protected virtual IEnumerator DeathFade(Renderer[] objects, float delay, float duration) {
        yield return new WaitForSeconds(delay); // Wait before destroying.

        // Fade this object away.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float dur = duration, opacity;
        while(dur > 0) {
            opacity = dur / deathFadeTime;
            foreach(Renderer sr in objects) {
                if(sr is SpriteRenderer) {
                    SpriteRenderer src = sr as SpriteRenderer;
                    src.color = new Color(src.color.r, src.color.g, src.color.b, opacity);
                } else {
                    sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, opacity);
                }
            }
            dur -= Time.deltaTime;
            yield return w;
        }
    }
    
}