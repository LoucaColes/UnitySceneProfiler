using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataParser
{
    public static GameObjectData ConvertGameObject(ref GameObject _object)
    {
        GameObjectData newGameObjectData = new GameObjectData();

        newGameObjectData.m_name = _object.name;
        newGameObjectData.m_tag = _object.tag;
        newGameObjectData.m_layer = _object.layer;
        newGameObjectData.m_position = _object.transform.position;
        newGameObjectData.m_rotation = _object.transform.eulerAngles;
        newGameObjectData.m_scale = _object.transform.localScale;
        if (_object.transform.parent != null)
        {
            newGameObjectData.m_parentObject = _object.transform.parent.name;
        }
        else
        {
            newGameObjectData.m_parentObject = "Null";
        }

        return newGameObjectData;
    }

    public static CameraObjectData ConvertCameraObject(ref Camera _camera)
    {
        CameraObjectData newCameraObjectData = new CameraObjectData();

        newCameraObjectData.m_name = _camera.name;
        newCameraObjectData.m_clearflag = _camera.clearFlags.ToString();
        newCameraObjectData.m_backgroundColour = _camera.backgroundColor;
        newCameraObjectData.m_cullingMask = _camera.cullingMask;
        newCameraObjectData.m_perspective = !_camera.orthographic;
        newCameraObjectData.m_fov = _camera.fieldOfView;
        newCameraObjectData.m_orthographic = _camera.orthographic;
        newCameraObjectData.m_orthographicSize = _camera.orthographicSize;
        newCameraObjectData.m_clippingPlaneNear = _camera.nearClipPlane;
        newCameraObjectData.m_clippingPlaneFar = _camera.farClipPlane;
        newCameraObjectData.m_viewportRect = _camera.rect;
        newCameraObjectData.m_depth = _camera.depth;
        newCameraObjectData.m_renderingPath = _camera.renderingPath.ToString();
        if (_camera.targetTexture != null)
        {
            newCameraObjectData.m_texture = _camera.targetTexture.name;
        }
        else
        {
            newCameraObjectData.m_texture = "Null";
        }
        newCameraObjectData.m_occlusionCulling = _camera.useOcclusionCulling;
        newCameraObjectData.m_hdr = _camera.allowHDR;
        newCameraObjectData.m_msaa = _camera.allowMSAA;
        newCameraObjectData.m_dynamicResolution = _camera.allowDynamicResolution;
        newCameraObjectData.m_targetDisplay = _camera.targetDisplay;

        return newCameraObjectData;
    }
}

[Serializable]
public class DataSnapshotConverted
{
    public List<GameObjectData> m_gameObjectData = new List<GameObjectData>();
    public List<CameraObjectData> m_cameraObjectData = new List<CameraObjectData>();
}

[Serializable]
public class GameObjectData
{
    public string m_name;
    public string m_tag;
    public int m_layer;
    public Vector3 m_position;
    public Vector3 m_rotation;
    public Vector3 m_scale;
    public string m_parentObject;
}

[Serializable]
public class CameraObjectData
{
    public string m_name;
    public string m_clearflag;
    public Color m_backgroundColour;
    public int m_cullingMask;
    public bool m_perspective;
    public float m_fov;
    public bool m_orthographic;
    public float m_orthographicSize;
    public float m_clippingPlaneNear;
    public float m_clippingPlaneFar;
    public Rect m_viewportRect;
    public float m_depth;
    public string m_renderingPath;
    public string m_texture;
    public bool m_occlusionCulling;
    public bool m_hdr;
    public bool m_msaa;
    public bool m_dynamicResolution;
    public int m_targetDisplay;
}

[Serializable]
public class MeshFilterObjectData
{
    public string m_name;
    public MeshObjectData m_sharedMeshData;
}

[Serializable]
public class MeshObjectData
{
    public Bounds m_bounds;
    public int m_vertexColourCount;
    public int m_normalsCount;
    public int m_tangentsCount;
    public int m_trianglesCount;
    public int m_verticesCount;
    public int m_subMeshCount;
}