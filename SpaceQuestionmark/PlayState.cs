using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Otter.TiledLoader;

namespace SpaceQuestionmark
{
    class PlayState : Scene
    {
        // Vars
        // Entities.Map theMap;

        Entities.Floor map;

        Entities.Human thePlayer;
        Entity cameraFocus = new Entity();
        

        public PlayState()
        {
            // Create Entities:

            map = new Entities.Floor();
            Add(map);

            // Player (part of map?)
            thePlayer = new Entities.Human();
            thePlayer.X = map.playerStartX * 64;
            thePlayer.Y = map.playerStartY * 64;
            thePlayer.AddComponent(new Components.MobMovement());
            thePlayer.GetComponent<Components.MobMovement>().myController = Global.controllerPlayerOne;
            thePlayer.Layer = -50;
            thePlayer.AddCollider(new CircleCollider(8, (int)Global.ColliderTags.LIVING));
            thePlayer.Collider.CenterOrigin();
            
            Add(thePlayer);
            
            // Scene Stuff
            ApplyCamera = true;
            CameraFocus = cameraFocus;

        }


        // Funcs
        public override void Update()
        {
            base.Update();

        }





        public void ResetGame()
        {

            
        }

        public override void UpdateLast()
        {
            base.UpdateLast();

            cameraFocus.X = Util.Lerp(thePlayer.X, thePlayer.X, 0.5f);
            cameraFocus.Y = Util.Lerp(thePlayer.Y, thePlayer.Y, 0.5f);
            //CameraZoom = 1.5f + (float)Math.Sin(Game.Instance.Timer * 0.001f);
            CameraZoom = 2.0f;
            //CameraAngle = (float)Math.Sin(Game.Instance.Timer * 0.002f);

        }

        public override void Render()
        {
            base.Render();

            // Debug renders
            if (Global.debugMode)
            {
                Draw.Circle(thePlayer.X, thePlayer.Y, 8, Color.None, Color.Custom("FaintRed"), 1);
            }
        }
         

    }
}
