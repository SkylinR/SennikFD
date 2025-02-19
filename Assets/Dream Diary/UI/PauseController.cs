using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class PauseController : MonoBehaviour {
    public Action OnClosePauseUI;

    [SerializeField] GameplayLevel level;
    [SerializeField] MenuController menuController;
    [SerializeField] GameObject firstElementToSelect;
    [SerializeField] InputSystemUIInputModule inputSystemUIInputModule;

    void OnEnable() {
        CursorController.EnableCursor(true);

        EventSystem.current.SetSelectedGameObject(null);
        if(InputController.PlayerInputActions != null)
            InputController.PlayerInputActions.UI.Navigate.performed += HandleOnNavigate;
    }

    void OnDisable() {
        if (InputController.PlayerInputActions != null)
            InputController.PlayerInputActions.UI.Navigate.performed -= HandleOnNavigate;
    }

    public void HandleOnResume() {
        gameObject.SetActive(false);
        CursorController.EnableCursor(false);
        OnClosePauseUI?.Invoke();
    }

    public void HandleOnRestart() {
        gameObject.SetActive(false);
        level.CleanUp();
        level.SetupGameplay();
        OnClosePauseUI?.Invoke();
    }

    public void HandleOnExitLevel() {
        gameObject.SetActive(false);
        level.CleanUp();
        menuController.gameObject.SetActive(true);
    }

    void HandleOnNavigate(CallbackContext ctx) {
        if (EventSystem.current.currentSelectedGameObject == null) {
            inputSystemUIInputModule.enabled = false;
            EventSystem.current.SetSelectedGameObject(firstElementToSelect);
            inputSystemUIInputModule.enabled = true;
        }
    }
}
