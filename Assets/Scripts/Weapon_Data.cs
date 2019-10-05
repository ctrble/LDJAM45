using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon_Data", menuName = "Weapon Data", order = 51)]
public class Weapon_Data : ScriptableObject {
  [SerializeField]
  private string weaponName;
  [SerializeField]
  private Sprite icon;
  [SerializeField]
  private int attackDamage;
  [SerializeField]
  private int attackRange;
  [SerializeField]
  private float attackInterval;

  public string WeaponName {
    get {
      return weaponName;
    }
  }

  public Sprite Icon {
    get {
      return icon;
    }
  }

  public int AttackDamage {
    get {
      return attackDamage;
    }
  }

  public int AttackRange {
    get {
      return attackRange;
    }
  }

  public float AttackInterval {
    get {
      return attackInterval;
    }
  }
}
