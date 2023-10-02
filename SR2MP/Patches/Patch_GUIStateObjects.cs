using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

//Patch by KomiksPL
namespace UnstrippedClasses
{
    [HarmonyPatch(typeof(GUIStateObjects))]
    internal static class Patch_GUIStateObjects
    {
        private static Dictionary<int, Il2CppSystem.Object> s_StateCache = new Dictionary<int, Il2CppSystem.Object>();

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GUIStateObjects.QueryStateObject))]
        private static bool QueryStateObject(Il2CppSystem.Type t, int controlID, ref Il2CppSystem.Object __result)
        {
            Il2CppSystem.Object o = s_StateCache[controlID];
            __result = t.IsInstanceOfType(o) ? o : null;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GUIStateObjects.GetStateObject))]
        private static bool GetStateObject(Il2CppSystem.Type t, int controlID, ref Il2CppSystem.Object __result)
        {
            if (!s_StateCache.TryGetValue(controlID, out var instance) || instance.GetIl2CppType() != t)
            {
                instance = Il2CppSystem.Activator.CreateInstance(t);
                s_StateCache[controlID] = instance;
            }
            __result = instance;
            return false;
        }
    }
}
