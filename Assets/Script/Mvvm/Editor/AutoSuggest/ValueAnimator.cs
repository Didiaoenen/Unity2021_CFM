using UnityEngine;

namespace AutoSuggest
{
    public class ValueAnimator
    {
        private readonly float _smoothTime;
        private float _velocity;

        public ValueAnimator(float initialValue, float smoothTime)
        {
            Current = initialValue;
            Target = initialValue;
            _smoothTime = smoothTime;
        }

        public float Current {  get; private set; }
        public float Target { get; set; }

        public bool Update()
        {
            var prey = Current;
            Current = Mathf.SmoothDamp(Current, Target, ref _velocity, _smoothTime);

            return !Mathf.Approximately(prey, Current);
        }
    }
}
