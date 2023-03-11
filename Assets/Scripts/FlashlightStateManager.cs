using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightStateManager : MonoBehaviour
{
  public FlashlightBaseState currentState; 
  public FlashlightYellowState yellowState = new FlashlightYellowState();  
  public FlashlightWhiteState whiteState = new FlashlightWhiteState(); 
  
  public SpriteRenderer square; 
  // Start is called before the first frame update
  void Start()
  {
    square = GetComponent<SpriteRenderer>(); 
    currentState = yellowState; 
    currentState.EnterState(this);  
  }
  
  void Update() {
    currentState.UpdateState(this); 
  }
  public void SwitchState(FlashlightBaseState state) {
    currentState = state; 
    state.EnterState(this); 
  }
  // Update is called once per frame
}
