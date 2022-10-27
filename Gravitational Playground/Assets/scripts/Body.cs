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
    public GameObject mainObject;
    public main MainScript;
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
        MainScript = mainObject.GetComponent<main>();
        transform.position = MainScript.NewPos;
        Debug.Log(MainScript.NewPos + "body Scrpt");
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
        else if ((mass >= 200) && (mass < 700))
        {
            planetType = "Star: M";
        }
        else if ((mass >= 700) && (mass < 1000))
        {
            planetType = "Star: K";
        }
        else if ((mass >= 1000) && (mass < 1500))
        {
            planetType = "Star: G";
        }
        else if ((mass >= 1500) && (mass < 2000))
        {
            planetType = "Star: F";
        }
        else if ((mass >= 2000) && (mass < 10000))
        {
            planetType = "Star: A";
        }
        else if ((mass >= 10000))
        {
            planetType = "Star: B";
        }


        if (planetType == "Terrestrial")
        {
            SP.color = new Color(0.5283019f, 0.5283019f, 0.5283019f);
        } else if(planetType == "GasGiant")
        {
            SP.color = new Color(0.549556f, 0.3852794f, 0.6981132f);
        }
        else if (planetType == "Star: M")
        {
            SP.color = new Color(0.8207547f, 0.1465867f, 0.1122731f);
        }
        else if (planetType == "Star: K")
        {
            SP.color = new Color(1f, 0.4533608f, 0f);
        }
        else if(planetType == "Star: G")
        {
            SP.color = new Color(1f, 0.8952988f, 0.2783019f);
        }
        else if (planetType == "Star: F")
        {
            SP.color = new Color(1f, 0.9931104f, 0.7877358f);
        }
        else if (planetType == "Star: A")
        {
            SP.color = new Color(1f, 1f, 1f);
        }
        else if (planetType == "Star: B")
        {
            SP.color = new Color(0.8160377f, 1f, 1f);
        }
    }
}