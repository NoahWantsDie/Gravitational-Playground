using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public Body ParentScript;
    public GameObject ParentBody;
    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        ParentBody = transform.parent.gameObject;

        ParentScript = ParentBody.GetComponent<Body>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = ParentBody.GetComponent<SpriteRenderer>().color;
        if ((ParentScript.mass >= 200))
        {
            sr.enabled = true;
        }
        else
        {
            sr.enabled = false;
        }
    }
}
