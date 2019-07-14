using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Transform Start_Point, End_Point;
    LineRenderer Laser_Beam;
    [SerializeField] float Laser_Width, Active_Time;
    public GameObject Death_Collider;
    
    bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        Laser_Beam = GetComponent<LineRenderer>();
        Laser_Beam.SetWidth(Laser_Width, Laser_Width);
    }

    // Update is called once per frame
    void Update()
    {
        /*float timer = Active_Time;
        timer -= Time.deltaTime;
        print(timer);*/

        Laser_Beam.SetPosition(0, Start_Point.position);
        Laser_Beam.SetPosition(1, End_Point.position);
    }
}
