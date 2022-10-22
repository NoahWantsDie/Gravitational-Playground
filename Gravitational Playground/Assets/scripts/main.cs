using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour
{

    public float TimeStep = 1;
    public GameObject sun;
    public GameObject particle;

    public Vector2 mousePos;
    public Vector2 mousePosWorld;

    private Camera cam;
    private float fixedDeltaTime;

    public GameObject focusObj;
    public GameObject explosion;

    public Vector2 mouseDownPoint;
    public Vector2 slingShotPoint;
    public CameraMove CamScript;
    public drawCircle circleScript;
    public drawCircle circleScript2;

    public LineRenderer line;
    private Vector3[] points;
    public Vector2 dragClickForce;

    public bool usingSlider;

    public Text massTxt;
    public Text timeTxt;

    public float newMass = 0.5f;

    public List<GameObject> bodies = new List<GameObject>();
    public List<GameObject> particles = new List<GameObject>();

    public bool linin;
    public bool trailToggle;

    //tool states
    public string ToolState;

    bool p;

    public Toggle togPlanet;

    bool f;

    GameObject bOne;
    public GameObject CircleObj;
    public GameObject CircleObj2;
    public Toggle togFocus;
    public Toggle togOrb;
    public Toggle togTrl;
    public GameObject MassCenterObj;
    
    public void PlanetPlace()
    {


        ToolState = "PlanetPlacer";
    }
    public void FocusPlace()
    {

        ToolState = "FocusPlacer";

    }
    public void OrbitPlace()
    {

        ToolState = "OrbitPlacer";

    }
    public void ToggleTrail()
    {


        trailToggle = !trailToggle;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        line.enabled = false;
        CamScript = Camera.main.GetComponent<CameraMove>();
        circleScript = CircleObj.GetComponent<drawCircle>();
        circleScript2 = CircleObj2.GetComponent<drawCircle>();
        bodies.Add(sun);
        particles.Add(particle);
        MassCenterObj.GetComponent<LineRenderer>().enabled = false;
        mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }
    float WeightedAverage(float num1, float weight1, float num2, float weight2)
    {
        var averag = ((num1 * weight1) + (num2 * weight2)) / (weight1 + weight2);
        return (averag);
    }

    void SetFocusObject(GameObject fo)
    {
        Debug.Log("set focus to : " + fo);
        focusObj = fo;
    }
    // Update is called once per frame
    public Vector2 SpawnPos;
    void Update()
    {
        CamScript.focusedObject = focusObj;
        mousePos = Input.mousePosition;
        mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragClickForce = SpawnPos - mousePosWorld;
        slingShotPoint = (SpawnPos - mousePosWorld) + SpawnPos;
        Time.timeScale = TimeStep;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        
        if (ToolState == "PlanetPlacer")
        {
            togPlanet.isOn = true;
        }
        else
        {
            togPlanet.isOn = false;
        }
        if (ToolState == "OrbitPlacer")
        {
            togOrb.isOn = true;
        }
        else
        {
            togOrb.isOn = false;
        }

        if (ToolState == "FocusPlacer")
        {
            togFocus.isOn = true;
        }
        else
        {
            togFocus.isOn = false;
        }

        if (focusObj == null)
        {
            CamScript.focused = false;
        }
        if (trailToggle)
        {
            togTrl.isOn = true;
        }
        else
        {
            togTrl.isOn = false;
        }
        massTxt.text = "New Mass : " + newMass;
        timeTxt.text = "TimeScale : " + TimeStep.ToString();



        
        if (ToolState == "PlanetPlacer")
        {
            line.SetPosition(0, SpawnPos);
            line.SetPosition(1, slingShotPoint);
            if (Input.GetMouseButtonDown(0))
            {
                if (!usingSlider)
                {
                    mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    SpawnPos = mousePosWorld;
                    
                    slingShotPoint = (SpawnPos - mousePosWorld) + SpawnPos;

                    
                    circleScript2.DrawPolygon(30, Mathf.Sqrt(newMass / Mathf.PI) / 2, mousePosWorld, 0.1f, 0.1f);
                    circleScript2.lineRenderer.enabled = true;

                    line.enabled = true;
                    line.SetPosition(0, SpawnPos);
                    line.SetPosition(1, slingShotPoint);
                }

                
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!usingSlider && line.enabled)
                {
                    SpawnNewBody(SpawnPos.x, SpawnPos.y, dragClickForce.x, dragClickForce.y, newMass, false);
                    
                    //Debug.Log("clicked");
                    line.enabled = false;
                    circleScript2.lineRenderer.enabled = false;
                    line.SetPosition(0, new Vector3(0, 0, 0));
                    line.SetPosition(1, new Vector3(0, 0, 0));
                }

            }
            
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            FocusPlace();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlanetPlace();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            newMass = newMass / 2;
        }

        if (usingSlider)
        {
            line.enabled = false;
            circleScript.lineRenderer.enabled = false;
            circleScript2.lineRenderer.enabled = false;
        }
        if (linin && (ToolState == "OrbitPlacer"))
        {
            Vector2 direction = new Vector2(bOne.transform.position.x, bOne.transform.position.y) - mousePosWorld;
            Body orbitTargetScript1 = bOne.GetComponent<Body>();
            mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float radius = Vector3.Distance(bOne.transform.position, mousePosWorld);
            circleScript.DrawPolygon(30, radius, bOne.transform.position, 0.1f, 0.1f);
            circleScript2.DrawPolygon(30, Mathf.Sqrt(newMass / Mathf.PI) / 2, mousePosWorld, 0.1f, 0.1f);
            line.SetPosition(0, bOne.transform.position);
            line.SetPosition(1, mousePosWorld);
            MassCenterObj.transform.position = GetMassCenter(bOne.transform.position, mousePosWorld, orbitTargetScript1.mass, newMass);
            line.enabled = true;
            if (usingSlider)
            {
                circleScript.lineRenderer.enabled = false;
                circleScript2.lineRenderer.enabled = false;
            }
            else
            {
                circleScript.lineRenderer.enabled = true;
                circleScript2.lineRenderer.enabled = true;
            }
            MassCenterObj.GetComponent<LineRenderer>().enabled = true;
            Vector2 diff = direction.normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            MassCenterObj.transform.eulerAngles = new Vector3(0f, 0f, rot_z - 90);
            MassCenterObj.transform.localScale = new Vector3(radius / 10, radius / 10, 1);
        }
        if (Input.GetMouseButtonUp(0) && linin && (ToolState == "OrbitPlacer"))
        {
            SpawnPos = mousePosWorld;
            Vector2 direction = new Vector2(bOne.transform.position.x, bOne.transform.position.y) - mousePosWorld;
            Body orbitTargetScript = bOne.GetComponent<Body>();
            float radius = Vector3.Distance(bOne.transform.position, mousePosWorld);

            float OrbitSpeed = Mathf.Sqrt((orbitTargetScript.G * (Mathf.Pow(orbitTargetScript.mass, 2))) / (radius * (newMass + orbitTargetScript.mass)));
            float OrbitSpeedP = Mathf.Sqrt((orbitTargetScript.G * (Mathf.Pow(newMass, 2))) / (radius * (newMass + orbitTargetScript.mass)));
            float OrbitSpeedOrbiter = OrbitSpeed;
            float OrbitSpeedParent = OrbitSpeedP;


            Vector2 NewOrbitVelOrbiter = new Vector2(direction.y, -direction.x).normalized * OrbitSpeedOrbiter;
            Vector2 NewOrbitVelParent = new Vector2(direction.y, -direction.x).normalized * OrbitSpeedParent;
            SpawnNewBody(mousePosWorld.x, mousePosWorld.y, NewOrbitVelOrbiter.x + orbitTargetScript.rb.velocity.x, NewOrbitVelOrbiter.y + orbitTargetScript.rb.velocity.y, newMass, false);


            orbitTargetScript.rb.velocity -= NewOrbitVelParent;
            line.SetPosition(0, new Vector3(0, 0, 0));
            line.SetPosition(1, new Vector3(0, 0, 0));
            line.enabled = false;
            linin = false;
            MassCenterObj.GetComponent<LineRenderer>().enabled = false;
        }
        if (ToolState != "OrbitPlacer")
        {
            linin = false;
        }
        else
        {
            if (!linin)
            {
                circleScript2.lineRenderer.enabled = false;
            }
        }
        if (!linin)
        {

            circleScript.lineRenderer.enabled = false;

        }
        
        line.startWidth = Camera.main.orthographicSize / 100;
        line.endWidth = Camera.main.orthographicSize / 100;

        circleScript.lineRenderer.startWidth = Camera.main.orthographicSize / 100;
        circleScript.lineRenderer.endWidth = Camera.main.orthographicSize / 100;

        circleScript2.lineRenderer.startWidth = Camera.main.orthographicSize / 100;
        circleScript2.lineRenderer.endWidth = Camera.main.orthographicSize / 100;

        MassCenterObj.GetComponent<LineRenderer>().startWidth = Camera.main.orthographicSize / 100;
        MassCenterObj.GetComponent<LineRenderer>().endWidth = Camera.main.orthographicSize / 100;



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
                    if (trailToggle)
                    {
                        b1Script.trailRend.emitting = true;
                    }
                    else
                    {
                        b1Script.trailRend.emitting = false;
                    }
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
                                b2Script.transform.position = new Vector2(NewPointX, NewPointY);
                                b2Script.transform.position = new Vector2(NewPointX, NewPointY);

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
                        if (ToolState == "FocusPlacer")
                        {
                            if ((distanceToMouse <= b1Script.radius) && (Input.GetMouseButtonDown(0)))
                            {
                                if (b1 != bodies[0])
                                {

                                    SetFocusObject(b1);
                                    CamScript.focused = true;
                                }
                            }
                        }
                        if (ToolState == "OrbitPlacer")
                        {
                            
                            if ((distanceToMouse <= b1Script.radius) && (Input.GetMouseButtonDown(0)))
                            {
                                linin = true;
                                if (b1 != bodies[0])
                                {
                                    bOne = b1;
                                }
                                
                            }
                        }

                    }
                    
                    
                }
            }
        }
    }
    public Vector2 NewPos;
    void SpawnNewBody(float x, float y, float xv, float yv, float m,bool isFocused)
    {
        
        //Debug.Log("planet spawned at " + new Vector2(x,y));
        GameObject newPlanet = Instantiate(sun);
        newPlanet.SetActive(true);
        Body newPlanetScript = newPlanet.GetComponent<Body>();
        newPlanetScript.locked = false;
        newPlanetScript.rb.bodyType = RigidbodyType2D.Dynamic;
        newPlanet.transform.position = new Vector3(x, y,0);
        newPlanetScript.transform.position = new Vector3(x, y, 0);
        NewPos = new Vector2(x, y);
        Debug.Log(NewPos + "mainScrpt");

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
        newParticle.transform.position = new Vector2(x, y);
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
        float nM = NM;
        nM = Mathf.Round(nM * 10f) / 10f;
        newMass = nM;
    }
    public void AdjustTime(float TS)
    {
        float ts = TS;
        ts = Mathf.Round(ts * 10f) / 10f;
        TimeStep = ts;
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
    Vector2 GetMassCenter(Vector2 obj1Position, Vector2 obj2Position,float obj1Mass, float obj2Mass)
    {
        
        float PointX = WeightedAverage(obj1Position.x, obj1Mass, obj2Position.x, obj2Mass);
        float PointY = WeightedAverage(obj1Position.y, obj1Mass, obj2Position.y, obj2Mass);
        Vector2 MassCenter = new Vector2(PointX, PointY);
        return (MassCenter);
    }
    float CircularOrbitThing(float m1, float m2)
    {
        float q = m1 / m2;
        float velMult = Mathf.Sqrt(q / 2)/m2;
        return (velMult);
    }
}
