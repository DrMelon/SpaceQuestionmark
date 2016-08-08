using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace SpaceQuestionmark.UI
{
    class Textbox : Entity
    {
        // Textboxes can be a size! Wow!
        public float BoxWidth, BoxHeight;
        public int MaxTextLength;
        public bool TextGraduallyAppears;
        Font ChosenFont;
        RichText Text;

        public Textbox(float ScrX, float ScrY, float Width, float Height, int MaxLength, bool GradualAppear, Font NewFont)
        {
            X = ScrX;
            Y = ScrY;
            BoxWidth = Width;
            BoxHeight = Height;
            MaxTextLength = MaxLength;
            TextGraduallyAppears = GradualAppear;
            ChosenFont = NewFont;
            
            // Create Box Graphic at the right size :D
            Graphic = Image.CreateRectangle((int)Width, (int)Height, Color.Black);
            Graphic.Scroll = 0;

            // Create RichText 
            Text = new RichText("")
            
            // Add borders & corners
 
        }
    }
}
