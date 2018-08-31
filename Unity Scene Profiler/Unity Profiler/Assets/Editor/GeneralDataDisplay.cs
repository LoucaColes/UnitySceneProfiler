using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GeneralDataDisplay : object
{
    public static void DisplayObject(GameObject _object)
    {
        EditorGUILayout.Toggle("Active: ", _object.activeSelf);
        EditorGUILayout.LabelField("Tag: ", _object.tag);
        EditorGUILayout.LabelField("Layer", _object.layer.ToString());
        EditorGUILayout.Toggle("Static: ", _object.isStatic);
        EditorGUILayout.Vector3Field("Position: ", _object.transform.position);
        EditorGUILayout.Vector3Field("Rotation: ", _object.transform.eulerAngles);
        EditorGUILayout.Vector3Field("Scale: ", _object.transform.lossyScale);
        EditorGUILayout.Vector3Field("Local Position: ", _object.transform.localPosition);
        EditorGUILayout.Vector3Field("Local Rotation: ", _object.transform.localEulerAngles);
        EditorGUILayout.Vector3Field("Local Scale: ", _object.transform.localScale);
        if (_object.transform.parent != null)
        {
            EditorGUILayout.ObjectField("Parent: ", _object.transform.parent, typeof(Transform), true);
            EditorGUILayout.ObjectField("Root: ", _object.transform.root, typeof(Transform), true);
        }
        EditorGUILayout.Space();
    }
}