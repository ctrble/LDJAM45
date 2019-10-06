﻿using System.Collections;
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

  public int MaxAmmo {
    get {
      return maxAmmo;
    }
  }
}
