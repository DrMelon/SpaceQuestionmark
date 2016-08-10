using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
namespace SpaceQuestionmark
{
    public static class Global
    {
        public static Game theGame;
        public static Session thePlayerSession;
        public static ControllerXbox360 controllerPlayerOne;
        public static string MsgString = "You wake up.\nPress W, S, A, D to move.\nInteract/Use Item with Left Mouse\nThrow Item with Right Mouse.\nHold Q to open item menu.\nRepair your machines!";
        public static bool ResetBox = true;

        public enum ColliderTags
        {
            DEFAULT = 0,
            ITEM,
            LIVING,
            MACHINE,
            FLOOR,
            WALL
        }

        public static void NewWords(string msg)
        {
            MsgString = msg;
            ResetBox = true;
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
    }
}
