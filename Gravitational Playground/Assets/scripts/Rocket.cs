using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    const float G = 12;
    public Vector2 InitialVel;
    public float mass;
    public Rigidbody2D rb;
    public bool locked;
    public float thrust;
    public float rotSpeed;
    public ParticleSystem ps;
    

    void Start()
    {
        rb.AddForce(InitialVel * 10);
        
        
        
    }
    void FixedUpdate()
    {
        var em = ps.emission;
        Body[] bodies = FindObjectsOfType<Body>();
        if (locked)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        foreach (Body body in bodies)
        {
            //if (body != this)
            //Gravitate(body);
        }

        rb.mass = mass;
        
        //positioning
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(thrust * transform.up * Time.deltaTime);
            em.enabled = true;
        }
        else
        {
            em.enabled = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(thrust * -transform.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += Vector3.forward * -rotSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles += Vector3.forward * rotSpeed;
        }

    }
    void Gravitate(Body objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract.rb;
        Vector2 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        float forceMagnitude = G * ((rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2));
        Vector2 force = direction.normalized * forceMagnitude;
        rbToAttract.AddForce(force);
    }
    
    // Start is called before the first frame update
    

    // Update is called once per frame
    
}