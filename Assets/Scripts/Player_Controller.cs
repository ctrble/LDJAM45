using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {
  public CharacterController characterController;
  public Camera mainCamera;
  public float speed = 6.0f;
  public float jumpSpeed = 8.0f;
  public float gravity = 20.0f;

  private Vector3 playerInput = Vector3.zero;
  [SerializeField]
  private Vector3 moveDirection = Vector3.zero;

  void Start() {
    if (characterController == null) {
      characterController = GetComponent<CharacterController>();
    }

    if (mainCamera == null) {
      mainCamera = Camera.main;
    }
  }

  void Update() {
    GetInputs();
    MovePlayer();
    RotatePlayer();
  }

  void GetInputs() {
    playerInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

    if (Input.GetButton("Jump")) {
      playerInput.y = 1;
    }
  }

  void MovePlayer() {
    if (characterController.isGrounded) {
      // move around! front, back, strafe
      moveDirection = playerInput * speed;

      // jump, since we can
      if (playerInput.y > 0) {
        moveDirection.y = jumpSpeed;
      }
    }

    // apply gravity
    moveDirection.y -= gravity * Time.deltaTime;

    // adjust to transform.forward
    moveDirection = transform.TransformDirection(moveDirection);
    characterController.Move(moveDirection * Time.deltaTime);
  }

  void RotatePlayer() {
    transform.forward = mainCamera.transform.forward;
  }
}
