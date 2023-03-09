using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class EnergyScript : MonoBehaviour
{
  public Text EnergyLevel; 

  void Start() 
  {
    EnergyLevel.text = FieldOfView.Instance.energy; 
  }
  
  void Update() 
  {
    EnergyLevel.text = FieldOfView.Instance.energy; 
  }
  
}
