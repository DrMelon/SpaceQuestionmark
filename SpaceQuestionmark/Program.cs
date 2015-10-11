using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;


namespace SpaceQuestionmark
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Global.theGame = new Game("SPACE QUESTIONMARK", 640, 480, 60, false);

            Global.thePlayerSession = Global.theGame.AddSession("PlayerOne");
            Global.thePlayerSession.Controller = new ControllerXbox360(0);
            Global.controllerPlayerOne = Global.thePlayerSession.GetController<ControllerXbox360>();
            Global.controllerPlayerOne.LeftStick.AddKeys(new Key[] { Otter.Key.W, Otter.Key.D, Otter.Key.S, Otter.Key.A });
            Global.controllerPlayerOne.Start.AddKey(Otter.Key.Return);
            // radial
            Global.controllerPlayerOne.LB.AddKey(Otter.Key.Q);
            // use/grab/inspect
            Global.controllerPlayerOne.X.AddMouseButton(Otter.MouseButton.Left);
            // throw/hit
            Global.controllerPlayerOne.B.AddMouseButton(Otter.MouseButton.Right);


            Global.theGame.AddScene(new PlayState());
            Global.theGame.LockMouseCenter = true;
            Global.theGame.Start();
        }
    }
}
