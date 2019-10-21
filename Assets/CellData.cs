using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum Occupier {computer, human, blank}
public class CellData : MonoBehaviour
{
   public Occupier OccupiedBy = Occupier.blank;
   public Vector2Int GridPosition;


   private void OnDrawGizmos()
   {
      if (OccupiedBy == Occupier.human)
      {
         Gizmos.color = Color.red;
         Gizmos.DrawCube(transform.position, Vector3.one/2f);
      }
      
      if (OccupiedBy == Occupier.computer)
      {
         Gizmos.color = Color.green;
         Gizmos.DrawCube(transform.position, Vector3.one/2f);
      }
      
      
   }
}
