using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class FieldOfView : Singleton<FieldOfView>
{
    // Start is called before the first frame update
    private Mesh mesh; 
    public int energy; 
    private float fov; 
    private Vector3 origin; 
    private float startingAngle; 
    private bool lightOn; 
     
    private void Start()
    {
      energy = 15; 
 
      mesh = new Mesh(); 
      GetComponent<MeshFilter>().mesh = mesh; 
      fov = 20f; // 90 
      origin = Vector3.zero; 
      lightOn = false;  
    }
    
    private void LateUpdate()
    {
      int rayCount = 50; 
      float angle = startingAngle; 
      float angleIncrease = fov / rayCount; 
      float viewDistance = 10f; // 50  
      
      
      Vector3[] vertices = new Vector3[rayCount + 1 + 1];
      Vector2[] uv = new Vector2[vertices.Length]; 
      int[] triangles = new int[rayCount * 3]; 
       
      vertices[0] = origin; 
      
      int vertexIndex = 1; 
      int triangleIndex = 0; 
      for (int i = 0; i <= rayCount; i++)
      {
        Vector3 vertex = origin + GetVectorFromAngle(angle) * viewDistance;  
        RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance);  
        vertices[vertexIndex] = vertex;
        
        if (i>0)
        {
          triangles[triangleIndex + 0] = 0; 
          triangles[triangleIndex + 1] = vertexIndex - 1; 
          triangles[triangleIndex + 2] = vertexIndex; 

          triangleIndex += 3; 
        }

        vertexIndex++; 
        angle -= angleIncrease; 
      }

      mesh.vertices = vertices; 
      mesh.uv = uv; 
      mesh.triangles = triangles; 
      
      // mesh.RecalculateBounds();
    }
     
    public static Vector3 GetVectorFromAngle (float angle)
    {
      float angleRad = angle * (Mathf.PI/180f); 
      return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)); 
    }
    
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
      dir = dir.normalized; 
      float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
      if (n<0) n+= 360; 

      return n; 
    }

    public void SetOrigin(Vector3 origin)
    {
      this.origin = origin; 
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
      startingAngle = GetAngleFromVectorFloat(aimDirection) - fov / 2f;   
    }

    public void SetStatus(bool lightOn)
    {
      this.lightOn = lightOn; 
    }

    public void ChangeColor()
    {
      if (this.lightOn) 
      {
        GetComponent<MeshRenderer>().material.color = Color.yellow; 
      } 
      else 
      {
        GetComponent<MeshRenderer>().material.color = Color.white; 
      }

    }

    public void decreaseEnergy() {
      //if (powerOn) energy -= 3;  
      //else energy -= 1; 
      energy -= 1;  
    }

    public void stopMovement() {
      // stop movement
    }
}
