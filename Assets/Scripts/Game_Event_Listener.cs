using UnityEngine;
using UnityEngine.Events;

public class Game_Event_Listener : MonoBehaviour {
  [SerializeField]
  private Game_Event gameEvent;
  [SerializeField]
  private UnityEvent response;

  private void OnEnable() {
    gameEvent.RegisterListener(this);
  }

  private void OnDisable() {
    gameEvent.UnregisterListener(this);
  }

  public void OnEventRaised() {
    response.Invoke();
  }
}
