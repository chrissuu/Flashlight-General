using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
public class EnergyScript : Singleton<FieldOfView>
{
  public TextMeshProUGUI EnergyLevel; 
  public int energyScore; 

  void Start() 
  {
    EnergyLevel = GetComponent<TextMeshProUGUI>(); 
  }
  
  void Update() 
  {
    FieldOfView fieldOfView = FieldOfView.Instance;  
    energyScore = fieldOfView.energy; 
    EnergyLevel.text = "Energy: " + energyScore;  
  }
  
}
