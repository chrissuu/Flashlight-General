using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpStopCommand : MonoBehaviour, ICommand
{
  // when "w" is pressed: power up (-3 energy) + stop movement 
  
  public void Execute() 
  {
    FieldOfView fieldOfView = FieldOfView.Instance; 
    fieldOfView.decreaseEnergy(true); 
    fieldOfView.stopMovement();  
  }
}
