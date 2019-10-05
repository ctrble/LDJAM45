using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Actions : MonoBehaviour {

  public GameObject inventoryObject;
  private int currentItem;
  public List<GameObject> heldItems;
  [SerializeField]
  private bool primaryAttack;

  void Start() {
    primaryAttack = false;

    foreach (Transform child in inventoryObject.transform) {
      heldItems.Add(child.gameObject);
    }

    currentItem = 0;
  }

  void Update() {
    GetInput();
    ChangeWeapon();
    Attack();
  }

  void GetInput() {
    primaryAttack = Input.GetButton("Fire1");

    // change weapons, but only if it's there
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

  void ChangeWeapon() {
    for (int i = 0; i < heldItems.Count; i++) {
      if (currentItem == i) {
        if (!heldItems[i].activeInHierarchy) {
          heldItems[i].SetActive(true);
        }
      }
      else {
        if (heldItems[i].activeInHierarchy) {
          heldItems[i].SetActive(false);
        }
      }
    }
  }

  void Attack() {
    if (primaryAttack) {
      Weapon currentWeapon = inventoryObject.GetComponentInChildren<Weapon>();
      currentWeapon.UseWeapon();
    }
  }
}
