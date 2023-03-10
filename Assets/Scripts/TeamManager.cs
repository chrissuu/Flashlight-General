using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Unit
{
    private int team;
    private GameObject soldier;
    private double[] cartesianCoords;
    private double[] polarCoords;
    private Unit prev;
    private Unit next;
    
    private double health = 200;
    private double attackdmg = 100;

    public double AttackDmg => attackdmg;
    public double Health
    {
        get => health;
        set => health = value;
    }
    public int Team => team;
    public GameObject Soldier => soldier;
    public double[] CartesianCoords
    {
        get => cartesianCoords;
        set => cartesianCoords = value;
    }
    public double[] PolarCoords
    {
        get => polarCoords;
        set => polarCoords = value;
    }
    public Unit Prev
    {
        get => prev;
        set => prev = value;

    }
    public Unit Next
    {
        get => next;
        set => next = value;
    }


    public Unit()
    {

    }

    public Unit(GameObject obj, int t, double[] CC, double[] PP)
    {
        team = t;
        soldier = obj;
        cartesianCoords = CC;
        polarCoords = PP;
    }

    public void handleAttack(Unit ot)
    {
        ot.health -= attackdmg;
        if(ot.health < 0)
        {
            UnityEngine.Object.Destroy(ot.Soldier, 0.0f);
        }
    }

    public void approach(Unit ot)
    {

    }
}
public class Grid
{
    private double _arenaRadius;
    private double _attackRange;
    private int _radialPartitionCount;
    private int _angularPartitionCount;
    private double[] _flashlightRegion;
    private Unit[,] _cells;

    public double AttackRange
    {
        get => _attackRange;
        set => _attackRange = value;
    }
    public double ArenaRadius
    {
        get => _arenaRadius;
        set => _arenaRadius = value;
    }
    public int RadialPartitionCount
    {
        get => _radialPartitionCount;
        set => _radialPartitionCount = value;
    }
    
    public int AngularPartitionCount
    {
        get => _angularPartitionCount;
        set => _angularPartitionCount = value;
    }
    public double[] FlashlightRegion
    {
        get => _flashlightRegion;
        set => _flashlightRegion = value;
    }
    public Unit[,] Cells
    {
        get => _cells;
        set => _cells = value;
    } 

    public Grid()
    {

    }
    //flashlight region, radial partition count, arena radius, angular partition count
    public Grid(double[] flRg, int rptct, double ar, int aptct)
    {
        _arenaRadius = ar;
        _radialPartitionCount = rptct;
        _angularPartitionCount = aptct;
        _flashlightRegion = flRg;
        _cells = new Unit[_radialPartitionCount, _angularPartitionCount];
    }
    //adds a unit to a place on a grid, should be called when initializing unit or moving unit to different partition
    public void addUnit(Unit go)
    {
        double[] pp = go.PolarCoords;


        int cellX = (int)Math.Floor(pp[0] / (_arenaRadius / _radialPartitionCount));
        //Debug.Log("cellX " + cellX);
        int cellY = (int)Math.Floor((pp[1] / (2*Math.PI / _angularPartitionCount)));
        //Debug.Log("cellY " + cellY);
        //Debug.Log("Rank " + _cells.Rank);
        //Debug.Log("Length " + _cells.Length);
       

        go.Prev = null;
        go.Next = _cells[cellX, cellY];
        if (go.Next != null)
        {
            go.Next.Prev = go;
        }
    }
    //basically starts the attack
    public void handleMelee()
    {
        for (int r = 0; r < _radialPartitionCount; r++)
        {
            int minFlashlightRegion = (int)Math.Floor((_flashlightRegion[0] * _angularPartitionCount / (2 * Math.PI)));
            int maxFlashlightRegion = (int)Math.Floor((_flashlightRegion[1] * _angularPartitionCount / (2 * Math.PI)));

            for (int a = minFlashlightRegion+1; a < maxFlashlightRegion; a++)
            {
                handleCells(r,a);
            }
        }
    }
    public void handleUnit(Unit unit, Unit ot)
    {
        while(ot !=null)
        {
            double distance = getEuclideanDistance(unit, ot);

            if(distance<AttackRange && (ot.Team != unit.Team))
            {
                unit.handleAttack(ot);
            } else if (distance>=AttackRange && (ot.Team != unit.Team))
            {
                unit.approach(ot);
                ot.apprach(unit);
            }
            ot = ot.Next;
        }
    }
    //where attack method is called. 
    public void handleCells(int x, int y)
    {
        Unit unit = _cells[x, y];
        while(unit!=null)
        {
            handleUnit(unit, unit.Next);

            if(x > 0 && y> 0) handleUnit(unit, _cells[x-1, y-1]);
            if (x>0) handleUnit(unit, _cells[x-1,y]);
            if (y>0) handleUnit(unit, _cells[x,y-1]);
            if(x>0 && y < _radialPartitionCount -1) handleUnit(unit, _cells[x-1,y+1]);
            unit = unit.Next;
        }

    }
    //returns the distance between two units
    public double getEuclideanDistance(Unit unit, Unit other)
    {
        double[] unitCartesianCoords = unit.CartesianCoords;
        double[] otherCartesianCoords = other.CartesianCoords;

        double unitX = unitCartesianCoords[0];
        double unitY = unitCartesianCoords[1];

        double otherX = unitCartesianCoords[0];
        double otherY = unitCartesianCoords[1];

        double diffX = unitX - otherX;
        double diffY = unitY - otherY;

        double euclDistance2D = Math.Sqrt(diffX * diffX + diffY * diffY);
        return euclDistance2D;
    }

    public void moveUnit(Unit unit, double[] newCartesianCoords)
    {
        int oldCellX = (int)(unit.PolarCoords[0] / (_arenaRadius / _radialPartitionCount));
        int oldCellY = (int)(unit.PolarCoords[1] / ((_flashlightRegion[1] - _flashlightRegion[0]) / _angularPartitionCount));

        double[] newPolarCoords = transformCartesianCoordinatesToPolar(newCartesianCoords);
        int newX = (int)(newPolarCoords[0] / (_arenaRadius / _radialPartitionCount));
        int newY = (int)(newPolarCoords[1] / ((_flashlightRegion[1] - _flashlightRegion[0]) / _angularPartitionCount));

        unit.PolarCoords = newPolarCoords;
        unit.CartesianCoords = newCartesianCoords;

        if (oldCellX == newX && oldCellY == newY) return;

        if (unit.Prev != null)
        {
            unit.Prev.Next = unit.Next;
        }

        if (unit.Next != null)
        {
            unit.Next.Prev = unit.Prev;
        }

        if (_cells[oldCellX,oldCellY] == unit)
        {
            _cells[oldCellX,oldCellY] = unit.Next;
        }

        addUnit(unit);
    }

    double[] transformPolarCoordinatesToCartesian(double radialPosition, double theta)
    {
        double xCoord = radialPosition * Math.Cos(theta);
        double yCoord = radialPosition * Math.Sin(theta);

        double[] storeCartesianCoordinates = new double[] { xCoord, yCoord };
        return storeCartesianCoordinates;
    }
    double[] transformPolarCoordinatesToCartesian(double[] polarPair)
    {

        return transformPolarCoordinatesToCartesian(polarPair[0], polarPair[1]);
    }
    double[] transformCartesianCoordinatesToPolar(double x, double y)
    {
        double radialPosition = Math.Sqrt(x * x + y * y);
        double angle = Math.Atan2(y, x);

        if (angle < 0) angle += 2 * Math.PI;

        double[] storePolarCoordinates = new double[] { radialPosition, angle };

        return storePolarCoordinates;
    }
    double[] transformCartesianCoordinatesToPolar(double[] cartesianCoords)
    {

        return transformCartesianCoordinatesToPolar(cartesianCoords[0], cartesianCoords[1]);
    }

}

public class TeamManager : MonoBehaviour
{
    public GameObject team1Template; //rocks
    public GameObject team2Template; //scissors

    // Start is called before the first frame update
    public static double arenaRadius; //~310
    public double minRadius;
    public int armySize;
    public Transform parent;

    public static int radialPartitionCount; //# of partitions for a full circle
    public static int angularPartitionCount;
    static double[] fullRegion = new double[] { 0, 2 * Math.PI };
    public Grid MASTERGRID = new Grid(fullRegion, radialPartitionCount, arenaRadius, angularPartitionCount);
    public bool gameOngoing = true;
    void Start()
    {   
        MASTERGRID.RadialPartitionCount = 5;
        MASTERGRID.AngularPartitionCount = 8;
        MASTERGRID.ArenaRadius = 310;
        MASTERGRID.AttackRange = 100;
        MASTERGRID.Cells = new Unit[5, 8];
        spawnArmy(armySize, armySize, 0, MASTERGRID.ArenaRadius);
        MASTERGRID.handleMelee();
    }

    // Update is called once per frame
    void Update()
    {
       
        if(gameOngoing)
        {
            //double[] flashlightRegion = getFlashlightRegion()

            double[] flashlightRegion = new double[2] { 0, 2 * Math.PI };
            MASTERGRID.FlashlightRegion = flashlightRegion;
            MASTERGRID.handleMelee();
            gameOngoing = checkFinished();
        }
    }
    bool checkFinished()
    {   
        int team = 1;
        foreach(Unit U in MASTERGRID.Cells) {
            Unit temp = U;
            while (temp != null)
            {
                if(temp.Team != team)
                {
                    return false;
                }
                temp = temp.Next;
            }
                
        }
        return true;
    }
    void updatePartitions(bool isFlashlightOn, double[] flashlightRegion)
    {
        int newPartitionCount = (int)((flashlightRegion[1] - flashlightRegion[0]) / (Math.PI * 2) * angularPartitionCount);

    }

    void placeSoldier(int team, double min, double max)
    {
        double[] randomPos = generatePolarCoordinates(arenaRadius, min, max);

        double[] cartesianCoords = transformPolarCoordinatesToCartesian(randomPos[0], randomPos[1]);

        spawnSoldier(team, cartesianCoords);

    }
    public void spawnSoldier(int team, double[] cartesianCoords)
    {
        if (team == 1)
        {
            GameObject tempGO = Instantiate(team1Template, cartesianToVector2(cartesianCoords), Quaternion.identity, parent);
            Unit temp = new Unit(tempGO, team, cartesianCoords, transformCartesianCoordinatesToPolar(cartesianCoords));
            MASTERGRID.addUnit(temp);

        }
        else
        {
            GameObject tempGO = Instantiate(team2Template, cartesianToVector2(cartesianCoords), Quaternion.identity, parent);
            Unit temp = new Unit(tempGO, team, cartesianCoords, transformCartesianCoordinatesToPolar(cartesianCoords));
            MASTERGRID.addUnit(temp);

            //support code for team selection
        }
    }
    void spawnArmy(int sizeTeam1, int sizeTeam2, double min, double max)
    {
        for (int i = 0; i < sizeTeam1; i++)
        {
            placeSoldier(1, min, max);
        }

        for (int i = 0; i < sizeTeam2; i++)
        {
            placeSoldier(2, min, max);
        }
    }

    double[] generatePolarCoordinates(double maxRadius, double min, double max)
    {
        System.Random rnd = new System.Random();

        double randRadialPosition = min + (max - min) * Math.Sqrt(rnd.NextDouble());
        double randAngleValue = 2 * Math.PI * rnd.NextDouble();

        double[] storePolarCoordinates = new double[] { randRadialPosition, randAngleValue };

        return storePolarCoordinates;
    }
    double[] generateGenericPolarCoordinates(double radius)
    {
        return generatePolarCoordinates(radius, 0, 2 * Math.PI);
    }
    double[] transformPolarCoordinatesToCartesian(double radialPosition, double theta)
    {
        double xCoord = radialPosition * Math.Cos(theta);
        double yCoord = radialPosition * Math.Sin(theta);

        double[] storeCartesianCoordinates = new double[] { xCoord, yCoord };
        return storeCartesianCoordinates;
    }
    double[] transformPolarCoordinatesToCartesian(double[] polarPair)
    {

        return transformPolarCoordinatesToCartesian(polarPair[0], polarPair[1]);
    }
    double[] transformCartesianCoordinatesToPolar(double x, double y)
    {
        double radialPosition = Math.Sqrt(x * x + y * y);
        double angle = Math.Atan2(y,x);

        if (angle < 0) angle += 2 * Math.PI;
       
        double[] storePolarCoordinates = new double[] { radialPosition, angle };

        return storePolarCoordinates;
    }
    double[] transformCartesianCoordinatesToPolar(double[] cartesianCoords)
    {

        return transformCartesianCoordinatesToPolar(cartesianCoords[0], cartesianCoords[1]);
    }
    Vector2 cartesianToVector2(double[] coords)
    {
        return new Vector2((float)coords[0], (float)coords[1]);
    }
}