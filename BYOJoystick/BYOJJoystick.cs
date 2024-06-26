using System;
using SharpDX.DirectInput;

namespace BYOJoystick
{
    public class BYOJJoystick : Joystick
    {
        private bool _isAcquired;

        public BYOJJoystick(DirectInput directInput, Guid deviceGuid) : base(directInput, deviceGuid)
        {
        }

        public BYOJJoystick(IntPtr nativePtr) : base(nativePtr)
        {
        }

        private new void Acquire()
        {
            if (_isAcquired)
                return;

            Properties.BufferSize = 128;
            base.Acquire();
            _isAcquired = true;
        }

        private new void Unacquire()
        {
            if (!_isAcquired)
                return;

            base.Unacquire();
            _isAcquired = false;
        }

        public new void Poll()
        {
            Acquire();
            base.Poll();
        }
    }
}