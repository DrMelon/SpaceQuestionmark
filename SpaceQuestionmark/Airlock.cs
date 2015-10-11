using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace SpaceQuestionmark
{
    class Airlock : Entity
    {
        Spritemap<string> mySprite;
        public bool Vert = true;
        public bool Open = false;
        public bool Opening = false;
        public bool Closing = false;

        float howLongOpen = 0;

        public Airlock(float x, float y, bool vert = true)
        {
            Vert = vert;
            X = x;
            Y = y;
            if(Vert)
            {
                mySprite = new Spritemap<string>(Assets.GFX_AIRLOCKV, 32, 64);
                mySprite.Add("stayclosed", new Anim(new int[] { 0 }, new float[] { 2 }));
                mySprite.Add("open", new Anim(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new float[] { 2 }));
                mySprite.Add("stayopen", new Anim(new int[] { 9 }, new float[] { 2 }));
                mySprite.Add("close", new Anim(new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new float[] { 2 }));
                AddCollider(new BoxCollider(32, 64, 10));
                Graphic = mySprite;
                //Graphic.CenterOrigin();
                //Collider.CenterOrigin();
            }
            else
            {
                mySprite = new Spritemap<string>(Assets.GFX_AIRLOCKH, 64, 32);
                mySprite.Add("stayclosed", new Anim(new int[] { 0 }, new float[] { 2 }));
                mySprite.Add("open", new Anim(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new float[] { 2 }));
                mySprite.Add("stayopen", new Anim(new int[] { 9 }, new float[] { 2 }));
                mySprite.Add("close", new Anim(new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new float[] { 2 }));
                AddCollider(new BoxCollider(64, 32, 10));
                Graphic = mySprite;
                //Graphic.CenterOrigin();
                //Collider.CenterOrigin();
            }

            Layer = 22;
            
        }

        public void DoorOpen()
        {
            Opening = true;
            Closing = false;
        }

        public void DoorClose()
        {
            Closing = true;
            Opening = false;
        }

        public override void Update()
        {
            base.Update();

            if(Opening)
            {
                if(mySprite.CurrentAnim != "open")
                {
                    mySprite.Play("open");
                }
                if(mySprite.CurrentFrameIndex == mySprite.Anims["open"].FrameCount - 1)
                {
                    Opening = false;
                    Open = true;
                    howLongOpen = 0;
                }
            }
            else if (Open && !Closing)
            {
                if (mySprite.CurrentAnim != "stayopen")
                {
                    mySprite.Play("stayopen");
                }
                howLongOpen += 1;
                if(howLongOpen > 300)
                {
                    DoorClose();
                }
            }
            else if (!Open && !Closing)
            {
                if (mySprite.CurrentAnim != "stayclosed")
                {
                    mySprite.Play("stayclosed");
                }
            }
            if (Closing)
            {
                if (mySprite.CurrentAnim != "close")
                {
                    mySprite.Play("close");
                }
                if (mySprite.CurrentFrameIndex == mySprite.Anims["close"].FrameCount - 1)
                {
                    Closing = false;
                    Open = false;
                }
            }





        }
    }
}
