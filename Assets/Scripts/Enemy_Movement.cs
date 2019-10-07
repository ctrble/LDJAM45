using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy_Movement : MonoBehaviour {

  private NavMeshAgent navAgent;
  private Transform player;
  public float maxDistanceFromPlayer = 12;
  public float retreatDistanceFromPlayer = 6;
  public float minDistanceFromPlayer = 3;
  public float currentDistanceFromPlayer;
  public Vector3 retreatPosition = Vector3.zero;
  public bool retreating;
  public bool orbiting;
  public float strafeSpeed = 2;
  public float maxStrafeInterval;
  public float minStrafeInterval;
  // public float currentStrafeInterval;
  [SerializeField]
  private float timeRemaining;
  [SerializeField]
  private bool changeStrafeDirection;
  private Vector3 lastPosition;
  [SerializeField]
  private float navSpeed;
  [SerializeField]
  private Vector3 strafeDirection;
  public float lookSpeed;
  private float orbitAngle = 15;
  private float cosAngle;
  private float sinAngle;
  public LayerMask floorMask;

  void OnEnable() {
    if (navAgent == null) {
      navAgent = GetComponent<NavMeshAgent>();
    }

    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    cosAngle = Mathf.Cos(orbitAngle);
    sinAngle = Mathf.Sin(orbitAngle);

    retreating = false;
    orbiting = false;
    // navAgent.destination = player.position;
    strafeDirection = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
    lastPosition = transform.position;

    // LookForPlayer();
  }

  void Update() {
    if (navAgent.pathStatus == NavMeshPathStatus.PathInvalid || navAgent.pathStatus == NavMeshPathStatus.PathPartial) {
      Debug.Log("yikes " + navAgent.pathStatus);
      navAgent.ResetPath();

      NavMeshHit hit;
      if (navAgent.FindClosestEdge(out hit)) {
        navAgent.SetDestination(hit.position);
      }
    }

    // face the player
    LookAtPlayer();

    // about how fast is this thing
    navSpeed = Mathf.Lerp(navSpeed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.5f);

    // and how far away is it?
    // currentDistanceFromPlayer = (transform.position - player.position).magnitude;

    // if (currentDistanceFromPlayer < minDistanceFromPlayer || retreating) {
    //   DoRetreat();
    // }
    // else
    // if (currentDistanceFromPlayer >= minDistanceFromPlayer && !retreating) {
    // if (CanSeePlayer()) {
    // FollowPlayer();

    // strafing
    StrafeTimer();

    // if (navSpeed <= 0.01f) {
    //   Strafe();
    // }
    // }
    // }
    // if (currentDistanceFromPlayer >= minDistanceFromPlayer) {
    //   // move towards
    //   Vector3 offsetFromLeader = player.transform.position - transform.position;
    // }
    // else if (currentDistanceFromPlayer) {
    //   // move away
    // }
    // else {
    OrbitPlayer();
    // }
  }

  void LookAtPlayer() {
    Vector3 direction = player.position - transform.position;
    Quaternion toRotation = Quaternion.LookRotation(direction.normalized);

    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, lookSpeed * Time.deltaTime);
  }

  void LateUpdate() {
    lastPosition = transform.position;
  }

  bool CanSeePlayer() {
    NavMeshHit hit;
    // true if didn't hit anything between the enemy and the player
    return !navAgent.Raycast(player.position, out hit);
  }

  void FollowPlayer() {
    retreating = false;
    navAgent.autoBraking = true;
    navAgent.stoppingDistance = maxDistanceFromPlayer;

    navAgent.destination = player.position;
  }

  void DoRetreat() {
    retreating = true;
    navAgent.autoBraking = false;
    navAgent.stoppingDistance = 0;

    if (retreatPosition == Vector3.zero) {
      // get a new position to retreat to
      Vector3 retreatDirection = transform.position - player.position;
      Vector3 retreatDistance = retreatDirection.normalized * retreatDistanceFromPlayer;
      retreatPosition = transform.position + retreatDistance;
    }

    navAgent.destination = retreatPosition;

    if (navAgent.remainingDistance <= 1f) {
      // we made it, reset
      retreating = false;
      retreatPosition = Vector3.zero;
    }
  }

  private void OrbitPlayer() {
    Vector3 offsetFromLeader = player.transform.position - transform.position;
    float distanceFromLeader = offsetFromLeader.sqrMagnitude;
    float maxDistanceFromLeader = maxDistanceFromPlayer * maxDistanceFromPlayer;
    float minDistanceFromLeader = minDistanceFromPlayer * minDistanceFromPlayer;

    if (distanceFromLeader > maxDistanceFromLeader) {
      // moveTowardsLeader();
      retreating = false;
      orbiting = false;
      Debug.Log(gameObject.name + " moving towards " + player.position);
      navAgent.destination = player.position;
    }
    else if (distanceFromLeader < minDistanceFromLeader) {
      // moveAwayFromLeader();
      retreating = true;
      orbiting = false;
      // navAgent.destination = -offsetFromLeader.normalized;
      // Vector3 worldDirection = transform.TransformDirection(-transform.forward);
      // Debug.Log(worldDirection);

      Vector3 newPosition = transform.position - offsetFromLeader.normalized;
      Debug.Log(gameObject.name + " moving away " + newPosition);
      navAgent.destination = newPosition;
    }
    else {
      // orbit
      retreating = false;
      orbiting = true;
      Vector3 rotatedPosition = player.transform.position - offsetFromLeader.normalized + RotateBy30Degrees(offsetFromLeader);
      // Vector3 rotatedPosition = player.transform.position + RotateBy30Degrees(offsetFromLeader);
      if (changeStrafeDirection) {
        // strafeDirection = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
        orbitAngle = -orbitAngle;
        cosAngle = Mathf.Cos(orbitAngle);
        sinAngle = Mathf.Sin(orbitAngle);
        changeStrafeDirection = false;
      }
      rotatedPosition += strafeDirection;
      Debug.Log(gameObject.name + " orbiting " + rotatedPosition);
      navAgent.destination = rotatedPosition;
    }
  }

  Vector3 RotateBy30Degrees(Vector3 originalVector) {
    Vector3 rotated = new Vector3((originalVector.x * cosAngle) - (originalVector.z * sinAngle), 0, (originalVector.x * sinAngle) + (originalVector.z * cosAngle));
    return rotated;
  }

  // public static Vector3 RandomNavSphere(Vector3 origin, float dist) {
  //   Vector3 randDirection = Random.insideUnitSphere * dist;

  //   randDirection += origin;

  //   NavMeshHit navHit;

  //   NavMesh.SamplePosition(randDirection, out navHit, dist, floorMask);

  //   return navHit.position;
  // }

  void Strafe() {
    if (changeStrafeDirection) {
      // strafeDirection = -strafeDirection;
      strafeDirection = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
      changeStrafeDirection = false;
    }

    // back and forth we go!
    Vector3 worldDirection = transform.TransformDirection(strafeDirection);
    navAgent.Move(worldDirection * strafeSpeed * Time.deltaTime);
  }

  void StrafeTimer() {
    if (!changeStrafeDirection) {
      timeRemaining -= Time.deltaTime;
      if (timeRemaining <= 0f) {
        float newStrafeInterval = Random.Range(minStrafeInterval, maxStrafeInterval);
        // float newStrafeInterval = 3;
        changeStrafeDirection = true;
        timeRemaining = newStrafeInterval;
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
