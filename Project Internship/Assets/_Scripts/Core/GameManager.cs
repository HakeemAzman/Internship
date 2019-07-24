using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject player;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        player = GameObject.FindWithTag("Player");

        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("PlayerZ"))
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX") - 1, PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
    }

    private void Update()
    {
        print(PlayerPrefs.GetInt("Highscore"));
    }
}
