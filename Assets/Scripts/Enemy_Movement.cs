using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy_Movement : MonoBehaviour {

  private NavMeshAgent navAgent;
  private Transform player;
  public float lookSpeed;
  public float maxDistanceFromPlayer = 12;
  public float minDistanceFromPlayer = 3;
  [SerializeField]
  private float timeRemaining;
  public float maxStrafeInterval;
  public float minStrafeInterval;
  private bool changeOrbitAngle;
  private float orbitAngle = 15;
  private float cosAngle;
  private float sinAngle;
  public LayerMask floorMask;
  private bool retreating;
  private bool orbiting;

  void OnEnable() {
    if (navAgent == null) {
      navAgent = GetComponent<NavMeshAgent>();
    }

    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // orbiting angles
    cosAngle = Mathf.Cos(orbitAngle);
    sinAngle = Mathf.Sin(orbitAngle);

    // debugs
    retreating = false;
    orbiting = false;
  }

  void Update() {
    CheckForBadPaths();

    StrafeTimer();
    OrbitPlayer();
    LookAtPlayer();
  }

  void LookAtPlayer() {
    Vector3 direction = player.position - transform.position;
    Quaternion toRotation = Quaternion.LookRotation(direction.normalized);
    Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, toRotation, lookSpeed * Time.deltaTime);

    transform.rotation = newRotation;
    Debug.DrawRay(transform.position, transform.forward * 3, Color.green);
  }

  void StrafeTimer() {
    if (!changeOrbitAngle) {
      timeRemaining -= Time.deltaTime;
      if (timeRemaining <= 0f) {
        float newStrafeInterval = Random.Range(minStrafeInterval, maxStrafeInterval);
        changeOrbitAngle = true;
        timeRemaining = newStrafeInterval;
      }
    }
  }

  bool CanSeePlayer() {
    NavMeshHit hit;
    // true if didn't hit anything between the enemy and the player
    return !navAgent.Raycast(player.position, out hit);
  }

  void OrbitPlayer() {
    Vector3 directionFromPlayer = player.transform.position - transform.position;

    float distanceFromLeader = directionFromPlayer.sqrMagnitude;
    float maxDistanceFromLeader = maxDistanceFromPlayer * maxDistanceFromPlayer;
    float minDistanceFromLeader = minDistanceFromPlayer * minDistanceFromPlayer;

    if (distanceFromLeader > maxDistanceFromLeader) {
      retreating = false;
      orbiting = false;
      MoveTowardsPlayer();
    }
    else if (distanceFromLeader < minDistanceFromLeader) {
      retreating = true;
      orbiting = false;
      MoveAwayFromPlayer(directionFromPlayer);
    }
    else {
      // orbit
      retreating = false;
      orbiting = true;
      StrafeAroundPlayer(directionFromPlayer);
    }
  }

  void MoveTowardsPlayer() {
    navAgent.destination = player.position;
  }

  void MoveAwayFromPlayer(Vector3 direction) {
    Vector3 newPosition = transform.position - direction.normalized;
    navAgent.destination = newPosition;
  }

  void StrafeAroundPlayer(Vector3 direction) {
    Vector3 rotatedPosition = player.transform.position - direction.normalized + RotateBy30Degrees(direction);
    UpdateStrafeAngle();
    navAgent.destination = rotatedPosition;
  }

  void UpdateStrafeAngle() {
    if (changeOrbitAngle) {
      orbitAngle = -orbitAngle;
      cosAngle = Mathf.Cos(orbitAngle);
      sinAngle = Mathf.Sin(orbitAngle);
      changeOrbitAngle = false;
    }
  }

  Vector3 RotateBy30Degrees(Vector3 originalVector) {
    Vector3 rotated = new Vector3((originalVector.x * cosAngle) - (originalVector.z * sinAngle), 0, (originalVector.x * sinAngle) + (originalVector.z * cosAngle));
    return rotated;
  }

  // wander
  // public static Vector3 RandomNavSphere(Vector3 origin, float dist) {
  //   Vector3 randDirection = Random.insideUnitSphere * dist;
  //   randDirection += origin;
  //   NavMeshHit navHit;
  //   NavMesh.SamplePosition(randDirection, out navHit, dist, floorMask);
  //   return navHit.position;
  // }

  // dont this this is needed
  // void LookAtPlayer() {
  //   Vector3 direction = player.position - transform.position;
  //   Quaternion toRotation = Quaternion.LookRotation(direction.normalized);
  //   transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, lookSpeed * Time.deltaTime);
  // }

  void CheckForBadPaths() {
    if (navAgent.pathStatus == NavMeshPathStatus.PathInvalid || navAgent.pathStatus == NavMeshPathStatus.PathPartial) {
      Debug.Log("yikes " + navAgent.pathStatus);
      navAgent.ResetPath();

      // this sucks, just run to a wall
      NavMeshHit hit;
      if (navAgent.FindClosestEdge(out hit)) {
        navAgent.SetDestination(hit.position);
      }
    }
  }

  private void OnDrawGizmos() {
    if (EditorApplication.isPlaying) {
      if (retreating) {
        Gizmos.color = Color.yellow;
      }
      else if (orbiting) {
        Gizmos.color = Color.blue;
      }
      else {
        Gizmos.color = Color.magenta;
      }
      if (navAgent.destination != Vector3.zero) {
        Gizmos.DrawSphere(navAgent.destination, 0.5f);
      }
    }
  }
}
