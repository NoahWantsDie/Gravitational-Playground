using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explsion_particle : MonoBehaviour
{
    public float radius;
    public float t;
    public float ExplodeSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        t = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(0, radius*10, t);
        if (t <= 1)
        {
            t += ExplodeSpeed * Time.deltaTime;
        } else
        {
            Destroy(transform.root.gameObject);
        }
        
        
        
    }
    void explode()
    {
        
        //
        
    }
}
