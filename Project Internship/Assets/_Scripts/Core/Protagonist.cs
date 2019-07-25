using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Protagonist : Pawn
{
    [Header("Score")]
    [SerializeField] static int Gems_Collected;

    [Header("Protagonist")]
    public bool canSwitch = false;
    Rigidbody2D rb2D;

    [Header("Knockback Settings")]
    public float Knockback_Amount;
    [HideInInspector] public bool isKnockbackRight;

    [Header("Spawn Points")]
    public Vector3 Current_SpawnPoint;

    [Header("Thief/Gladiator Animators")]
    public Animator thiefAnim;
    public Animator gladiatorAnim;

    GameObject[] G_Platforms;

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

    private void Start() {
        Init();

        // Set the current form. If the <forms> array is not set, show an error.
        if(forms == null) Debug.LogError("Please reset the Protagonist component to fill in the Forms field.");

        // Apply effects of current form.
        Switch(0);

        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        UpdateInAir();
        UpdateThiefAnimator();
        UpdateGladiatorAnimator();
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
        if (nextForm.name == "Thief")
        {
            G_Platforms = GameObject.FindGameObjectsWithTag("G_Plat");

            foreach (GameObject GP in G_Platforms)
            {
                GP.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject GP in G_Platforms)
            {
                GP.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        currentFormIndex = nextFormIndex;
        return currentFormIndex;
    }

    #region OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D enter)
    {
        if(enter.CompareTag("Enemy"))
        {
            Knockback();
            Damage(1);
        }

        if(enter.CompareTag("Death"))
        {
            Damage(maxHealth);
        }

        if(enter.CompareTag("Gem"))
        {
            Gems_Collected++;
            PlayerPrefs.SetInt("Highscore", Gems_Collected);
            Destroy(enter.gameObject);
        }

        if(enter.CompareTag("Spawner"))
        {
            Current_SpawnPoint = enter.gameObject.transform.position;
            enter.gameObject.GetComponentInChildren<Light>().color = Color.green;
            enter.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        if(enter.CompareTag("LoadPrev"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if(enter.CompareTag("LoadNext"))
        {
            PlayerPrefs.SetFloat("PlayerX", enter.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", enter.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", enter.transform.position.z);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    #endregion

    void Knockback()
    {
        if(health > 1)
        {
            if (isKnockbackRight)
                rb2D.velocity = new Vector2(-Knockback_Amount, Knockback_Amount + 0.2f);

            if (!isKnockbackRight)
                rb2D.velocity = new Vector2(Knockback_Amount, Knockback_Amount + 0.2f);
        }
    }

    void UpdateThiefAnimator()
    {
        float velocity = thiefAnim.GetComponentInParent<Rigidbody2D>().velocity.x;

        if (velocity < -0.1f) velocity = 5f;

        thiefAnim.GetComponent<Animator>().SetFloat("forwardSpeed", velocity);
    }
    
    void UpdateGladiatorAnimator()
    {
        float velocity = gladiatorAnim.GetComponentInParent<Rigidbody2D>().velocity.x;

        if (velocity < -0.1f) velocity = 1.7f;

        gladiatorAnim.GetComponent<Animator>().SetFloat("forwardSpeed", velocity);
    }
}
