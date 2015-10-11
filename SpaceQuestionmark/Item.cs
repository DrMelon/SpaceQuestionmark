using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
namespace SpaceQuestionmark
{
    class Item : Entity
    {

        Image mySprite;
        Speed mySpeed;
        public bool InsideShip;
        public int itemType;

        public Item(float x, float y, int type)
        {
            X = x;
            Y = y;
            itemType = type;
            switch(type)
            {
                case 1:
                    mySprite = new Image(Assets.GFX_BOOTS);
                    break;
                case 2:
                    mySprite = new Image(Assets.GFX_CIRCUIT);
                    break;
                case 3:
                    mySprite = new Image(Assets.GFX_TILE);
                    break;
                case 4:
                    mySprite = new Image(Assets.GFX_WRENCH);
                    break;
                case 5:
                    mySprite = new Image(Assets.GFX_BATTERY);
                    break;
                case 6:
                    mySprite = new Image(Assets.GFX_DONUT);
                    break;
                case 7:
                    mySprite = new Image(Assets.GFX_CRISPS);
                    break;
                case 8:
                    mySprite = new Image(Assets.GFX_EXTINGUISHER);
                    break;
                case 9:
                    mySprite = new Image(Assets.GFX_O2TANK);
                    break;
                default:
                    return;
            }
            AddGraphic(mySprite);
            Graphic.CenterOrigin();
            mySpeed = new Speed(4);

            AddCollider(new CircleCollider(8, 1));
            Collider.CenterOrigin();
            Layer = 30;




        }

        public void Throw(Vector2 direction)
        {
            mySpeed.X = direction.X * 5.0f;
            mySpeed.Y = direction.Y * 5.0f;
        }

        public override void Update()
        {
            base.Update();

            InsideShip = Overlap(X, Y, 8);

            if (InsideShip)
            {
                mySpeed.X *= 0.92f;
            }

            if (InsideShip)
            {
                mySpeed.Y *= 0.92f;
            }

            if (Overlap(X + mySpeed.X, Y + mySpeed.Y, 3) || Overlap(X + mySpeed.X, Y + mySpeed.Y, 6))
            {
                //try just xspeed
                if (Overlap(X + mySpeed.X, Y, 3) || Overlap(X + mySpeed.X, Y, 6))
                {
                    mySpeed.X = -mySpeed.X;
                }
                if (Overlap(X, mySpeed.Y + Y, 3) || Overlap(X + mySpeed.X, Y, 6))
                {
                    mySpeed.Y = -mySpeed.Y;
                }

            }

            X += mySpeed.X;
            Y += mySpeed.Y;


        }
    }
}
