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
    Vector3 input = playerInput;
    Vector3 worldDirection = transform.TransformDirection(input);

    // zero y to prevent moving up when we don't want to
    worldDirection.y = 0;

    if (characterController.isGrounded) {
      // move around: front, back, strafe
      moveDirection = worldDirection * speed;

      // jump, since we can
      if (input.y > 0) {
        moveDirection.y = jumpSpeed;
      }
    }

    // apply gravity
    moveDirection.y -= gravity * Time.deltaTime;

    // finally, move
    characterController.Move(moveDirection * Time.deltaTime);
  }

  void RotatePlayer() {
    transform.forward = mainCamera.transform.forward;
  }
}
