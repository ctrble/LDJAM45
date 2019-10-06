using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Actions : MonoBehaviour {
  public GameObject inventoryObject;
  public Entity enemyEntity;
  public Transform player;
  private int currentItem;
  public List<GameObject> heldItems;
  [SerializeField]
  private bool primaryAction;
  public float angleFromPlayer = 0;
  public float attackAngleThreshold = 20f;

  void Start() {
    if (enemyEntity == null) {
      enemyEntity = gameObject.GetComponent<Entity>();
    }

    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    primaryAction = false;

    GetHeldItems();

    currentItem = 0;
    ChangeItem();
  }

  void Update() {
    if (heldItems.Count != inventoryObject.transform.childCount || !heldItems[currentItem].activeInHierarchy) {
      GetHeldItems();
      ChangeItem();
    }

    Vector3 playerDirection = player.position - transform.position;
    angleFromPlayer = Vector3.Angle(playerDirection, transform.forward);

    // close enough to attack?
    if (angleFromPlayer <= attackAngleThreshold) {
      primaryAction = true;
    }
    else {
      primaryAction = false;
    }

    if (primaryAction) {
      UseItem();
    }
  }

  void GetHeldItems() {
    heldItems.Clear();

    foreach (Transform child in inventoryObject.transform) {
      heldItems.Add(child.gameObject);
    }
  }

  void ChangeItem() {
    for (int i = 0; i < heldItems.Count; i++) {
      heldItems[i].SetActive(currentItem == i);
    }
  }

  void UseItem() {
    Weapon currentWeapon = inventoryObject.GetComponentInChildren<Weapon>();
    if (currentWeapon != null) {
      currentWeapon.UseWeapon();
    }
  }
}
