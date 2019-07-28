using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Protagonist : Pawn {

    [Header("Score")]
    [SerializeField] static int gemsCollected;

    [Header("Protagonist")]
    public bool canSwitch = false;
    public Attack2D[] attacks = new Attack2D[1];
    public Collider2D[] hitboxes = new Collider2D[1];
    protected Attack2D currentAttack;

    // Container for <attacks>. Easier to access since its a name to attack pairing.
    readonly Dictionary<string,Attack2D> attackData = new Dictionary<string, Attack2D>();

    [Header("Knockback Settings")]
    public float Knockback_Amount;
    [HideInInspector] public bool isKnockbackRight;

    [Header("Spawn Points")]
    public Vector3 currentSpawnPoint;

    [Header("Thief/Gladiator Animators")]
    public Animator thiefAnim;
    public Animator gladiatorAnim;

    [System.Serializable]
    public class Form {
        public string name;
        public GameObject mesh;
        public MovementModifier movementModifier;
    }

    public Form[] forms;
    protected int currentFormIndex;

    void Reset() {
        Autofill();
        forms = new Form[2] {
            new Form() {
                name = "Thief", mesh = transform.Find("Thief").gameObject,
                movementModifier = new MovementModifier()
            },
            new Form() {
                name = "Gladiator", mesh = transform.Find("Gladiator").gameObject,
                movementModifier = new MovementModifier() {
                    jumpAccelerationMultiplier = 0, maxLandSpeedMultiplier = 0.34f
                }
            }
        };
    }

    void Start() {
        Init();

        // Populate the <attackData> variable, and arm its events.
        for(int i = 0; i < attacks.Length; i++) {
            attackData.Add(attacks[i].name, attacks[i]);
            attacks[i].onStartup = OnAttackStartup;
            attacks[i].onActive = OnAttackActive;
            attacks[i].onRecovery = OnAttackRecovery;
            attacks[i].onEnd = OnAttackEnded;
            attacks[i].onDamage = OnAttackDamage;
        }

        // Set the current form. If the <forms> array is not set, show an error.
        if(forms == null) Debug.LogError("Please reset the Protagonist component to fill in the Forms field.");

        // Apply effects of current form.
        Switch(0);
    }

    public override float Move(float dir = 0) {
        // Do not allow movement if we are currently attacking.
        if(currentAttack) return 0;
        return base.Move(dir);
    }

    #region Attack Functionality
    // Executes the attack.
    public void Attack(Attack2D atk) {
        if(currentMovementFacing == 0)
            Debug.LogError("Can't attack because there is no facing direction.");

        currentAttack = atk;
        Quaternion direction = currentMovementFacing > 0 ? Quaternion.LookRotation(Vector3.right) : Quaternion.LookRotation(-Vector3.right);
        atk.Execute(this, hitboxes, direction);
    }

    public void OnAttackStartup(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        gladiatorAnim.SetTrigger("attack");
    }

    public void OnAttackActive(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        hitboxes[0].enabled = true;
    }

    public void OnAttackRecovery(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        hitboxes[0].enabled = false;
    }

    public void OnAttackEnded(MonoBehaviour instigator, Collider2D[] hitboxes, Quaternion direction, float attackSpeed) {
        currentAttack = null;
    }

    public void OnAttackDamage(int amount, MonoBehaviour target, MonoBehaviour instigator, Vector3? damageLocation) {
        Actor a = target.GetComponent<Actor>();
        if(a) {
            a.Damage(amount,instigator.gameObject);
        }
    }

    // Picks out the most suitable attack from a given <inputName>.
    // For PlayerController to use.
    public void ReceiveAttackInput(string inputName) {

        // Ignore this if we are not in Gladiator form.
        if(!gladiatorAnim.gameObject.activeInHierarchy) return;

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
    #endregion

    void FixedUpdate() {
        UpdateInAir();
        // The conditions prevent the code from updating the animator if GameObject is inactive.
        if(thiefAnim.gameObject.activeInHierarchy) UpdateThiefAnimator();
        else if(gladiatorAnim.gameObject.activeInHierarchy) UpdateGladiatorAnimator();
    }

    public virtual int Switch(int formIndex = -1) {
        
        if(!canSwitch) return currentFormIndex;

        int nextFormIndex = formIndex;
        if(nextFormIndex == -1)
            nextFormIndex = (currentFormIndex+1) % forms.Length; // nextFormIndex never exceeds 1.

        // Get references to the form based on index.
        Form currForm = forms[currentFormIndex],
             nextForm = forms[nextFormIndex];

        // Disables the current form mesh and enables the next.
        currForm.mesh.SetActive(false);
        nextForm.mesh.SetActive(true);

        // Apply movement modifier of new form
        // (the old one is automatically overwritten as they have the same name).
        AddMovementModifier("form_movement_modifier", forms[nextFormIndex].movementModifier);
        
        // Shows / hides the platforms.
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("G_Plat");
        if (nextForm.name == "Thief") {
            foreach (GameObject GP in platforms) {
                MeshRenderer mr = GP.GetComponent<MeshRenderer>();
                if(mr) mr.enabled = false;
            }
        } else {
            foreach (GameObject GP in platforms) {
                MeshRenderer mr = GP.GetComponent<MeshRenderer>();
                if(mr) mr.enabled = true;
            }
        }

        currentFormIndex = nextFormIndex;
        return currentFormIndex;
    }

    // Overrides the default death behaviour.
    public override void Death(GameObject instigator = null) {
        //base.Death(instigator);
        transform.position = currentSpawnPoint;
    }

    #region OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D enter) {

        if(enter.CompareTag("Enemy")) {
            Knockback();
            Damage(1);
        } else if(enter.CompareTag("Death")) {
            Damage(maxHealth);
        } else if(enter.CompareTag("Gem")) {
            gemsCollected++;
            PlayerPrefs.SetInt("Highscore", gemsCollected);
            Destroy(enter.gameObject);
        } else if(enter.CompareTag("Spawner")) {
            currentSpawnPoint = enter.gameObject.transform.position;
            enter.gameObject.GetComponentInChildren<Light>().color = Color.green;
            enter.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        } else if(enter.CompareTag("LoadPrev")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        } else if(enter.CompareTag("LoadNext")) {
            PlayerPrefs.SetFloat("PlayerX", enter.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", enter.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", enter.transform.position.z);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    #endregion

    void Knockback() {
        if(health > 1) {
            if (isKnockbackRight)
                rigidbody.velocity = new Vector2(-Knockback_Amount, Knockback_Amount + 0.2f);

            if (!isKnockbackRight)
                rigidbody.velocity = new Vector2(Knockback_Amount, Knockback_Amount + 0.2f);
        }
    }

    void UpdateThiefAnimator() {
        float vx = rigidbody.velocity.x;
        if (vx < -0.1f) vx = 5f;
        thiefAnim.SetFloat("forwardSpeed", vx);
        thiefAnim.SetBool("isGrounded",inAir);
    }
    
    void UpdateGladiatorAnimator() {
        float vx = rigidbody.velocity.x;
        if (vx < -0.1f) vx = 1.7f;
        gladiatorAnim.SetFloat("forwardSpeed", vx);
    }
}
