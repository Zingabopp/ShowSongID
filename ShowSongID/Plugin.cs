using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using Harmony;
using System.Reflection;

namespace ShowSongID
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static Ref<PluginConfig> config;
        internal static IConfigProvider configProvider;
        private bool customUIExists = false;

        public static bool DisplayBeforeAuthor
        {
            get { return config.Value.DisplayBeforeAuthor; }
            set
            {
                config.Value.DisplayBeforeAuthor = value;
                configProvider.Store(config.Value);
            }
        }

        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;
            configProvider = cfgProvider;

            config = configProvider.MakeLink<PluginConfig>((p, v) => {
                // Build new config file if it doesn't exist or RegenerateConfig is true
                if (v.Value == null || v.Value.RegenerateConfig)
                {
                    Logger.log.Debug("Regenerating PluginConfig");
                    p.Store(v.Value = new PluginConfig() {
                        // Set your default settings here.
                        RegenerateConfig = false,
                        DisplayBeforeAuthor = false
                    });
                }
                config = v;
            });
        }

        public void OnApplicationStart()
        {
            //Logger.log.Debug("OnApplicationStart");
            customUIExists = IPA.Loader.PluginManager.AllPlugins.FirstOrDefault(c => c.Metadata.Name == "Custom UI") != null;
            try
            {
                var harmony = HarmonyInstance.Create("com.github.zingabopp.showsongid");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Logger.log.Critical("This plugin requires Harmony. Make sure you " +
                    "installed the plugin properly, as the Harmony DLL should have been installed with it.\n" +
                    ex.StackTrace);
            }

            if (customUIExists)
                CustomUI.Utilities.BSEvents.menuSceneLoadedFresh += MenuLoadedFresh;
            else
                Logger.log.Info("Could not find CustomUI, in game settings UI will not be loaded.");
        }

        public void MenuLoadedFresh()
        {
            {
                Logger.log.Debug("Creating plugin's UI");
                UI.ShowSongID_UI.CreateUI();
            }
        }

        public void OnApplicationQuit()
        {
            //Logger.log.Debug("OnApplicationQuit");
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {

        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
