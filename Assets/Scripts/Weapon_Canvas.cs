using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Canvas : MonoBehaviour {
  [SerializeField]
  private Text weaponName;
  [SerializeField]
  private Image weaponIcon;

  public void UpdateDisplayUI(Weapon_Data weaponData) {
    weaponName.text = weaponData.WeaponName;
    weaponIcon.sprite = weaponData.Icon;
  }
}
