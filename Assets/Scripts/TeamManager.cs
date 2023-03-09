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


    public int Team => team;
    public GameObject Soldier => soldier;
    public double[] CartesianCoords => cartesianCoords;
    public double[] PolarCoords => polarCoords;
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
        soldier= obj;
        cartesianCoords = CC;
        polarCoords = PP;
    }

    
}
public class Grid
{
    private double _arenaRadius;
    private int _radialPartitionCount;
    private int _angularPartitionCount;
    private double[] _flashlightRegion;
    private Unit[,] _cells;

    public double ArenaRadius => _arenaRadius;
    public int RadialPartitionCount => _radialPartitionCount;
    public int AngularPartitionCount => _angularPartitionCount;
    public double[] FlashlightRegion => _flashlightRegion;
    public Unit[,] Cells => _cells;

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
        _cells = new Unit[rptct, aptct];
    }

    public void addUnit(Unit go, int team)
    {
        double[] pp = go.PolarCoords;


        int cellX = (int)(pp[0] / (_arenaRadius / _radialPartitionCount));
        int cellY = (int)(pp[1] / ((_flashlightRegion[1] - _flashlightRegion[0])/_angularPartitionCount));
        go.Prev = null;
        go.Next = _cells[cellX, cellY];
        if (go.Next != null)
        {
            go.Next.Prev= go;
        }
    }

    public void handleMelee()
    {
        for (int r = 0; r < _radialPartitionCount; r++)
        {
            for (int a = 0; a < _angularPartitionCount; a++)
            {
                handleCells(_cells[r, a]);
            }
        }
    }

    public void handleCells(Unit unit)
    {
        while(unit !=null)
        {
            Unit ot = unit.Next;
            while(ot!=null)
            {
                if(unit
            }
        }
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
        double angle = Math.Tan(x / y);

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

    public Queue<GameObject> storeSoldiersTeam1;
    public Queue<GameObject> storeSoldiersTeam2;
    // Start is called before the first frame update
    public static double arenaRadius; //~310
    public double minRadius;
    public int armySize;
    public Transform parent;

    public static int radialPartitionCount; //# of partitions for a full circle
    public static int angularPartitionCount;
    static double[] fullRegion = new double[] { 0, 2 * Math.PI };
    public Grid MASTERGRID = new Grid(fullRegion, radialPartitionCount, arenaRadius, angularPartitionCount);
    void Start()
    {
        spawnArmy(armySize, armySize, minRadius, arenaRadius);
        //createPartitions(//initial flashlight area);
    }

    // Update is called once per frame
    void Update()
    {
        //updatePartitions(//is flashlight on, updated flashlight area)
        

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
            MASTERGRID.addUnit(temp, team);

        }
        else
        {
            GameObject tempGO = Instantiate(team2Template, cartesianToVector2(cartesianCoords), Quaternion.identity, parent);
            Unit temp = new Unit(tempGO, team, cartesianCoords, transformCartesianCoordinatesToPolar(cartesianCoords));
            MASTERGRID.addUnit(temp, team);

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
            placeSoldier(2, min,max);
        }
    }

    double[] generatePolarCoordinates(double maxRadius, double min, double max)
    {
        System.Random rnd = new System.Random();

        double randRadialPosition = min+(max-min) * Math.Sqrt(rnd.NextDouble());
        double randAngleValue = 2* Math.PI * rnd.NextDouble();

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
        double angle = Math.Tan(x / y);

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
