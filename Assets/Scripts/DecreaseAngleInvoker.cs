using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseAngleInvoker : MonoBehaviour
{
  ICommand _decreaseCommand; 

  public DecreaseAngleInvoker(ICommand decreaseCommand)
  {
    _decreaseCommand = decreaseCommand; 
  }

  public void DecreaseAngle()
  {
    _decreaseCommand.Execute(); 
  }
}
