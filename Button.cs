using System;
using Android.Animation;
using Android.Graphics;

namespace Segmentus
{
    abstract class Button : TouchablePart
    {
        const float DiveScale = 0.8f;
        const int DiveDuration = 200;

        protected DrawablePart face;

        public event Action Pressed;

        bool disturbed = false;
        ValueAnimator diveAnim;
        float currentDiveScale;
        float currentDiveTime;

        float CurrentDiveTime
        {
            get { return currentDiveTime; }
            set
            {
                currentDiveTime = value;
                currentDiveScale = 1 - (1 - DiveScale) * currentDiveTime / DiveDuration;
                OnAppearanceChanged();
            }
        }
        public Button(DrawablePart face, Rect localBounds, Pivot parentPivot, 
            float x = 0, float y = 0) : base(localBounds, parentPivot, x, y) {
            CurrentDiveTime = 0;
            this.face = face;
        }

        public override void OnTouchDown(int x, int y)
        {
            disturbed = true;
            AnimateDivingTo(DiveDuration);
        }

        public override void OnTouchUp(int x, int y)
        {
            if (disturbed)
            {
                disturbed = false;
                AnimateDivingTo(0);
                Pressed?.Invoke();
            }
        }

        public override void OnTouchCancel(int x, int y)
        {
            if (disturbed)
            {
                disturbed = false;
                AnimateDivingTo(0);
            }
        }

        void AnimateDivingTo(int divingTimeDest)
        {
            diveAnim?.Cancel();
            diveAnim = AnimatorFactory.CreateAnimator(currentDiveTime,
                divingTimeDest, (int)Math.Abs(currentDiveTime - divingTimeDest));
            diveAnim.Update += (sender, e) => CurrentDiveTime = (float)e.Animation.AnimatedValue;
            diveAnim.Start();
        }

        protected override void Draw(Canvas canvas)
        {
            canvas.Save();
            canvas.Scale(currentDiveScale, currentDiveScale);
            face.OnDraw(canvas);
            canvas.Restore();
        }

    }
}