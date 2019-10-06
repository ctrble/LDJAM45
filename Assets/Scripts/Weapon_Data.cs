using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon_Data", menuName = "Weapon Data", order = 51)]
public class Weapon_Data : Item {

  [SerializeField]
  private int attackDamage = 1;
  [SerializeField]
  private int attackRange = 50;
  [SerializeField]
  private float attackInterval = 0.1f;
  [SerializeField]
  private bool infiniteAmmo = false;

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

  public bool InfiniteAmmo {
    get {
      return infiniteAmmo;
    }
  }
}
