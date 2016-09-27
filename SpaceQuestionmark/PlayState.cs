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

        public PlayState()
        {
            
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
