using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    #region Variables
    [SerializeField] Protagonist ps;

    [SerializeField] GameObject Start_Cutscene, lamp, fireparticle, fireparticle2, fireparticle3;

    [SerializeField] Light Spotlight_lamp;

    [SerializeField] Animator rocks, hidden_door;

    [SerializeField] string rockFall, hiddenDoorMove;
    float r = 255f, g = 128f, b = 124f;
    //FF807C
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ps.gameObject.GetComponent<Protagonist>().canSwitch = true;
            StartCoroutine(StartFirstCutscene());

        }
    }

    IEnumerator StartFirstCutscene()
    {
        Start_Cutscene.SetActive(true);
        lamp.GetComponent<MeshRenderer>().enabled = false;
        Spotlight_lamp.gameObject.GetComponent<Light>().color = Color.red;

        yield return new WaitForSeconds(1f);

        fireparticle.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        fireparticle2.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        fireparticle3.SetActive(false);
        
        yield return new WaitForSeconds(0.5f);

        rocks.gameObject.GetComponent<Animator>().Play(rockFall);

        yield return new WaitForSeconds(2.5f);

        hidden_door.gameObject.GetComponent<Animator>().Play(hiddenDoorMove);

        yield return new WaitForSeconds(2f);
        
        Destroy(this.gameObject);
    }
}
