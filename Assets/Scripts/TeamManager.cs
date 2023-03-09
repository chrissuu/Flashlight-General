using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Unit
{
    public Unit()
    {

    }

    public Unit(GameObject obj, int t, double[] cartesianCoords)
    {
        int team = t;
        GameObject unit = obj;
    }
    public double[] getCartesianCoords()
    {
        return ca
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
    public Unit[,] cells => _cells;

    public Grid()
    {

    }
    //flashlight region, radial partition count, arena radius, angular partition count
    public Grid(double[] flRg, int rptct, double ar, int aptct)
    {
        arenaRadius = ar;
        radialPartitionCount = rptct;
        angularPartitionCount = aptct;
        flashlightRegion = flRg;
        cells = new Unit[aptct, rptct];
    }

    public void addUnit(Unit go, int team)
    {
        go.get
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
            Unit temp = new Unit(tempGO, team, cartesianCoords);
            MASTERGRID.addUnit(temp, team);

        }
        else
        {
            GameObject tempGO = Instantiate(team2Template, cartesianToVector2(cartesianCoords), Quaternion.identity, parent);
            Unit temp = new Unit(tempGO, team, cartesianCoords);
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

    double[] transformCartesianCoordinatesToPolar(double x, double y)
    {
        double radialPosition = Math.Sqrt(x * x + y * y);
        double angle = Math.Tan(x / y);

        double[] storePolarCoordinates = new double[] { radialPosition, angle };

        return storePolarCoordinates;
    }

    Vector2 cartesianToVector2(double[] coords)
    {
        return new Vector2((float)coords[0], (float)coords[1]);
    }
}
