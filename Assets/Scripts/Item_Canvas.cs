using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Canvas : MonoBehaviour {
  // suppress warnings about default values
#pragma warning disable CS0649

  [SerializeField]
  private Text itemName;
  [SerializeField]
  private Image itemIcon;


  [SerializeField]
  private Text remainingAmmo;

#pragma warning restore CS0649

  public void UpdateDisplayUI(Weapon_Data weaponData) {
    itemName.text = weaponData.ItemName;
    itemIcon.sprite = weaponData.ItemIcon;
    if (weaponData.InfiniteAmmo) {
      remainingAmmo.text = "∞";
    }
  }

  public void UpdateRemainingAmmo(int ammo) {
    remainingAmmo.text = ammo.ToString();
  }
}
