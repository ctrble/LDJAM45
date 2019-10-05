using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {
  [SerializeField]
  private string itemName;
  [SerializeField]
  private Sprite itemIcon;

  public string ItemName {
    get {
      return itemName;
    }
  }

  public Sprite ItemIcon {
    get {
      return itemIcon;
    }
  }
}
