using System;
using BYOJoystick.Bindings;
using BYOJoystick.Controls;

namespace BYOJoystick.Actions
{
    public class ControlAction
    {
        public string         Name     { get; }
        public ActionCategory Category { get; }
        public ActionInput    Input    { get; }

        private readonly string                                    _controlName;
        private          IControl                                  _control;
        private readonly Func<string, string, bool, int, IControl> _mapper;
        private readonly Action<IControl, Binding, int>            _action;
        private readonly int                                       _state;
        private readonly string                                    _root;
        private readonly bool                                      _nullable;
        private readonly int                                       _idx;


        public ControlAction(string                                    name,
                             ActionCategory                            category,
                             ActionInput                               input,
                             string                                    controlName,
                             Func<string, string, bool, int, IControl> mapper,
                             Action<IControl, Binding, int>            action,
                             int                                       state,
                             string                                    root,
                             bool                                      nullable,
                             int                                       idx)
        {
            Name         = name;
            Category     = category;
            Input        = input;
            _controlName = controlName;
            _mapper      = mapper;
            _action      = action;
            _state       = state;
            _root        = root;
            _nullable    = nullable;
            _idx         = idx;
        }

        public ControlAction(string name, ActionCategory category, ActionInput input)
        {
            Name     = name;
            Category = category;
            Input    = input;
        }

        public void Invoke(Binding binding)
        {
            _action(_control, binding, _state);
        }

        public void Map()
        {
            if (_mapper != null)
                _control = _mapper(_controlName, _root, _nullable, _idx);
        }

        public void Clear()
        {
            _control = null;
        }
    }
}