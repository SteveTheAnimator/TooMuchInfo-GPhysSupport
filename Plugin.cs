using BepInEx;
using HarmonyLib;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TooMuchInfo
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;

        // could be shortened, but eh, its a fucking mod that shows info, not a performance mod lmao
        public BepInEx.Configuration.ConfigEntry<bool> enableCreationDate;
        public BepInEx.Configuration.ConfigEntry<bool> enableCosmetics;
        public BepInEx.Configuration.ConfigEntry<bool> enableMods;
        public BepInEx.Configuration.ConfigEntry<bool> enableModMiniProps;
        public BepInEx.Configuration.ConfigEntry<bool> enablePlatform;
        public BepInEx.Configuration.ConfigEntry<bool> enableFPS;
        public BepInEx.Configuration.ConfigEntry<bool> enableTagged;
        public BepInEx.Configuration.ConfigEntry<bool> enableTurn;
        public BepInEx.Configuration.ConfigEntry<bool> enableColor;
        public BepInEx.Configuration.ConfigEntry<bool> enableID;
        public BepInEx.Configuration.ConfigEntry<bool> enableName;
        public BepInEx.Configuration.ConfigEntry<bool> enableSelfTest;

        public void Start()
        {
            instance = this;
            new Harmony(PluginInfo.GUID).PatchAll();
            // config stuff
            enableCreationDate = Config.Bind("General", "Enable Creation Date", true, "Show account creation date in nametag.");
            enableCosmetics = Config.Bind("General", "Enable Cosmetics", true, "Show cosmetics in nametag.");
            enableMods = Config.Bind("General", "Enable Mods", true, "Show mods in nametag.");
            enableModMiniProps = Config.Bind("General", "Enable Mod Mini Props", true, "Show mod mini props in nametag.");
            enablePlatform = Config.Bind("General", "Enable Platform", true, "Show platform in nametag.");
            enableFPS = Config.Bind("General", "Enable FPS", true, "Show FPS in nametag.");
            enableTagged = Config.Bind("General", "Enable Tagged By", true, "Show who tagged you in nametag.");
            enableTurn = Config.Bind("General", "Enable Turn Type", true, "Show turn type in nametag.");
            enableColor = Config.Bind("General", "Enable Color", true, "Show player color in nametag.");
            enableID = Config.Bind("General", "Enable ID", true, "Show player ID in nametag.");
            enableName = Config.Bind("General", "Enable Name", true, "Show player name in nametag.");
            enableSelfTest = Config.Bind("General", "Enable Self Test", false, "Show TMI info on your own player.");

            Patches.NamePatch.selfTest = enableSelfTest.Value; // why ii? why is the file name not the same as the class name?
        }
    }

    public static class TMIDataCache
    {
        public static readonly Dictionary<string, string> CreationDates = new Dictionary<string, string>();

        public static string GetCreationDate(VRRig rig)
        {
            string id = rig.Creator.UserId;

            if (CreationDates.TryGetValue(id, out string cached))
                return cached;

            CreationDates[id] = "LOADING";

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = id }, r =>
            {
                CreationDates[id] = r.AccountInfo.Created.ToString("MMM dd, yyyy HH:mm").ToUpper();
                rig.UpdateName();
            },
            e =>
            {
                CreationDates[id] = "ERROR";
                rig.UpdateName();
            });

            return "LOADING";
        }
    }

    public static class TMIMod
    {
        public class TMIModInfo
        {
            public string Name;
            public string Color;

            public TMIModInfo(string name, string color)
            {
                Name = name;
                Color = color;
            }
        }

        public static readonly Dictionary<string, TMIModInfo> Cosmetics =
            new Dictionary<string, TMIModInfo>
        {
        { "LBAAD.", new TMIModInfo("ADMINISTRATOR", "FF0000") },
        { "LBAAK.", new TMIModInfo("FOREST GUIDE", "867556") },
        { "LBADE.", new TMIModInfo("FINGER PAINTER", "00FF00") },
        { "LBAGS.", new TMIModInfo("ILLUSTRATOR", "C76417") },
        { "LMAPY.", new TMIModInfo("FOREST GUIDE MOD STICK", "FF8000") },
        { "LBANI.", new TMIModInfo("AA CREATOR BADGE", "291447") }
        };

        public static readonly Dictionary<string, TMIModInfo> Mods =
            new Dictionary<string, TMIModInfo>
        {
        { "genesis", new TMIModInfo("GENESIS", "07019C") },
        { "HP_Left", new TMIModInfo("HOLDABLEPAD", "332316") },
        { "GrateVersion", new TMIModInfo("GRATE", "707070") },
        { "void", new TMIModInfo("VOID", "FFFFFF") },
        { "BANANAOS", new TMIModInfo("BANANAOS", "FFFF00") },
        { "GC", new TMIModInfo("GORILLACRAFT", "43B581") },
        { "CarName", new TMIModInfo("GORILLAVEHICLES", "43B581") },
        { "6p72ly3j85pau2g9mda6ib8px", new TMIModInfo("CCMV2", "BF00FC") },
        { "FPS-Nametags for Zlothy", new TMIModInfo("FPSTAGS", "B103FC") },
        { "cronos", new TMIModInfo("CRONOS", "0000FF") },
        { "ORBIT", new TMIModInfo("ORBIT", "FFFFFF") },
        { "Violet On Top", new TMIModInfo("VIOLET", "DF6BFF") },
        { "MP25", new TMIModInfo("MONKEPHONE", "707070") },
        { "GorillaWatch", new TMIModInfo("GORILLAWATCH", "707070") },
        { "InfoWatch", new TMIModInfo("GORILLAINFOWATCH", "707070") },
        { "BananaPhone", new TMIModInfo("BANANAPHONE", "FFFC45") },
{          
        "GPhys", new TMIModInfo(
            "GPHYS",
            "5271FF"
        )
    },
                    { "Vivid", new TMIModInfo("VIVID", "F000BC") },
    { "RGBA", new TMIModInfo("CUSTOMCOSMETICS", "FF0000") },
    { "cheese is gouda", new TMIModInfo("WHOSICHEATING", "707070") },
    { "shirtversion", new TMIModInfo("GORILLASHIRTS", "707070") },
    { "gpronouns", new TMIModInfo("GORILLAPRONOUNS", "707070") },
    { "gfaces", new TMIModInfo("GORILLAFACES", "707070") },
    { "monkephone", new TMIModInfo("MONKEPHONE", "707070") },
    { "pmversion", new TMIModInfo("PLAYERMODELS", "707070") },
    { "gtrials", new TMIModInfo("GORILLATRIALS", "707070") },
    { "msp", new TMIModInfo("MONKESMARTPHONE", "707070") },
    { "gorillastats", new TMIModInfo("GORILLASTATS", "707070") },
    { "using gorilladrift", new TMIModInfo("GORILLADRIFT", "707070") },
    { "monkehavocversion", new TMIModInfo("MONKEHAVOC", "707070") },
    { "tictactoe", new TMIModInfo("TICTACTOE", "a89232") },
    { "ccolor", new TMIModInfo("INDEX", "0febff") },
    { "imposter", new TMIModInfo("GORILLAAMONGUS", "ff0000") },
    { "spectapeversion", new TMIModInfo("SPECTAPE", "707070") },
    { "cats", new TMIModInfo("CATS", "707070") },
    { "made by biotest05 :3", new TMIModInfo("DOGS", "707070") },
    { "fys cool magic mod", new TMIModInfo("FYSMAGICMOD", "707070") },
    { "colour", new TMIModInfo("CUSTOMCOSMETICS", "707070") },
    { "chainedtogether", new TMIModInfo("CHAINED TOGETHER", "707070") },
    { "goofywalkversion", new TMIModInfo("GOOFYWALK", "707070") },
    { "void_menu_open", new TMIModInfo("VOID", "303030") },
    { "violetpaiduser", new TMIModInfo("VIOLETPAID", "DF6BFF") },
    { "violetfree", new TMIModInfo("VIOLETFREE", "DF6BFF") },
    { "obsidianmc", new TMIModInfo("OBSIDIAN.LOL", "303030") },
    { "dark", new TMIModInfo("SHIBAGT DARK", "303030") },
    { "hidden menu", new TMIModInfo("HIDDEN", "707070") },
    { "oblivionuser", new TMIModInfo("OBLIVION", "5055d3") },
    { "hgrehngio889584739_hugb\n", new TMIModInfo("RESURGENCE", "470050") },
    { "eyerock reborn", new TMIModInfo("EYEROCK", "707070") },
    { "asteroidlite", new TMIModInfo("ASTEROID LITE", "707070") },
    { "elux", new TMIModInfo("ELUX", "707070") },
    { "cokecosmetics", new TMIModInfo("COKE COSMETX", "00ff00") },
    { "GFaces", new TMIModInfo("gFACES", "707070") },
    { "github.com/maroon-shadow/SimpleBoards", new TMIModInfo("SIMPLEBOARDS", "707070") },
    { "ObsidianMC", new TMIModInfo("OBSIDIAN", "DC143C") },
    { "hgrehngio889584739_hugb", new TMIModInfo("RESURGENCE", "707070") },
    { "GTrials", new TMIModInfo("gTRIALS", "707070") },
    { "github.com/ZlothY29IQ/GorillaMediaDisplay", new TMIModInfo("GMD", "B103FC") },
    { "github.com/ZlothY29IQ/TooMuchInfo", new TMIModInfo("TOOMUCHINFO", "B103FC") },
    { "github.com/ZlothY29IQ/RoomUtils-IW", new TMIModInfo("ROOMUTILS-IW", "B103FC") },
    { "github.com/ZlothY29IQ/MonkeClick", new TMIModInfo("MONKECLICK", "B103FC") },
    { "github.com/ZlothY29IQ/MonkeClick-CI", new TMIModInfo("MONKECLICK-CI", "B103FC") },
    { "github.com/ZlothY29IQ/MonkeRealism", new TMIModInfo("MONKEREALISM", "B103FC") },
    { "MediaPad", new TMIModInfo("MEDIAPAD", "B103FC") },
    { "GorillaCinema", new TMIModInfo("gCINEMA", "B103FC") },
    { "ChainedTogetherActive", new TMIModInfo("CHAINEDTOGETHER", "B103FC") },
    { "GPronouns", new TMIModInfo("gPRONOUNS", "707070") },
    { "CSVersion", new TMIModInfo("CustomSkin", "707070") },
    { "github.com/ZlothY29IQ/Zloth-RecRoomRig", new TMIModInfo("ZLOTH-RRR", "B103FC") },
    { "ShirtProperties", new TMIModInfo("SHIRTS-OLD", "707070") },
    { "GorillaShirts", new TMIModInfo("SHIRTS", "707070") },
    { "GS", new TMIModInfo("OLD SHIRTS", "707070") },
    { "6XpyykmrCthKhFeUfkYGxv7xnXpoe2", new TMIModInfo("CCMV2", "DC143C") },
    { "Body Tracking", new TMIModInfo("BODYTRACK-OLD", "7AA11F") },
    { "Body Estimation", new TMIModInfo("HANBodyEst", "7AA11F") },
    { "Gorilla Track", new TMIModInfo("BODYTRACK", "7AA11F") },
    { "CustomMaterial", new TMIModInfo("CUSTOMCOSMETICS", "707070") },
    { "I like cheese", new TMIModInfo("RECROOMRIG", "FE8232") },
    { "silliness", new TMIModInfo("SILLINESS", "FFBAFF") },
    { "emotewheel", new TMIModInfo("EMOTEWHEEL", "1E2030") },
    { "untitled", new TMIModInfo("UNTITLED", "2D73AF") }
        };

        public static bool TryGetMod(string key, out TMIModInfo info) => Mods.TryGetValue(key, out info);
        public static bool TryGetCosmetic(string key, out TMIModInfo info) => Cosmetics.TryGetValue(key, out info);
    }

    public static class TMIHelpers
    {
        public static string CheckCosmetics(VRRig rig)
        {
            string result = "";

            string[] cosKeys = new string[TMIMod.Cosmetics.Count];
            TMIMod.TMIModInfo[] cosVals = new TMIMod.TMIModInfo[TMIMod.Cosmetics.Count];
            TMIMod.Cosmetics.Keys.CopyTo(cosKeys, 0);
            TMIMod.Cosmetics.Values.CopyTo(cosVals, 0);

            for (int i = 0; i < cosKeys.Length; i++)
            {
                string key = cosKeys[i];
                TMIMod.TMIModInfo val = cosVals[i];

                if (rig.concatStringOfCosmeticsAllowed.Contains(key))
                    result += (result == "" ? "" : ", ") + $"<color=#{val.Color}>{val.Name}</color>";
            }

            return result == "" ? null : result;
        }

        public static string CheckMods(VRRig rig, bool showMini = true)
        {
            string result = "";
            Dictionary<string, object> props = new Dictionary<string, object>();

            var custom = rig.Creator.GetPlayerRef().CustomProperties;
            var keysCustom = new object[custom.Count];
            var valsCustom = new object[custom.Count];
            custom.Keys.CopyTo(keysCustom, 0);
            custom.Values.CopyTo(valsCustom, 0);

            for (int i = 0; i < keysCustom.Length; i++)
                props[keysCustom[i].ToString().ToLower()] = valsCustom[i];

            string[] modKeys = new string[TMIMod.Mods.Count];
            TMIMod.TMIModInfo[] modVals = new TMIMod.TMIModInfo[TMIMod.Mods.Count];
            TMIMod.Mods.Keys.CopyTo(modKeys, 0);
            TMIMod.Mods.Values.CopyTo(modVals, 0);

            for (int i = 0; i < modKeys.Length; i++)
            {
                string k = modKeys[i].ToLower();
                TMIMod.TMIModInfo mod = modVals[i];

                if (!props.ContainsKey(k))
                    continue;

                Dictionary<string, object> displayData = new Dictionary<string, object>();

                if (custom.ContainsKey(modKeys[i]) && showMini)
                {
                    object value = custom[modKeys[i]];

                    void ParseValue(object val, string prefix = "")
                    {
                        if (val == null) return;

                        switch (val)
                        {
                            case IDictionary dict:
                                foreach (DictionaryEntry entry in dict)
                                {
                                    ParseValue(entry.Value, prefix + entry.Key + ".");
                                }
                                break;
                            case IEnumerable enumerable when !(val is string):
                                int index = 0;
                                foreach (var item in enumerable)
                                {
                                    ParseValue(item, prefix + index + ".");
                                    index++;
                                }
                                break;
                            default:
                                // cheeck if it can be parsed, if it cannot (returns a blank string that shows the type or is empty)
                                // return "CNTPRSE"
                                if (ObjectToString(val) == val.GetType().ToString() || ObjectToString(val) == "")
                                {
                                    displayData[prefix.TrimEnd('.')] = "CNTPRSE";
                                    break;
                                }
                                // if the type is null, return "NULL"
                                if (val == null)
                                {
                                    displayData[prefix.TrimEnd('.')] = "NULL";
                                    break;
                                }
                                displayData[prefix.TrimEnd('.')] = val;
                                break;
                        }
                    }

                    ParseValue(value);
                }

                string modString = mod.Name;
                if (displayData.Count > 0)
                {
                    modString += ": {";
                    string sep = "";
                    foreach (var kv in displayData)
                    {
                        modString += sep + kv.Key + ": " + ObjectToString(kv.Value);
                        sep = ", ";
                    }
                    modString += "}";
                }

                result += (result == "" ? "" : ", ") + $"<color=#{mod.Color}>{modString}</color>";
            }

            var items = rig.cosmeticSet.items;
            if (items.Length == 0)
                result += (result == "" ? "" : ", ") + "<color=green>HIDDEN COSMETX (?)</color>";

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (!item.isNullItem && !rig.concatStringOfCosmeticsAllowed.Contains(item.itemName))
                {
                    result += (result == "" ? "" : ", ") + "<color=green>COSMETX</color>";
                    break;
                }
            }

            return result == "" ? null : result;
        }
        private static string ObjectToString(object obj) // this code sucks!
        {
            if (obj == null) return "null";

            if (obj is Array arr)
            {
                string[] elems = new string[arr.Length];
                for (int i = 0; i < arr.Length; i++) elems[i] = ObjectToString(arr.GetValue(i));
                return "[" + string.Join(", ", elems) + "]";
            }

            if (obj is IList list)
            {
                string[] elems = new string[list.Count];
                for (int i = 0; i < list.Count; i++) elems[i] = ObjectToString(list[i]);
                return "[" + string.Join(", ", elems) + "]";
            }

            if (obj is IDictionary dict)
            {
                string[] elems = new string[dict.Count];
                int idx = 0;
                foreach (DictionaryEntry de in dict)
                    elems[idx++] = de.Key + ": " + ObjectToString(de.Value);
                return "{" + string.Join(", ", elems) + "}";
            }

            return obj.ToString();
        }

        public static string GetFPS(VRRig rig)
        {
            var t = Traverse.Create(rig).Field("fps");
            return t != null ? "FPS " + t.GetValue() : null;
        }

        public static string GetTagged(VRRig rig)
        {
            int id = (int)Traverse.Create(rig).Field("taggedById").GetValue();
            var tagger = PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(id, false);
            return tagger != null ? "TAGGED BY " + tagger.NickName : null;
        }

        public static string GetPlatform(VRRig rig)
        {
            string c = rig.concatStringOfCosmeticsAllowed;
            if (c.Contains("S. FIRST LOGIN"))
                return "STEAM";
            if (c.Contains("FIRST LOGIN") || rig.Creator.GetPlayerRef().CustomProperties.Count >= 2)
                return "PC";
            return "STANDALONE";
        }

        public static string GetTurn(VRRig rig)
        {
            var tt = Traverse.Create(rig).Field("turnType");
            var tf = Traverse.Create(rig).Field("turnFactor");

            if (tt == null || tf == null) return null;
            string type = (string)tt.GetValue();
            return type == "NONE" ? "NONE" : type + " " + tf.GetValue();
        }

        public static string FormatColor(Color c)
        {
            return "COLOR <color=red>" + Math.Round(c.r * 255) +
                   "</color> <color=green>" + Math.Round(c.g * 255) +
                   "</color> <color=blue>" + Math.Round(c.b * 255) + "</color>";
        }
        public static void UpdateName(VRRig __instance)
        {
            try
            {
                if (__instance.Creator == null) return;

                var lines = new List<string>
        {
            "",
            "",
            ""
        };

                if (Plugin.instance.enableName.Value)
                    lines.Add(__instance.Creator.NickName);

                if (Plugin.instance.enableID.Value)
                    lines.Add("ID " + __instance.Creator.UserId);

                if (Plugin.instance.enableCreationDate.Value)
                {
                    string creation = TMIDataCache.GetCreationDate(__instance);
                    if (creation != null) lines.Add(creation);
                }

                if (Plugin.instance.enableColor.Value)
                    lines.Add(TMIHelpers.FormatColor(__instance.playerColor));

                if (Plugin.instance.enablePlatform.Value)
                {
                    string platform = TMIHelpers.GetPlatform(__instance);
                    if (platform != null) lines.Add(platform);
                }

                if (Plugin.instance.enableCosmetics.Value)
                {
                    string cos = TMIHelpers.CheckCosmetics(__instance);
                    if (cos != null) lines.Add(cos);
                }

                if (Plugin.instance.enableMods.Value)
                {
                    string mods = TMIHelpers.CheckMods(__instance, Plugin.instance.enableModMiniProps.Value);
                    if (mods != null) lines.Add("MODS " + mods);
                }

                if (Plugin.instance.enableTagged.Value)
                {
                    string tagged = TMIHelpers.GetTagged(__instance);
                    if (tagged != null) lines.Add(tagged);
                }

                if (Plugin.instance.enableFPS.Value)
                {
                    string fps = TMIHelpers.GetFPS(__instance);
                    if (fps != null) lines.Add(fps);
                }

                if (Plugin.instance.enableTurn.Value)
                {
                    string turn = TMIHelpers.GetTurn(__instance);
                    if (turn != null) lines.Add(turn);
                }

                __instance.playerText1.text = string.Join("\n", lines);
            }
            catch { }
        }
    }
}