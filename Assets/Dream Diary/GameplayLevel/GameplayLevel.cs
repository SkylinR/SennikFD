using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GameplayLevel : MonoBehaviour {

    
    public PlayerController spawnedPlayer { get; private set; }

    [SerializeField] LevelGenerator levelGenerator;

    [SerializeField] PlayerController playerPrefab;
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] PauseController pauseMenu;
    [SerializeField] EndGameController endGameMenu;
    [SerializeField] LevelDecorator levelDecorator;
    [SerializeField] GameObject commandToolCanvas;

    [Space(5)]
    [SerializeField] StepTracker stepTracker;
    [SerializeField] AdsSystem adsSystem;

    private void OnEnable() {

        if (InputController.PlayerInputActions != null) {
            InputController.PlayerInputActions.Player.Pause.performed += HandleOnEnterPause;
            InputController.PlayerInputActions.UI.Cancel.performed += HandleOnExitPause;
        }

        pauseMenu.OnClosePauseUI += HandleOnCloseUI;
        levelDecorator.OnEndGameReached += HandleEndGame;
    }

    private void OnDisable() {

        if (InputController.PlayerInputActions != null) {
            InputController.PlayerInputActions.Player.Pause.performed -= HandleOnEnterPause;
            InputController.PlayerInputActions.UI.Cancel.performed -= HandleOnExitPause;
        }

        pauseMenu.OnClosePauseUI -= HandleOnCloseUI;
        levelDecorator.OnEndGameReached -= HandleEndGame;
    }

    public void SetupGameplay() {
        CursorController.EnableCursor(false);

        if (levelGenerator != null) {
            levelGenerator.Generate();
        }

        spawnedPlayer = InstantiatePlayer();
        cameraFollow.SetTarget(spawnedPlayer.transform);

        stepTracker.Setup(spawnedPlayer.transform);
        adsSystem.Setup();
        InputController.ActivateMap(InputController.PlayerInputMap.Player);
    }

    public void CleanUp() {
        levelGenerator.Clear();
        Destroy(spawnedPlayer.gameObject);
        spawnedPlayer = null;
        pauseMenu.gameObject.SetActive(false);
        adsSystem.DisableAd();
    }

    PlayerController InstantiatePlayer()
            => Instantiate(playerPrefab, GetRandomPosition(), rotation: Quaternion.identity, transform);

    Vector3 GetRandomPosition() {

        var randomCellPosition = levelGenerator.GetRandomCellCenter();
        randomCellPosition.y = 0;
        return randomCellPosition;
    }

    void HandleOnCloseUI() {
        if (spawnedPlayer != null) {
            InputController.ActivateMap(InputController.PlayerInputMap.Player);
        }
    }

    void HandleEndGame() {
        endGameMenu.gameObject.SetActive(true);
    }

    void HandleOnEnterPause(CallbackContext ctx) {

        if(commandToolCanvas.activeSelf) {
            return;
        }

        if (levelGenerator.IsLevelGenerated && spawnedPlayer != null) {
            pauseMenu.gameObject.SetActive(true);
            InputController.ActivateMap(InputController.PlayerInputMap.UI);
        }
    }

    void HandleOnExitPause(CallbackContext ctx) {

        if (pauseMenu.gameObject.activeSelf && levelGenerator.IsLevelGenerated && spawnedPlayer != null) {
            pauseMenu.gameObject.SetActive(false);
            InputController.ActivateMap(InputController.PlayerInputMap.Player);
        }
    }
}
