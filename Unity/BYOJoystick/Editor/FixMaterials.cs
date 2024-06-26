using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FixMaterialsInProject : EditorWindow
{
    [Serializable]
    public struct ShaderList
    {
        public ShaderList(string shaderName, Shader targetShader, string shaderToFind = "", string nullDefault = "")
        {
            this.nullDefault = "HUH";
            this.shaderName = shaderName;
            this.targetShader = targetShader;
            this.shaderToFind = shaderToFind;
        }

        public string nullDefault;
        public string shaderName;
        public Shader targetShader;
        [HideInInspector]
        public string shaderToFind;
    }

    [SerializeField]
    public ShaderList[] shaderLists = new ShaderList[]
    {
        new ShaderList("MF-Standard", null, "Standard (Cube Fog)"),
        new ShaderList("MF-Standard (Specular setup)", null, "Standard (Specular setup Cube Fog)"),
        new ShaderList("Instanced/InstancedDiffuse", null, "Standard (Cube Fog)"),
        new ShaderList("Instanced/InstancedStandard", null, "Standard (Cube Fog)"),
        new ShaderList("Instanced/InstancedStandard19", null, "Standard (Cube Fog)"),
        new ShaderList("Instanced/InstancedAdditiveParticle", null, "Legacy Shaders/Particles/Additive"),
        new ShaderList("Instanced/SkinShader", null, "Instanced/SkinColor (Danku)"),
        new ShaderList("Instanced/MultiColoredDetail", null, "Instanced/MultiColoredSuit (Danku)"),
        new ShaderList("Custom Shaders/GlassScratch", null, "Custom Shaders/GlassScratch (Danku)"),
        new ShaderList("Particles/MF-Additive", null, "Legacy Shaders/Particles/Additive"),
        new ShaderList("Particles/MF-Alpha Blended", null, "Legacy Shaders/Particles/Alpha Blended"),
        new ShaderList("VTOL VR/AircraftLivery", null, "VTOL VR/Aircraft Livery (Danku)"),
        new ShaderList("Unlit/VolumetricRotorBlur", null, "Unlit/VolumetricRotorBlur (Danku)"),
        new ShaderList("UI/DefaultOverlayScrolling", null, "UI/DefaultOverlayScrolling (Danku)"),
        new ShaderList("UI/DefaultOverlay", null, "UI/DefaultOverlay (Danku)"),
        new ShaderList("UI/DefaultOverlay2", null, "UI/DefaultOverlay2 (Danku)"),
        new ShaderList("UI/DefaultOverlay2Mask", null, "UI/DefaultOverlay2Mask (Danku)"),
        new ShaderList("Custom/DrawOverAllMasked", null, "Custom/DrawOverAllMasked (Danku)"),
        new ShaderList("Custom/NoiseCloudCone", null, "Custom/NoiseCloudCone (Danku)"),
        new ShaderList("Instanced/InstancedAdditiveNVGONLY", null, "Instanced/InstancedAdditiveNVGONLY (Danku)"),
        new ShaderList("Custom/BlackoutEffect", null, "Danku/BlackoutEffect"),
        new ShaderList("Ethical Motion/Particles/Lit", null),
        new ShaderList("Ethical Motion/Particles/Lit MultiLight", null)
    };

    public string[] builtInShaders = new[]
    {
        "Standard",
        "Standard (Specular setup)",
        "Particles/Standard Surface",
        "Particles/Standard Unlit",
        "UI/Default",
        "Legacy Shaders/Particles/Additive",
        "Legacy Shaders/Particles/Additive (Soft)",
        "Legacy Shaders/Particles/~Additive-Multiply",
        "Legacy Shaders/Particles/Alpha Blended",
        "Legacy Shaders/Particles/Alpha Blended Premultiply",
        "Legacy Shaders/Particles/Multiply",
        "Legacy Shaders/Particles/Multiply (Double)"
    };


    [MenuItem("Editor Utils/Tools/Fix Materials")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FixMaterialsInProject));
    }

    private void Awake()
    {
        for (var index = 0; index < shaderLists.Length; index++)
        {
            var shaderList = shaderLists[index];
            shaderList.targetShader = Shader.Find(shaderList.shaderToFind);
            bool isNull = shaderList.targetShader == null;
            shaderList.nullDefault = isNull ? "Add a shader to me!" : "Im all set!";
            shaderLists[index] = shaderList;
        }
    }

    private void OnGUI()
    {
        ScriptableObject scriptableObject = this;
        SerializedObject serializedObject = new SerializedObject(scriptableObject);
        SerializedProperty serializedProperty = serializedObject.FindProperty("shaderLists");

        
        
        EditorGUILayout.LabelField("List contains a shaderName (to find) and a shader");
        EditorGUILayout.LabelField("to switch to. The existing list contains most");
        EditorGUILayout.LabelField("of the normal shaders to find, but the shader");
        EditorGUILayout.LabelField("to switch to is null, so assign it.");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Shaders To Fix");
        for (var index = 0; index < shaderLists.Length; index++)
        {
            var shaderList = shaderLists[index];
            bool isNull = shaderList.targetShader == null;
            shaderList.nullDefault = isNull ? "Add a shader to me!" : "I'm all set!";
            shaderLists[index] = shaderList;
        }
        EditorGUILayout.PropertyField(serializedProperty);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Fix Materials"))
        {
            FixMaterials();
        }
    }
     
    private void FixMaterials()
    {
        foreach (var assetGUID in AssetDatabase.FindAssets("t:material"))
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
            var asset = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
            try
            {
                if (shaderLists.All(e => asset.shader.name != e.shaderName))
                {
                    continue;
                }

                var shaderListItem = shaderLists.FirstOrDefault(e => asset.shader.name == e.shaderName);
                if (shaderListItem.targetShader == null)
                {
                    Debug.LogError($"Target shader is null?");
                    continue;
                }
                
                string targetGUID = String.Empty;
                long targetFileID = -1;

                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(shaderListItem.targetShader.GetInstanceID(), out targetGUID,
                    out targetFileID);


                int type = 3;

                if (builtInShaders.Any(e => e == shaderListItem.targetShader.name))
                    type = 0;
                
                if (type == 0)
                {
                    Debug.Log($"Fixing shader from {asset.shader.name} to {shaderListItem.targetShader.name} for material {asset}", asset);
                    FixMaterialFile(assetPath, targetGUID, targetFileID, type);
                    continue;
                }
                
                FixMaterialFile(assetPath, targetGUID, targetFileID, type);
            }
            catch (Exception e)
            {
                Debug.LogError($"Asset at {assetPath} brokey? {e}");
            }
        }
    }

    private void FixMaterialFile(string assetPath, string targetGUID, long targetFileID, int type)
    {
        var lines = File.ReadAllLines(assetPath);
        
        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            if (line.Contains("m_Shader:"))
            {
                line = $"  m_Shader: {{fileID: {targetFileID}, guid: {targetGUID}, type: {type}}}";
            }

            lines[index] = line;
        }
        
        File.WriteAllLines(assetPath, lines);
    }
}