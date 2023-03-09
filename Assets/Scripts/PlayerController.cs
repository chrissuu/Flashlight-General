using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Subject
{

  [SerializeField] private FieldOfView fieldOfView;
  
  // private FieldOfView _fieldOfView; 
  private IncreaseAngleInvoker _increaseAngleInvoker; 
  private DecreaseAngleInvoker _decreaseAngleInvoker; 

  void Start()
  {
    ICommand increaseAngleCommand = new IncreaseAngleCommand(); 
    _increaseAngleInvoker = new IncreaseAngleInvoker(increaseAngleCommand);
    ICommand decreaseAngleCommand = new DecreaseAngleCommand();
    _decreaseAngleInvoker = new DecreaseAngleInvoker(decreaseAngleCommand);
    
  }

  void Update() 
  {
    // observer pattern 
    Vector3 targetPosition = GetMouseWorldPosition(); 
    Vector3 aimDir = targetPosition;   

    fieldOfView.SetAimDirection(aimDir); 
    fieldOfView.SetOrigin(transform.position);
    
    if (Input.GetMouseButtonDown(0))
    {
      fieldOfView.SetStatus(true); 
      NotifyObservers(FlashlightActions.ChangeColor);
    }
    
    if (Input.GetMouseButtonDown(1))
    {
      fieldOfView.SetStatus(false);
      NotifyObservers(FlashlightActions.ChangeColor); 
    }
    
    // command pattern 
    if (Input.GetKeyDown(KeyCode.W))
    {
      _increaseAngleInvoker.IncreaseAngle();
    }
    if (Input.GetKeyDown(KeyCode.S))
    {
      _decreaseAngleInvoker.DecreaseAngle();
    }

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
    transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
  }
  
  // START UTILS . . . 
  public static Vector3 GetMouseWorldPosition()
  {
    Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    vec.z = 0f; 
    return vec; 
  }

  public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
    Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
    return worldPosition;
  }
  // END UTILS . . . 
}
