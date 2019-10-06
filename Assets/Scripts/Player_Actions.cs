using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Actions : MonoBehaviour {

  public GameObject inventoryObject;
  public Entity playerEntity;
  private int currentItem;
  public List<GameObject> heldItems;
  [SerializeField]
  private bool primaryAction;

  void Start() {
    if (playerEntity == null) {
      playerEntity = gameObject.GetComponent<Entity>();
    }

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
      currentItem = 0;
    }
    else if (Input.GetButtonDown("Item2") && heldItems.Count >= 2) {
      currentItem = 1;
    }
    else if (Input.GetButtonDown("Item3") && heldItems.Count >= 3) {
      currentItem = 2;
    }
    else if (Input.GetButtonDown("Item4") && heldItems.Count >= 4) {
      currentItem = 3;
    }
    else if (Input.GetButtonDown("Item5") && heldItems.Count >= 5) {
      currentItem = 4;
    }
    else if (Input.GetButtonDown("Item6") && heldItems.Count >= 6) {
      currentItem = 5;
    }
  }

  void PickupItem(Collider item) {
    // turn off the collider and item object
    item.enabled = false;

    // only add it if we don't already have it
    Ammo ammoPickup = item.gameObject.GetComponent<Ammo>();
    Armor armorPickup = item.gameObject.GetComponent<Armor>();
    Weapon weaponPickup = item.gameObject.GetComponent<Weapon>();

    if (ammoPickup != null) {
      // consume ammo
      int amount = ammoPickup.amount;

      // check type here!
      Weapon matchedWeapon = MatchAmmoType(ammoPickup.kind);
      if (matchedWeapon != null) {
        matchedWeapon.ChangeAmmo(amount);
      }

      item.gameObject.SetActive(false);
      Destroy(item.gameObject);
    }
    else if (armorPickup != null) {
      // consume armor
      int amount = armorPickup.amount;
      Debug.Log("get some armor");

      playerEntity.GetArmor(amount);
      item.gameObject.SetActive(false);
      Destroy(item.gameObject);
    }
    else if (weaponPickup != null) {
      // pickup weapon
      if (!ItemHeld(item.gameObject)) {
        item.transform.parent = inventoryObject.transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        heldItems.Add(item.gameObject);
        ChangeItem();
      }
      else {
        // get the rest of the ammo
        Weapon matchedWeapon = MatchAmmoType(weaponPickup.name);
        if (matchedWeapon != null) {
          int pickupAmount = weaponPickup.remainingAmmo;
          matchedWeapon.ChangeAmmo(pickupAmount);
          weaponPickup.remainingAmmo -= pickupAmount;
        }
      }
    }
    else {
      Debug.Log("what's this? not a valid weapon or ammo type");
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

  void PickupAvailableAmmo(Weapon weaponToPickup) {
    // get the rest of the ammo
    Weapon matchedWeapon = MatchAmmoType(weaponToPickup.name);
    if (matchedWeapon != null) {
      int pickupAmount = weaponToPickup.remainingAmmo;
      matchedWeapon.ChangeAmmo(pickupAmount);
    }
  }

  void UseItem() {
    Weapon currentWeapon = inventoryObject.GetComponentInChildren<Weapon>();
    if (currentWeapon != null) {
      currentWeapon.UseWeapon();
    }
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

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Item")) {
      PickupItem(other);
    }
  }
}
