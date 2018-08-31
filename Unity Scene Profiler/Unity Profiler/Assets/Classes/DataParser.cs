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

    public static MeshFilterObjectData ConvertMeshFilterObject(ref MeshFilter _meshFilter)
    {
        MeshFilterObjectData newMeshFilterObjectData = new MeshFilterObjectData();

        newMeshFilterObjectData.m_name = _meshFilter.name;
        newMeshFilterObjectData.m_sharedMeshData = new MeshObjectData();
        newMeshFilterObjectData.m_sharedMeshData.m_bounds = _meshFilter.sharedMesh.bounds;
        newMeshFilterObjectData.m_sharedMeshData.m_vertexColourCount = _meshFilter.sharedMesh.colors.Length;
        newMeshFilterObjectData.m_sharedMeshData.m_normalsCount = _meshFilter.sharedMesh.normals.Length;
        newMeshFilterObjectData.m_sharedMeshData.m_tangentsCount = _meshFilter.sharedMesh.tangents.Length;
        newMeshFilterObjectData.m_sharedMeshData.m_trianglesCount = _meshFilter.sharedMesh.triangles.Length;
        newMeshFilterObjectData.m_sharedMeshData.m_verticesCount = _meshFilter.sharedMesh.vertices.Length;
        newMeshFilterObjectData.m_sharedMeshData.m_subMeshCount = _meshFilter.sharedMesh.subMeshCount;

        return newMeshFilterObjectData;
    }

    public static MeshRendererObjectData ConvertMeshRendererObject(ref MeshRenderer _meshRenderer)
    {
        MeshRendererObjectData newMeshRendererObjectData = new MeshRendererObjectData();

        newMeshRendererObjectData.m_name = _meshRenderer.name;

        newMeshRendererObjectData.m_lightingObjectData = new LightingObjectData();
        newMeshRendererObjectData.m_lightingObjectData.m_lightProbe = _meshRenderer.lightProbeUsage.ToString();
        newMeshRendererObjectData.m_lightingObjectData.m_reflectionProbe = _meshRenderer.reflectionProbeUsage.ToString();
        if (_meshRenderer.probeAnchor != null)
        {
            newMeshRendererObjectData.m_lightingObjectData.m_anchorOverride = _meshRenderer.probeAnchor.name;
        }
        else
        {
            newMeshRendererObjectData.m_lightingObjectData.m_anchorOverride = "Null";
        }
        newMeshRendererObjectData.m_lightingObjectData.m_castShadow = _meshRenderer.shadowCastingMode.ToString();
        newMeshRendererObjectData.m_lightingObjectData.m_recieveShadow = _meshRenderer.receiveShadows;
        newMeshRendererObjectData.m_lightingObjectData.m_motionVectors = _meshRenderer.motionVectorGenerationMode.ToString();

        newMeshRendererObjectData.m_materialCount = _meshRenderer.sharedMaterials.Length;

        newMeshRendererObjectData.m_materialsObjectData = new List<MaterialObjectData>();
        for (int index = 0; index < _meshRenderer.sharedMaterials.Length; index++)
        {
            MaterialObjectData newMaterialObjectData = new MaterialObjectData();
            newMaterialObjectData.m_name = _meshRenderer.sharedMaterials[index].name;
            newMaterialObjectData.m_colour = _meshRenderer.sharedMaterials[index].color;
            newMaterialObjectData.m_doubleSidedGI = _meshRenderer.sharedMaterials[index].doubleSidedGI;
            newMaterialObjectData.m_enableInstancing = _meshRenderer.sharedMaterials[index].enableInstancing;
            newMaterialObjectData.m_globalIlluminationFlag =
                _meshRenderer.sharedMaterials[index].globalIlluminationFlags.ToString();
            if (_meshRenderer.sharedMaterials[index].mainTexture != null)
            {
                newMaterialObjectData.m_mainTexture = _meshRenderer.sharedMaterials[index].mainTexture.name;
            }
            else
            {
                newMaterialObjectData.m_mainTexture = "Null";
            }
            newMaterialObjectData.m_mainTextureOffset = _meshRenderer.sharedMaterials[index].mainTextureOffset;
            newMaterialObjectData.m_mainTextureScale = _meshRenderer.sharedMaterials[index].mainTextureScale;
            newMaterialObjectData.m_passCount = _meshRenderer.sharedMaterials[index].passCount;
            newMaterialObjectData.m_renderQueue = _meshRenderer.sharedMaterials[index].renderQueue;
            newMaterialObjectData.m_shader = _meshRenderer.sharedMaterials[index].shader.name;
            newMeshRendererObjectData.m_materialsObjectData.Add(newMaterialObjectData);
        }

        newMeshRendererObjectData.m_dynamicOcclusion = _meshRenderer.allowOcclusionWhenDynamic;

        return newMeshRendererObjectData;
    }
}

[Serializable]
public class DataSnapshotConverted
{
    public List<GameObjectData> m_gameObjectData = new List<GameObjectData>();
    public List<CameraObjectData> m_cameraObjectData = new List<CameraObjectData>();
    public List<MeshFilterObjectData> m_meshFilterObjectData = new List<MeshFilterObjectData>();
    public List<MeshRendererObjectData> m_meshRendererObjectData = new List<MeshRendererObjectData>();
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

[Serializable]
public class MeshRendererObjectData
{
    public string m_name;
    public LightingObjectData m_lightingObjectData;
    public int m_materialCount;
    public List<MaterialObjectData> m_materialsObjectData;
    public bool m_dynamicOcclusion;
}

[Serializable]
public class LightingObjectData
{
    public string m_lightProbe;
    public string m_reflectionProbe;
    public string m_anchorOverride;
    public string m_castShadow;
    public bool m_recieveShadow;
    public string m_motionVectors;
}

[Serializable]
public class MaterialObjectData
{
    public string m_name;
    public Color m_colour;
    public bool m_doubleSidedGI;
    public bool m_enableInstancing;
    public string m_globalIlluminationFlag;
    public string m_mainTexture;
    public Vector2 m_mainTextureOffset;
    public Vector2 m_mainTextureScale;
    public int m_passCount;
    public int m_renderQueue;
    public string m_shader;
}