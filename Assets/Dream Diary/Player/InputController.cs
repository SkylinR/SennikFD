using UnityEngine;

public class InputController : MonoBehaviour {

    private static InputController instance = null;
    public static InputController Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<InputController>();
                return instance;
            } else {
                return instance;
            }
        }
    }

    public enum PlayerInputMap {
        Null,
        Player,
        UI
    }

    public static PlayerInputMap ActiveMap { get; private set; }

    public static PlayerInputActions PlayerInputActions { get; private set; }

   public static void InitializeInput() {
        PlayerInputActions = new PlayerInputActions();
   }

    public static void ActivateMap(PlayerInputMap map) {

        if (PlayerInputActions == null) {
            Debug.LogError("Cannot activate map because PlayerInputActions is null");
            return;
        }

        if (ActiveMap == map) return;

        switch (map) {
            case PlayerInputMap.UI:
                PlayerInputActions.Player.Disable();
                PlayerInputActions.UI.Enable();
                break;
            case PlayerInputMap.Player:
                PlayerInputActions.Player.Enable();
                PlayerInputActions.UI.Disable();
                break;
            default:
                PlayerInputActions.Player.Disable();
                PlayerInputActions.UI.Disable();
                break;
        }

        ActiveMap = map;
    }

    [ContextMenu("Current active map")]
    public void GetActiveMap() {
        Debug.Log($"Active input map: {ActiveMap}");
    }
}
