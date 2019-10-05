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
  private Text maxAmmo;
  [SerializeField]
  private Text remainingAmmo;

  public void UpdateDisplayUI(Weapon_Data weaponData) {
    itemName.text = weaponData.ItemName;
    itemIcon.sprite = weaponData.ItemIcon;
    maxAmmo.text = weaponData.MaxAmmo.ToString();
    remainingAmmo.text = weaponData.RemainingAmmo.ToString();
  }
}
