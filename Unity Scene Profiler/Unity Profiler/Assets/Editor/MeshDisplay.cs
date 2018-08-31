using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MeshDisplay : object
{
    public static void DisplayMeshFilterData(MeshFilter _meshFilter)
    {
        EditorGUILayout.LabelField("Mesh Filter Data");
        EditorGUILayout.LabelField("Shared Mesh Data");
        DisplayMeshData(_meshFilter.sharedMesh);
        EditorGUILayout.Space();
    }

    public static void DisplayMeshData(Mesh _mesh)
    {
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.ObjectField(_mesh.name,
                _mesh,
                typeof(Mesh), true);
            EditorGUILayout.BoundsField("Bounds: ", _mesh.bounds);
            EditorGUILayout.IntField("Vertex Colours Count: ",
                _mesh.colors.Length);
            EditorGUILayout.IntField("Normals Count: ", _mesh.normals.Length);
            EditorGUILayout.IntField("Tangents Count: ", _mesh.tangents.Length);
            EditorGUILayout.IntField("Triangles Count: ", _mesh.triangles.Length);
            EditorGUILayout.IntField("Vertices Count: ", _mesh.vertices.Length);
            EditorGUILayout.IntField("Sub Mesh Count: ", _mesh.subMeshCount);
        }
    }
}