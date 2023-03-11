using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlashlightBaseState
{
  public abstract void EnterState(FlashlightStateManager flashlight); 
  public abstract void UpdateState(FlashlightStateManager flashlight); 

}
