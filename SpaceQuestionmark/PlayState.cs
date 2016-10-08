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

        public Entities.Human thePlayer;
        Entity cameraFocus = new Entity();

        Spritemap<string> hpMeter = new Spritemap<string>(Assets.GFX_HP_METER, 32, 32);
        Spritemap<string> o2Meter = new Spritemap<string>(Assets.GFX_O2_METER, 32, 32);
        Entity hud = new Entity();

        // Starfield stuff
        Entity stars = new Entity();
        
        Image starFieldFar;
        Image starFieldMid;
        Image starFieldClose;

        float camShake = 0.0f;
        float camShakeMag = 0.0f;

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

            starFieldFar = new Image(Assets.GFX_STARFIELD);
            starFieldFar.Repeat = true;
            starFieldFar.Scroll = 0.1f;
            starFieldMid = new Image(Assets.GFX_STARFIELD);
            starFieldMid.Repeat = true;
            starFieldMid.Scroll = 0.6f;
            starFieldMid.Scale = 1.5f;
            starFieldClose = new Image(Assets.GFX_STARFIELD);
            starFieldClose.Repeat = true;
            starFieldClose.Scroll = 1.3f;
            starFieldClose.Scale = 3.0f;
            starFieldClose.Alpha = 0.5f;

            stars.AddGraphic(starFieldFar);
            stars.AddGraphic(starFieldMid);
            stars.AddGraphic(starFieldClose);
            stars.Layer = 100;
            Add(stars);

            Add(thePlayer);
            
            // Scene Stuff
            ApplyCamera = true;
            CameraFocus = cameraFocus;

            o2Meter.Scroll = 0;
            o2Meter.CenterOrigin();
            hpMeter.Scroll = 0;
            hpMeter.CenterOrigin();

            o2Meter.Add("idle", new Anim(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new float[] { 3f }));
            o2Meter.Add("warn", new Anim(new int[] { 13, 14, 15 }, new float[] { 6f }));
            hpMeter.Add("idle", new Anim(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new float[] { 3.4f }));
            hpMeter.Add("idle60", new Anim(new int[] { 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 }, new float[] { 3.0f }));
            hpMeter.Add("idle30", new Anim(new int[] { 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 }, new float[] { 2.0f }));
            hpMeter.Add("dead", new Anim(new int[] { 39, 40, 41, 40 }, new float[] { 6f }));

            o2Meter.Play("idle");
            hpMeter.Play("idle");

            o2Meter.X = -32 + Game.Instance.HalfWidth;
            o2Meter.Y = -32 + Game.Instance.HalfHeight;
            hpMeter.X = 32 + Game.Instance.HalfWidth;
            hpMeter.Y = -32 + Game.Instance.HalfHeight;
            o2Meter.Alpha = 0.0f;
            hpMeter.Alpha = 0.0f;

            hud.AddGraphic(o2Meter);
            hud.AddGraphic(hpMeter);
            hud.Layer = -99;
            hud.X = 0;
            hud.Y = 0;
            
            Add(hud);

        }

        public void UpdateAtmosphericsInteractions()
        {
            MoveItemsWithGasFlow();
            MovePlayerWithGasFlow();
            ProcessLivingWithAtmos();
            
        }

        public void MovePlayerWithGasFlow()
        {
            if(Global.controllerPlayerOne.LB.Down)
            {
                return;
            }
            // Push player along with gases
            Systems.Atmospherics.GasMixture gmAtPlayer = map.myGasManager.GetMixtureAt((int)(thePlayer.X) / 64, (int)(thePlayer.Y) / 64);
            if (gmAtPlayer != null)
            {
                Vector2 gasVelocityMix = new Vector2(Otter.MathHelper.Clamp(gmAtPlayer.GasMotion.X * 1.0f, -15, 15), Otter.MathHelper.Clamp(gmAtPlayer.GasMotion.Y * 1.0f, -15, 15));

                if (gmAtPlayer.NorthNeighbour != null)
                {
                    gasVelocityMix += new Vector2(Otter.MathHelper.Clamp(gmAtPlayer.NorthNeighbour.GasMotion.X * 1.0f, -15, 15), Otter.MathHelper.Clamp(gmAtPlayer.NorthNeighbour.GasMotion.Y * 1.0f, -15, 15));
                }

                if (gmAtPlayer.EastNeighbour != null)
                {
                    gasVelocityMix += new Vector2(Otter.MathHelper.Clamp(gmAtPlayer.EastNeighbour.GasMotion.X * 1.0f, -15, 15), Otter.MathHelper.Clamp(gmAtPlayer.EastNeighbour.GasMotion.Y * 1.0f, -15, 15));
                }

                if (gmAtPlayer.WestNeighbour != null)
                {
                    gasVelocityMix += new Vector2(Otter.MathHelper.Clamp(gmAtPlayer.WestNeighbour.GasMotion.X * 1.0f, -15, 15), Otter.MathHelper.Clamp(gmAtPlayer.WestNeighbour.GasMotion.Y * 1.0f, -15, 15));
                }

                if (gmAtPlayer.SouthNeighbour != null)
                {
                    gasVelocityMix += new Vector2(Otter.MathHelper.Clamp(gmAtPlayer.SouthNeighbour.GasMotion.X * 1.0f, -15, 15), Otter.MathHelper.Clamp(gmAtPlayer.SouthNeighbour.GasMotion.Y * 1.0f, -15, 15));
                }

                thePlayer.GetComponent<Components.MobMovement>().myVelocity.X += gasVelocityMix.X * 2.5f;
                thePlayer.GetComponent<Components.MobMovement>().myVelocity.Y += gasVelocityMix.Y * 2.5f;

            }
        }

        public void MoveItemsWithGasFlow()
        {

        }

        public void ProcessLivingWithAtmos()
        {
            ProcessPlayerWithAtmos();
        }

        public void ProcessPlayerWithAtmos()
        {
            Systems.Atmospherics.GasMixture gmAtPlayer = map.myGasManager.GetMixtureAt((int)(thePlayer.X) / 64, (int)(thePlayer.Y) / 64);
            thePlayer.LungMixture = gmAtPlayer;
        }


        // Funcs
        public override void Update()
        {
            base.Update();


            UpdateAtmosphericsInteractions();
           
           

            if (Global.controllerPlayerOne.Y.Down && o2Meter.Alpha < 1.0f)
            {
                o2Meter.Alpha += Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.UITHINK) * 5.0f;
                hpMeter.Alpha += Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.UITHINK) * 5.0f;
            }
            else if (!Global.controllerPlayerOne.Y.Down && o2Meter.Alpha > 0.0f)
            {
                o2Meter.Alpha -= Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.UITHINK) * 5.0f;
                hpMeter.Alpha -= Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.UITHINK) * 5.0f;
            }

            if(!thePlayer.CanBreathe())
            {
                o2Meter.Play("warn", false);
            }
            else
            {
                o2Meter.Play("idle", false);
            }

            if(Global.controllerPlayerOne.Back.Pressed)
            {
                //thePlayer.Lungs.Hurt(90);
                ShakeCamera(1.0f, 50.0f);
                map.WallClearTile(map.playerStartX + 2, map.playerStartY);
                map.NeedUpdateSpace = true;
            }

            if (Global.controllerPlayerOne.Start.Pressed)
            {
                //thePlayer.Lungs.Hurt(90);
                ShakeCamera(1.0f, 50.0f);
                map.WallFillTile(map.playerStartX + 2, map.playerStartY, 1);
                map.NeedUpdateSpace = true;
            }


            if (thePlayer.myBody.currentVitality > 60)
            {
                hpMeter.Play("idle", false);
            }
            else if(thePlayer.myBody.currentVitality > 30)
            {
                hpMeter.Play("idle60", false);
            }
            else if(thePlayer.myBody.currentVitality > 0)
            {
                hpMeter.Play("idle30", false);
            }

            if(thePlayer.myBody.dead)
            {
                hpMeter.Play("dead", false);
            }

        }





        public void ResetGame()
        {

            
        }

        public override void UpdateLast()
        {
            base.UpdateLast();

            cameraFocus.X = Util.Lerp(thePlayer.X, thePlayer.X, 0.5f);
            cameraFocus.Y = Util.Lerp(thePlayer.Y, thePlayer.Y, 0.5f);
            
            if(camShake > 0.0f)
            {
                camShake -= Systems.Time.GetDeltaTime(Systems.Time.TimeGroup.WORLDTHINK);

                camShakeMag = Util.Lerp(camShakeMag, 0.0f, camShakeMag * 0.01f);

                cameraFocus.X += Rand.Float(-camShakeMag, camShakeMag);
                cameraFocus.Y += Rand.Float(-camShakeMag, camShakeMag);
            }

            //CameraZoom = 1.5f + (float)Math.Sin(Game.Instance.Timer * 0.001f);
            //CameraZoom = 2.0f;
            //CameraAngle = (float)Math.Sin(Game.Instance.Timer * 0.002f);
            if(Input.KeyPressed(Key.PageUp))
            {
                Global.debugMode = true;
            }
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

        public void ShakeCamera(float shk, float mag)
        {
            camShake = shk;
            camShakeMag = mag;
        }

         

    }
}
