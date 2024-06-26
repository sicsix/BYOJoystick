using System;
using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CHelmet : IControl
    {
        protected readonly HelmetController               HelmetController;
        protected readonly Action<HelmetController, bool> SetVisorDown;
        protected readonly Action<HelmetController, bool> SetNVGEnabled;

        public CHelmet(HelmetController helmetController)
        {
            HelmetController = helmetController;
            SetVisorDown     = CompiledExpressions.CreateFieldSetter<HelmetController, bool>("visorDown");
            SetNVGEnabled    = CompiledExpressions.CreateFieldSetter<HelmetController, bool>("nvgEnabled");
        }

        public void PostUpdate()
        {
        }

        public static void ToggleVisor(CHelmet c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.HelmetController.ToggleVisor();
        }

        public static void OpenVisor(CHelmet c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.SetVisorDown(c.HelmetController, true);
                c.HelmetController.ToggleVisor();
            }
        }

        public static void CloseVisor(CHelmet c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.SetVisorDown(c.HelmetController, false);
                c.HelmetController.ToggleVisor();
            }
        }

        public static void ToggleNightVision(CHelmet c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.HelmetController.ToggleNVG();
        }

        public static void EnableNightVision(CHelmet c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.SetNVGEnabled(c.HelmetController, false);
                c.HelmetController.ToggleNVG();
            }
        }

        public static void DisableNightVision(CHelmet c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.SetNVGEnabled(c.HelmetController, true);
                c.HelmetController.ToggleNVG();
            }
        }
    }
}