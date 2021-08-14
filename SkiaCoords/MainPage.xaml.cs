using SkiaSharp;
using SkiaSharp.Views.UWP;
using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SkiaCoords
{
    public sealed partial class MainPage : Page
    {
        private Point mousePosition;

        public MainPage()
        {
            InitializeComponent();
            mousePosition = new Point();
        }

        private void canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = ((Color)Application.Current.Resources["SystemAccentColor"]).ToSKColor(),
                StrokeWidth = 1,
                IsAntialias = true,
                PathEffect = SKPathEffect.CreateDash(new float[] { 5, 5 }, 0)
            })
            {
                canvas.DrawLine(0, (float)mousePosition.Y, info.Width, (float)mousePosition.Y, paint);
                canvas.DrawLine((float)mousePosition.X, 0, (float)mousePosition.X, info.Height , paint);
            }

            using (SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                StrokeWidth = 1,
                BlendMode = SKBlendMode.DstOut,
                IsAntialias = true
            })
            {
                canvas.DrawCircle((float)mousePosition.X, (float)mousePosition.Y, 10, paint);
            }

            float textHeight = 12;
            using (SKPaint paint = new SKPaint
            {
                TextSize = textHeight,
                Style = SKPaintStyle.Stroke,
                Color = ((Color)Application.Current.Resources["SystemAccentColor"]).ToSKColor(),
                StrokeWidth = 1,
                IsAntialias = true
            })
            {
                float textWidth = paint.MeasureText((int)Math.Round(mousePosition.X) + ", " + (int)Math.Round(mousePosition.Y));

                float spacing = 12;

                float labelOffsetX = spacing;
                float labelOffsetY = -spacing;

                if (mousePosition.Y + labelOffsetY < textHeight)
                    labelOffsetY = textHeight + spacing;
                if (mousePosition.X + labelOffsetX + textWidth > info.Width)
                    labelOffsetX = - (textWidth + spacing);
                canvas.DrawText(mousePosition.X + ", " + mousePosition.Y, (float)mousePosition.X + labelOffsetX, (float)mousePosition.Y + labelOffsetY, paint);
            };

        }

        private void SKXamlCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            mousePosition = new Point(Math.Round(e.GetCurrentPoint(this).Position.X), Math.Round(e.GetCurrentPoint(this).Position.Y));
            skCanvas.Invalidate();
        }
    }
}