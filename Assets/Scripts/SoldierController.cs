using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoldierController : MonoBehaviour
{
    public float speed = 0.0005f;
    public GameObject soldier;
    public bool canMove = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }

    public void stopMoving()
    {
        canMove = false;
    }

    public void moveSoldierRandomly(double[] polarPair)
    {
       
    }
    public float[] getPos()
    {
        Transform transform = GetComponent<Transform>();
        Vector2 currpos = transform.position;
        float x = currpos.x;
        float y = currpos.y;
        float[] pos = new float[2] { x, y };
        return pos;
    }
    public void moveSoldier(double[] cartesianCoords)
    {
        Transform transform = GetComponent<Transform>();
        Vector2 currpos = transform.position;
        float x = currpos.x;
        float y = currpos.y;

        Vector2 temp = new Vector2((float)cartesianCoords[0] - x, (float)cartesianCoords[1] - y);
        
        transform.Translate(speed * Time.deltaTime * temp.normalized);
    }

}
