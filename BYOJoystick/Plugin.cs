using System.Collections;
using System.IO;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace BYOJoystick
{
    public class Plugin : VTOLMOD
    {
        private static Plugin _instance;

        public new static void Log(object msg)
        {
            if (_instance != null)
                ((VTOLMOD)_instance).Log(msg);
            else
                Debug.Log($"[BYOJ] {msg}");
        }

        public override void ModLoaded()
        {
            _instance = this;
            Log("Loading BYO Joystick Plugin");
            base.ModLoaded();
            
            Log("Creating Harmony Patches");
            HarmonyInstance.Create("com.BYOJoystick").PatchAll(Assembly.GetExecutingAssembly());
            
            StartCoroutine(LoadAssetBundle());
        }

        private IEnumerator LoadAssetBundle()
        {
            Log("Loading Asset Bundle...");
            string path = Directory.GetCurrentDirectory() + @"\VTOLVR_ModLoader\mods\BYO_Joystick\byoj";
            if (!File.Exists(path))
            {
                Log("Loading from dev path...");
                path = @"D:\Projects\VTOLVR\My Mods\BYO Joystick\Builds\byoj";
            }

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
                var byoj = Instantiate(prefab, transform.position, transform.rotation);
                DontDestroyOnLoad(byoj);
                Log("BYOJ Started!");
            }
            else
                Log("Failed to load prefab from Asset Bundle");

            bundle.Unload(false);
        }
    }
}