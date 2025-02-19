using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button {
    public TextMeshProUGUI buttonText;
    [SerializeField] Color normalColor;
    [SerializeField] Color highlightColor;
    [SerializeField] Color pressedColor;
    [SerializeField] Color selectedColor;
    [SerializeField] Color disabledColor;

    AudioSource _audioSource;

    protected override void Awake() {
        base.Awake();
        TryGetComponent<AudioSource>(out _audioSource);
    }

    protected override void DoStateTransition(SelectionState state, bool instant) {
        base.DoStateTransition(state, instant);

        if (state == SelectionState.Highlighted) {
            buttonText.color = highlightColor;
        } else if (state == SelectionState.Pressed) {
            buttonText.color = pressedColor;
        } else if (state == SelectionState.Selected) {
            buttonText.color = selectedColor;
        } else if (state == SelectionState.Disabled) {
            buttonText.color = disabledColor;
        } else {
            buttonText.color = normalColor;
        }
    }

    public void PlayAudio() {
        if (_audioSource != null) {
            _audioSource.Play();
        }
    }
}
