using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStopInvoker
{
  ICommand _normalStopCommand;  

  public NormalStopInvoker(ICommand normalStopCommand)
  {
    _normalStopCommand = normalStopCommand;  
  }

  public void InvokeNormalStop()
  {
    _normalStopCommand.Execute(); 
  }
}
