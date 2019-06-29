using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : Pawn {

    Protagonist alternateForm;
    GameObject thiefMesh, gladiatorMesh;

    public bool canSwitch = false;

    protected override void Init() {
        base.Init();
        if(this is Thief) alternateForm = GetComponent<Gladiator>();
        else alternateForm = GetComponent<Thief>();

        thiefMesh = transform.Find("Thief_BlockUp").gameObject;
        gladiatorMesh = transform.Find("Gladiator_BlockUp").gameObject;
    }

    public virtual Protagonist Switch() {
        
        if(!canSwitch) return this;

        if(this is Thief) {
            thiefMesh.SetActive(false);
            gladiatorMesh.SetActive(true);
        } else {
            thiefMesh.SetActive(true);
            gladiatorMesh.SetActive(false);
        }

        alternateForm.health = health;
        alternateForm.desiredRotation = desiredRotation;
        alternateForm.enabled = true;
        alternateForm.canSwitch = true;

        enabled = false;

        return alternateForm;
    }
}
