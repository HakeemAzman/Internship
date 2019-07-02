﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : Pawn {

    [Header("Protagonist")]
    public bool canSwitch = false;

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
                name = "Thief", mesh = transform.Find("Thief_BlockUp").gameObject,
                movementModifier = new MovementModifier()
            },
            new Form() {
                name = "Gladiator", mesh = transform.Find("Gladiator_BlockUp").gameObject,
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

        G_Platforms = GameObject.FindGameObjectsWithTag("G_Plat");
    }

    void FixedUpdate() {
        UpdateInAir();
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
        if(nextForm.name == "Thief")
        {
            foreach (GameObject GP in G_Platforms)
            {
                GP.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            foreach (GameObject GP in G_Platforms)
            {
                GP.GetComponent<MeshRenderer>().enabled = false;
            }

        }

        currentFormIndex = nextFormIndex;
        return currentFormIndex;
    }
}
