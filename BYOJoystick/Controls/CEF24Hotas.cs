using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;
using VTOLVR.DLC.EW;

namespace BYOJoystick.Controls
{
    public class CEF24Hotas : IControl
    {
        protected readonly EF24Hotas EF24Hotas;
        protected          bool      PowerWasZero;

        protected readonly DigitalToAxisSmoothed PowerSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 1f);


        public CEF24Hotas(EF24Hotas ef24Hotas)
        {
            EF24Hotas = ef24Hotas;
        }

        public void PostUpdate()
        {
            if (PowerSmoothed.Calculate())
            {
                PowerWasZero = false;
                EF24Hotas.ead.HOTASPowerAdjust(PowerSmoothed.Value);
            }
            else if (!PowerWasZero)
            {
                PowerWasZero = true;
                EF24Hotas.ead.HOTASPowerAdjust(0);
            }
        }

        public static void CycleEMBand(CEF24Hotas c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.EF24Hotas.ead.MFD_CycleEMBand();
                c.EF24Hotas.inputAudioSource.PlayOneShot(c.EF24Hotas.emBandSound);
            }
        }

        public static void CycleTxMode(CEF24Hotas c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                c.EF24Hotas.ead.MFD_TransmitMode();
                c.EF24Hotas.inputAudioSource.PlayOneShot(c.EF24Hotas.xmtrModeSound);
            }
        }

        public static void NextTx(CEF24Hotas c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.EF24Hotas.ewarCon.CycleAllTransmitters();
        }

        public static void PrevTx(CEF24Hotas c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.EF24Hotas.ewarCon.CycleAllTransmittersBack();
        }

        public static void IncreaseTxPower(CEF24Hotas c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.PowerSmoothed.IncreaseButtonDown();
            else
                c.PowerSmoothed.IncreaseButtonUp();
        }

        public static void DecreaseTxPower(CEF24Hotas c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.PowerSmoothed.DecreaseButtonDown();
            else
                c.PowerSmoothed.DecreaseButtonUp();
        }
    }
}