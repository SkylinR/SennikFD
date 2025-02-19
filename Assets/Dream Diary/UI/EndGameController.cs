using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class EndGameController : MonoBehaviour
{
    public Action OnCloseEndGameUI;

    [SerializeField] GameplayLevel level;
    [SerializeField] MenuController menuController;
    [SerializeField] GameObject firstElementToSelect;
    [SerializeField] InputSystemUIInputModule inputSystemUIInputModule;

    void OnEnable() {
        CursorController.EnableCursor(true);

        EventSystem.current.SetSelectedGameObject(null);
        InputController.PlayerInputActions.UI.Navigate.performed += HandleOnNavigate;
    }

    private void OnDisable() {
        InputController.PlayerInputActions.UI.Navigate.performed -= HandleOnNavigate;
    }

    public void HandleOnTryAgain() {
        gameObject.SetActive(false);
        level.CleanUp();
        level.SetupGameplay();
        OnCloseEndGameUI?.Invoke();
    }

    public void HandleOnExit() {
        gameObject.SetActive(false);
        level.CleanUp();
        menuController.gameObject.SetActive(true);
    }

    void HandleOnNavigate(CallbackContext ctx) {
        inputSystemUIInputModule.enabled = false;
        EventSystem.current.SetSelectedGameObject(firstElementToSelect);
        inputSystemUIInputModule.enabled = true;
    }
}
