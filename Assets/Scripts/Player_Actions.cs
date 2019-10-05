using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Actions : MonoBehaviour {

  [SerializeField]
  private bool primaryAttack;

  void Start() {
    primaryAttack = false;
  }

  void Update() {
    GetInput();
  }

  void GetInput() {
    primaryAttack = Input.GetButton("Fire1");
  }

  void Attack() {

  }
}
