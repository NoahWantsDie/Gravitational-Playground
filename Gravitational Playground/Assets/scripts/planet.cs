using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planet : MonoBehaviour
{
    public GameObject[] bodies;

    public float mass;
    public Vector2 Pos;
    public Vector2 Vel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //positioning
        Pos += Vel * Time.deltaTime;
        transform.position = Pos;

        //set size relative to mass\
        float planetRadius;
        planetRadius = Mathf.Sqrt(mass/Mathf.PI);
        transform.localScale = new Vector2(planetRadius,planetRadius);
    }
}
