using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using PieceManager;
using ItemManager;
using ServerSync;
using UnityEngine;

namespace ArtisanHammer
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class ArtisanHammerPlugin : BaseUnityPlugin
    {
        internal const string ModName = "ArtisanHammer";
        internal const string ModVersion = "0.0.1";
        internal const string Author = "sbirvin1s";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static string ConnectionError = "";

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource ArtisanHammerLogger =
        BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
        { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public void Awake()
        {
            Item artisanHammer = new Item("artisanhammerbundle", "artisan_hammer");
            artisanHammer.Name.English("Artisan Hammer");
            artisanHammer.Description.English("A refined hammer used by craftsman to create works of art");
            artisanHammer.RequiredItems.Add("BlackMarble", 10);
            artisanHammer.RequiredItems.Add("SurtlingCore", 1);
            artisanHammer.RequiredItems.Add("Eitr", 1);

            artisanHammer.DropsFrom.Add("Greydwarf", 0.9f, 1, 2);
            artisanHammer.DropsFrom.Add("Greyling", 0.9f, 1, 2);
            artisanHammer.DropsFrom.Add("Neck", 0.9f, 1, 2);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }
        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                ArtisanHammerLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                ArtisanHammerLogger.LogError($"There was an issue loading your {ConfigFileName}");
                ArtisanHammerLogger.LogError("Please check your config entries for spelling and format!");
            }
        }
    }
}

//namespace MasonHammer
//{
//  [BepInPlugin(ModGUID, ModName, ModVersion)]
//  public class MasonHammerPlugin : BaseUnityPlugin
//  {
//      internal const string ModVersion = "1.0.0";
//      internal const string Author = "sbirvin1s";
//      internal const string ModName = "MasonHammer";
//      private const string ModGUID = Author + "." + ModName;
//      private static string ConfigFileName = ModGUID + ".cfg";
//      private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
//      internal static string ConnectionError = "";
//      private readonly Harmony _harmony = new(ModGUID);

//      public static readonly ManualLogSource MasonHammerLogger =
//          BepInEx.Logging.Logger.CreateLogSource(ModName);

//      private static readonly ConfigSync ConfigSync = new(ModGUID)
//          { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

//      public enum Toggle
//      {
//          On = 1,
//          Off = 0
//      }

//      public void Awake()
//      {
//          _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On,
//              "If on, the configuration is locked and can be changed by server admins only.");
//          _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

//          // Globally turn off configuration options for your pieces, omit if you don't want to do this.
//          BuildPiece.ConfigurationEnabled = false;

//          // Format: new("AssetBundleName", "PrefabName", "FolderName");
//          BuildPiece examplePiece1 = new("funward", "funward", "FunWard");
//          examplePiece1.Name.English("Fun Ward"); // Localize the name and description for the building piece for a language.
//          examplePiece1.Description.English("Ward For testing the Piece Manager");
//          examplePiece1.RequiredItems.Add("FineWood", 20, false); // Set the required items to build. Format: ("PrefabName", Amount, Recoverable)
//          examplePiece1.RequiredItems.Add("SurtlingCore", 20, false);
//          examplePiece1.Category.Add(PieceManager.BuildPieceCategory.Misc);
//          examplePiece1.Crafting.Set(PieceManager.CraftingTable.ArtisanTable); // Set a crafting station requirement for the piece.
//          //examplePiece1.Extension.Set(CraftingTable.Forge, 2); // Makes this piece a station extension, can change the max station distance by changing the second value. Use strings for custom tables.

//          // Or you can do it for a custom table (### Default maxStationDistance is 5. I used 2 as an example here.)
//          //examplePiece1.Extension.Set("MYCUSTOMTABLE", 2); // Makes this piece a station extension, can change the max station distance by changing the second value. Use strings for custom tables.

//          //examplePiece1.Crafting.Set("CUSTOMTABLE"); // If you have a custom table you're adding to the game. Just set it like this.

//          //examplePiece1.SpecialProperties.NoConfig = true;  // Do not generate a config for this piece, omit this line of code if you want to generate a config.
//          examplePiece1.SpecialProperties = new SpecialProperties() { AdminOnly = true, NoConfig = true}; // You can declare multiple properties in one line           

//          Assembly assembly = Assembly.GetExecutingAssembly();
//          _harmony.PatchAll(assembly);
//          SetupWatcher();
//      }

//      private void OnDestroy()
//      {
//          Config.Save();
//      }

//      private void SetupWatcher()
//      {
//          FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
//          watcher.Changed += ReadConfigValues;
//          watcher.Created += ReadConfigValues;
//          watcher.Renamed += ReadConfigValues;
//          watcher.IncludeSubdirectories = true;
//          watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
//          watcher.EnableRaisingEvents = true;
//      }

//      private void ReadConfigValues(object sender, FileSystemEventArgs e)
//      {
//          if (!File.Exists(ConfigFileFullPath)) return;
//          try
//          {
//              MasonHammerLogger.LogDebug("ReadConfigValues called");
//              Config.Reload();
//          }
//          catch
//          {
//              MasonHammerLogger.LogError($"There was an issue loading your {ConfigFileName}");
//              MasonHammerLogger.LogError("Please check your config entries for spelling and format!");
//          }
//      }


//      #region ConfigOptions

//      private static ConfigEntry<Toggle> _serverConfigLocked = null!;

//      private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
//          bool synchronizedSetting = true)
//      {
//          ConfigDescription extendedDescription =
//              new(
//                  description.Description +
//                  (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
//                  description.AcceptableValues, description.Tags);
//          ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
//          //var configEntry = Config.Bind(group, name, value, description);

//          SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
//          syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

//          return configEntry;
//      }

//      private ConfigEntry<T> config<T>(string group, string name, T value, string description,
//          bool synchronizedSetting = true)
//      {
//          return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
//      }

//      private class ConfigurationManagerAttributes
//      {
//          public bool? Browsable = false;
//      }

//      #endregion
//  }
//}