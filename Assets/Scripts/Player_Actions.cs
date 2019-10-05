using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Actions : MonoBehaviour {

  public GameObject weaponObject;
  public List<GameObject> allWeapons;
  [SerializeField]
  private bool primaryAttack;

  void Start() {
    primaryAttack = false;

    foreach (Transform child in weaponObject.transform) {
      allWeapons.Add(child.gameObject);
    }
  }

  void Update() {
    GetInput();
    Attack();
  }

  void GetInput() {
    primaryAttack = Input.GetButton("Fire1");
  }

  void Attack() {
    if (primaryAttack) {
      Weapon currentWeapon = weaponObject.GetComponentInChildren<Weapon>();
      currentWeapon.UseWeapon();
      currentWeapon.SelectWeapon();
    }
  }
}
