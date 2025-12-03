using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TooMuchInfo.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("SerializeReadShared", MethodType.Normal)]
    public class OnDataReceived
    {
        private static void Postfix(VRRig __instance) => __instance.UpdateName();
    }
}
