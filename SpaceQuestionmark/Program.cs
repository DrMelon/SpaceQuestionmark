using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Steamworks;

namespace SpaceQuestionmark
{
    class Program
    {
        static void Main(string[] args)
        {
#if STEAM
            Global.SteamInitialised = SteamAPI.Init();
#endif

            Global.theGame = new Game("SPACE QUESTIONMARK", 1280/2, 800/2, 60, false);
            Global.theGame.SetWindowScale(2);
            //Global.theGame.FixedFramerate = false;

            Global.thePlayerSession = Global.theGame.AddSession("PlayerOne");
            Global.thePlayerSession.Controller = new AbstractedController(true, 0);
            Global.controllerPlayerOne = Global.thePlayerSession.GetController<AbstractedController>();
            Global.controllerPlayerOne.Movement.AddKeys(new Key[] { Otter.Key.W, Otter.Key.D, Otter.Key.S, Otter.Key.A });
            Global.controllerPlayerOne.OpenMenu.AddKey(Otter.Key.Return);
            // radial
            Global.controllerPlayerOne.ShowQuickUI.AddKey(Otter.Key.Q);
            // use/grab/inspect
            Global.controllerPlayerOne.GraspLeftHand.AddMouseButton(Otter.MouseButton.Left);
            // throw/hit
            Global.controllerPlayerOne.Sprint.AddMouseButton(Otter.MouseButton.Right);
            

            // Create Colours
            Color.AddCustom(new Color(1.0f, 0.0f, 0.0f, 0.66f), "FaintRed");
            Color.AddCustom(new Color(0.0f, 1.0f, 0.0f, 0.66f), "FaintGreen");
            Color.AddCustom(new Color(0.0f, 0.0f, 1.0f, 0.66f), "FaintBlue");
            Color.AddCustom(new Color(0.0f, 1.0f, 1.0f, 0.66f), "FaintCyan");
            Color.AddCustom(new Color(1.0f, 0.0f, 1.0f, 0.66f), "FaintMagenta");
            Color.AddCustom(new Color(1.0f, 1.0f, 0.0f, 0.66f), "FaintYellow");

            Global.theGame.AddScene(new PlayState());
            Global.theGame.LockMouseCenter = true;

            // Steam Checks
            Util.Log("========= STEAM CHECKS ===========");
            if (Global.SteamInitialised)
            {
                Util.Log("\tSteam Name: " + SteamFriends.GetPersonaName());
                Util.Log("\tSteam Friend Count: " + SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll).ToString());
                SteamScreenshots.TriggerScreenshot();
            }
            else
            {
                Util.Log("Steam Not Initialized");
            }


            Global.theGame.Start();



            



        }
    }
}
