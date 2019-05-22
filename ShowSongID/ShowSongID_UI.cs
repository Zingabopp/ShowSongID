using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomUI.Settings;
using CustomUI.GameplaySettings;

namespace ShowSongID.UI
{
    public class ShowSongID_UI
    {
        public static void CreateUI()
        {
            CreateSettingsUI();
        }

        /// <summary>
        /// This is the code used to create a submenu in Beat Saber's Settings menu.
        /// </summary>
        public static void CreateSettingsUI()
        {
            //This will create a menu tab in the settings menu for your plugin
            var pluginSettingsSubmenu = SettingsUI.CreateSubMenu("ShowSongID");

            // Example code for creating a true/false toggle button 
            var displayBeforeAuthorToggle = pluginSettingsSubmenu.AddBool("Display Before Author", "Display the song ID before the author name.");
            displayBeforeAuthorToggle.GetValue += delegate { return Plugin.DisplayBeforeAuthor; };
            displayBeforeAuthorToggle.SetValue += delegate (bool value) {
                // This code is run when the toggle is toggled.
                Plugin.DisplayBeforeAuthor = value;
            };
        }
    }
}
