//
// @Author: J Brown (@DrMelon)
// 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
namespace SpaceQuestionmark
{
    // globals suck and are terrible but i'll use them anyway lmao
    public static class Global
    {
        public static Game theGame;
        public static Session thePlayerSession;
        public static AbstractedController controllerPlayerOne;
        public static bool SteamInitialised;

        public static bool debugMode = false;

        public enum ColliderTags
        {
            DEFAULT = 0,
            ITEM,
            LIVING,
            MACHINE,
            FLOOR,
            WALL
        }



        public static ColliderTags GetColliderTagForType<T>()
        {
            if(typeof(T) == typeof(Entities.Floor))
            {
                return ColliderTags.FLOOR;
            }
            if (typeof(T) == typeof(Entities.Wall))
            {
                return ColliderTags.WALL;
            }
            if (typeof(T) == typeof(Entities.Machine))
            {
                return ColliderTags.MACHINE;
            }
            if (typeof(T) == typeof(Entities.Living))
            {
                return ColliderTags.LIVING;
            }
            if (typeof(T) == typeof(Entities.Item))
            {
                return ColliderTags.ITEM;
            }

            return ColliderTags.DEFAULT;
        }


        [OtterCommand]
        public static void ToggleDebugMode(bool on)
        {
            debugMode = on;
            Util.Log("Debug Mode: " + on.ToString());
        }

        public static void DebugLog(string text)
        {
            if(debugMode)
            {
                Util.Log(text);
            }
        }

        [OtterCommand]
        public static void SetTimeScale(float time)
        {
            Systems.Time.deltaTimeWorldThinkScale = time;
        }
    }
}
