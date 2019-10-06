using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
  // suppress warnings about default values
#pragma warning disable CS0649

  [SerializeField]
  private Weapon_Data weaponData;

#pragma warning disable CS0649

  [SerializeField]
  private Game_Event OnWeaponSelected;
  public Item_Canvas itemCanvas;
  public LayerMask hitLayer;
  private Camera mainCamera;
  public Transform crosshairs;
  [SerializeField]
  private SpriteRenderer spriteRenderer;
  [SerializeField]
  private float timeRemaining;
  [SerializeField]
  private bool canAttack;
  public int remainingAmmo;

  public bool heldByPlayer = false;


  void OnEnable() {
    // player only weapon stuff
    heldByPlayer = transform.root.CompareTag("Player");

    if (heldByPlayer) {
      SelectWeapon();

      if (itemCanvas == null) {
        itemCanvas = GameObject.FindGameObjectWithTag("Item Canvas").GetComponent<Item_Canvas>();
      }

      // if (mainCamera == null) {
      //   mainCamera = Camera.main;
      // }

      // if (crosshairs == null) {
      //   crosshairs = GameObject.FindGameObjectWithTag("Crosshairs").transform;
      // }

    }

    if (spriteRenderer == null) {
      spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    ChangeAmmo(0);
    timeRemaining = weaponData.AttackInterval;
    canAttack = true;

    if (spriteRenderer != null) {
      spriteRenderer.sprite = weaponData.ItemIcon;
    }
  }

  void Update() {
    AttackTimer();
    DestroyIfEmpty();
  }

  void AttackTimer() {
    if (!canAttack) {
      timeRemaining -= Time.deltaTime;
      if (timeRemaining <= 0f) {
        canAttack = true;
        timeRemaining = weaponData.AttackInterval;
      }
    }
  }

  void DestroyIfEmpty() {
    if (transform.parent == null && remainingAmmo <= 0) {
      Destroy(gameObject);
    }
  }

  void WeaponEffect(Vector3 position, Vector3 direction) {
    switch (weaponData.ItemName) {
      case "Fists":
        HitScanBullet(position, direction, true);
        break;
      case "Pistol":
        HitScanBullet(position, direction, false);
        break;
      default:
        break;
    }
  }

  void DisarmEnemy(GameObject enemy) {
    Weapon weapon = enemy.GetComponentInChildren<Weapon>();
    if (weapon != null && weapon.CompareTag("Item")) {
      weapon.DropWeapon(weapon.transform);
    }
  }

  void HitScanBullet(Vector3 position, Vector3 direction, bool disarm) {
    int ammoCost = 1;
    RaycastHit hit;
    Vector3 attackPosition = position;
    Vector3 attackDirection = direction;

    if (remainingAmmo >= ammoCost || weaponData.InfiniteAmmo) {
      ChangeAmmo(-ammoCost);

      bool hitCast = Physics.Raycast(attackPosition, attackDirection, out hit, weaponData.AttackRange, hitLayer);
      if (hitCast) {
        Debug.DrawRay(attackPosition, attackDirection * weaponData.AttackRange, Color.red);

        Entity entity = hit.transform.GetComponent<Entity>();
        if (entity != null) {
          entity.Damage(weaponData.AttackDamage);

          // should they be disarmed?
          if (disarm) {
            DisarmEnemy(hit.transform.gameObject);
          }
        }
      }
      else {
        Debug.DrawRay(attackPosition, attackDirection * weaponData.AttackRange, Color.black);
      }
    }
  }

  public void SelectWeapon() {
    OnWeaponSelected.Raise();
  }

  public void UseWeapon(Vector3 position, Vector3 direction) {
    if (canAttack) {
      canAttack = false;
      WeaponEffect(position, direction);
    }
  }

  public void ChangeAmmo(int amount) {
    if (!weaponData.InfiniteAmmo) {
      remainingAmmo += amount;

      // only update UI if it's currently equipped and if it's the player
      if (gameObject.activeInHierarchy && heldByPlayer) {
        itemCanvas.UpdateRemainingAmmo(remainingAmmo);
      }
    }
  }

  public void DropWeapon(Transform item) {
    item.parent = null;
  }
}
