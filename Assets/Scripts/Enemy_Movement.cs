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

  void OnEnable() {
    if (navAgent == null) {
      navAgent = GetComponent<NavMeshAgent>();
    }

    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    retreating = false;
    navAgent.destination = player.position;
  }

  void Update() {
    currentDistanceFromPlayer = (transform.position - player.position).magnitude;

    if (currentDistanceFromPlayer < minDistanceFromPlayer) {
      DoRetreat();
    }
    else if (currentDistanceFromPlayer >= minDistanceFromPlayer) {
      FollowPlayer();
    }
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

    Vector3 retreatDirection = transform.position - player.position;
    Vector3 retreatDistance = retreatDirection.normalized * retreatDistanceFromPlayer;
    retreatPosition = transform.position + retreatDistance;

    navAgent.destination = retreatPosition;
  }

  private void OnDrawGizmos() {
    if (EditorApplication.isPlaying) {
      Gizmos.color = Color.magenta;
      if (navAgent.destination != Vector3.zero) {
        Gizmos.DrawSphere(navAgent.destination, 0.5f);
      }
    }
  }
}
