using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event", order = 52)]
public class Game_Event : ScriptableObject {
  private List<Game_Event_Listener> listeners = new List<Game_Event_Listener>();

  public void Raise() {
    for (int i = listeners.Count - 1; i >= 0; i--) {
      listeners[i].OnEventRaised();
    }
  }

  public void RegisterListener(Game_Event_Listener listener) {
    listeners.Add(listener);
  }

  public void UnregisterListener(Game_Event_Listener listener) {
    listeners.Remove(listener);
  }
}
