using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataConfig", menuName = "Custom Profiler/Data Config", order = 1)]
public class DataConfig : ScriptableObject
{
    public MeshDataLimit m_meshDataLimit;
    public CameraDataLimit m_cameraDataLimit;
}

[Serializable]
public struct GeneralDataLimit
{
}

[Serializable]
public struct CameraDataLimit
{
    public bool m_hdrAllowed;
    public string m_errorMessageHDR;
    public bool m_msaaAllowed;
    public string m_errorMessageMsaa;
}

[Serializable]
public class MeshDataLimit
{
    [Header("Vertex Colour Count")]
    public ConfigTypes.IntConfig vertexColourConfig;

    [Space()]
    [Header("Normals Count")]
    public ConfigTypes.IntConfig normalsConfig;

    [Space()]
    [Header("Tangents Count")]
    public ConfigTypes.IntConfig tangentsConfig;

    [Space()]
    [Header("Triangles Count")]
    public ConfigTypes.IntConfig trianglesConfig;

    [Space()]
    [Header("Vertices Count")]
    public ConfigTypes.IntConfig verticesConfig;

    [Space()]
    [Header("Sub Mesh Count")]
    public ConfigTypes.IntConfig subMeshConfig;
}