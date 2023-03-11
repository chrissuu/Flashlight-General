using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    public SoldierController sc;
    public Queue<GameObject> storeSoldiersTeam1;
    public Queue<GameObject> storeSoldiersTeam2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnSoldier(int team, double[] cartesianCoords)
    {
        if (team == 1)
        {
            GameObject soldier = storeSoldiersTeam1.Dequeue();
            soldier.transform.position = new Vector2((float)cartesianCoords[0], (float)cartesianCoords[1]);
        }
        else
        {
            GameObject soldier = storeSoldiersTeam2.Dequeue();
            soldier.transform.position = new Vector2((float)cartesianCoords[0], (float)cartesianCoords[1]);
            //support code for team selection
        }
    }
}
