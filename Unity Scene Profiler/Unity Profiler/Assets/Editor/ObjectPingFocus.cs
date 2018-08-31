using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ObjectPingFocus : object
{
    public static void PingAndFocus(GameObject _object)
    {
        EditorGUIUtility.PingObject(_object);

        Selection.activeGameObject = _object;
        SceneView.lastActiveSceneView.FrameSelected();
        Selection.activeGameObject = _object;
    }

    public static void DisplayPingButton(GameObject _object)
    {
        if (GUILayout.Button("Ping"))
        {
            PingAndFocus(_object);
        }
    }
}