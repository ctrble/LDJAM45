using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {

  public GameObject winGame;
  public GameObject loseGame;
  public GameObject player;
  public bool wonGame = false;
  public Vector3 playerPosition;

  void Start() {
    Cursor.visible = false;

    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player");
    }

    wonGame = false;
  }

  void Update() {
    if (wonGame) {
      player.transform.position = playerPosition;
    }
    else if (!player.activeInHierarchy) {
      loseGame.SetActive(true);
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")) {
      wonGame = true;
      winGame.SetActive(true);

      // turn stuff off
      playerPosition = player.transform.position;
      foreach (Transform child in player.transform) {
        child.gameObject.SetActive(false);
      }
    }
  }
}
