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

        Entities.Human thePlayer = new Entities.Human();
        

        public PlayState()
        {
            thePlayer.AddComponent(new Components.MobMovement());
            thePlayer.GetComponent<Components.MobMovement>().myController = Global.controllerPlayerOne;
            Add(thePlayer);
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

        }

        public override void Render()
        {
            base.Render();
        }


    }
}
