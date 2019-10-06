using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamagable, IKillable {
  public int health;
  public int armor;

  public void GetArmor(int amount) {
    armor += amount;
  }

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
    if (!gameObject.CompareTag("Player")) {
      Component[] weapons = gameObject.GetComponentsInChildren(typeof(Weapon), true);
      foreach (Weapon weapon in weapons) {
        Debug.Log("unparenting on death: " + weapon);
        weapon.transform.parent = null;
      }

      Destroy(gameObject);
    }
  }
}
