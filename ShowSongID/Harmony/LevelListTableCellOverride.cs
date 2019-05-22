using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using TMPro;

namespace ShowSongID.Harmony
{
    [HarmonyPatch(typeof(LevelListTableCell), "SetDataFromLevelAsync",
        new Type[] {
        typeof(IPreviewBeatmapLevel)})]
    class LevelListTableCellOverride
    {
        private const string SongFolder = "CustomSongs/";
        static void Postfix(LevelListTableCell __instance, ref IPreviewBeatmapLevel level, ref TextMeshProUGUI ____authorText)
        {
            var lId = level.levelID;
            var song = SongLoaderPlugin.SongLoader.CustomLevels.Where(s => s.customSongInfo.levelId == lId).FirstOrDefault();
            if (song?.customSongInfo != null)
                if (Plugin.DisplayBeforeAuthor)
                    ____authorText.text = $"({GetBeatSaverID(song.customSongInfo?.path)}) {____authorText.text}";
                else
                    ____authorText.text = $"{____authorText.text} ({GetBeatSaverID(song.customSongInfo?.path)})";
        }

        private static int GetBeatSaverID(string path)
        {
            path = path.Replace('\\', '/');
            if (path == null)
                return 0;
            int id = 0;
            path = path.Substring(path.IndexOf(SongFolder) + SongFolder.Length);
            int dashIndex = path.IndexOf('-');
            if (dashIndex < 0)
            {
                dashIndex = path.Length;
                //Console.WriteLine($"DashIndex not found in {path}");
            }

            int.TryParse(path.Substring(0, dashIndex), out id);

            return id;
        }
    }
}
