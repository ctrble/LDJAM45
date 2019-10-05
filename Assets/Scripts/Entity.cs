﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamagable, IKillable {
  public int health;
  public int armor;

  public void Damage(int amount) {

    if (armor >= amount) {
      armor -= amount;
    }
    else if (armor > 0) {
      int difference = armor - amount;
      armor = 0;
      health -= Mathf.Abs(difference);
    }
    else {
      health -= amount;
    }

    if (health <= 0) {
      Kill();
    }
  }

  public void Kill() {
    Destroy(gameObject);
  }
}
