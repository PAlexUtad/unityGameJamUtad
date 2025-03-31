using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
  //waypoints
  public Transform[] waypoints;
    //current waypoint
    private int currentWaypointIndex = 0;
  //Speed
  public float speed = 5f; 
  // Start is called before the first frame update
  void Start()
  {
    
  }

    // Update is called once per frame
  void Update()
  {
    if (waypoints.Length == 0) return;

    //Debug.Log(currentWaypointIndex);
    // Move to waypoint
    Transform targetWaypoint = waypoints[currentWaypointIndex];
    transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

    // Cuando llega al waypoint, avanza al siguiente
    if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
    {
      currentWaypointIndex++;
      if (currentWaypointIndex >= waypoints.Length)
      {
        //restart path
        currentWaypointIndex = waypoints.Length - 1;
      }
    }
  }
}
