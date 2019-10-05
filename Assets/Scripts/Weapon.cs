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
    AttackTimer();
  }

  public void SelectWeapon() {
    OnWeaponSelected.Raise();
  }

  public void UseWeapon() {
    if (canAttack) {
      canAttack = false;
      WeaponEffect();
    }
  }

  void WeaponEffect() {
    switch (weaponData.ItemName) {
      case "Fists":
        HitScanBullet(true);
        break;
      case "Pistol":
        HitScanBullet();
        break;
      default:
        break;
    }
  }

  public void DropWeapon(Transform item) {
    Debug.Log("dude, drop it! " + item.name);
    item.parent = null;
  }

  void DisarmEnemy(GameObject enemy) {
    Weapon weapon = enemy.GetComponentInChildren<Weapon>();
    if (weapon != null && weapon.CompareTag("Item")) {
      weapon.DropWeapon(weapon.transform);
    }
  }

  void HitScanBullet(bool disarm = false) {
    RaycastHit hit;
    Vector3 attackPosition = mainCamera.transform.position;
    Vector3 attackDirection = crosshairs.position - mainCamera.transform.position;

    bool hitCast = Physics.Raycast(attackPosition, attackDirection, out hit, weaponData.AttackRange, hitLayer);
    if (hitCast) {
      Debug.DrawRay(attackPosition, attackDirection * weaponData.AttackRange, Color.red);
      Debug.Log("hit: " + hit.transform.name);

      Entity entity = hit.transform.GetComponent<Entity>();
      if (entity != null) {
        entity.Damage(weaponData.AttackDamage);

        // should they be disarmed?
        if (disarm) {
          DisarmEnemy(hit.transform.gameObject);
        }
      }
    }
    else {
      Debug.DrawRay(attackPosition, attackDirection * weaponData.AttackRange, Color.black);
    }
  }

  void AttackTimer() {
    if (!canAttack) {
      timeRemaining -= Time.deltaTime;
      if (timeRemaining <= 0f) {
        canAttack = true;
        timeRemaining = weaponData.AttackInterval;
      }
    }
  }
}
