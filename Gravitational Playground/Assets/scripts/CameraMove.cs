﻿using System.Collections;
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
    private void Start()
    {
        ResetCamera = Camera.main.transform.position;
    }


    private void LateUpdate()
    {
        
        if (Input.GetMouseButton(1))
        {
            Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if(drag == false)
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
            Camera.main.orthographicSize -= scrollSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            Camera.main.orthographicSize += scrollSpeed * Time.deltaTime;
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
        Camera.main.transform.position = obj.transform.position + new Vector3(0,0,-10);
        if (Input.GetMouseButtonDown(1))
        {
            focused = false;
        }

    }
    void FixedUpdate()
    {
        
        
        
    }
}
