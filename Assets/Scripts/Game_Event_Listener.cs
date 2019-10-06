using UnityEngine;
using UnityEngine.Events;

public class Game_Event_Listener : MonoBehaviour {
  // suppress warnings about default values
#pragma warning disable CS0649

  [SerializeField]
  private Game_Event gameEvent;
  [SerializeField]
  private UnityEvent response;

#pragma warning restore CS0649

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
