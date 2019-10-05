using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
  [SerializeField]
  private Weapon_Data weaponData;
  [SerializeField]
  private Game_Event OnWeaponSelected;
  public LayerMask hitLayer;
  private Camera mainCamera;
  public Transform crosshairs;

  void OnEnable() {
    SelectWeapon();

    if (mainCamera == null) {
      mainCamera = Camera.main;
    }

    if (crosshairs == null) {
      crosshairs = GameObject.FindGameObjectWithTag("Crosshairs").transform;
    }
  }

  public void SelectWeapon() {
    OnWeaponSelected.Raise();
  }

  public void UseWeapon() {
    // Debug.Log(weaponData.WeaponName);
    // Debug.Log(weaponData.Icon.name);
    // Debug.Log(weaponData.AttackDamage);
    // Debug.Log(weaponData.AttackRange);

    RaycastHit hit;
    Vector3 attackPosition = mainCamera.transform.position;
    Vector3 attackDirection = crosshairs.position - mainCamera.transform.position;

    bool hitCast = Physics.Raycast(attackPosition, attackDirection, out hit, weaponData.AttackRange, hitLayer);
    if (hitCast) {
      Debug.Log(hit.transform.name);
      Debug.DrawRay(attackPosition, attackDirection * weaponData.AttackRange, Color.red);
    }
    else {
      Debug.DrawRay(attackPosition, attackDirection * weaponData.AttackRange, Color.black);
    }
  }
}
