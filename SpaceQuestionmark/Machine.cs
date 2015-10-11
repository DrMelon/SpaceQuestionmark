using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
namespace SpaceQuestionmark
{
    class Machine : Entity
    {
        Spritemap<string> gfxTop;
        public Spritemap<string> gfxDetail;
        public string Name;
        public string Description;
        public string FlavBroke;
        public string FlavFixing;
        float FixTime;
        float BreakTime;
        float CurTime;
        public int Status;
        bool BeingInspected;
        Entity myFire = null;

        public Machine(float x, float y, string machineName, string description, string flav_broke, string flav_fixin, float fixtime, string asset_top, string asset_detail)
        {
            X = x;
            Y = y;
            gfxTop = new Spritemap<string>(asset_top, 64, 64);
            gfxDetail = new Spritemap<string>(asset_detail, 64, 64);
            gfxTop.Add("default", new Anim(new int[] { 0 }, new float[] { 1 }));
            gfxDetail.Add("normal", new Anim(new int[] { 0 }, new float[] { 1 }));
            gfxDetail.Add("broken", new Anim(new int[] { 1 }, new float[] { 1 }));
            gfxDetail.Add("fixing", new Anim(new int[] { 2 }, new float[] { 1 }));

            Name = machineName;
            Description = description;
            FlavBroke = flav_broke;
            FlavFixing = flav_fixin;
            BreakTime = Rand.Float(60 * 5, 60 * 60);
            FixTime = fixtime;
            AddGraphic(gfxTop);
            Graphic.CenterOrigin();
            Status = 1;
            CurTime = Global.theGame.Timer;
            Layer = 31;

            AddCollider(new BoxCollider(32, 32, 6));
            Collider.CenterOrigin();


        }

        

        public void BeginFix()
        {
            if(Status == 2)
            {
                Status = 3;
            }
            CurTime = Global.theGame.Timer;
            Global.MachineBroken = false;
            Global.MachineBrokenTime -= 60 * 60 * 0.2f;

        }

        public void Break()
        {
            Status = 2;
            Spritemap<string> newMap = new Spritemap<string>(Assets.GFX_FIRE, 64, 64);
            newMap.Add("fire", new Anim(new int[] { 0, 1, 2, 3, 4 }, new float[] { 3f }));
            newMap.Play("fire");
            myFire = new Entity(X, Y, newMap);
            myFire.Graphic.CenterOrigin();
            myFire.Graphic.Shake = 4;
            myFire.Layer = 20;
            this.Scene.Add(myFire);
        }

        public override void Update()
        {
            base.Update();
            if(Status == 1)
            {
                if(Global.theGame.Timer >= CurTime + BreakTime)
                {
                    Break();
                    CurTime = Global.theGame.Timer;
                    BreakTime = Rand.Float(60 * 5, 60 * 60);
                    if (Global.MachineBrokenTime < 60*60*3)
                    {
                        Global.NewWords("Warning: " + Name + " Broken!");
                        Global.MachineBroken = true;
                    }

                }
                

            }
            if(Status == 3)
            {
                if(Global.theGame.Timer >= CurTime + FixTime)
                {
                    Status = 1;

                }
                if (myFire != null)
                {
                    myFire.RemoveSelf();
                    myFire = null;
                }
            }
        }

    }
}
