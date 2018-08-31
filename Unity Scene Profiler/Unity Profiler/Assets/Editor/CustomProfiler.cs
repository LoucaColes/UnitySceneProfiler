using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class CustomProfiler : EditorWindow
{
    private DataConfig m_dataConfig;

    private DataSnapshot m_dataSnapshot = new DataSnapshot();

    private DisplayBools m_displayBools = new DisplayBools();

    private DataFilter m_dataFilter = DataFilter.Null;

    private Vector2 m_scrollPos;

    private string m_exportFileName = "DataSnapshot";

    private string m_objectSearch = "";

    private List<GameObject> m_objectsFound = new List<GameObject>();

    [MenuItem("Window/Scene Profiler %#&P")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindowWithRect(typeof(CustomProfiler), new Rect(0, 0, 600, 600), false, "Scene Profiler");
        window.maxSize = new Vector2(600, 600);
        window.minSize = new Vector2(300, 300);
    }

    private void OnEnable()
    {
        RefreshData();
    }

    private void RefreshData()
    {
        m_dataSnapshot.m_gameObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

        m_dataSnapshot.m_lightObjects.Clear();
        m_dataSnapshot.m_meshObjects.Clear();
        m_dataSnapshot.m_meshRendererObjects.Clear();
        m_dataSnapshot.m_cameraObjects.Clear();

        for (int index = 0; index < m_dataSnapshot.m_gameObjects.Length; index++)
        {
            if (m_dataSnapshot.m_gameObjects[index].GetComponent<Light>() != null)
            {
                LightData newLightData = new LightData();
                newLightData.m_light = m_dataSnapshot.m_gameObjects[index].GetComponent<Light>();
                newLightData.m_displayGeneric = false;
                newLightData.m_displayType = false;
                m_dataSnapshot.m_lightObjects.Add(newLightData);
            }
            if (m_dataSnapshot.m_gameObjects[index].GetComponent<Camera>() != null)
            {
                CameraData newCameraData = new CameraData();
                newCameraData.m_camera = m_dataSnapshot.m_gameObjects[index].GetComponent<Camera>();
                newCameraData.m_displayData = false;
                m_dataSnapshot.m_cameraObjects.Add(newCameraData);
            }
            if (m_dataSnapshot.m_gameObjects[index].GetComponent<MeshFilter>() != null)
            {
                MeshData newMeshData = new MeshData();
                newMeshData.m_meshFilter = m_dataSnapshot.m_gameObjects[index].GetComponent<MeshFilter>();
                newMeshData.m_sharedMesh = newMeshData.m_meshFilter.sharedMesh;
                newMeshData.m_displaySharedMeshData = false;
                m_dataSnapshot.m_meshObjects.Add(newMeshData);
            }
            if (m_dataSnapshot.m_gameObjects[index].GetComponent<MeshRenderer>() != null)
            {
                MeshRendererData newMeshRendererData = new MeshRendererData();
                newMeshRendererData.m_meshRenderer = m_dataSnapshot.m_gameObjects[index].GetComponent<MeshRenderer>();
                newMeshRendererData.m_materialsData = new List<MaterialData>();

                if (newMeshRendererData.m_meshRenderer.sharedMaterials.Length == 1)
                {
                    MaterialData newMaterialData = new MaterialData();
                    newMaterialData.m_material = newMeshRendererData.m_meshRenderer.sharedMaterial;
                    newMaterialData.m_displayMaterialData = false;
                    newMeshRendererData.m_materialsData.Add(newMaterialData);
                }
                else
                {
                    for (int matIndex = 0; matIndex < newMeshRendererData.m_meshRenderer.sharedMaterials.Length; matIndex++)
                    {
                        MaterialData newMaterialData = new MaterialData();
                        newMaterialData.m_material = newMeshRendererData.m_meshRenderer.sharedMaterials[matIndex];
                        newMaterialData.m_displayMaterialData = false;
                        newMeshRendererData.m_materialsData.Add(newMaterialData);
                    }
                }

                newMeshRendererData.m_displayMeshRendererData = false;
                newMeshRendererData.m_displayMeshRendererLightingData = false;
                newMeshRendererData.m_displayMaterialsData = false;
                m_dataSnapshot.m_meshRendererObjects.Add(newMeshRendererData);
            }
        }
    }

    private void OnGUI()
    {
        DisplayOptions();

        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);

        if (m_objectSearch.Length == 0)
        {
            DisplayComponents();
        }
        else
        {
            SearchForObjects();
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Export Data Snapshot"))
        {
            ExportSnapshot();
        }
    }

    private void DisplayOptions()
    {
        m_displayBools.m_displayConfig = EditorGUILayout.Foldout(m_displayBools.m_displayConfig, "Config");
        if (m_displayBools.m_displayConfig)
        {
            m_dataConfig = (DataConfig)EditorGUILayout.ObjectField("Data Config File", m_dataConfig,
                typeof(DataConfig), true);
        }

        if (m_dataConfig == null)
        {
            EditorGUILayout.HelpBox("Warning: No config file found, no warnings will be displayed.", MessageType.Warning);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Refresh"))
        {
            RefreshData();
        }

        EditorGUILayout.Space();

        m_displayBools.m_displayFilter = EditorGUILayout.Foldout(m_displayBools.m_displayFilter, "Filter");

        if (m_displayBools.m_displayFilter)
        {
            m_dataFilter = (DataFilter)EditorGUILayout.EnumPopup("Data Filter: ", m_dataFilter);

            EditorGUILayout.HelpBox("Filter data based on components, set to null to see all.", MessageType.Info);

            EditorGUILayout.Space();
        }

        m_displayBools.m_displaySearch = EditorGUILayout.Foldout(m_displayBools.m_displaySearch, "Search");

        if (m_displayBools.m_displaySearch)
        {
            m_objectSearch = EditorGUILayout.DelayedTextField("Object Search: ", m_objectSearch);
            EditorGUILayout.HelpBox("Search for a specific object within the scene, set to null to see all.",
                MessageType.Info);

            EditorGUILayout.Space();
        }
    }

    private void DisplayComponents()
    {
        if (m_dataFilter == DataFilter.Null || m_dataFilter == DataFilter.General)
        {
            m_displayBools.m_displayGeneral =
                EditorGUILayout.Foldout(m_displayBools.m_displayGeneral, "General Data");

            if (m_displayBools.m_displayGeneral)
            {
                DisplayGeneral();
            }
        }

        if (m_dataFilter == DataFilter.Null || m_dataFilter == DataFilter.Light)
        {
            m_displayBools.m_displayLights =
                EditorGUILayout.Foldout(m_displayBools.m_displayLights, "Light Objects");

            if (m_displayBools.m_displayLights)
            {
                DisplayLightsData();
            }
        }

        if (m_dataFilter == DataFilter.Null || m_dataFilter == DataFilter.Camera)
        {
            m_displayBools.m_displayCameras =
                EditorGUILayout.Foldout(m_displayBools.m_displayCameras, "Camera Objects");

            if (m_displayBools.m_displayCameras)
            {
                DisplayCamerasData();
            }
        }

        if (m_dataFilter == DataFilter.Null || m_dataFilter == DataFilter.Mesh)
        {
            m_displayBools.m_displayMeshes =
                EditorGUILayout.Foldout(m_displayBools.m_displayMeshes, "Mesh Objects");

            if (m_displayBools.m_displayMeshes)
            {
                DisplayMeshesData();
            }
        }

        if (m_dataFilter == DataFilter.Null || m_dataFilter == DataFilter.MeshRenderer)
        {
            m_displayBools.m_displayMeshRenderers =
                EditorGUILayout.Foldout(m_displayBools.m_displayMeshRenderers, "Mesh Renderer Objects");

            if (m_displayBools.m_displayMeshRenderers)
            {
                DisplayMeshRenderersData();
            }
        }
    }

    private void SearchForObjects()
    {
        if (m_objectsFound.Count > 0)
        {
            if (m_objectsFound[0].name != m_objectSearch)
            {
                m_objectsFound.Clear();
            }
        }
        if (m_objectsFound.Count == 0)
        {
            bool foundAny = false;
            for (int index = 0; index < m_dataSnapshot.m_gameObjects.Length; index++)
            {
                if (m_dataSnapshot.m_gameObjects[index].name == m_objectSearch)
                {
                    foundAny = true;
                    m_objectsFound.Add(m_dataSnapshot.m_gameObjects[index]);
                }
            }
            if (!foundAny)
            {
                EditorGUILayout.HelpBox("No objects found.", MessageType.Error);
            }
        }
        else
        {
            for (int index = 0; index < m_objectsFound.Count; index++)
            {
                DisplaySingleObject(m_objectsFound[index]);
                EditorGUILayout.Space();
            }
        }
    }

    #region General

    private void DisplayGeneral()
    {
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.IntField("Total Objects In Scene: ", m_dataSnapshot.m_gameObjects.Length);
            EditorGUILayout.IntField("Total Lights In Scene: ", m_dataSnapshot.m_lightObjects.Count);
            EditorGUILayout.IntField("Total Cameras In Scene: ", m_dataSnapshot.m_cameraObjects.Count);
            EditorGUILayout.IntField("Total Meshs In Scene: ", m_dataSnapshot.m_meshObjects.Count);
            EditorGUILayout.IntField("Total Mesh Renderers In Scene: ", m_dataSnapshot.m_meshObjects.Count);
        }
    }

    private void DisplaySingleObject(GameObject _object)
    {
        EditorGUILayout.ObjectField("Object: ", _object, typeof(GameObject), true);

        GeneralDataDisplay.DisplayObject(_object);

        if (m_dataFilter == DataFilter.Null || m_dataFilter == DataFilter.Mesh)
        {
            MeshFilter tempMeshFilter = _object.GetComponent<MeshFilter>();
            if (tempMeshFilter != null)
            {
                MeshDisplay.DisplayMeshFilterData(tempMeshFilter);
            }
        }

        if (m_dataFilter == DataFilter.Null || m_dataFilter == DataFilter.MeshRenderer)
        {
            MeshRenderer tempMeshRenderer = _object.GetComponent<MeshRenderer>();
            if (tempMeshRenderer != null)
            {
                MeshRendererDisplay.DisplayMeshRendererData(tempMeshRenderer);
            }
        }
    }

    #endregion General

    #region Lights

    private void DisplayLightsData()
    {
        if (m_dataSnapshot.m_lightObjects.Count > 0)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < m_dataSnapshot.m_lightObjects.Count; i++)
                {
                    Debug.Log("Light Object: " + m_dataSnapshot.m_lightObjects[i].m_light.name);
                    if (m_dataSnapshot.m_lightObjects[i].m_light.gameObject != null)
                    {
                        EditorGUILayout.ObjectField(m_dataSnapshot.m_lightObjects[i].m_light.name,
                            m_dataSnapshot.m_lightObjects[i].m_light, typeof(Light), true);

                        ObjectPingFocus.DisplayPingButton(m_dataSnapshot.m_lightObjects[i].m_light.gameObject);

                        using (new EditorGUI.IndentLevelScope())
                        {
                            m_dataSnapshot.m_lightObjects[i].m_displayGeneric =
                                EditorGUILayout.Foldout(m_dataSnapshot.m_lightObjects[i].m_displayGeneric,
                                "Generic Information");
                            if (m_dataSnapshot.m_lightObjects[i].m_displayGeneric)
                            {
                                EditorGUILayout.EnumFlagsField("Type: ", m_dataSnapshot.m_lightObjects[i].m_light.type);
                                EditorGUILayout.ColorField("Colour: ", m_dataSnapshot.m_lightObjects[i].m_light.color);
                                EditorGUILayout.FloatField("Intensity: ", m_dataSnapshot.m_lightObjects[i].m_light.intensity);
                                EditorGUILayout.FloatField("Indirect Multiplier: ", m_dataSnapshot.m_lightObjects[i].m_light.bounceIntensity);
                                EditorGUILayout.ObjectField("Flare: ", m_dataSnapshot.m_lightObjects[i].m_light.flare, typeof(Flare), false);
                                EditorGUILayout.EnumFlagsField("Render Mode: ", m_dataSnapshot.m_lightObjects[i].m_light.renderMode);
                                EditorGUILayout.IntField("Culling Mask: ", m_dataSnapshot.m_lightObjects[i].m_light.cullingMask);
                            }
                            m_dataSnapshot.m_lightObjects[i].m_displayType = EditorGUILayout.Foldout(m_dataSnapshot.m_lightObjects[i].m_displayType,
                                "Type Specific Information");
                            if (m_dataSnapshot.m_lightObjects[i].m_displayType)
                            {
                                if (m_dataSnapshot.m_lightObjects[i].m_light.type == LightType.Directional)
                                {
                                    DisplayDirectionalLightData(i);
                                }
                                else if (m_dataSnapshot.m_lightObjects[i].m_light.type == LightType.Area)
                                {
                                    DisplayAreaData(i);
                                }
                                else if (m_dataSnapshot.m_lightObjects[i].m_light.type == LightType.Point)
                                {
                                    DisplayPointData(i);
                                }
                                else
                                {
                                    DisplaySpotData(i);
                                }
                            }
                        }
                        EditorGUILayout.Space();
                    }
                }
            }
        }
    }

    private void DisplayDirectionalLightData(int i)
    {
        EditorGUILayout.EnumFlagsField("Mode: ", m_dataSnapshot.m_lightObjects[i].m_light.lightmapBakeType);
        DisplayShadowData(i);
        EditorGUILayout.ObjectField("Cookie: ", m_dataSnapshot.m_lightObjects[i].m_light.cookie, typeof(Texture), false);
        EditorGUILayout.FloatField("Cookie Size: ", m_dataSnapshot.m_lightObjects[i].m_light.cookieSize);
    }

    private void DisplayShadowData(int i)
    {
        EditorGUILayout.EnumFlagsField("Shadow Type: ", m_dataSnapshot.m_lightObjects[i].m_light.shadows);
        if (m_dataSnapshot.m_lightObjects[i].m_light.shadows == LightShadows.Soft ||
            m_dataSnapshot.m_lightObjects[i].m_light.shadows == LightShadows.Hard)
        {
            if (m_dataSnapshot.m_lightObjects[i].m_light.shadows == LightShadows.Soft &&
                (m_dataSnapshot.m_lightObjects[i].m_light.lightmapBakeType == LightmapBakeType.Mixed ||
                 m_dataSnapshot.m_lightObjects[i].m_light.lightmapBakeType == LightmapBakeType.Baked))
            {
                EditorGUILayout.LabelField("Baked Shadow");
                EditorGUILayout.FloatField("Baked Shadow Angle: ", m_dataSnapshot.m_lightObjects[i].m_light.shadowAngle);
            }
            if (m_dataSnapshot.m_lightObjects[i].m_light.lightmapBakeType == LightmapBakeType.Mixed ||
                m_dataSnapshot.m_lightObjects[i].m_light.lightmapBakeType == LightmapBakeType.Realtime)
            {
                EditorGUILayout.LabelField("Realtime Shadow");
                EditorGUILayout.FloatField("Strength: ", m_dataSnapshot.m_lightObjects[i].m_light.shadowStrength);
                EditorGUILayout.EnumFlagsField("Resolution: ", m_dataSnapshot.m_lightObjects[i].m_light.shadowResolution);
                EditorGUILayout.FloatField("Bias: ", m_dataSnapshot.m_lightObjects[i].m_light.shadowBias);
                EditorGUILayout.FloatField("Normal Bias: ", m_dataSnapshot.m_lightObjects[i].m_light.shadowNormalBias);
                EditorGUILayout.FloatField("Near Plane: ", m_dataSnapshot.m_lightObjects[i].m_light.shadowNearPlane);
            }
        }
    }

    private void DisplayAreaData(int i)
    {
        EditorGUILayout.FloatField("Range: ", m_dataSnapshot.m_lightObjects[i].m_light.range);
        EditorGUILayout.FloatField("Width: ", m_dataSnapshot.m_lightObjects[i].m_light.areaSize.x);
        EditorGUILayout.FloatField("Height: ", m_dataSnapshot.m_lightObjects[i].m_light.areaSize.y);
    }

    private void DisplaySpotData(int i)
    {
        EditorGUILayout.FloatField("Range: ", m_dataSnapshot.m_lightObjects[i].m_light.range);
        EditorGUILayout.FloatField("Spot Angle: ", m_dataSnapshot.m_lightObjects[i].m_light.spotAngle);

        EditorGUILayout.EnumFlagsField("Mode: ", m_dataSnapshot.m_lightObjects[i].m_light.lightmapBakeType);
        DisplayShadowData(i);
    }

    private void DisplayPointData(int i)
    {
        EditorGUILayout.FloatField("Range: ", m_dataSnapshot.m_lightObjects[i].m_light.range);

        EditorGUILayout.EnumFlagsField("Mode: ", m_dataSnapshot.m_lightObjects[i].m_light.lightmapBakeType);
        DisplayShadowData(i);
    }

    #endregion Lights

    #region Cameras

    private void DisplayCamerasData()
    {
        if (m_dataSnapshot.m_cameraObjects.Count > 0)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < m_dataSnapshot.m_cameraObjects.Count; i++)
                {
                    Debug.Log("Camera Object: " + m_dataSnapshot.m_cameraObjects[i].m_camera.name);
                    if (m_dataSnapshot.m_cameraObjects[i].m_camera.gameObject != null)
                    {
                        EditorGUILayout.ObjectField(m_dataSnapshot.m_cameraObjects[i].m_camera.name, m_dataSnapshot.m_cameraObjects[i].m_camera,
                            typeof(Camera), true);

                        ObjectPingFocus.DisplayPingButton(m_dataSnapshot.m_cameraObjects[i].m_camera.gameObject);

                        using (new EditorGUI.IndentLevelScope())
                        {
                            m_dataSnapshot.m_cameraObjects[i].m_displayData = EditorGUILayout.Foldout(m_dataSnapshot.m_cameraObjects[i].m_displayData,
                                "Generic Information");
                            if (m_dataSnapshot.m_cameraObjects[i].m_displayData)
                            {
                                using (new EditorGUI.IndentLevelScope())
                                {
                                    EditorGUILayout.EnumFlagsField("Clear Flag: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.clearFlags);
                                    EditorGUILayout.ColorField("Background Colour: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.backgroundColor);
                                    EditorGUILayout.IntField("Culling Mask: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.cullingMask);

                                    if (!m_dataSnapshot.m_cameraObjects[i].m_camera.orthographic)
                                    {
                                        EditorGUILayout.Toggle("Perspective: ",
                                            !m_dataSnapshot.m_cameraObjects[i].m_camera.orthographic);
                                        EditorGUILayout.FloatField("Field Of View: ",
                                            m_dataSnapshot.m_cameraObjects[i].m_camera.fieldOfView);
                                    }
                                    if (m_dataSnapshot.m_cameraObjects[i].m_camera.orthographic)
                                    {
                                        EditorGUILayout.Toggle("Orthographic: ",
                                            m_dataSnapshot.m_cameraObjects[i].m_camera.orthographic);
                                        EditorGUILayout.FloatField("Orthographic Size: ",
                                            m_dataSnapshot.m_cameraObjects[i].m_camera.orthographicSize);
                                    }
                                    EditorGUILayout.FloatField("Clipping Plane Near: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.nearClipPlane);
                                    EditorGUILayout.FloatField("Clipping Plane Far: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.farClipPlane);
                                    EditorGUILayout.RectField("Viewport Rect: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.rect);
                                    EditorGUILayout.FloatField("Depth: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.depth);
                                    EditorGUILayout.EnumFlagsField("Rendering Path: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.renderingPath);
                                    EditorGUILayout.ObjectField("Target Texture",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.targetTexture,
                                        typeof(Texture), false);
                                    EditorGUILayout.Toggle("Occlusion Culling: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.useOcclusionCulling);
                                    bool hdr = EditorGUILayout.Toggle("Allow HDR: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.allowHDR);
                                    //if (hdr && !m_cameraDataLimit.m_hdrAllowed)
                                    //{
                                    //    EditorGUILayout.HelpBox(m_cameraDataLimit.m_errorMessageHDR, MessageType.Error);
                                    //}
                                    EditorGUILayout.Toggle("Allow MSAA: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.allowMSAA);
                                    EditorGUILayout.Toggle("Allow Dynamic Resolution: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.allowDynamicResolution);
                                    EditorGUILayout.IntField("Target Display: ",
                                        m_dataSnapshot.m_cameraObjects[i].m_camera.targetDisplay);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion Cameras

    #region Meshes

    private void DisplayMeshesData()
    {
        if (m_dataSnapshot.m_meshObjects.Count > 0)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < m_dataSnapshot.m_meshObjects.Count; i++)
                {
                    if (m_dataSnapshot.m_meshObjects[i].m_meshFilter.gameObject != null)
                    {
                        EditorGUILayout.ObjectField(m_dataSnapshot.m_meshObjects[i].m_meshFilter.name,
                            m_dataSnapshot.m_meshObjects[i].m_meshFilter,
                            typeof(MeshFilter), true);

                        ObjectPingFocus.DisplayPingButton(m_dataSnapshot.m_meshObjects[i].m_meshFilter.gameObject);

                        using (new EditorGUI.IndentLevelScope())
                        {
                            if (m_dataSnapshot.m_meshObjects[i].m_sharedMesh != null)
                            {
                                m_dataSnapshot.m_meshObjects[i].m_displaySharedMeshData =
                                    EditorGUILayout.Foldout(m_dataSnapshot.m_meshObjects[i].m_displaySharedMeshData,
                                        "Display Shared Mesh Data");

                                if (m_dataSnapshot.m_meshObjects[i].m_displaySharedMeshData)
                                {
                                    MeshDisplay.DisplayMeshData(m_dataSnapshot.m_meshObjects[i].m_sharedMesh);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion Meshes

    private void DisplayMeshRenderersData()
    {
        if (m_dataSnapshot.m_meshRendererObjects.Count > 0)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < m_dataSnapshot.m_meshRendererObjects.Count; i++)
                {
                    if (m_dataSnapshot.m_meshRendererObjects[i].m_meshRenderer.gameObject != null)
                    {
                        EditorGUILayout.ObjectField(m_dataSnapshot.m_meshRendererObjects[i].m_meshRenderer.name,
                            m_dataSnapshot.m_meshRendererObjects[i].m_meshRenderer,
                            typeof(MeshRenderer), true);

                        ObjectPingFocus.DisplayPingButton(m_dataSnapshot.m_meshRendererObjects[i].m_meshRenderer.gameObject);

                        using (new EditorGUI.IndentLevelScope())
                        {
                            m_dataSnapshot.m_meshRendererObjects[i].m_displayMeshRendererLightingData = EditorGUILayout.Foldout(
                                m_dataSnapshot.m_meshRendererObjects[i].m_displayMeshRendererLightingData,
                                "Display Lighting Data");

                            if (m_dataSnapshot.m_meshRendererObjects[i].m_displayMeshRendererLightingData)
                            {
                                MeshRendererDisplay.DisplayLightingData(m_dataSnapshot.m_meshRendererObjects[i].m_meshRenderer);
                            }

                            m_dataSnapshot.m_meshRendererObjects[i].m_displayMaterialsData = EditorGUILayout.Foldout(
                                m_dataSnapshot.m_meshRendererObjects[i].m_displayMaterialsData,
                                "Display Materials Data");
                            if (m_dataSnapshot.m_meshRendererObjects[i].m_displayMaterialsData)
                            {
                                MeshRendererDisplay.DisplayAllMaterialData(m_dataSnapshot.m_meshRendererObjects[i].m_meshRenderer);
                            }

                            EditorGUILayout.Toggle("Dynamic Occluded: ",
                                m_dataSnapshot.m_meshRendererObjects[i].m_meshRenderer.allowOcclusionWhenDynamic);
                        }
                        EditorGUILayout.Space();
                    }
                }
            }
        }
    }

    private void ExportSnapshot()
    {
        DataSnapshotConverted newDataSnapshotConverted = new DataSnapshotConverted();
        newDataSnapshotConverted.m_gameObjectData = new List<GameObjectData>();
        for (int index = 0; index < m_dataSnapshot.m_gameObjects.Length; index++)
        {
            newDataSnapshotConverted.m_gameObjectData.Add(DataParser.ConvertGameObject(ref m_dataSnapshot.m_gameObjects[index]));
        }
        newDataSnapshotConverted.m_cameraObjectData = new List<CameraObjectData>();
        for (int index = 0; index < m_dataSnapshot.m_cameraObjects.Count; index++)
        {
            newDataSnapshotConverted.m_cameraObjectData.Add(DataParser.ConvertCameraObject(ref m_dataSnapshot.m_cameraObjects[index].m_camera));
        }

        newDataSnapshotConverted.m_meshFilterObjectData = new List<MeshFilterObjectData>();
        for (int index = 0; index < m_dataSnapshot.m_meshObjects.Count; index++)
        {
            newDataSnapshotConverted.m_meshFilterObjectData.Add(DataParser.ConvertMeshFilterObject(ref m_dataSnapshot.m_meshObjects[index].m_meshFilter));
        }

        newDataSnapshotConverted.m_meshRendererObjectData = new List<MeshRendererObjectData>();
        for (int index = 0; index < m_dataSnapshot.m_meshRendererObjects.Count; index++)
        {
            newDataSnapshotConverted.m_meshRendererObjectData.Add(DataParser.ConvertMeshRendererObject(ref m_dataSnapshot.m_meshRendererObjects[index].m_meshRenderer));
        }

        string json = JsonUtility.ToJson(newDataSnapshotConverted, true);

        // Create the full file path
        string filePath = Application.dataPath + "/" + m_exportFileName + ".txt";

        // Write all data in the string array to the text file
        File.WriteAllText(filePath, json);
    }
}

public class DisplayBools
{
    public bool m_displayConfig = false;
    public bool m_displayFilter = true;
    public bool m_displaySearch = true;
    public bool m_displayGeneral = false;
    public bool m_displayLights = false;
    public bool m_displayCameras = false;
    public bool m_displayMeshes = false;
    public bool m_displayMeshRenderers = false;
}

[Serializable]
public class DataSnapshot
{
    public GameObject[] m_gameObjects;
    public List<LightData> m_lightObjects = new List<LightData>();
    public List<CameraData> m_cameraObjects = new List<CameraData>();
    public List<MeshData> m_meshObjects = new List<MeshData>();
    public List<MeshRendererData> m_meshRendererObjects = new List<MeshRendererData>();
}

[Serializable]
public class LightData
{
    public Light m_light;
    public bool m_displayGeneric;
    public bool m_displayType;
}

[Serializable]
public class CameraData
{
    public Camera m_camera;
    public bool m_displayData;
}

[Serializable]
public class MeshData
{
    public MeshFilter m_meshFilter;
    public Mesh m_sharedMesh;
    public bool m_displaySharedMeshData;
}

[Serializable]
public class MeshRendererData
{
    public MeshRenderer m_meshRenderer;
    public List<MaterialData> m_materialsData;
    public bool m_displayMeshRendererData;
    public bool m_displayMeshRendererLightingData;
    public bool m_displayMaterialsData;
}

[Serializable]
public class MaterialData
{
    public Material m_material;
    public bool m_displayMaterialData;
}

public enum DataFilter
{
    Null,
    General,
    Light,
    Camera,
    Mesh,
    MeshRenderer
}