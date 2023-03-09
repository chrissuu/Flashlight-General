using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAngleInvoker
{
  ICommand _increaseCommand; 

  public IncreaseAngleInvoker(ICommand increaseCommand)
  {
    _increaseCommand = increaseCommand; 
  }

  public void IncreaseAngle()
  {
    _increaseCommand.Execute(); 
  }
}
