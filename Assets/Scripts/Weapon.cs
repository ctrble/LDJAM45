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
  [SerializeField]
  private float timeRemaining;
  [SerializeField]
  private bool canAttack;

  void OnEnable() {
    SelectWeapon();

    if (mainCamera == null) {
      mainCamera = Camera.main;
    }

    if (crosshairs == null) {
      crosshairs = GameObject.FindGameObjectWithTag("Crosshairs").transform;
    }

    timeRemaining = weaponData.AttackInterval;
    canAttack = true;
  }

  void Update() {
    ShotTimer();
  }

  public void SelectWeapon() {
    OnWeaponSelected.Raise();
  }

  public void UseWeapon() {
    // Debug.Log(weaponData.WeaponName);
    // Debug.Log(weaponData.Icon.name);
    // Debug.Log(weaponData.AttackDamage);
    // Debug.Log(weaponData.AttackRange);

    if (canAttack) {
      canAttack = false;

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

  void ShotTimer() {
    if (!canAttack) {
      timeRemaining -= Time.deltaTime;
      if (timeRemaining <= 0f) {
        canAttack = true;
        timeRemaining = weaponData.AttackInterval;
      }
    }
  }
}
