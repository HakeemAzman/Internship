using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Transform Start_Point, End_Point;
    LineRenderer Laser_Beam;
    [SerializeField] float Laser_Width, timeSet;
    float Active_Time, Cooldown_time;
    public GameObject Essential_Holder;

    // Start is called before the first frame update
    void Start()
    {
        Laser_Beam = GetComponent<LineRenderer>();
        Laser_Beam.SetWidth(Laser_Width, Laser_Width);
        Active_Time = timeSet;
    }

    // Update is called once per frame
    void Update()
    {
        Laser_Beam.SetPosition(0, Start_Point.position);
        Laser_Beam.SetPosition(1, End_Point.position);

        Active_Time -= Time.deltaTime;
        
        if(Active_Time <= 0)
        {
            Laser_Beam.enabled = false;
            Essential_Holder.SetActive(false);
            

            Cooldown_time += Time.deltaTime;
            Active_Time = 0;
        }

        if (Cooldown_time >= timeSet)
        {
            Laser_Beam.enabled = true;
            Essential_Holder.SetActive(true);

            Active_Time = timeSet;
            Cooldown_time = 0;
        }
    }
}
