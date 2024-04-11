using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpgradeParametrs))]
public class UpgradeCustorEditor : Editor
{
    UpgradeParametrs parametrs;
    public override void OnInspectorGUI()
    {
        parametrs = (UpgradeParametrs)target;

        EditorGUILayout.LabelField(parametrs.name, EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Icon");
        parametrs.Icon = EditorGUILayout.ObjectField(parametrs.Icon, typeof(Sprite), true,
                            GUILayout.Height(48), GUILayout.Width(48)) as Sprite;

        EditorGUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
