using BYOJoystick.Bindings;

namespace BYOJoystick.Controls
{
    public class CEject : IControl
    {
        protected readonly EjectHandle EjectHandle;

        public CEject(EjectHandle ejectHandle)
        {
            EjectHandle = ejectHandle;
        }

        public void PostUpdate()
        {
        }

        public static void Pull(CEject c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.EjectHandle.OnHandlePull?.Invoke();
        }
    }
}