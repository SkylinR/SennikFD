#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CustomButton), true)]
public class CustomButtonEditor : ButtonEditor {
    SerializedProperty buttonTextProperty;

    SerializedProperty normalColorProperty;
    SerializedProperty highlightColorProperty;
    SerializedProperty pressedColorProperty;
    SerializedProperty selectedColorProperty;
    SerializedProperty disabledColorProperty;

    private void Awake() {
        
    }

    protected override void OnEnable() {
        base.OnEnable();
        buttonTextProperty = serializedObject.FindProperty("buttonText");

        normalColorProperty = serializedObject.FindProperty("normalColor");
        highlightColorProperty = serializedObject.FindProperty("highlightColor");
        pressedColorProperty = serializedObject.FindProperty("pressedColor");
        selectedColorProperty = serializedObject.FindProperty("selectedColor");
        disabledColorProperty = serializedObject.FindProperty("disabledColor");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(buttonTextProperty);
        EditorGUILayout.PropertyField(normalColorProperty);
        EditorGUILayout.PropertyField(highlightColorProperty);
        EditorGUILayout.PropertyField(pressedColorProperty);
        EditorGUILayout.PropertyField(disabledColorProperty);
        EditorGUILayout.PropertyField(selectedColorProperty);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif