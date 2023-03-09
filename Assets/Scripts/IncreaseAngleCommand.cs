using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAngleCommand : MonoBehaviour, ICommand
{

  //[SerializeField] FieldOfView fieldOfView;
  

  public void Execute()
  {
    FieldOfView fieldOfView = FieldOfView.Instance; 
    fieldOfView.decreaseEnergy();  
  }
}
// w = stop movement 
// s = powerup stop movement 
//
