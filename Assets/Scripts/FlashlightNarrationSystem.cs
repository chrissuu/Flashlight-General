using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightNarrationSystem : Singleton<FieldOfView>, IObserver
{
  [SerializeField] Subject _flashlightSubject;
  //[SerializeField] private FieldOfView fieldOfView;

  public void OnNotify(FlashlightActions action)
  {
    FieldOfView fieldOfView = FieldOfView.Instance; 
    if (action == FlashlightActions.ChangeColor)
    {
      fieldOfView.ChangeColor(); 
    }
  }

  private void OnEnable()
  {
    _flashlightSubject.AddObserver(this);
    Debug.Log("Flashlight Observer Added.");
  }

  private void OnDisable()
  {
    _flashlightSubject.RemoveObserver(this);
    Debug.Log("Flashlight Observer Removed."); 
  }
}

