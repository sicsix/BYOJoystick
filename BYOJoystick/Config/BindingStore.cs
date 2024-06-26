using System.Collections.Generic;
using BYOJoystick.Bindings;

namespace BYOJoystick.Config
{
    public class BindingStore<T> where T : Binding
    {
        private readonly Dictionary<string, List<T>> _bindings = new Dictionary<string, List<T>>();

        public void Add(string actionName, T binding)
        {
            if (!_bindings.TryGetValue(actionName, out var bindingList))
            {
                bindingList           = new List<T>();
                _bindings[actionName] = bindingList;
            }

            bindingList.Add(binding);
        }

        public void Remove(string actionName, T binding)
        {
            if (!_bindings.TryGetValue(actionName, out var bindingList))
                return;

            bindingList.Remove(binding);
        }

        public void Clear(string actionName)
        {
            if (!_bindings.TryGetValue(actionName, out var bindingList))
                return;

            bindingList.Clear();
        }

        public List<T> Get(string actionName)
        {
            if (!_bindings.TryGetValue(actionName, out var bindingList))
            {
                bindingList           = new List<T>();
                _bindings[actionName] = bindingList;
            }

            return bindingList;
        }

        public List<T> GetAllBindings()
        {
            var allBindings = new List<T>();
            foreach (var bindingList in _bindings.Values)
            {
                allBindings.AddRange(bindingList);
            }

            return allBindings;
        }

        public void ClearAll()
        {
            _bindings.Clear();
        }
    }
}