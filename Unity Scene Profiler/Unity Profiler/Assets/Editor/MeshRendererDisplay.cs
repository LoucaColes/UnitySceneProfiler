using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MeshRendererDisplay : object
{
    public static void DisplayMeshRendererData(MeshRenderer _meshRenderer)
    {
        EditorGUILayout.LabelField("Mesh Renderer Data");
        EditorGUILayout.LabelField("Lighting Data");
        DisplayLightingData(_meshRenderer);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Material Data");
        DisplayAllMaterialData(_meshRenderer);
        EditorGUILayout.Space();

        EditorGUILayout.Toggle("Dynamic Occluded: ",
            _meshRenderer.allowOcclusionWhenDynamic);
    }

    public static void DisplayLightingData(MeshRenderer _meshRenderer)
    {
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.EnumPopup("Light Probes: ", _meshRenderer.lightProbeUsage);
            EditorGUILayout.EnumPopup("Reflection Probes: ", _meshRenderer.reflectionProbeUsage);
            EditorGUILayout.ObjectField("Anchor Override: ", _meshRenderer.probeAnchor, typeof(Transform), true);
            EditorGUILayout.EnumPopup("Cast Shadows: ", _meshRenderer.shadowCastingMode);
            EditorGUILayout.Toggle("Receive Shadows: ", _meshRenderer.receiveShadows);
            EditorGUILayout.EnumPopup("Motion Vectors: ", _meshRenderer.motionVectorGenerationMode);
        }
    }

    public static void DisplayAllMaterialData(MeshRenderer _meshRenderer)
    {
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.IntField("Materials Count: ",
                _meshRenderer.sharedMaterials.Length);
            for (int index = 0;
                index < _meshRenderer.sharedMaterials.Length;
                index++)
            {
                DisplayMaterialData(_meshRenderer.sharedMaterials[index]);
            }
        }
    }

    public static void DisplayMaterialData(Material _material)
    {
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUILayout.ObjectField(_material.name,
                _material,
                typeof(Material), true);
            EditorGUILayout.ColorField("Material Colour: ", _material.color);
            EditorGUILayout.Toggle("Double Sided GI: ", _material.doubleSidedGI);
            EditorGUILayout.Toggle("Enable Instancing: ", _material.enableInstancing);
            EditorGUILayout.EnumPopup("Global Illumination Flags: ",
                _material.globalIlluminationFlags);
            EditorGUILayout.ObjectField("Main Texture: ", _material.mainTexture, typeof(Texture), true);
            EditorGUILayout.Vector2Field("Main Texture Offset: ", _material.mainTextureOffset);
            EditorGUILayout.Vector2Field("Main Texture Scale: ", _material.mainTextureScale);
            EditorGUILayout.IntField("Pass Count: ", _material.passCount);
            EditorGUILayout.IntField("Render Queue: ", _material.renderQueue);
            EditorGUILayout.LabelField("Shader: ", _material.shader.ToString());
        }
    }
}