using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Subject
                                
{

  // [SerializeField] private FieldOfView fieldOfView;
  FieldOfView fieldOfView;  
  // private FieldOfView _fieldOfView; 
  private PowerUpStopInvoker _powerUpStopInvoker; 
  private NormalStopInvoker _normalStopInvoker; 

  void Start()
  {
    fieldOfView = FieldOfView.Instance; 
    ICommand powerUpStopCommand = new PowerUpStopCommand(); 
    _powerUpStopInvoker = new PowerUpStopInvoker(powerUpStopCommand);
    ICommand normalStopCommand = new NormalStopCommand();
    _normalStopInvoker = new NormalStopInvoker(normalStopCommand);
    Cursor.lockState = CursorLockMode.None;

  }

  void Update() 
  {
    /*
    PAUSING GAME WHEN BATTLE ONGOING 
    while (___.battleOngoing) {
      yield WaitForSeconds(1); 
    }
     */
     
    // observer pattern 
    Vector3 targetPosition = GetMouseWorldPosition(); 
    Vector3 aimDir = targetPosition;   

    //FieldOfView fieldOfView = FieldOfView.Instance; 
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
      _powerUpStopInvoker.InvokePowerUpStop(); 
    }
    if (Input.GetKeyDown(KeyCode.S))
    {
      _normalStopInvoker.InvokeNormalStop(); 
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
