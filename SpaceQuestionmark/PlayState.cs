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
        Entity newTest = new Entity(0, 0, new Image(Assets.SPRITE_TEST));

        // BG Stuf
        // Starfield stuff
        Image starFieldFar;
        Image starFieldMid;
        Image starFieldClose;

        // Map
        TiledProject mapProject = new TiledProject(Assets.MAP_STATION);
        Tilemap floorTiles;
        Tilemap wallTiles;
        GridCollider wallCollision;
        GridCollider floorCollision;
        Entity wallTest;
        Entity floorTest;

        float swayAmt = 1.0f;

        // Shaders
        Shader VHSShader;
        Shader VHSShader2;

        // Player
        Player thePlayer;

        // HUD
        Image msgBox;
        RichText msgText;
        int msgCurrentChar;
        int bip;
        // we need text-reveal mode or sth. edit richtext?
        
        
          

        public PlayState()
        {

            //Load shader
            VHSShader = new Shader(ShaderType.Fragment, Assets.VHS_SHADER);
            VHSShader2 = new Shader(ShaderType.Fragment, Assets.VHS_SHADER2);
            Global.theGame.Surface.AddShader(VHSShader);
            Global.theGame.Surface.AddShader(VHSShader2);

            // Create starfield.
            starFieldFar = new Image(Assets.GFX_STARFIELD);
            starFieldFar.Repeat = true;
            starFieldFar.Scroll = 0.3f;
            starFieldMid = new Image(Assets.GFX_STARFIELD);
            starFieldMid.Repeat = true;
            starFieldMid.Scroll = 0.6f;
            starFieldMid.Scale = 1.5f;
            starFieldClose = new Image(Assets.GFX_STARFIELD);
            starFieldClose.Repeat = true;
            starFieldClose.Scroll = 1.3f;
            starFieldClose.Scale = 3.0f;
            starFieldClose.Alpha = 0.5f;

            

            AddGraphic(starFieldFar);
            AddGraphic(starFieldMid);
            AddGraphic(starFieldClose);


            // Load map
            floorTiles = mapProject.CreateTilemap((TiledTileLayer)mapProject.Layers[0]);
            wallTiles = mapProject.CreateTilemap((TiledTileLayer)mapProject.Layers[1], mapProject.TileSets[1]);

            // Make sure walls have the correct tiles    
            wallCollision = mapProject.CreateGridCollider((TiledTileLayer)mapProject.Layers[1], 3);
            floorCollision = mapProject.CreateGridCollider((TiledTileLayer)mapProject.Layers[0], 8);

            // Move camera to start point
            TiledObjectGroup mapObjects = (TiledObjectGroup)mapProject.Layers[2];
            TiledObject strt = mapObjects.Objects[0];

            // Make machines




            CameraX = strt.X - 320;
            CameraY = strt.Y - 240;

            // Add player
            thePlayer = new Player(strt.X, strt.Y);
            thePlayer.Layer = 20;


            // Add station & player to scene
            
            AddGraphic(floorTiles);
            Add(thePlayer);

            // Make items / machines
            for (int i = 0; i < mapObjects.Objects.Count; i++)
            {
                TiledObject curObj = mapObjects.Objects[i];
                if (curObj.Name == "Wrench")
                {
                    Item newWrench = new Item(curObj.X, curObj.Y, 4);
                    Add(newWrench);
                }
                if (curObj.Name == "Crisps")
                {
                    Item newWrench = new Item(curObj.X, curObj.Y, 7);
                    Add(newWrench);
                }
                if (curObj.Name == "Donut")
                {
                    Item newWrench = new Item(curObj.X, curObj.Y, 6);
                    Add(newWrench);
                }
                if (curObj.Name == "VendingMachine")
                {
                    Machine newMachine = new Machine(curObj.X, curObj.Y, "Vending Machine", "Full of delicious snacks.", "Looks like it's busted.", "It's rebooting.", 300, Assets.GFX_MACHINETOP, Assets.GFX_VENDING);
                    Add(newMachine);
                }
                if (curObj.Name == "Computer")
                {
                    Machine newMachine = new Machine(curObj.X, curObj.Y, "Computer", "The communications terminal... me.", "ERROR: HHHH\nhelp\n \n{color:ff0000}ERRCODE#391293EFF3{color:ffffff}", "It was just a screensaver.\nYou idiot.", 600, Assets.GFX_PC_TOP, Assets.GFX_PC_FRONT);
                    Add(newMachine);
                }
                if (curObj.Type == "AirlockVert")
                {
                    Airlock newAirlock = new Airlock(curObj.X, curObj.Y, true);
                    Add(newAirlock);

                }
                if (curObj.Type == "AirlockHoriz")
                {
                    Airlock newAirlock = new Airlock(curObj.X, curObj.Y, false);
                    Add(newAirlock);

                }
            }

            AddGraphic(wallTiles);
            Add(thePlayer.crossHair);

            wallTest = new Entity(0, 0, null, wallCollision);
            Add(wallTest);
            floorTest = new Entity(0, 0, null, floorCollision);
            Add(floorTest);

            // Add hud
            msgBox = new Image(Assets.GFX_HUD);
            msgBox.Scroll = 0;
            msgBox.CenterOrigin();
            msgBox.X = 320;
            msgBox.Y = 255 + msgBox.Height + 16;
            Entity hud1 = new Entity();
        
            hud1.AddGraphic(msgBox);
            hud1.Layer = 10;
            Add(hud1);


            msgText = new RichText("", Assets.FONT_MSG, 8, 270, 50);
            msgText.DefaultShakeX = 0.4f;
            msgText.DefaultShakeY = 0.4f;
            msgText.X = 325 - msgBox.HalfWidth;
            msgText.Y = 255 + msgBox.Height - 8;
            msgText.Scroll = 0;
            msgText.String = "";
            Entity hud2 = new Entity();

            hud2.AddGraphic(msgText);
            hud2.Layer = 5;
            Add(hud2);
            

        }


        // Funcs
        public override void Update()
        {
            base.Update();

            starFieldFar.X -= 0.1f;
            starFieldMid.X -= 0.5f;
            starFieldClose.X -= 1.0f;

            // bounce zoom?
            CameraZoom = 2.0f + (((float)Math.Sin(Global.theGame.Timer * 0.01f) * 0.2f) * swayAmt);
            CameraAngle = 0.0f + (((float)Math.Sin(Global.theGame.Timer * 0.02f) * 4.0f) * swayAmt);

           // VHSShader.SetParameter("time", Global.theGame.Timer);
            VHSShader2.SetParameter("time", Global.theGame.Timer);
            
            if (Global.ResetBox)
            {
                Global.ResetBox = false;
                msgText.String = "";
                msgCurrentChar = 0;
                bip = 0;
            }
            
            if(msgCurrentChar < Global.MsgString.Length && bip == 0)
            {
                bip = 2;
                msgText.String += Global.MsgString[msgCurrentChar];
                msgCurrentChar++;
            }

            bip--;

            if(Global.MachineBroken)
            {
                Global.MachineBrokenTime++;
                if (Global.MachineBrokenTime > 60 * 60 * 3)
                {
                    if (!thePlayer.Dead)
                    {
                        thePlayer.Dead = true;
                        Global.NewWords("You neglected to fix the machines.\nThey were all vital to life support.\nEven the vending machine.\n [Press P to Restart.]");
                    }
                }
            }

            if(Input.KeyPressed(Key.P))
            {
                ResetGame();
            }

        }

        public void ResetGame()
        {
            Global.MachineBroken = false;
            Global.MachineBrokenTime = 0;
            Global.MsgString = "You wake up.\nPress W, S, A, D to move.\nInteract / Use Item with Left Mouse\nThrow Item with Right Mouse.\nHold Q to open item menu.\nRepair your machines!";
            Global.theGame.Surface.ClearShaders();
            Global.theGame.SwitchScene(new PlayState());
        }

        public override void UpdateLast()
        {
            base.UpdateLast();
            CenterCamera(thePlayer.X, thePlayer.Y);
        }

        public override void Render()
        {
            base.Render();
        }


    }
}
