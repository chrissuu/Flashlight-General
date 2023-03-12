using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class SoldierController : MonoBehaviour
{   
    public float speed = 2.0f;
    public bool canMove = true;
    public double Health = 200;
    public double AttackDamage = 100;
    public double AttackRange = 10;
    public int Team;
    public SoldierController target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {   
            var step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
            
            if(getEuclideanDistance(this, target) < AttackRange)
            {
                handleAttack(target);
                target.handleAttack(this);
            }
        }
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
    public void moveSoldier(SoldierController sc)
    {
        Vector2 moveLoc = sc.transform.position;
        float speedf = 2.0f;
        Vector2 currpos = transform.position;
        //Debug.Log("Move x " + moveLoc.x+ " curr X " + currpos.x);

        Vector2 newVec = new Vector2( moveLoc.x - currpos.x, moveLoc.y - currpos.y);
        Debug.Log("checkx " +  newVec.x + " Check Y " + newVec.y);
        sc.transform.Translate(speed * Time.deltaTime * newVec.normalized);
        Vector3 translate = newVec.normalized * speedf * Time.deltaTime; 
        Debug.Log("translation vec" + translate.x + " " + translate.y + " " + translate.z);
        Debug.Log("moveSoldier");

        
    }

  
    public void handleAttack(SoldierController ot)
    {
        ot.Health -= AttackDamage;
        if (ot.Health < 0)
        {
            Destroy(ot);
            Destroy(ot.gameObject);
        }

    }
    public double getEuclideanDistance(SoldierController unit, SoldierController other)
    {
        Vector2 unitCoords = unit.transform.position;
        Vector2 otherCoords = other.transform.position;

        double unitX = unitCoords.x;
        double unitY = unitCoords.y;

        double otherX = otherCoords.x;
        double otherY = otherCoords.y;

        double diffX = unitX - otherX;
        double diffY = unitY - otherY;

        double euclDistance2D = Math.Sqrt(diffX * diffX + diffY * diffY);
        //Debug.Log("dist " + euclDistance2D);
        return euclDistance2D;
    }
}
