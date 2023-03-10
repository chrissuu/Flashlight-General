using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpStopInvoker
{
  ICommand _powerUpStopCommand; 

  public PowerUpStopInvoker(ICommand powerUpStopCommand)
  {
    _powerUpStopCommand = powerUpStopCommand;  
  }

  public void InvokePowerUpStop()
  {
    _powerUpStopCommand.Execute(); 
  }
}
