using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class GameMaker : EditorWindow
{

    [MenuItem("SwiKinGs Studio/Game Maker")]
    public static void ShowWindow()
    {
        GetWindow<GameMaker>("Game Maker");
    }

    public enum GroupType
    {
        All,
        Weapons,
        Upgrades,
        Units
    }
}