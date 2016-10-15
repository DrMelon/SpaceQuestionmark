using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace SpaceQuestionmark
{
    class Assets
    {
        public static string ASSET_BASE_PATH = "../../../Assets/";

        public static string SPRITE_TEST = ASSET_BASE_PATH + "test.png";
        public static string GFX_STARFIELD = ASSET_BASE_PATH + "starfield.png";
        public static string MAP_STATION = ASSET_BASE_PATH + "map.tmx";
        public static string GFX_PLAYER = ASSET_BASE_PATH + "player.png";
        public static string XHAIR = ASSET_BASE_PATH + "crosshair.png";
        public static string VHS_SHADER = ASSET_BASE_PATH + "vhs.ps";
        public static string VHS_SHADER2 = ASSET_BASE_PATH + "vhs2.ps";
        public static string FONT_MSG = ASSET_BASE_PATH + "font.ttf";
        public static string GFX_HUD = ASSET_BASE_PATH + "msgs.png";
        public static string GFX_PC_TOP = ASSET_BASE_PATH + "pc.png";
        public static string GFX_PC_FRONT = ASSET_BASE_PATH + "pc_front.png";
        public static string GFX_BATTERY = ASSET_BASE_PATH + "battery.png";
        public static string GFX_CIRCUIT = ASSET_BASE_PATH + "circuit.png";
        public static string GFX_BOOTS = ASSET_BASE_PATH + "boots.png";
        public static string GFX_DONUT = ASSET_BASE_PATH + "donut.png";
        public static string GFX_EXTINGUISHER = ASSET_BASE_PATH + "extinguisher.png";
        public static string GFX_HAND = ASSET_BASE_PATH + "hand.png";
        public static string GFX_O2TANK = ASSET_BASE_PATH + "o2.png";
        public static string GFX_FLOORTILES = ASSET_BASE_PATH + "floor.png";
        public static string GFX_WALLTILES = ASSET_BASE_PATH + "walls.png";
        public static string GFX_CRISPS = ASSET_BASE_PATH + "wongers.png";
        public static string GFX_WRENCH = ASSET_BASE_PATH + "wrench.png";
        public static string GFX_MACHINETOP = ASSET_BASE_PATH + "machinefromabove.png";
        public static string GFX_VENDING = ASSET_BASE_PATH + "vendingmachine.png";
        public static string GFX_AIRLOCKH = ASSET_BASE_PATH + "airlock2.png";
        public static string GFX_AIRLOCKV = ASSET_BASE_PATH + "airlock.png";
        public static string GFX_FIRE = ASSET_BASE_PATH + "fire.png";
        public static string GFX_HP_METER = ASSET_BASE_PATH + "hpmeter.png";
        public static string GFX_O2_METER = ASSET_BASE_PATH + "o2meter.png";


        // Player Clothing
        public static Dictionary<string, string> GFX_PLAYER_DEBUG_COSTUME = new Dictionary<string, string>
        {
            { "torso", ASSET_BASE_PATH + "player/debug_torso.png" },
            { "head", ASSET_BASE_PATH + "player/debug_head.png" },
            { "arms", ASSET_BASE_PATH + "player/debug_arms.png" },
            { "legs", ASSET_BASE_PATH + "player/debug_legs.png" }
        };

        /*
        public static string SPRITE_TEST = "Assets/test.png";
        public static string GFX_STARFIELD = "Assets/starfield.png";
        public static string MAP_STATION = "Assets/map.tmx";
        public static string GFX_PLAYER = "Assets/player.png";
        public static string XHAIR = "Assets/crosshair.png";
        public static string VHS_SHADER = "Assets/vhs.ps";
        public static string VHS_SHADER2 = "Assets/vhs2.ps";
        public static string FONT_MSG = "Assets/font.ttf";
        public static string GFX_HUD = "Assets/msgs.png";
        public static string GFX_PC_TOP = "Assets/pc.png";
        public static string GFX_PC_FRONT = "Assets/pc_front.png";
        public static string GFX_BATTERY = "Assets/battery.png";
        public static string GFX_CIRCUIT = "Assets/circuit.png";
        public static string GFX_BOOTS = "Assets/boots.png";
        public static string GFX_DONUT = "Assets/donut.png";
        public static string GFX_EXTINGUISHER = "Assets/extinguisher.png";
        public static string GFX_HAND = "Assets/hand.png";
        public static string GFX_O2TANK = "Assets/o2.png";
        public static string GFX_TILE = "Assets/tile.png";
        public static string GFX_CRISPS = "Assets/wongers.png";
        public static string GFX_WRENCH = "Assets/wrench.png";
        public static string GFX_MACHINETOP = "Assets/machinefromabove.png";
        public static string GFX_VENDING = "Assets/vendingmachine.png";
        public static string GFX_AIRLOCKH = "Assets/airlock2.png";
        public static string GFX_AIRLOCKV = "Assets/airlock.png";
        public static string GFX_FIRE = "Assets/fire.png";*/

    }
}
