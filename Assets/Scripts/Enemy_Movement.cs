using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy_Movement : MonoBehaviour {

  private NavMeshAgent navAgent;

  [Header("Following")]
  private Transform player;
  public float lookSpeed;
  public float maxDistanceFromPlayer = 12;
  public float minDistanceFromPlayer = 3;

  [Header("Strafing")]
  [SerializeField]
  private float strafeTimeRemaining;
  public float maxStrafeInterval = 6f;
  public float minStrafeInterval = 2f;
  private bool changeStrafeAngle;
  private float orbitAngle = 15;
  private float cosAngle;
  private float sinAngle;

  [Header("Searching")]
  [SerializeField]
  private float searchTimeRemaining;
  public float searchTime = 1.5f;
  private bool giveUpSearch;
  private Vector3 lastKnownPlayerPosition;

  [Header("Wandering")]
  [SerializeField]
  private float wanderTimeRemaining;
  public float wanderDistance = 5f;
  public float maxWanderInterval = 5f;
  public float minWanderInterval = 2f;
  private bool changeWanderDestination;
  private Vector3 wanderDestination;

  // gizmo debugs
  private bool following;
  private bool retreating;
  private bool orbiting;
  private bool wandering;
  private bool searching;

  void OnEnable() {
    if (navAgent == null) {
      navAgent = GetComponent<NavMeshAgent>();
    }

    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    wanderDestination = RandomNavSphere(transform.position, wanderDistance);
    giveUpSearch = true;
    lastKnownPlayerPosition = Vector3.zero;

    // orbiting angles
    cosAngle = Mathf.Cos(orbitAngle);
    sinAngle = Mathf.Sin(orbitAngle);

    // debugs
    following = false;
    retreating = false;
    orbiting = false;
    wandering = false;
    searching = false;
  }

  void Update() {
    CheckForBadPaths();

    // Vector3 lastKnownPlayerPosition = Vector3.zero;
    if (CanSeePlayer()) {
      StrafeTimer();
      OrbitPlayer();
      LookAtPlayer();

      giveUpSearch = false;
      lastKnownPlayerPosition = player.position;
    }
    else if (!CanSeePlayer() && !giveUpSearch) {
      SearchTimer();
      // following = false;
      // retreating = false;
      // orbiting = false;
      // wandering = false;
      // searching = true;
      // navAgent.destination = RandomNavSphere(lastKnownPlayerPosition, wanderDistance * 0.5f);
      // // navAgent.destination = lastKnownPlayerPosition;
    }
    else {
      giveUpSearch = true;
      WanderTimer();
      Wander();
    }

  }

  void LookAtPlayer() {
    Vector3 direction = player.position - transform.position;
    Quaternion toRotation = Quaternion.LookRotation(direction.normalized);
    Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, toRotation, lookSpeed * Time.deltaTime);

    transform.rotation = newRotation;
    Debug.DrawRay(transform.position, transform.forward * 3, Color.green);
  }

  void StrafeTimer() {
    if (!changeStrafeAngle) {
      strafeTimeRemaining -= Time.deltaTime;
      if (strafeTimeRemaining <= 0f) {
        float newStrafeInterval = Random.Range(minStrafeInterval, maxStrafeInterval);
        changeStrafeAngle = true;
        strafeTimeRemaining = newStrafeInterval;
      }
    }
  }

  void WanderTimer() {
    if (!changeWanderDestination) {
      wanderTimeRemaining -= Time.deltaTime;
      if (wanderTimeRemaining <= 0f) {
        float newWanderInterval = Random.Range(minWanderInterval, maxWanderInterval);
        changeWanderDestination = true;
        wanderTimeRemaining = newWanderInterval;
      }
    }
  }

  void SearchTimer() {
    if (!giveUpSearch) {
      searchTimeRemaining -= Time.deltaTime;
      if (searchTimeRemaining <= 0f) {
        float newSearchTime = searchTime;
        giveUpSearch = true;
        searchTimeRemaining = newSearchTime;
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
      following = true;
      retreating = false;
      orbiting = false;
      wandering = false;
      searching = false;
      MoveTowardsPlayer();
    }
    else if (distanceFromLeader < minDistanceFromLeader) {
      following = false;
      retreating = true;
      orbiting = false;
      wandering = false;
      searching = false;
      MoveAwayFromPlayer(directionFromPlayer);
    }
    else {
      // orbit
      following = false;
      retreating = false;
      orbiting = true;
      wandering = false;
      searching = false;
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
    if (changeStrafeAngle) {
      orbitAngle = -orbitAngle;
      cosAngle = Mathf.Cos(orbitAngle);
      sinAngle = Mathf.Sin(orbitAngle);
      changeStrafeAngle = false;
    }
  }

  void Wander() {
    following = false;
    retreating = false;
    orbiting = false;
    wandering = true;
    searching = true;
    if (changeWanderDestination) {
      wanderDestination = RandomNavSphere(transform.position, wanderDistance);
      changeWanderDestination = false;
    }
    navAgent.destination = wanderDestination;
  }

  void Search() {
    following = false;
    retreating = false;
    orbiting = false;
    wandering = false;
    searching = true;

    // navAgent.destination = RandomNavSphere(lastKnownPlayerPosition, wanderDistance * 0.5f);
    navAgent.destination = lastKnownPlayerPosition;
  }

  Vector3 RotateBy30Degrees(Vector3 originalVector) {
    Vector3 rotated = new Vector3((originalVector.x * cosAngle) - (originalVector.z * sinAngle), 0, (originalVector.x * sinAngle) + (originalVector.z * cosAngle));
    return rotated;
  }

  Vector3 RandomNavSphere(Vector3 origin, float dist) {
    Vector3 randDirection = Random.insideUnitSphere * dist;
    randDirection += origin;
    NavMeshHit navHit;
    int layerMask = 1 << NavMesh.GetAreaFromName("Walkable");
    NavMesh.SamplePosition(randDirection, out navHit, dist, layerMask);
    return navHit.position;
  }

  void CheckForBadPaths() {
    if (navAgent.pathStatus == NavMeshPathStatus.PathInvalid || navAgent.pathStatus == NavMeshPathStatus.PathPartial) {
      Debug.Log("yikes we got pretty lost: " + navAgent.pathStatus);
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
      if (following) {
        Gizmos.color = Color.red;
      }
      else if (retreating) {
        Gizmos.color = Color.yellow;
      }
      else if (orbiting) {
        Gizmos.color = Color.blue;
      }
      else if (wandering) {
        Gizmos.color = Color.green;
      }
      else if (searching) {
        Gizmos.color = Color.magenta;
      }

      if (navAgent.destination != Vector3.zero) {
        Gizmos.DrawSphere(navAgent.destination, 0.5f);
      }
    }
  }
}
