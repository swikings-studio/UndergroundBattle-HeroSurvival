using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using UnityEditor.Graphs;

public class GameMaker : EditorWindow
{
    private GroupType selectedGroup;

    [MenuItem("SwiKinGs Studio/Game Maker")]
    public static void ShowWindow()
    {
        GetWindow<GameMaker>("Game Maker");
    }
    void OnGUI()
    {
        GUILayout.Label("Select Group", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        var groups = Enum.GetValues(typeof(GroupType));
        foreach (GroupType group in groups)
        {
            string groupName = group.ToString();

            if (GUILayout.Button(groupName))
            {
                selectedGroup = group;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Selected Group - " + selectedGroup.ToString(), EditorStyles.boldLabel);
    }

    public enum GroupType
    {
        All,
        Weapons,
        Upgrades,
        Units
    }
}