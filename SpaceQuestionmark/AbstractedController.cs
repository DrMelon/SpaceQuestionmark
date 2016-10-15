//
// @Author: J Brown (@DrMelon)
// 2016


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

// Abstracted Controller class - instead of treating it like a gamepad, we should use actions, as suggested at steam dev days 2016
// Input is basically the user trying to perform an action, so instead of asking which button was pressed/tapped/held/whatever,
// we should only query what they were trying to do

namespace SpaceQuestionmark
{
    public class AbstractedController : Controller
    {
        enum Actions
        {
            Movement,
            Looking,
            Sprint,
            ShowQuickUI,
            OpenMenu,
            OpenInventory,
            InteractLeftHand,
            InteractRightHand,
            GraspLeftHand,
            GraspRightHand,

            MenuUp,
            MenuDown,
            MenuLeft,
            MenuRight,
            MenuAccept,
            MenuCancel,

            PrimaryContext,
            SecondaryContext
            
            

        }

        public Button Sprint { get { return Button(Actions.Sprint); } }
        public Button ShowQuickUI { get { return Button(Actions.ShowQuickUI); } }
        public Button OpenMenu { get { return Button(Actions.OpenMenu); } }
        public Button OpenInventory { get { return Button(Actions.OpenInventory); } }
        public Button InteractLeftHand { get { return Button(Actions.InteractLeftHand); } }
        public Button InteractRightHand { get { return Button(Actions.InteractRightHand); } }
        public Button GraspLeftHand { get { return Button(Actions.GraspLeftHand); } }
        public Button GraspRightHand { get { return Button(Actions.GraspRightHand); } }

        public Button MenuUp { get { return Button(Actions.MenuUp); } }
        public Button MenuDown { get { return Button(Actions.MenuDown); } }
        public Button MenuLeft { get { return Button(Actions.MenuLeft); } }
        public Button MenuRight { get { return Button(Actions.MenuRight); } }
        public Button MenuAccept { get { return Button(Actions.MenuAccept); } }
        public Button MenuCancel { get { return Button(Actions.MenuCancel); } }

        public Button PrimaryContext { get { return Button(Actions.PrimaryContext); } }
        public Button SecondaryContext { get { return Button(Actions.SecondaryContext); } }

        public Axis Movement { get { return Axis(Actions.Movement); } }
        public Axis Looking { get { return Axis(Actions.Looking); } }

        public AbstractedController(bool doDefault, params int[] joystickId)
        {
            AddButton(Actions.Sprint);
            AddButton(Actions.ShowQuickUI);
            AddButton(Actions.OpenMenu);
            AddButton(Actions.OpenInventory);
            AddButton(Actions.InteractLeftHand);
            AddButton(Actions.InteractRightHand);
            AddButton(Actions.GraspLeftHand);
            AddButton(Actions.GraspRightHand);

            AddButton(Actions.MenuUp);
            AddButton(Actions.MenuDown);
            AddButton(Actions.MenuLeft);
            AddButton(Actions.MenuRight);
            AddButton(Actions.MenuAccept);
            AddButton(Actions.MenuCancel);

            AddButton(Actions.PrimaryContext);
            AddButton(Actions.SecondaryContext);

            AddAxis(Actions.Movement);
            AddAxis(Actions.Looking);

            if (doDefault)
            {
                foreach (var joy in joystickId)
                {
                    MenuUp.AddAxisButton(AxisButton.PovYMinus, joy).AddAxisButton(AxisButton.YMinus, joy);
                    MenuDown.AddAxisButton(AxisButton.PovYPlus, joy).AddAxisButton(AxisButton.YPlus, joy);
                    MenuLeft.AddAxisButton(AxisButton.PovXMinus, joy).AddAxisButton(AxisButton.XMinus, joy);
                    MenuRight.AddAxisButton(AxisButton.PovXPlus, joy).AddAxisButton(AxisButton.XPlus, joy);

                    MenuAccept.AddJoyButton(0, joy);
                    MenuCancel.AddJoyButton(1, joy);

                    Sprint.AddJoyButton(1, joy);
                    OpenMenu.AddJoyButton(7, joy);
                    OpenInventory.AddJoyButton(6, joy);
                    ShowQuickUI.AddJoyButton(3, joy);

                    InteractLeftHand.AddJoyButton(4, joy);
                    InteractRightHand.AddJoyButton(5, joy);

                    GraspLeftHand.AddAxisButton(AxisButton.ZPlus, joy);
                    GraspRightHand.AddAxisButton(AxisButton.ZMinus, joy);

                    Movement.AddJoyAxis(JoyAxis.X, JoyAxis.Y, joy);
                    Movement.AddJoyAxis(JoyAxis.PovX, JoyAxis.PovY, joy);
                    Looking.AddJoyAxis(JoyAxis.U, JoyAxis.R, joy);

                    PrimaryContext.AddJoyButton(8, joy);
                    SecondaryContext.AddJoyButton(9, joy);
                }
            }
        }
    }
}
