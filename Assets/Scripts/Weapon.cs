using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
  [SerializeField]
  private Weapon_Data weaponData;

  public void UseWeapon() {
    Debug.Log(weaponData.WeaponName);
    Debug.Log(weaponData.Icon.name);
    Debug.Log(weaponData.AttackDamage);
    Debug.Log(weaponData.AttackRange);
  }
}
