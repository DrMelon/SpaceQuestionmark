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
        Tilemap floorMap;
        GridCollider floorCol;
        Tilemap wallMap;
        GridCollider wallCol;
        Entities.Floor map;

        Entities.Human thePlayer;
        Entity cameraFocus = new Entity();
        

        public PlayState()
        {
            // Create Entities:


            // Map
            map = new Entities.Floor();
            floorMap = new Tilemap(Assets.GFX_FLOORTILES, 256 * 64, 64);
            floorCol = new GridCollider(256 * 64, 256 * 64, 64, 64, (int)Global.ColliderTags.FLOOR);
            wallMap = new Tilemap(Assets.GFX_WALLTILES, 256 * 64, 64);
            wallCol = new GridCollider(256 * 64, 256 * 64, 64, 64, (int)Global.ColliderTags.WALL);
            map.AddGraphic(floorMap);
            map.AddCollider(floorCol);
            map.AddGraphic(wallMap);
            map.AddCollider(wallCol);

            // map gen??
            for(int i = 0; i < 256; i++)
            {
                for(int j = 0; j < 256; j++)
                {
                    floorMap.SetTile(i, j, 2, "");
                    floorCol.SetTile(i, j, true);
                }
            }

            Add(map);
            
            
            // Player (part of map?)
            thePlayer = new Entities.Human();
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
            CameraZoom = 1.5f + (float)Math.Sin(Game.Instance.Timer * 0.001f);
            CameraAngle = (float)Math.Sin(Game.Instance.Timer * 0.002f);

        }

        public override void Render()
        {
            base.Render();

            thePlayer.Collider.Render(Color.Red);
        }
         

    }
}
