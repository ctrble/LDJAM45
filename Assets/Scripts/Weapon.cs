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

  public GameObject shotEffect;
  public int amountToPool;
  public List<GameObject> pooledShotEffects;


  void OnEnable() {
    // player only weapon stuff
    heldByPlayer = transform.root.CompareTag("Player");

    if (heldByPlayer) {
      SelectWeapon();

      if (itemCanvas == null) {
        itemCanvas = GameObject.FindGameObjectWithTag("Item Canvas").GetComponent<Item_Canvas>();
      }
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

    PoolShotEffects();
  }

  void PoolShotEffects() {
    if (shotEffect != null) {
      pooledShotEffects = new List<GameObject>();
      for (int i = 0; i < amountToPool; i++) {
        GameObject obj = Instantiate(shotEffect, transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.SetActive(false);
        pooledShotEffects.Add(obj);
      }
    }
  }

  public GameObject GetPooledObject() {
    for (int i = 0; i < pooledShotEffects.Count; i++) {
      if (!pooledShotEffects[i].activeInHierarchy) {
        return pooledShotEffects[i];
      }
    }
    return null;
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

  void StartShotEffect() {
    GameObject effect = GetPooledObject();
    if (effect != null) {
      effect.SetActive(true);

      ParticleSystem particles = effect.GetComponent<ParticleSystem>();
      particles.Clear();
      particles.Play();
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
      if (!weaponData.InfiniteAmmo) {
        StartShotEffect();
      }
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
