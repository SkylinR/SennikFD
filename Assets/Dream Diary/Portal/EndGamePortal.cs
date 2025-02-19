using System;
using UnityEngine;

public class EndGamePortal : MonoBehaviour {

    public Action OnEndGameReached;

    void OnDestroy() {
        OnEndGameReached = null;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {

            InputController.ActivateMap(InputController.PlayerInputMap.UI);
            OnEndGameReached?.Invoke();
        }
    }
}
