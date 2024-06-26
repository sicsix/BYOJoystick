using BYOJoystick.Bindings;
using BYOJoystick.Controls.Converters;
using UnityEngine;

namespace BYOJoystick.Controls
{
    public class CSOI : IControl
    {
        protected readonly MFDManager                   MFDManager;
        protected readonly MFDPortalManager[]           MFDPortalManagers;
        protected readonly TargetingMFDPage             TGPPage;
        protected readonly MFDRadarUI                   RadarPage;
        protected readonly DashMapDisplay               MapPage;
        protected readonly MFDPTacticalSituationDisplay TSDPage;
        protected readonly ThrottleSOISwitcher          SOISwitcher;
        protected readonly MultiPortalSOISwitcher       PortalSOISwitcher;
        protected readonly bool                         HasPortalManager;
        protected readonly bool                         HasTSDPage;
        protected readonly bool                         HasRadarPage;
        protected readonly bool                         HasMultiPortalSOISwitcher;

        protected Vector3 SlewVector;
        protected Vector3 DigitalSlewVector;
        protected bool    SlewWasZero;
        protected bool    SlewButtonPressed;
        protected float   RadarElevationChange;
        protected bool    RadarElevationChangeWasZero;

        protected readonly DigitalToAxisSmoothed SlewXSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 1f);
        protected readonly DigitalToAxisSmoothed SlewYSmoothed = new DigitalToAxisSmoothed(0.25f, 1f, 1f);

        public CSOI(MFDManager                   mfdManager,
                    MFDPortalManager[]           mfdPortalManagers,
                    TargetingMFDPage             tgpPage,
                    MFDRadarUI                   radarPage,
                    DashMapDisplay               mapPage,
                    MFDPTacticalSituationDisplay tsdPage,
                    ThrottleSOISwitcher          soiSwitcher,
                    MultiPortalSOISwitcher       portalSOISwitcher)
        {
            MFDManager                = mfdManager;
            MFDPortalManagers         = mfdPortalManagers;
            TGPPage                   = tgpPage;
            RadarPage                 = radarPage;
            MapPage                   = mapPage;
            TSDPage                   = tsdPage;
            SOISwitcher               = soiSwitcher;
            PortalSOISwitcher         = portalSOISwitcher;
            HasPortalManager          = mfdPortalManagers.Length > 0;
            HasTSDPage                = tsdPage                  != null;
            HasRadarPage              = radarPage                != null;
            HasMultiPortalSOISwitcher = portalSOISwitcher        != null;
        }

        public void PostUpdate()
        {
            if (HasRadarPage && RadarElevationChange != 0 && RadarPage.isSOI)
            {
                RadarPage.radarCtrlr.OnElevationInput(RadarElevationChange);
                RadarElevationChangeWasZero = false;
            }
            else if (HasRadarPage && !RadarElevationChangeWasZero)
            {
                RadarPage.radarCtrlr.OnElevationInput(0);
                RadarElevationChangeWasZero = true;
            }

            if (SlewXSmoothed.Calculate())
                DigitalSlewVector.x = SlewXSmoothed.Value;

            if (SlewYSmoothed.Calculate())
                DigitalSlewVector.y = SlewYSmoothed.Value;

            if (SlewVector.magnitude < 0.03f)
                SlewVector = Vector3.zero;

            if (DigitalSlewVector != Vector3.zero)
            {
                SlewWasZero = false;
                if (HasPortalManager)
                {
                    for (int i = 0; i < MFDPortalManagers.Length; i++)
                    {
                        MFDPortalManagers[i].OnInputAxis(DigitalSlewVector);
                    }
                }
                else
                    MFDManager.OnInputAxis(DigitalSlewVector);
            }
            else if (SlewVector != Vector3.zero)
            {
                SlewWasZero = false;
                if (HasPortalManager)
                {
                    for (int i = 0; i < MFDPortalManagers.Length; i++)
                    {
                        MFDPortalManagers[i].OnInputAxis(SlewVector);
                    }
                }
                else
                    MFDManager.OnInputAxis(SlewVector);
            }
            else if (!SlewWasZero)
            {
                SlewWasZero = true;
                if (HasPortalManager)
                {
                    for (int i = 0; i < MFDPortalManagers.Length; i++)
                    {
                        MFDPortalManagers[i].OnInputAxis(Vector3.zero);
                        MFDPortalManagers[i].OnInputAxisReleased();
                    }
                }
                else
                {
                    MFDManager.OnInputAxis(Vector3.zero);
                    MFDManager.OnInputAxisReleased();
                }
            }

            DigitalSlewVector = Vector3.zero;
        }

        public void DoZoom(bool zoomIn)
        {
            if (TGPPage.isSOI)
                DoTGPZoom(zoomIn);
            else if (HasRadarPage && RadarPage.isSOI)
                DoRadarZoom(zoomIn);
            else if ((!HasPortalManager && MapPage.mfdPage.isSOI) || MapPage.portalPage.isSOI)
                DoMapZoom(zoomIn);
            else if (HasTSDPage && TSDPage.isSOI)
                DoTSDZoom(zoomIn);
        }

        private void DoMapZoom(bool zoomIn)
        {
            if (zoomIn)
                MapPage.ZoomIn();
            else
                MapPage.ZoomOut();
        }

        private void DoTGPZoom(bool zoomIn)
        {
            if (zoomIn)
                TGPPage.ZoomIn();
            else
                TGPPage.ZoomOut();
        }

        private void DoRadarZoom(bool zoomIn)
        {
            if (zoomIn)
                RadarPage.RangeUp();
            else
                RadarPage.RangeDown();
        }

        private void DoTSDZoom(bool zoomIn)
        {
            if (zoomIn)
                TSDPage.PrevViewScale();
            else
                TSDPage.NextViewScale();
        }

        public static void SlewButton(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
            {
                if (c.HasPortalManager)
                {
                    for (int i = 0; i < c.MFDPortalManagers.Length; i++)
                    {
                        c.MFDPortalManagers[i].OnInputButton();
                    }
                }
                else
                    c.MFDManager.OnInputButton();
            }

            if (!c.SlewButtonPressed && binding.GetAsBool())
            {
                c.SlewButtonPressed = true;
                if (c.HasPortalManager)
                {
                    for (int i = 0; i < c.MFDPortalManagers.Length; i++)
                    {
                        c.MFDPortalManagers[i].OnInputButtonDown();
                    }
                }
                else
                    c.MFDManager.OnInputButtonDown();
            }

            if (c.SlewButtonPressed && !binding.GetAsBool())
            {
                c.SlewButtonPressed = false;
                if (c.HasPortalManager)
                {
                    for (int i = 0; i < c.MFDPortalManagers.Length; i++)
                    {
                        c.MFDPortalManagers[i].OnInputButtonUp();
                    }
                }
                else
                    c.MFDManager.OnInputButtonUp();
            }
        }

        public static void SlewX(CSOI c, Binding binding, int state)
        {
            c.SlewVector.x = binding.GetAsFloatCentered();
        }

        public static void SlewY(CSOI c, Binding binding, int state)
        {
            c.SlewVector.y = binding.GetAsFloatCentered();
        }

        public static void SlewUp(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SlewYSmoothed.IncreaseButtonDown();
            else
                c.SlewYSmoothed.IncreaseButtonUp();
        }

        public static void SlewDown(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SlewYSmoothed.DecreaseButtonDown();
            else
                c.SlewYSmoothed.DecreaseButtonUp();
        }

        public static void SlewLeft(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SlewXSmoothed.DecreaseButtonDown();
            else
                c.SlewXSmoothed.DecreaseButtonUp();
        }

        public static void SlewRight(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.SlewXSmoothed.IncreaseButtonDown();
            else
                c.SlewXSmoothed.IncreaseButtonUp();
        }

        public static void Next(CSOI c, Binding binding, int state)
        {
            if (c.HasMultiPortalSOISwitcher)
            {
                if (binding.GetAsBool())
                    c.PortalSOISwitcher.OnSetThumbstick(Vector3.right);
                else
                    c.PortalSOISwitcher.OnSetThumbstick(Vector3.zero);
            }
            else
            {
                if (binding.GetAsBool())
                    c.SOISwitcher.OnSetThumbstick(Vector3.right);
                else
                    c.SOISwitcher.OnSetThumbstick(Vector3.zero);
            }
        }

        public static void Prev(CSOI c, Binding binding, int state)
        {
            if (c.HasMultiPortalSOISwitcher)
            {
                if (binding.GetAsBool())
                    c.PortalSOISwitcher.OnSetThumbstick(Vector3.left);
                else
                    c.PortalSOISwitcher.OnSetThumbstick(Vector3.zero);
            }
            else
            {
                if (binding.GetAsBool())
                    c.SOISwitcher.OnSetThumbstick(Vector3.left);
                else
                    c.SOISwitcher.OnSetThumbstick(Vector3.zero);
            }
        }

        public static void ZoomIn(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.DoZoom(true);
        }

        public static void ZoomOut(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.DoZoom(false);
        }

        public static void RadarElevUp(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.RadarElevationChange = 1f;
            else
                c.RadarElevationChange = 0f;
        }

        public static void RadarElevDown(CSOI c, Binding binding, int state)
        {
            if (binding.GetAsBool())
                c.RadarElevationChange = -1f;
            else
                c.RadarElevationChange = 0f;
        }
    }
}