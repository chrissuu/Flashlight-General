using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    public SoldierController sc;
    public List<SoldierController> storeSoldiersTeam1;
    public List<SoldierController> storeSoldiersTeam2;
    public List<SoldierController> allSoldiers;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void addSoldierController(SoldierController soldierController, int team)
    {   
        allSoldiers.Add(soldierController);
        if(team == 1)
        {
            storeSoldiersTeam1.Add(soldierController);
        } else
        {
            storeSoldiersTeam2.Add(soldierController);
        }
    }

}
