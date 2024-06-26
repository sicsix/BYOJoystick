using UnityEngine;

namespace BYOJoystick.Controls.Converters
{
    public class DigitalToAxisSmoothed
    {
        public float Value { get; private set; }
        public float Delta { get; private set; }

        private readonly float _rate;
        private readonly float _maxRate;
        private readonly float _timeToMax;
        private          bool  _increasing;
        private          bool  _decreasing;
        private          float _timeIncreaseHeld;
        private          float _timeDecreaseHeld;

        public DigitalToAxisSmoothed(float rate)
        {
            _rate      = rate;
            _maxRate   = 0;
            _timeToMax = 0;
        }

        public DigitalToAxisSmoothed(float rate, float maxRate, float timeToMax)
        {
            _rate      = rate;
            _maxRate   = maxRate;
            _timeToMax = timeToMax;
        }

        public bool Calculate()
        {
            Value = 0;
            Delta = 0;

            if (_increasing)
            {
                _timeIncreaseHeld += Time.deltaTime;
                if (_timeToMax <= 0)
                {
                    Value += _rate;
                    Delta += _rate * Time.deltaTime;
                }
                else
                {
                    float value = Mathf.Lerp(_rate, _maxRate, Mathf.Clamp01(_timeIncreaseHeld / _timeToMax));
                    Value += value;
                    Delta += value * Time.deltaTime;
                }
            }

            if (_decreasing)
            {
                _timeDecreaseHeld += Time.deltaTime;
                if (_timeToMax <= 0)
                {
                    Value -= _rate;
                    Delta -= _rate * Time.deltaTime;
                }
                else
                {
                    float value = Mathf.Lerp(_rate, _maxRate, Mathf.Clamp01(_timeDecreaseHeld / _timeToMax));
                    Value -= value;
                    Delta -= value * Time.deltaTime;
                }
            }

            return _increasing || _decreasing;
        }

        public void IncreaseButtonDown()
        {
            _increasing       = true;
            _timeIncreaseHeld = 0;
        }

        public void IncreaseButtonUp()
        {
            _increasing = false;
        }

        public void DecreaseButtonDown()
        {
            _decreasing       = true;
            _timeDecreaseHeld = 0;
        }

        public void DecreaseButtonUp()
        {
            _decreasing = false;
        }
    }
}