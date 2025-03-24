using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementRandom : MonoBehaviour
{
  //Speed
  public float speed = 5f;
  //path
  public GameObject area;
  private List<Vector3> waypoints = new List<Vector3>();
  private int currentwaypointIndex = 0;
  bool endpath = false;
  //La dirección cuando termina el path
  private Vector3 endDirection;
  //timer en el que sigue recto
  public float endFlyTime = 5f;
  private float endTimer = 0f;
  //altura
  public float offsetAltitud;
  void Awake()
  {
    area = GameObject.FindWithTag("Floor");
  }
  Vector3 GetRandomPointInPlane(GameObject plane)
  {
    Vector3 pos = plane.transform.position;
    Vector3 size = new Vector3(10f * plane.transform.localScale.x, 0f, 10f * plane.transform.localScale.z);

    float randomX = Random.Range(pos.x - size.x / 2f, pos.x + size.x / 2f);
    float randomZ = Random.Range(pos.z - size.z / 2f, pos.z + size.z / 2f);

    return new Vector3(randomX, pos.y + offsetAltitud, randomZ);
  }
  // Start is called before the first frame update
  void Start()
  {

    Debug.Log("Creo path aleatorio en area");
    for (int i = 0; i < 3; i++)
    {
      waypoints.Add(GetRandomPointInPlane(area));
      //Debug.Log(waypoints[i]);
    }
  }

  // Update is called once per frame
  void Update()
  {

    if(endpath)
    {
      transform.position += endDirection * speed * Time.deltaTime;
      endTimer += Time.deltaTime;
      if (endTimer >= endFlyTime)
      {
        Destroy(gameObject);
      }
      return;
    }

    if (waypoints.Count == 0) return;

    //Debug.Log(currentWaypointIndex);
    // Move to waypoint
    Vector3 targetWaypoint = waypoints[currentwaypointIndex];
    transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

    // Cuando llega al waypoint, avanza al siguiente
    if (Vector3.Distance(transform.position, targetWaypoint) < 0.1f)
    {
      currentwaypointIndex++;
      if (currentwaypointIndex >= waypoints.Count)
      {
        //restart path
        currentwaypointIndex = waypoints.Count - 1;

        endDirection = (targetWaypoint - transform.position).normalized;

        endpath = true;
      }
    }
  }

}
