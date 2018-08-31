using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataConfig", menuName = "Custom Profiler/Data Config", order = 1)]
public class DataConfig : ScriptableObject
{
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
public struct MeshDataLimit
{
}