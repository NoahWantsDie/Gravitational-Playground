using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlel : MonoBehaviour
{
    private float t;
    public float deletespeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        t = 1;
    }

    // Update is called once per frame
    void Update()
    {
        t -= deletespeed * Time.deltaTime;
        if (t <= 0)
        {
            Destroy(transform.root.gameObject);
        }
        
    }
}
