using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightYellowState : FlashlightBaseState
{
  public override void EnterState(FlashlightStateManager flashlight) {
    flashlight.square.color = Color.yellow;   
  }

  public override void UpdateState(FlashlightStateManager flashlight) {
    FieldOfView fieldOfView = FieldOfView.Instance; 
    Debug.Log(fieldOfView.lightOn); 
    if (fieldOfView.lightOn == false) {
      Debug.Log("2"); 
      flashlight.SwitchState(flashlight.whiteState); 
    }
  }
}
