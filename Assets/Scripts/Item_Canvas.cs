using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Canvas : MonoBehaviour {
  [SerializeField]
  private Text itemName;
  [SerializeField]
  private Image itemIcon;
  [SerializeField]
  private Text remainingAmmo;
  [SerializeField]
  private Text maxAmmo;

  public void UpdateDisplayUI(Weapon_Data weaponData) {
    itemName.text = weaponData.ItemName;
    itemIcon.sprite = weaponData.ItemIcon;
    if (weaponData.InfiniteAmmo) {
      remainingAmmo.text = "∞";
      // maxAmmo.text = "∞";
    }
    // else {
    //   maxAmmo.text = weaponData.MaxAmmo.ToString();
    // }
  }

  public void UpdateRemainingAmmo(int ammo) {
    remainingAmmo.text = ammo.ToString();
  }
}
