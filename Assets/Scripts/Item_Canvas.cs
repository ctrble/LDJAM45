using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Canvas : MonoBehaviour {
  [SerializeField]
  private Text itemName;
  [SerializeField]
  private Image itemIcon;

  public void UpdateDisplayUI(Weapon_Data weaponData) {
    itemName.text = weaponData.ItemName;
    itemIcon.sprite = weaponData.ItemIcon;
  }
}
