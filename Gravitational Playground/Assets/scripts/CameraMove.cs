using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    private bool drag = false;

    public float scrollSpeed = 1;
    public bool focused;
    public GameObject focusedObject;
    public GameObject MainObj;
    main MainScript;

    public GameObject Grid;
    private void Start()
    {
        ResetCamera = Camera.main.transform.position;
        MainScript = MainObj.GetComponent<main>();
    }


    private void LateUpdate()
    {

        if (Input.GetMouseButton(1))
        {
            Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (drag == false)
            {
                drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            Camera.main.transform.position = Origin - Difference;
        }


        if (Input.GetKey(KeyCode.X))
        {
            Camera.main.orthographicSize -= (scrollSpeed * Time.deltaTime * 10) / MainScript.TimeStep;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            Camera.main.orthographicSize += (scrollSpeed * Time.deltaTime * 10) / MainScript.TimeStep;
        }
        if (Camera.main.orthographicSize <= 1)
        {
            Camera.main.orthographicSize = 1;
        }
        if (Camera.main.orthographicSize >= 100)
        {
            Camera.main.orthographicSize = 100;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {

            //focused = !focused;

        }

        if (focused == true)
        {
            setFocus(focusedObject);

        }
        if (Input.GetMouseButtonDown(1))
        {
            focused = false;
        }

    }
    public void setFocus(GameObject obj)
    {

        drag = false;
        Camera.main.transform.position = obj.transform.position + new Vector3(0, 0, -10);
        if (Input.GetMouseButtonDown(1))
        {
            focused = false;
        }

    }
    public int roundNum;
    void FixedUpdate()
    {
        Grid.transform.position = new Vector3(round_to_num(Camera.main.transform.position.x, roundNum), round_to_num(Camera.main.transform.position.y, roundNum));
        Grid.transform.localScale = new Vector3(roundNum, roundNum, roundNum);
        scrollSpeed = Camera.main.orthographicSize / 15;
        if (Camera.main.orthographicSize <= 5)
        {
            roundNum = 1;
            
        }
        else if (Camera.main.orthographicSize > 5 && Camera.main.orthographicSize <= 15)
        {
            roundNum = 2;
            
}
        else if (Camera.main.orthographicSize > 15 && Camera.main.orthographicSize <= 25)
        {
            roundNum = 4;
            
        }
        else if (Camera.main.orthographicSize > 25 && Camera.main.orthographicSize <= 50)
        {
            roundNum = 8;
            
        }
        else if (Camera.main.orthographicSize > 80 && Camera.main.orthographicSize <= 100)
        {
            roundNum = 16;
            
        }
        
    }
    private int round_to_num(float input, int roundAmount)
    {
        return Mathf.RoundToInt(input / roundAmount) * roundAmount;
    }
}
