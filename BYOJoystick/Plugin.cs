using System.Collections;
using System.IO;
using System.Reflection;
using ModLoader.Framework;
using ModLoader.Framework.Attributes;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace BYOJoystick
{
    [ItemId("BYOJ")]
    public class Plugin : VtolMod
    {
        public BYOJ BYOJ;
        
        public static void Log(object msg)
        {
            Debug.Log($"[BYOJ] {msg}");
        }

        private void Awake()
        {
            Log("Loading BYO Joystick Plugin");
            
            Log("Creating Harmony Patches");
            HarmonyInstance.Create("com.BYOJoystick").PatchAll(Assembly.GetExecutingAssembly());
            
            StartCoroutine(LoadAssetBundle());
        }

        private IEnumerator LoadAssetBundle()
        {
            Log("Loading Asset Bundle...");
            string path = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\byoj";

            if (!File.Exists(path))
            {
                Log("Asset Bundle not found");
                yield break;
            }

            var request = AssetBundle.LoadFromFileAsync(path);
            yield return request;
            Log("Asset Bundle Loaded");
            Initialise(request.assetBundle);
        }

        private void Initialise(AssetBundle bundle)
        {
            Log("Loading prefab from Asset Bundle...");

            var assetRequest = bundle.LoadAssetAsync<GameObject>("BYOJ");
            var prefab       = assetRequest.asset as GameObject;

            if (prefab != null)
            {
                Log("Loaded prefab from Asset Bundle");
                BYOJ = Instantiate(prefab, transform.position, transform.rotation).GetComponent<BYOJ>();
                DontDestroyOnLoad(BYOJ);
                Log("BYOJ Started!");
            }
            else
                Log("Failed to load prefab from Asset Bundle");

            bundle.Unload(false);
        }

        public override void UnLoad()
        {
            Log("Unloading BYO Joystick Plugin");
            BYOJ.Unload();
            Destroy(BYOJ.gameObject);
            Destroy(this);
        }
    }
}