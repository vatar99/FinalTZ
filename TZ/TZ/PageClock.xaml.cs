using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZ.ListViewTimeZone;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _keyTimeZone = TZ.ListViewTimeZone._keyTimeZone;

namespace TZ
{
    public partial class PageClock : ContentPage
    {
        struct HandParams
        {
            public HandParams(double width, double height, double offset) : this()
            {
                Width = width;
                Height = height;
                Offset = offset;
            }

            public double Width { private set; get; }   // fraction of radius
            public double Height { private set; get; }  // ditto
            public double Offset { private set; get; }  // relative to center pivot
        }

        static readonly HandParams secondParams = new HandParams(0.02, 1.1, 0.85);
        static readonly HandParams minuteParams = new HandParams(0.05, 0.8, 0.9);
        static readonly HandParams hourParams = new HandParams(0.125, 0.65, 0.9);

        BoxView[] tickMarks = new BoxView[60];

        public PageClock()
        {
            InitializeComponent();

            // Create the tick marks (to be sized and positioned later).
            for (int i = 0; i < tickMarks.Length; i++)
            {
                tickMarks[i] = new BoxView { Color = Color.Black };
                absoluteLayout.Children.Add(tickMarks[i]);
            }

            Device.StartTimer(TimeSpan.FromSeconds(1.0 / 60), OnTimerTick);
            OnTimerTick();
        }

        void OnAbsoluteLayoutSizeChanged(object sender, EventArgs args)
        {
            // Get the center and radius of the AbsoluteLayout.
            Point center = new Point(absoluteLayout.Width / 2, absoluteLayout.Height / 2);
            double radius = 0.45 * Math.Min(absoluteLayout.Width, absoluteLayout.Height);

            // Position, size, and rotate the 60 tick marks.
            for (int index = 0; index < tickMarks.Length; index++)
            {
                double size = radius / (index % 5 == 0 ? 15 : 30);
                double radians = index * 2 * Math.PI / tickMarks.Length;
                double x = center.X + radius * Math.Sin(radians) - size / 2;
                double y = center.Y - radius * Math.Cos(radians) - size / 2;
                AbsoluteLayout.SetLayoutBounds(tickMarks[index], new Rectangle(x, y, size, size));
                tickMarks[index].Rotation = 180 * radians / Math.PI;
            }

            // Position and size the three hands.
            LayoutHand(secondHand, secondParams, center, radius);
            LayoutHand(minuteHand, minuteParams, center, radius);
            LayoutHand(hourHand, hourParams, center, radius);
        }

        void LayoutHand(BoxView boxView, HandParams handParams, Point center, double radius)
        {
            double width = handParams.Width * radius;
            double height = handParams.Height * radius;
            double offset = handParams.Offset;

            AbsoluteLayout.SetLayoutBounds(boxView,
                new Rectangle(center.X - 0.5 * width,
                              center.Y - offset * height,
                              width, height));

            // Set the AnchorY property for rotations.
            boxView.AnchorY = handParams.Offset;
        }

        bool OnTimerTick()
        {
           
            _keyTimeZone ti = new _keyTimeZone();
            ti.Time = +2;
            if (ti.Key == 1)
            {
                ti.Time = +3;
            }
            else if (ti.Key == 2)
            {
                ti.Time = +8;
            }
            else if (ti.Key == 3)
            {
                ti.Time = +9;
            }

            var localTime = DateTimeOffset.Now;
            var offset = TimeSpan.FromHours(ti.Time);
            var pstTime = localTime.ToOffset(offset);
            hourHand.Rotation = 30 * (pstTime.Hour % 12) + 0.5 * pstTime.Minute;
            minuteHand.Rotation = 6 * pstTime.Minute + 0.1 * pstTime.Second;

            // Do an animation for the second hand.
            double t = pstTime.Millisecond / 1000.0;

            if (t < 0.5)
            {
                t = 0.5 * Easing.SpringIn.Ease(t / 0.5);
            }
            else
            {
                t = 0.5 * (1 + Easing.SpringOut.Ease((t - 0.5) / 0.5));
            }

            secondHand.Rotation = 6 * (pstTime.Second + t);
            return true;
        }


        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            double r = hourHand.Rotation;
            switch (e.StatusType)
            {
                case GestureStatus.Running:


                    break;

                case GestureStatus.Completed:
                    //set rotation according to TranslationX and TranslationY
                    hourHand.Rotation = 180;

                    break;
            }


        }
    }
}