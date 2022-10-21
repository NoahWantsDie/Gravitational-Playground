using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public float G = 12;
    public Vector2 InitialVel;
    public float mass;
    public Rigidbody2D rb;
    public bool locked;
    public float radius;

    public string planetType;

    public SpriteRenderer SP;
    public bool notAttractOther;

    public TrailRenderer trailRend;
    void FixedUpdate()
    {
        Body[] bodies = FindObjectsOfType<Body>();
        Rocket[] rockets = FindObjectsOfType<Rocket>();
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
            if (body != this)
            {
                if (!notAttractOther)
                {
                    Gravitate(body);
                }
                
            }
            
        }
        foreach (Rocket rocket in rockets)
        {

            GravitateRocket(rocket);
        }

        rb.mass = mass;
        radius = Mathf.Sqrt(mass/Mathf.PI);
        transform.localScale = new Vector2(radius,radius);
        //positioning

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
    void GravitateRocket(Rocket objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract.rb;
        Vector2 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        float forceMagnitude = G * ((rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2));
        Vector2 force = direction.normalized * forceMagnitude;
        
        rbToAttract.AddForce(force);
    }
    

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(InitialVel*10);
        SP = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((mass < 60))
        {
            planetType = "Terrestrial";
        }
        else if ((mass >= 60) && (mass < 200))
        {
            planetType = "GasGiant";
        }
        else if (mass >= 200)
        {
            planetType = "Star";
        }

        if (planetType == "Terrestrial")
        {
            SP.color = new Color(0.5283019f, 0.5283019f, 0.5283019f);
        } else if(planetType == "GasGiant")
        {
            SP.color = new Color(0.549556f, 0.3852794f, 0.6981132f);
        }
        else if(planetType == "Star")
        {
            SP.color = new Color(0.9824101f, 1f, 0.75f);
        }
    }
}