using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour {

  public Rigidbody itemBody;
  public BoxCollider boxCollider;
  public SphereCollider sphereCollider;
  public float pickupInterval = 0.5f;
  public float dropForce = 0.5f;

  void OnEnable() {
    if (itemBody == null) {
      itemBody = gameObject.GetComponent<Rigidbody>();
    }

    if (boxCollider == null) {
      boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    if (sphereCollider == null) {
      sphereCollider = gameObject.GetComponent<SphereCollider>();
    }

    // init physics on if there's no parent
    bool hasParent = transform.parent != null;
    EnablePhysics(!hasParent);
    sphereCollider.enabled = !hasParent;
  }

  void FixedUpdate() {
    bool dropped = transform.parent == null;
    if (dropped && itemBody.isKinematic) {
      EnablePhysics(true);


      float amountX = Random.Range(-dropForce, dropForce);
      float amountY = Random.Range(-dropForce, dropForce);
      Vector3 randomDirection = new Vector3(amountX, dropForce, amountY);
      itemBody.AddForce(randomDirection, ForceMode.Impulse);

      Invoke("EnableTrigger", pickupInterval);
    }
  }

  void EnablePhysics(bool shouldEnable) {
    // physics on and kinematic off when there's no parent
    boxCollider.enabled = shouldEnable;
    itemBody.isKinematic = !shouldEnable;
  }

  void EnableTrigger() {
    // don't allow actual stealing while it's held, turn off the trigger
    sphereCollider.enabled = true;
  }

  void OnDisable() {
    CancelInvoke();
  }
}
