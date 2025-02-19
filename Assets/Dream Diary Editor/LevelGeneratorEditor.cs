#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator), true)]
public class LevelGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Editor generator section", EditorStyles.boldLabel);

        LevelGenerator generator = (LevelGenerator)target;
        if(generator.Seed > 0) {
            EditorGUILayout.LabelField("Used seed: ", generator.Seed.ToString());
        } else {
            EditorGUILayout.LabelField("Used seed: ", "-");
        }

        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Clear")) {

            generator.Clear();
        }

        if (GUILayout.Button("Generate")) {
            generator.Generate();
        }

        GUILayout.EndHorizontal();
    }
}

#endif