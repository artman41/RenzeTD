using System.CodeDom;
using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEngine;

namespace RenzeTD.Scripts.Misc {
    public static class Settings {

        public class Menu {
            
        }

        public class Level {
            public static string TileLocation { get; } = "Game/Tiles/Tile_";
            public static string MapLocation { get; } = "Game/Maps/";
        }
        
    }
}