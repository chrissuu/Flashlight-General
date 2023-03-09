using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseAngleCommand : MonoBehaviour, ICommand
{
  
  public void Execute()
  {
    FieldOfView fieldOfView = FieldOfView.Instance; 
    fieldOfView.decreaseEnergy;
  }
}
