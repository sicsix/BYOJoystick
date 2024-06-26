using BYOJoystick.Bindings;
using VTOLVR.Multiplayer;

namespace BYOJoystick.Controls
{
    public class CRadio : IControl
    {
        protected readonly CockpitTeamRadioManager Radio;

        public CRadio(CockpitTeamRadioManager radio)
        {
            Radio = radio;
        }

        public void PostUpdate()
        {
        }

        public static void Transmit(CRadio c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.Radio.ptt?.StartVoice();
            else
                c.Radio.ptt?.StopVoice();
        }
    }
}