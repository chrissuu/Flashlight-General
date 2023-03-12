using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : Singleton<FieldOfView>
{

  private List<IObserver> _observers = new List<IObserver>(); 

  public void AddObserver(IObserver observer)
  {
    _observers.Add(observer); 
  }

  public void RemoveObserver(IObserver observer)
  {
    _observers.Remove(observer); 
  }
  
  protected void NotifyObservers(FlashlightActions action) 
  {
    _observers.ForEach((_observer) => {_observer.OnNotify(action);});
  }
  
}
