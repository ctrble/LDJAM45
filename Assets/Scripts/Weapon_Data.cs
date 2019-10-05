using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon_Data", menuName = "Weapon Data", order = 51)]
public class Weapon_Data : Item {

  [SerializeField]
  private int attackDamage;
  [SerializeField]
  private int attackRange;
  [SerializeField]
  private float attackInterval;
  [SerializeField]
  private bool infiniteAmmo;
  [SerializeField]
  private int maxAmmo;
  [SerializeField]
  private int remainingAmmo;

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

  public float MaxAmmo {
    get {
      return maxAmmo;
    }
  }

  public float RemainingAmmo {
    get {
      return remainingAmmo;
    }
  }
}
