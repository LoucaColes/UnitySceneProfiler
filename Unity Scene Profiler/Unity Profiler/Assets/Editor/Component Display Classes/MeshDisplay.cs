using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MeshDisplay : object
{
    public static void DisplayMeshFilterData(MeshFilter _meshFilter, MeshDataLimit _dataLimit)
    {
        EditorGUILayout.LabelField("Mesh Filter Data");
        EditorGUILayout.LabelField("Shared Mesh Data");
        DisplayMeshData(_meshFilter.sharedMesh, _dataLimit);
        EditorGUILayout.Space();
    }

    public static void DisplayMeshData(Mesh _mesh, MeshDataLimit _dataLimit)
    {
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.ObjectField(_mesh.name,
                _mesh,
                typeof(Mesh), true);
            EditorGUILayout.BoundsField("Bounds: ", _mesh.bounds);

            DisplayVertexColourCount(_mesh, _dataLimit);

            DisplayNormalsCount(_mesh, _dataLimit);

            DisplayTangentsCount(_mesh, _dataLimit);

            DisplayTrianglesCount(_mesh, _dataLimit);

            DisplayVerticesCount(_mesh, _dataLimit);

            DisplaySubMeshesCount(_mesh, _dataLimit);
        }
    }

    private static void DisplayVertexColourCount(Mesh _mesh, MeshDataLimit _dataLimit)
    {
        EditorGUILayout.IntField("Vertex Colours Count: ",
                        _mesh.colors.Length);

        if (_dataLimit != null && _mesh.colors.Length >= _dataLimit.vertexColourConfig.warningLimit && _mesh.colors.Length < _dataLimit.vertexColourConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.vertexColourConfig.warningMessage, MessageType.Warning);
        }
        else if (_dataLimit != null && _mesh.colors.Length >= _dataLimit.vertexColourConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.vertexColourConfig.errorMessage, MessageType.Error);
        }
    }

    private static void DisplayNormalsCount(Mesh _mesh, MeshDataLimit _dataLimit)
    {
        EditorGUILayout.IntField("Normals Count: ", _mesh.normals.Length);

        if (_dataLimit != null && _mesh.normals.Length >= _dataLimit.normalsConfig.warningLimit && _mesh.normals.Length < _dataLimit.normalsConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.normalsConfig.warningMessage, MessageType.Warning);
        }
        else if (_dataLimit != null && _mesh.normals.Length >= _dataLimit.normalsConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.normalsConfig.errorMessage, MessageType.Error);
        }
    }

    private static void DisplayTangentsCount(Mesh _mesh, MeshDataLimit _dataLimit)
    {
        EditorGUILayout.IntField("Tangents Count: ", _mesh.tangents.Length);

        if (_dataLimit != null && _mesh.tangents.Length >= _dataLimit.tangentsConfig.warningLimit && _mesh.tangents.Length < _dataLimit.tangentsConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.tangentsConfig.warningMessage, MessageType.Warning);
        }
        else if (_dataLimit != null && _mesh.tangents.Length >= _dataLimit.tangentsConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.tangentsConfig.errorMessage, MessageType.Error);
        }
    }

    private static void DisplayTrianglesCount(Mesh _mesh, MeshDataLimit _dataLimit)
    {
        EditorGUILayout.IntField("Triangles Count: ", _mesh.triangles.Length);

        if (_dataLimit != null && _mesh.triangles.Length >= _dataLimit.trianglesConfig.warningLimit && _mesh.triangles.Length < _dataLimit.trianglesConfig.warningLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.trianglesConfig.warningMessage, MessageType.Warning);
        }
        else if (_dataLimit != null && _mesh.triangles.Length >= _dataLimit.trianglesConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.trianglesConfig.errorMessage, MessageType.Error);
        }
    }

    private static void DisplayVerticesCount(Mesh _mesh, MeshDataLimit _dataLimit)
    {
        EditorGUILayout.IntField("Vertices Count: ", _mesh.vertices.Length);

        if (_dataLimit != null && _mesh.vertices.Length >= _dataLimit.verticesConfig.warningLimit && _mesh.vertices.Length < _dataLimit.verticesConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.verticesConfig.warningMessage, MessageType.Warning);
        }
        else if (_dataLimit != null && _mesh.vertices.Length >= _dataLimit.verticesConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.verticesConfig.errorMessage, MessageType.Error);
        }
    }

    private static void DisplaySubMeshesCount(Mesh _mesh, MeshDataLimit _dataLimit)
    {
        EditorGUILayout.IntField("Sub Mesh Count: ", _mesh.subMeshCount);

        if (_dataLimit != null && _mesh.subMeshCount >= _dataLimit.subMeshConfig.warningLimit && _mesh.subMeshCount < _dataLimit.subMeshConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.subMeshConfig.warningMessage, MessageType.Warning);
        }
        else if (_dataLimit != null && _mesh.subMeshCount >= _dataLimit.subMeshConfig.errorLimit)
        {
            EditorGUILayout.HelpBox(_dataLimit.subMeshConfig.errorMessage, MessageType.Error);
        }
    }
}