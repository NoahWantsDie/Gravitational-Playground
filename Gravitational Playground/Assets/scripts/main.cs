using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour
{
    public GameObject sun;
    public GameObject particle;

    public Vector2 mousePos;
    public Vector2 mousePosWorld;

    private Camera cam;

    public GameObject focusObj;
    public GameObject explosion;

    public Vector2 mouseDownPoint;
    public Vector2 slingShotPoint;
    public CameraMove CamScript;

    public LineRenderer line;
    private Vector3[] points;
    public Vector2 dragClickForce;

    public bool usingSlider;

    public Text massTxt;
    
    public float newMass = 0.5f;

    public List<GameObject> bodies = new List<GameObject>();
    public List<GameObject> particles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        line.enabled = false;
        CamScript = Camera.main.GetComponent<CameraMove>();
        bodies.Add(sun);
        particles.Add(particle);
        
    }
    float WeightedAverage(float num1, float weight1, float num2, float weight2)
    {
        var averag = ((num1 * weight1) + (num2 * weight2)) / (weight1 + weight2);
        return (averag);
    }
    // Update is called once per frame
    void Update()
    {
        massTxt.text = "New Mass : " + newMass.ToString();
        CamScript.focusedObject = focusObj;
        mousePos = Input.mousePosition;
        mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragClickForce = mouseDownPoint - mousePosWorld;
        slingShotPoint = (mouseDownPoint - mousePosWorld) + mouseDownPoint;
        
        line.SetPosition(0, mouseDownPoint);
        line.SetPosition(1, slingShotPoint);

        if (Input.GetMouseButtonDown(0))
        {
            if (!usingSlider)
            {
                mouseDownPoint = mousePosWorld;
                mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                slingShotPoint = (mouseDownPoint - mousePosWorld) + mouseDownPoint;

                line.SetPosition(0, mouseDownPoint);
                line.SetPosition(1, slingShotPoint);

                line.enabled = true;
            }

            //SpawnNewBody(mousePosWorld.x, mousePosWorld.y, 0, 0, 10);
            //Debug.Log("clicked");
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!usingSlider && line.enabled)
            {
                SpawnNewBody(mouseDownPoint.x, mouseDownPoint.y, dragClickForce.x, dragClickForce.y, newMass, false);
                Debug.Log("clicked");
                line.enabled = false;
                line.SetPosition(0, new Vector3(0, 0, 0));
                line.SetPosition(1, new Vector3(0, 0, 0));
            }
            
        }
        
        if (usingSlider)
            line.enabled = false;

        for (int i = 0; i < bodies.Count; i++)
        {
            for (int j = 0; j < bodies.Count; j++)
            {
                if ((i != j))
                {
                    var b1 = bodies[i];
                    var b2 = bodies[j];
                    
                    var b1Script = b1.GetComponent<Body>();
                    var b2Script = b2.GetComponent<Body>();
                    var distanceToMouse = Mathf.Pow(Mathf.Pow((b1.transform.position.x - mousePosWorld.x), 2) + Mathf.Pow((b1.transform.position.y - mousePosWorld.y), 2), 0.5f);

                    var PointDistance = Mathf.Pow(Mathf.Pow((b1.transform.position.x - b2.transform.position.x),2)  + Mathf.Pow((b1.transform.position.y - b2.transform.position.y),2),0.5f);
                    if((b1Script.notAttractOther == false)&& (b2Script.notAttractOther == false))
                    {
                        if (PointDistance < ((b2Script.radius + b1Script.radius) / 2))
                        {
                            float NewPointX = WeightedAverage(b1.transform.position.x, b1Script.mass, b2.transform.position.x, b2Script.mass);
                            float NewPointY = WeightedAverage(b1.transform.position.y, b1Script.mass, b2.transform.position.y, b2Script.mass);

                            float NewVelX = WeightedAverage(b1Script.rb.velocity.x, b1Script.mass, b2Script.rb.velocity.x, b2Script.mass);
                            float NewVelY = WeightedAverage(b1Script.rb.velocity.y, b1Script.mass, b2Script.rb.velocity.y, b2Script.mass);

                            //SpawnNewBody(NewPointX, NewPointY, NewVelX, NewVelY, b1Script.mass + b2Script.mass, false);
                            //Debug.Log(b2Script.mass + b1Script.mass);
                            if (b1Script.mass >= b2Script.mass)
                            {
                                b1Script.transform.position = new Vector2(NewPointX, NewPointY);
                                b1Script.transform.position = new Vector2(NewPointX, NewPointY);
                                b1Script.rb.velocity = new Vector2(NewVelX, NewVelY);
                                b1Script.mass = b1Script.mass + b2Script.mass;
                                Destroy(b2);
                                bodies.Remove(b2);
                            }
                            else
                            {


                                b2Script.rb.velocity = new Vector2(NewVelX, NewVelY);
                                b2Script.mass = b1Script.mass + b2Script.mass;
                                Destroy(b1);
                                bodies.Remove(b1);
                            }


                        }
                        if ((distanceToMouse <= b1Script.radius) && (Input.GetMouseButtonDown(2)))
                        {
                            if (b1 != bodies[0])
                            {

                                GameObject Explosion = Instantiate(explosion);
                                explsion_particle particleScript = Explosion.GetComponent<explsion_particle>();
                                particleScript.radius = b1Script.radius;
                                Explosion.SetActive(true);
                                Explosion.transform.position = b1.transform.position;
                                //createRingObj(b1, 0, b1Script.radius, Mathf.RoundToInt(b1Script.mass / 5));
                                Destroy(b1);
                                bodies.Remove(b1);
                            }
                        }
                    }
                    
                    
                }
            }
        }
    }
    void SpawnNewBody(float x, float y, float xv, float yv, float m,bool isFocused)
    {
        
        //Debug.Log("planet spawned at " + new Vector2(x,y));
        GameObject newPlanet = Instantiate(sun);
        newPlanet.SetActive(true);
        Body newPlanetScript = newPlanet.GetComponent<Body>();
        newPlanetScript.locked = false;
        newPlanetScript.rb.bodyType = RigidbodyType2D.Dynamic;
        newPlanet.transform.position = new Vector2(x, y);
        newPlanetScript.mass = m;

        

        if (isFocused)
            focusObj = newPlanet;

        newPlanetScript.rb.velocity = new Vector2(xv, yv);
        bodies.Add(newPlanet);
    }
    void SpawnNewParticle(float x, float y, float xv, float yv, float m, bool isFocused)
    {

        //Debug.Log("planet spawned at " + new Vector2(x,y));
        GameObject newParticle = Instantiate(particle);
        newParticle.SetActive(true);
        Body newParticleScript = newParticle.GetComponent<Body>();
        newParticleScript.locked = false;
        newParticleScript.rb.bodyType = RigidbodyType2D.Dynamic;
        newParticleScript.transform.position = new Vector2(x, y);
        newParticleScript.mass = m;




        newParticleScript.rb.velocity = new Vector2(xv, yv);
        particles.Add(newParticle);
    }
    void OnMouseDown()
    {
        // Destroy the gameObject after clicking on it
        
    }
    public void AdjustNewMass(float NM)
    {
        newMass = NM;
    }
    public void SliderHoverOver()
    {
        usingSlider = true;
    }
    public void SliderNotHoverOver()
    {
        usingSlider = false;
    }
    public void createRingObj(GameObject parentObj,float minDist,float maxDist,float ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            Body parentObjScript = parentObj.GetComponent<Body>();
            var dist = Random.Range(minDist, maxDist); //random(minDist,maxDist);
                                                       //print(minDist +', '+maxDist);
            var angledeg = Random.Range(0, 360);
            var angleRad = (angledeg * Mathf.PI) / 180;
            var orbitAngle = angleRad - 1.5708f;
            var vectX = dist * Mathf.Cos(angleRad);
            var vectY = dist * Mathf.Sin(angleRad);

            var OrbVel = CalculateVel(parentObjScript.G, parentObjScript.mass, dist);
            var velocX = OrbVel * Mathf.Cos(orbitAngle);
            var velocY = OrbVel * Mathf.Sin(orbitAngle);
            //print(OrbVel);
            SpawnNewParticle(parentObj.transform.position.x, parentObj.transform.position.x, velocX + parentObjScript.rb.velocity.x, velocY + parentObjScript.rb.velocity.y, 1, false);
        }
        
        //print(parentObj.x + vectX );
    }
    float CalculateVel(float g, float m, float r)
    {
        var v = Mathf.Sqrt((g * m) / r);
        return (v);
    }
}
