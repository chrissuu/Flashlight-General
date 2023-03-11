using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit
{
    private int team;
    private GameObject soldier;
    private SoldierController sc;
    private double[] cartesianCoords;
    private double[] polarCoords;
    private Unit prev;
    private Unit next;

    private double health = 200;
    private double attackdmg = 100;
    public double attackRange = 10;
    public SoldierController SoldierC
    { 
        get => sc; 
        set => sc = value;
    }

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
        SoldierC = obj.GetComponent<SoldierController>();
    }

    public void handleAttack(Unit ot)
    {
        SoldierC.stopMoving();
        ot.health -= attackdmg;
        Debug.Log("health " + ot.health);
        if (ot.health < 0)
        {
            UnityEngine.Object.Destroy(ot.Soldier, 0.0f);
            UnityEngine.Object.Destroy(ot.SoldierC, 0.0f);

        }
    }

    public void approach(Unit ot)
    {
        SoldierC.moveSoldier(ot.CartesianCoords);
        ot.SoldierC.moveSoldier(CartesianCoords);

        if(getEuclideanDistance(ot, this) < attackRange)
        {
            handleAttack(ot);
            ot.handleAttack(this);
        }
        
    }
    public double getEuclideanDistance(Unit unit, Unit other)
    {
        Vector2 unitCoords = unit.SoldierC.transform.position;
        Vector2 otherCoords = other.SoldierC.transform.position;

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
public class Grid
{
    private double _arenaRadius;
    private double _attackRange;
    private int _radialPartitionCount;
    private int _angularPartitionCount;
    private double[] _flashlightRegion;
    private Unit[,] _cells;
    private List<Unit> units;

    public List<Unit> UList
    {
        get => units;
        set => units = value;
    }
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
        Debug.Log("cellX " + cellX);
        int cellY = (int)Math.Floor((pp[1] / (2*Math.PI / _angularPartitionCount)));
        Debug.Log("cellY " + cellY);
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
    public void handleMelee(bool entry)
    {
        double[] testFlashlightRegion = new double[2] { Math.PI / 2, Math.PI };
        foreach(Unit U in UList)
        {
            if(U.SoldierC ==null)
            {
                UList.Remove(U);
            }
        }
        if (entry) {
            for (int r = 0; r < _radialPartitionCount; r++)
            {
                int minFlashlightRegion = (int)Math.Floor((_flashlightRegion[0] * _angularPartitionCount / (2 * Math.PI)));
                int maxFlashlightRegion = (int)Math.Floor((_flashlightRegion[1] * _angularPartitionCount / (2 * Math.PI)));
                for (int a = minFlashlightRegion; a < maxFlashlightRegion; a++)
                {
                    handleCells(r,a);
                }
            }
        } else
        {
            List<Unit> winPolarRegion1 = new List<Unit>();
            List<Unit> winPolarRegion2 = new List<Unit>();


            foreach (Unit U in UList)
            {
                float[] pos = U.SoldierC.getPos();
                double x = (double)pos[0];
                double y = (double)pos[1];

                
                if(inPolarRegion(x,y, FlashlightRegion))
                {
                    if (U.Team == 1)
                    {
                        winPolarRegion1.Add(U);
                    } else
                    {
                        winPolarRegion2.Add(U);
                    }
                } 
            }
            int len = winPolarRegion1.Count < winPolarRegion2.Count ? winPolarRegion1.Count : winPolarRegion2.Count;
            if(winPolarRegion1.Count > winPolarRegion2.Count)
            {
                foreach(Unit U in winPolarRegion1)
                {
                    U.Health = U.Health * winPolarRegion1.Count / winPolarRegion2.Count;
                }
            }
            if (winPolarRegion2.Count > winPolarRegion1.Count)
            {
                foreach (Unit U in winPolarRegion2)
                {
                    U.Health = U.Health * winPolarRegion2.Count / winPolarRegion2.Count;
                }
            }
            for (int i = 0; i<len; i++)
            {
                winPolarRegion1[i].approach(winPolarRegion2[i]);
                winPolarRegion2[i].approach(winPolarRegion1[i]);
                
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
                ot.handleAttack(unit);
            } else if (distance>=AttackRange && (ot.Team != unit.Team))
            {
                unit.approach(ot);
                ot.approach(unit);
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
        Vector2 unitCoords = unit.SoldierC.transform.position;
        Vector2 otherCoords = other.SoldierC.transform.position;

        double unitX = unitCoords.x;
        double unitY = unitCoords.y;

        double otherX = otherCoords.x;
        double otherY = otherCoords.y;

        double diffX = unitX - otherX;
        double diffY = unitY - otherY;

        double euclDistance2D = Math.Sqrt(diffX * diffX + diffY * diffY);
        Debug.Log("dist " + euclDistance2D);
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
    bool inPolarRegion(double x, double y, double[] flashlightRegion)
    {
        double[] polarPair = transformCartesianCoordinatesToPolar(x, y);
        if (polarPair[1] >= flashlightRegion[0] && polarPair[1] <= flashlightRegion[1])
        {
            return true;
        }
        return false;
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
    //static double[] fullRegion = new double[] { Math.PI, 3/2*Math.PI };
    static double[] fullRegion = new double[] { 0, 2*Math.PI};

    public Grid MASTERGRID = new Grid(fullRegion, radialPartitionCount, arenaRadius, angularPartitionCount);
    public bool gameOngoing = true;
    void Start()
    {   
        MASTERGRID.RadialPartitionCount = 1;
        MASTERGRID.AngularPartitionCount = 1;
        MASTERGRID.ArenaRadius = 310;
        MASTERGRID.AttackRange = 100;
        MASTERGRID.Cells = new Unit[5, 8];
        MASTERGRID.UList = new List<Unit>();
        spawnArmy(armySize, armySize, 0, MASTERGRID.ArenaRadius);
        MASTERGRID.handleMelee(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOngoing)
        {
            //double[] flashlightRegion = getFlashlightRegion()
            //FieldOfView fieldOfView = FieldOfView.Instance; 
            //double[] flashlightRegion = new double[2] {fieldOfView.viewDistance, (fieldOfView.fov + fieldOfView.angle)*(Math.PI/180)};
            //MASTERGRID.FlashlightRegion = flashlightRegion;
            MASTERGRID.handleMelee(false);
            gameOngoing = checkFinished();
        }
        else endGame();

    }
    void endGame()
    {
        Debug.Log("END GAME"); 
        //FieldOfView fieldOfView = FieldOfView.Instance; 
        //ifFlashlightenergy is zero,
        /*
        if (fieldOfView.energy<=0) {
            MASTERGRID.FlashlightRegion = new double[2] { 0, 2 * Math.PI };
        }*/
        MASTERGRID.FlashlightRegion = new double[2] { 0, 2 * Math.PI };

    }
    bool checkFinished()
    {
        foreach (Unit U in MASTERGRID.UList)
        {
            if (U.SoldierC == null)
            {
                MASTERGRID.UList.Remove(U);
            }
        }

        int team = 1;

        foreach(Unit U in MASTERGRID.UList)
        {
            if (U.Team != team)
            {
                return false;
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
            SoldierController sc = tempGO.GetComponent<SoldierController>();
            Unit temp = new Unit(tempGO, team, cartesianCoords, transformCartesianCoordinatesToPolar(cartesianCoords));
            temp.SoldierC = sc;
            MASTERGRID.addUnit(temp);
            MASTERGRID.UList.Add(temp);
        }
        else
        {	
          
            GameObject tempGO = Instantiate(team2Template, cartesianToVector2(cartesianCoords), Quaternion.identity, parent);
            Unit temp = new Unit(tempGO, team, cartesianCoords, transformCartesianCoordinatesToPolar(cartesianCoords));
            SoldierController sc = tempGO.GetComponent<SoldierController>();
            temp.SoldierC = sc;
            MASTERGRID.addUnit(temp);
            MASTERGRID.UList.Add(temp);

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

        double randRadialPosition = min + (max - min) * (rnd.NextDouble());
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
