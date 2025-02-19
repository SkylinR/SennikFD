using DG.Tweening;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class CommandToolSystem : MonoBehaviour {
    [SerializeField] GameObject commandToolCanvas;
    [SerializeField] TextMeshProUGUI commandLineText;
    [SerializeField] ScrollRect logScrollRect;
    [SerializeField] TMP_InputField input;
    [Header("External elements")]
    [SerializeField] PlayerConfig playerConfig;
    [SerializeField] GameplayLevel gameplayLevel;
    [SerializeField] LevelGenerator levelGenerator;

    string _logText;

#if CHEATS
    private void OnEnable() {
        if(InputController.PlayerInputActions != null) {
            InputController.PlayerInputActions.Player.CommandTool.performed += HandleCommandTool;
            InputController.PlayerInputActions.UI.CommandTool.performed += HandleCommandTool;
            InputController.PlayerInputActions.UI.Cancel.performed += HandleCommandToolExit;
        }
    }

    private void OnDisable() {
        if (InputController.PlayerInputActions != null) {
            InputController.PlayerInputActions.Player.CommandTool.performed -= HandleCommandTool;
            InputController.PlayerInputActions.UI.CommandTool.performed -= HandleCommandTool;
            InputController.PlayerInputActions.UI.Cancel.performed -= HandleCommandToolExit;
        }
    }
    //void Update() {
    //    if (Input.GetKeyDown(KeyCode.BackQuote)) {
    //        commandToolCanvas.SetActive(!commandToolCanvas.activeSelf);
    //        if (gameplayLevel.spawnedPlayer != null) {
    //            gameplayLevel.spawnedPlayer.SetActivePlayerInput(!commandToolCanvas.activeSelf);
    //            CursorController.EnableCursor(commandToolCanvas.activeSelf);
    //        }
    //    }
    //}

    public void HandleOnEditEnd(string text) {
        SubmitText(text);
    }

    public void CloseTool()
    {
        commandToolCanvas.SetActive(false);
        if (gameplayLevel.spawnedPlayer != null) {
            InputController.ActivateMap(InputController.PlayerInputMap.Player);
            CursorController.EnableCursor(false);
        }
    }

    void SubmitText(string text) {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            CheckCommad(text);
            KeepFocus();
        }
    }

    void KeepFocus() {
        EventSystem.current.SetSelectedGameObject(input.gameObject);
        input.ActivateInputField();
    }

    void CheckCommad(string command) {

        if (!string.IsNullOrEmpty(_logText)) {
            _logText += "\n";
        }

        string commandNameStartStyle = $"<b><color=#0000FF>";
        string commandNameEndStyle = $"</b></color>";

        string parsedCommand;

        switch (command.ToLower()) {
            case "help": {
                    parsedCommand = $"List of commands:\n" +
                   $"{commandNameStartStyle}SET PLAYER SPEED = value{commandNameEndStyle} - Set player movement speed (takes float value which cannot be negative)\n" +
                   $"{commandNameStartStyle}GET PLAYER SPEED{commandNameEndStyle} - Get player movement speed.\n" +
                   $"{commandNameStartStyle}SET CAMERA SENSIVITY = value{commandNameEndStyle} - Set camera sensivity (value in range 0.01 - 1)\n" +
                   "{commandNameStartStyle}GET CAMERA SENSIVITY{commandNameEndStyle} - Get camera sensivity\n" +
                   $"{commandNameStartStyle}GET MAZE SEED{commandNameEndStyle} - Log currently generated maze seed\n" +
                   $"{commandNameStartStyle}SAVE LOG{commandNameEndStyle} - Save log to file\n" +
                   $"{commandNameStartStyle}CLEAR{commandNameEndStyle} - Clear console text";
                    break;
                }

            case string s when s.Contains("set player speed = "): {
                    var index = command.IndexOf("=");
                    var valueString = command[(index + 1)..];

                    if (float.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) && value > 0) {
                        playerConfig.SetMovementSpeed(value);
                        parsedCommand = $"SET PLAYER SPEED = {value}";
                    } else {
                        parsedCommand = $"Cannot parse value in command: {command}";
                    }

                    break;
                }

            case "get player speed": {
                    parsedCommand = $"PLAYER SPEED = {playerConfig.GetMovementSpeed()}";
                    break;
                }

            case string s when s.Contains("set camera sensivity = "): {
                    var index = command.IndexOf("=");
                    var valueString = command[(index + 1)..];
                    Debug.Log($"string: {valueString}");

                    if (float.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)) {
                        PlayerOptions.SetCameraSensivity(value);
                        parsedCommand = $"SET CAMERA SENSIVITY = {value}";
                    } else {
                        parsedCommand = $"Cannot parse value in command: {command}";
                    }

                    break;
                }

            case "get camera sensivity": {
                    parsedCommand = $"CAMERA SENSIVITY = {PlayerOptions.CameraSensivity}";
                    break;
                }

            case "get maze seed": {
                    var seed = levelGenerator.GetGeneratedMazeSeed();
                    if(seed < 1) {
                        parsedCommand = $"Cannot get maze seed because there is no maze generated";
                    } else {
                        GUIUtility.systemCopyBuffer = seed.ToString();
                        parsedCommand = $"MAZE SEED: {seed} (coppied to clipboard)";
                    }
                    
                    break;
                }

            case "save log": {
                    parsedCommand = command;
                    SaveTextToFile(_logText);
                    break;
                }

            case "clear": {
                    _logText = string.Empty;
                    parsedCommand = string.Empty;
                    break;
                }

            default: {
                    parsedCommand = $"Unknown command: {command}";
                    break;
                }

        }

        _logText += $"{parsedCommand}";

        commandLineText.SetText(_logText);
        var prefferedHeight = commandLineText.preferredHeight;
        logScrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, prefferedHeight);
        logScrollRect.DOKill();
        logScrollRect.DOVerticalNormalizedPos(0, 0.2f);
    }

    void SaveTextToFile(string content) {
        string path = Path.Combine(Application.persistentDataPath, "COMMAND TOOL LOG.txt");
        File.WriteAllText(path, content);
        Debug.Log("File saved to: " + path);
    }

    void HandleCommandTool(CallbackContext ctx) {
        commandToolCanvas.SetActive(!commandToolCanvas.activeSelf);
        if (gameplayLevel.spawnedPlayer != null) {
            if(commandToolCanvas.activeSelf) {
                InputController.ActivateMap(InputController.PlayerInputMap.UI);
            } else {
                InputController.ActivateMap(InputController.PlayerInputMap.Player);
            }
            CursorController.EnableCursor(commandToolCanvas.activeSelf);
        }
    }

    void HandleCommandToolExit(CallbackContext ctx) {
        if(commandToolCanvas.activeSelf) {
            CloseTool();
        }
    }

#else
    void Awake() {
        Destroy(this.gameObject);
    }
#endif
}
