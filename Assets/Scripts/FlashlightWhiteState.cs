using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightWhiteState : FlashlightBaseState
{
   
  public override void EnterState(FlashlightStateManager flashlight) {
    flashlight.square.color = Color.white;  
  }

  public override void UpdateState(FlashlightStateManager flashlight) {
    FieldOfView fieldOfView = FieldOfView.Instance; 
    
    if (fieldOfView.lightOn == true ){
      flashlight.SwitchState(flashlight.yellowState); 
    }
  }
}
