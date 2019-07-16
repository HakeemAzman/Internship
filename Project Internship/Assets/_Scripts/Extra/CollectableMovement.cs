using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMovement : MonoBehaviour
{
    Vector3 Start_Pos = new Vector3();
    Vector3 Float_Pos = new Vector3();

    void Start()
    {
        Start_Pos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(50, 10, 50) * Time.deltaTime, Space.Self);

        Float_Pos = Start_Pos;
        Float_Pos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 1f) * 0.1f;

        transform.position = Float_Pos;
    }
}
