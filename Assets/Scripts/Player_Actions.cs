﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Actions : MonoBehaviour {

  public GameObject inventoryObject;
  private int currentItem;
  public List<GameObject> heldItems;
  [SerializeField]
  private bool primaryAction;

  void Start() {
    primaryAction = false;

    GetHeldItems();

    currentItem = 0;
    ChangeItem();
  }

  void Update() {
    GetInput();

    if (!heldItems[currentItem].activeInHierarchy) {
      ChangeItem();
    }

    if (primaryAction) {
      UseItem();
    }
  }

  void GetInput() {
    primaryAction = Input.GetButton("Fire1");

    // change weapons, but only if there's one to change to
    if (Input.GetButtonDown("Item1") && heldItems.Count >= 1) {
      Debug.Log("change to item 1");
      currentItem = 0;
    }
    else if (Input.GetButtonDown("Item2") && heldItems.Count >= 2) {
      Debug.Log("change to item 2");
      currentItem = 1;
    }
    else if (Input.GetButtonDown("Item3") && heldItems.Count >= 3) {
      Debug.Log("change to item 3");
      currentItem = 2;
    }
    else if (Input.GetButtonDown("Item4") && heldItems.Count >= 4) {
      Debug.Log("change to item 4");
      currentItem = 3;
    }
    else if (Input.GetButtonDown("Item5") && heldItems.Count >= 5) {
      Debug.Log("change to item 5");
      currentItem = 4;
    }
    else if (Input.GetButtonDown("Item6") && heldItems.Count >= 6) {
      Debug.Log("change to item 6");
      currentItem = 5;
    }
  }

  void GetHeldItems() {
    foreach (Transform child in inventoryObject.transform) {
      heldItems.Add(child.gameObject);
    }
  }

  void ChangeItem() {
    for (int i = 0; i < heldItems.Count; i++) {
      heldItems[i].SetActive(currentItem == i);
    }
  }

  bool ItemHeld(GameObject item) {
    bool alreadyHeld = false;
    for (int i = 0; i < heldItems.Count; i++) {
      if (heldItems[i].name == item.name) {
        alreadyHeld = true;
        break;
      }
    }
    return alreadyHeld;
  }

  Weapon MatchAmmoType(string type) {
    Weapon matchedType = null;
    for (int i = 0; i < heldItems.Count; i++) {
      if (heldItems[i].name == type) {
        matchedType = heldItems[i].GetComponent<Weapon>();
        break;
      }
    }
    return matchedType;
  }

  void PickupItem(Collider item) {
    // turn off the collider and item object
    item.enabled = false;

    // only add it if we don't already have it
    Ammo ammo = item.gameObject.GetComponent<Ammo>();
    Weapon weapon = item.gameObject.GetComponent<Weapon>();

    if (ammo != null) {
      // consume ammo
      int amount = ammo.amount;
      Debug.Log("ammo get: " + amount + " " + ammo.kind);

      // check type here!
      Weapon matchedWeapon = MatchAmmoType(ammo.kind);
      if (matchedWeapon != null) {
        Debug.Log(matchedWeapon);
        matchedWeapon.ChangeAmmo(amount);
      }

      item.gameObject.SetActive(false);
    }
    else if (weapon != null) {
      // pickup weapon
      if (!ItemHeld(item.gameObject)) {
        item.transform.parent = inventoryObject.transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        heldItems.Add(item.gameObject);
        ChangeItem();
      }
      else {
        Debug.Log("already got one");
      }
    }
    else {
      Debug.Log("what's this?");
    }
  }

  void UseItem() {
    Weapon currentWeapon = inventoryObject.GetComponentInChildren<Weapon>();
    if (currentWeapon != null) {
      currentWeapon.UseWeapon();
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Item")) {
      Debug.Log("can pickup: " + other.name);

      PickupItem(other);
    }
  }
}
