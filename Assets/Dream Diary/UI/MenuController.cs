using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class MenuController : MonoBehaviour {
    [SerializeField] CanvasGroup mainMenuCanvas;
    [SerializeField] float fadeAnimationDuration = 0.3f;
    [SerializeField] GameplayLevel gameplayLevel;
    [SerializeField] Camera mainCamera;
    [SerializeField] List<GameObject> interactableFirstElements;
    [SerializeField] InputSystemUIInputModule inputSystemUIInputModule;

    CanvasGroup _activeButtons;
    Sequence _menuSequence;
    Vector3 cameraPosition = new Vector3(12.7f,- 198f, 10f);
    Vector3 cameraRotation = new Vector3(6.9f, -126, 0f);

    private void Awake() {

    }

    private void OnEnable() {
        InputController.ActivateMap(InputController.PlayerInputMap.UI);

        HandleOnChangeMenuButtons(mainMenuCanvas);
        mainMenuCanvas.alpha = 0;
        mainMenuCanvas.DOFade(1, fadeAnimationDuration);
        mainCamera.transform.SetPositionAndRotation(cameraPosition, Quaternion.Euler(cameraRotation));
        CursorController.EnableCursor(true);

        InputController.PlayerInputActions.UI.Navigate.performed += HandleOnNavigate;
    }

    private void OnDisable() {
        if (_activeButtons != null) {
            _activeButtons.alpha = 0;
        }
        mainMenuCanvas.alpha = 0;

        InputController.PlayerInputActions.UI.Navigate.performed -= HandleOnNavigate;
    }

    public void HandleOnClickSoloGame() {
        _menuSequence?.Kill();
        this.gameObject.SetActive(false);
        gameplayLevel.SetupGameplay();
    }

    public void HandleOnChangeMenuButtons(CanvasGroup menuToShow) {
        _menuSequence?.Kill();
        EventSystem.current.SetSelectedGameObject(null);

        if (_activeButtons != null) {
            _menuSequence = DOTween.Sequence()
            .Append(_activeButtons.DOFade(0, fadeAnimationDuration)
            .OnComplete(() => {
                if (_activeButtons != null) {
                    _activeButtons.gameObject.SetActive(false);
                }

                _activeButtons = menuToShow;
                _activeButtons.gameObject.SetActive(true);
                _activeButtons.alpha = 0;
                _activeButtons.DOFade(1, fadeAnimationDuration);
            }));
        } else {
            _activeButtons = menuToShow;
            _activeButtons.gameObject.SetActive(true);
            _activeButtons.alpha = 0;

            _menuSequence = DOTween.Sequence()
            .Append(_activeButtons.DOFade(1, fadeAnimationDuration));
        }
    }

    void HandleOnNavigate(CallbackContext ctx) {
        if (EventSystem.current.currentSelectedGameObject == null) {
            inputSystemUIInputModule.enabled = false;
            var element = interactableFirstElements.FirstOrDefault(e => e.activeInHierarchy);
            if (element != null) {
                EventSystem.current.SetSelectedGameObject(element);
            }
            inputSystemUIInputModule.enabled = true;
        }
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
