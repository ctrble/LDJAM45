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
  public float strafeSpeed = 2;
  public float strafeInterval;
  [SerializeField]
  private float timeRemaining;
  [SerializeField]
  private bool changeStrafeDirection;
  private Vector3 lastPosition;
  [SerializeField]
  private float navSpeed;

  void OnEnable() {
    if (navAgent == null) {
      navAgent = GetComponent<NavMeshAgent>();
    }

    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    retreating = false;
    navAgent.destination = player.position;
    lastPosition = transform.position;
  }

  void Update() {
    // face the player
    transform.LookAt(player);

    // about how fast is this thing
    navSpeed = Mathf.Lerp(navSpeed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.5f);
    if (navSpeed <= 0.2f) {
      Strafe();
    }

    currentDistanceFromPlayer = (transform.position - player.position).magnitude;

    if (currentDistanceFromPlayer < minDistanceFromPlayer) {
      DoRetreat();
    }
    else if (currentDistanceFromPlayer >= minDistanceFromPlayer) {
      FollowPlayer();
      StrafeTimer();
    }
  }

  void LateUpdate() {
    lastPosition = transform.position;
  }

  void FollowPlayer() {
    retreating = false;
    navAgent.autoBraking = true;
    navAgent.stoppingDistance = maxDistanceFromPlayer;

    navAgent.destination = player.position;
  }

  void Strafe() {
    Vector3 strafeDirection = transform.right;
    if (changeStrafeDirection) {
      strafeDirection = -strafeDirection;
      changeStrafeDirection = false;
    }
    Debug.DrawRay(transform.position, strafeDirection * strafeSpeed, Color.blue);
    navAgent.Move(strafeDirection * strafeSpeed * Time.deltaTime);
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

  void StrafeTimer() {
    if (!changeStrafeDirection) {
      timeRemaining -= Time.deltaTime;
      if (timeRemaining <= 0f) {
        changeStrafeDirection = true;
        timeRemaining = strafeInterval;
      }
    }
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
