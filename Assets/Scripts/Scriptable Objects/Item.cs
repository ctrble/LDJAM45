using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {
  // suppress warnings about default values
#pragma warning disable CS0649

  [SerializeField]
  private string itemName;
  [SerializeField]
  private Sprite itemIcon;

#pragma warning restore CS0649

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
