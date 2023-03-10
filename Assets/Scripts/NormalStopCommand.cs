using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStopCommand : MonoBehaviour, ICommand
{
  // when "s" is pressed: normal (-1 energy) + stop movement

  public void Execute()
  {
    FieldOfView fieldOfView = FieldOfView.Instance; 
    fieldOfView.decreaseEnergy(false); 
    fieldOfView.stopMovement(); 
  }
}
