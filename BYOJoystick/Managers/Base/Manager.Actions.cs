using System;
using BYOJoystick.Actions;
using BYOJoystick.Bindings;
using BYOJoystick.Controls;

namespace BYOJoystick.Managers.Base
{
    public abstract partial class Manager
    {
        private void AddAction<T>(string                             name,
                                  ActionCategory                     category,
                                  ActionInput                        input,
                                  string                             controlName,
                                  Func<string, string, bool, int, T> mapper,
                                  Action<T, Binding, int>            action,
                                  int                                state,
                                  string                             root,
                                  bool                               nullable,
                                  int                                idx) where T : IControl
        {
            var controlAction = mapper == null
                                    ? new ControlAction(name, category, input, controlName, null, Action, state, root, nullable, idx)
                                    : new ControlAction(name, category, input, controlName, Mapper, Action, state, root, nullable, idx);
            ControlActions.Add(name, controlAction);

            if (!ControlActionsByCategory.TryGetValue(category, out var categoryControlActions))
                throw new InvalidOperationException($"Category {category} not found.");
            categoryControlActions.Add(controlAction);
            return;

            IControl Mapper(string mName, string mRoot, bool mNullable, int mIdx)
            {
                return mapper(mName, mRoot, mNullable, mIdx);
            }

            void Action(IControl aC, Binding aBinding, int aState)
            {
                if (aC == null)
                {
                    Plugin.Log($"Control for {aBinding.Target} not found.");
                    return;
                }

                action((T)aC, aBinding, aState);
            }
        }

        protected void StaticButton(string name, ActionCategory category, Action<IControl, Binding, int> action)
        {
            AddAction(name, category, ActionInput.Button, name, (n, c, f, i) => new CNone(), action, -1, null, false, -1);
        }

        private void ModifierButton()
        {
            var controlAction = new ControlAction("Modifier", ActionCategory.Modifier, ActionInput.Button);
            ControlActions.Add("Modifier", controlAction);
            ControlActionsByCategory[ActionCategory.Modifier].Add(controlAction);
        }

        protected void FlightAxis<T>(string                             name,
                                     string                             controlName,
                                     Func<string, string, bool, int, T> mapper,
                                     Action<T, Binding, int>            action,
                                     int                                s = -1,
                                     bool                               n = false,
                                     string                             r = null,
                                     int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Flight, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void FlightAxisC<T>(string                             name,
                                      string                             controlName,
                                      Func<string, string, bool, int, T> mapper,
                                      Action<T, Binding, int>            action,
                                      int                                s = -1,
                                      bool                               n = false,
                                      string                             r = null,
                                      int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Flight, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void FlightButton<T>(string                             name,
                                       string                             controlName,
                                       Func<string, string, bool, int, T> mapper,
                                       Action<T, Binding, int>            action,
                                       int                                s = -1,
                                       bool                               n = false,
                                       string                             r = null,
                                       int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Flight, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void AssistAxis<T>(string                             name,
                                     string                             controlName,
                                     Func<string, string, bool, int, T> mapper,
                                     Action<T, Binding, int>            action,
                                     int                                s = -1,
                                     bool                               n = false,
                                     string                             r = null,
                                     int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.FlightAssist, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void AssistAxisC<T>(string                             name,
                                      string                             controlName,
                                      Func<string, string, bool, int, T> mapper,
                                      Action<T, Binding, int>            action,
                                      int                                s = -1,
                                      bool                               n = false,
                                      string                             r = null,
                                      int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.FlightAssist, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void AssistButton<T>(string                             name,
                                       string                             controlName,
                                       Func<string, string, bool, int, T> mapper,
                                       Action<T, Binding, int>            action,
                                       int                                s = -1,
                                       bool                               n = false,
                                       string                             r = null,
                                       int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.FlightAssist, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void NavAxis<T>(string                             name,
                                  string                             controlName,
                                  Func<string, string, bool, int, T> mapper,
                                  Action<T, Binding, int>            action,
                                  int                                s = -1,
                                  bool                               n = false,
                                  string                             r = null,
                                  int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Navigation, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void NavAxisC<T>(string                             name,
                                   string                             controlName,
                                   Func<string, string, bool, int, T> mapper,
                                   Action<T, Binding, int>            action,
                                   int                                s = -1,
                                   bool                               n = false,
                                   string                             r = null,
                                   int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Navigation, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void NavButton<T>(string                             name,
                                    string                             controlName,
                                    Func<string, string, bool, int, T> mapper,
                                    Action<T, Binding, int>            action,
                                    int                                s = -1,
                                    bool                               n = false,
                                    string                             r = null,
                                    int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Navigation, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void SystemsAxis<T>(string                             name,
                                      string                             controlName,
                                      Func<string, string, bool, int, T> mapper,
                                      Action<T, Binding, int>            action,
                                      int                                s = -1,
                                      bool                               n = false,
                                      string                             r = null,
                                      int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Systems, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void SystemsAxisC<T>(string                             name,
                                       string                             controlName,
                                       Func<string, string, bool, int, T> mapper,
                                       Action<T, Binding, int>            action,
                                       int                                s = -1,
                                       bool                               n = false,
                                       string                             r = null,
                                       int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Systems, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void SystemsButton<T>(string                             name,
                                        string                             controlName,
                                        Func<string, string, bool, int, T> mapper,
                                        Action<T, Binding, int>            action,
                                        int                                s = -1,
                                        bool                               n = false,
                                        string                             r = null,
                                        int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Systems, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void HUDAxis<T>(string                             name,
                                  string                             controlName,
                                  Func<string, string, bool, int, T> mapper,
                                  Action<T, Binding, int>            action,
                                  int                                s = -1,
                                  bool                               n = false,
                                  string                             r = null,
                                  int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.HUD, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void HUDAxisC<T>(string                             name,
                                   string                             controlName,
                                   Func<string, string, bool, int, T> mapper,
                                   Action<T, Binding, int>            action,
                                   int                                s = -1,
                                   bool                               n = false,
                                   string                             r = null,
                                   int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.HUD, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void HUDButton<T>(string                             name,
                                    string                             controlName,
                                    Func<string, string, bool, int, T> mapper,
                                    Action<T, Binding, int>            action,
                                    int                                s = -1,
                                    bool                               n = false,
                                    string                             r = null,
                                    int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.HUD, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void DisplayAxis<T>(string                             name,
                                      string                             controlName,
                                      Func<string, string, bool, int, T> mapper,
                                      Action<T, Binding, int>            action,
                                      int                                s = -1,
                                      bool                               n = false,
                                      string                             r = null,
                                      int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Displays, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void DisplayAxisC<T>(string                             name,
                                       string                             controlName,
                                       Func<string, string, bool, int, T> mapper,
                                       Action<T, Binding, int>            action,
                                       int                                s = -1,
                                       bool                               n = false,
                                       string                             r = null,
                                       int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Displays, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void DisplayButton<T>(string                             name,
                                        string                             controlName,
                                        Func<string, string, bool, int, T> mapper,
                                        Action<T, Binding, int>            action,
                                        int                                s = -1,
                                        bool                               n = false,
                                        string                             r = null,
                                        int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Displays, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void RadioAxis<T>(string                             name,
                                    string                             controlName,
                                    Func<string, string, bool, int, T> mapper,
                                    Action<T, Binding, int>            action,
                                    int                                s = -1,
                                    bool                               n = false,
                                    string                             r = null,
                                    int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Radio, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void RadioAxisC<T>(string                             name,
                                     string                             controlName,
                                     Func<string, string, bool, int, T> mapper,
                                     Action<T, Binding, int>            action,
                                     int                                s = -1,
                                     bool                               n = false,
                                     string                             r = null,
                                     int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Radio, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void RadioButton<T>(string                             name,
                                      string                             controlName,
                                      Func<string, string, bool, int, T> mapper,
                                      Action<T, Binding, int>            action,
                                      int                                s = -1,
                                      bool                               n = false,
                                      string                             r = null,
                                      int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Radio, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void MusicAxis<T>(string                             name,
                                    string                             controlName,
                                    Func<string, string, bool, int, T> mapper,
                                    Action<T, Binding, int>            action,
                                    int                                s = -1,
                                    bool                               n = false,
                                    string                             r = null,
                                    int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Music, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void MusicAxisC<T>(string                             name,
                                     string                             controlName,
                                     Func<string, string, bool, int, T> mapper,
                                     Action<T, Binding, int>            action,
                                     int                                s = -1,
                                     bool                               n = false,
                                     string                             r = null,
                                     int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Music, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void MusicButton<T>(string                             name,
                                      string                             controlName,
                                      Func<string, string, bool, int, T> mapper,
                                      Action<T, Binding, int>            action,
                                      int                                s = -1,
                                      bool                               n = false,
                                      string                             r = null,
                                      int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Music, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void LightsAxis<T>(string                             name,
                                     string                             controlName,
                                     Func<string, string, bool, int, T> mapper,
                                     Action<T, Binding, int>            action,
                                     int                                s = -1,
                                     bool                               n = false,
                                     string                             r = null,
                                     int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Lights, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void LightsAxisC<T>(string                             name,
                                      string                             controlName,
                                      Func<string, string, bool, int, T> mapper,
                                      Action<T, Binding, int>            action,
                                      int                                s = -1,
                                      bool                               n = false,
                                      string                             r = null,
                                      int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Lights, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void LightsButton<T>(string                             name,
                                       string                             controlName,
                                       Func<string, string, bool, int, T> mapper,
                                       Action<T, Binding, int>            action,
                                       int                                s = -1,
                                       bool                               n = false,
                                       string                             r = null,
                                       int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Lights, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }

        protected void MiscAxis<T>(string                             name,
                                   string                             controlName,
                                   Func<string, string, bool, int, T> mapper,
                                   Action<T, Binding, int>            action,
                                   int                                s = -1,
                                   bool                               n = false,
                                   string                             r = null,
                                   int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Misc, ActionInput.Axis, controlName, mapper, action, s, r, n, i);
        }

        protected void MiscAxisC<T>(string                             name,
                                    string                             controlName,
                                    Func<string, string, bool, int, T> mapper,
                                    Action<T, Binding, int>            action,
                                    int                                s = -1,
                                    bool                               n = false,
                                    string                             r = null,
                                    int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Misc, ActionInput.AxisCentered, controlName, mapper, action, s, r, n, i);
        }

        protected void MiscButton<T>(string                             name,
                                     string                             controlName,
                                     Func<string, string, bool, int, T> mapper,
                                     Action<T, Binding, int>            action,
                                     int                                s = -1,
                                     bool                               n = false,
                                     string                             r = null,
                                     int                                i = -1) where T : IControl
        {
            AddAction(name, ActionCategory.Misc, ActionInput.Button, controlName, mapper, action, s, r, n, i);
        }
    }
}