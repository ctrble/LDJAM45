using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Canvas : MonoBehaviour {
  private GameObject player;
  private Entity playerEntity;
  public Text health;
  public Text armor;

  void Start() {
    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player");
    }

    if (playerEntity == null) {
      playerEntity = player.GetComponent<Entity>();
    }
  }

  void Update() {
    health.text = "Health: " + playerEntity.health.ToString();
    armor.text = "Armor: " + playerEntity.armor.ToString();
  }
}
