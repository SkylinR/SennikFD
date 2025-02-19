using UnityEngine;

public class Bootstrap : MonoBehaviour {

    static Bootstrap instance = null;
    public static Bootstrap Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Bootstrap>();
                return instance;
            } else {
                return instance;
            }
        }
    }

    [SerializeField] GameObject firstToActivate;

    void Awake() {
        InputController.InitializeInput();
        InitializeGame();
    }

    private void OnApplicationQuit() {

        if (InputController.PlayerInputActions != null) {
            InputController.ActivateMap(InputController.PlayerInputMap.Null);
        }
    }

    void InitializeGame() {
        if (firstToActivate != null) {
            firstToActivate.gameObject.SetActive(true);
        } else {
            Debug.LogError("Cannot run game because there is no first object to activate set.");
        }
    }
}