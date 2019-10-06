using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Billboard : MonoBehaviour {
  public Transform target;

  void OnEnable() {
    if (target == null) {
      target = GameObject.FindWithTag("Player").transform;
    }
  }

  void Update() {
    transform.forward = target.forward;
  }
}
