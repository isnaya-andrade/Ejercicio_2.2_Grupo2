using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using System.IO;
using System.Linq;
using System.Diagnostics;
using SkiaSharp;
using PointF = Microsoft.Maui.Graphics.PointF;

namespace Ejercicio_2._2_Grupo2
{
    public class SignaturePadView : GraphicsView, IDrawable
    {
        private ObservableCollection<PointF> points = new ObservableCollection<PointF>();
        private bool isDrawing;
        private PointF lastPoint;

        public SignaturePadView()
        {
            this.Drawable = this;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);
            this.StartInteraction += OnStartInteraction;
            this.DragInteraction += OnDragInteraction;
            this.EndInteraction += OnEndInteraction;
        }

        private void OnStartInteraction(object sender, TouchEventArgs e)
        {
            isDrawing = true;
            var point = e.Touches.FirstOrDefault();
            if (point != null)
            {
                lastPoint = new PointF((float)point.X, (float)point.Y);
                points.Add(lastPoint);
                Invalidate();
            }
        }

        private void OnDragInteraction(object sender, TouchEventArgs e)
        {
            if (!isDrawing) return;
            var point = e.Touches.FirstOrDefault();
            if (point != null)
            {
                var currentPoint = new PointF((float)point.X, (float)point.Y);
                points.Add(currentPoint);
                lastPoint = currentPoint;
                Invalidate();
            }
        }

        private void OnEndInteraction(object sender, TouchEventArgs e)
        {
            isDrawing = false;
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    isDrawing = true;
                    lastPoint = new PointF((float)e.TotalX, (float)e.TotalY);
                    points.Add(lastPoint);
                    break;
                case GestureStatus.Running:
                    if (isDrawing)
                    {
                        var currentPoint = new PointF((float)e.TotalX, (float)e.TotalY);
                        points.Add(currentPoint);
                        lastPoint = currentPoint;
                    }
                    break;
                case GestureStatus.Completed:
                    isDrawing = false;
                    break;
            }
            Invalidate();
        }

        public void Clear()
        {
            points.Clear();
            Invalidate();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 3;
            if (points.Count > 1)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    canvas.DrawLine(points[i - 1].X, points[i - 1].Y,
                                  points[i].X, points[i].Y);
                }
            }
        }

        public async Task<Stream> GetImageStreamAsync()
        {
            try
            {
                if (IsBlank())
                {
                    return null;
                }

                float minX = points.Min(p => p.X);
                float maxX = points.Max(p => p.X);
                float minY = points.Min(p => p.Y);
                float maxY = points.Max(p => p.Y);

                float padding = 20;
                int width = (int)(maxX - minX + 2 * padding);
                int height = (int)(maxY - minY + 2 * padding);

                using (var bitmap = new SKBitmap(width, height))
                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(SKColors.White);

                    var paint = new SKPaint
                    {
                        Color = SKColors.Black,
                        StrokeWidth = 3,
                        IsAntialias = true,
                        Style = SKPaintStyle.Stroke
                    };

                    var adjustedPoints = points.Select(p => new SKPoint(
                        p.X - minX + padding,
                        p.Y - minY + padding
                    )).ToList();

                    for (int i = 1; i < adjustedPoints.Count; i++)
                    {
                        canvas.DrawLine(
                            adjustedPoints[i - 1].X,
                            adjustedPoints[i - 1].Y,
                            adjustedPoints[i].X,
                            adjustedPoints[i].Y,
                            paint
                        );
                    }

                    using (var image = SKImage.FromBitmap(bitmap))
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        var memoryStream = new MemoryStream();
                        data.SaveTo(memoryStream);
                        memoryStream.Position = 0;
                        return memoryStream;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al crear la imagen de la firma: {ex.Message}");
                return null;
            }
        }

        public bool IsBlank()
        {
            if (points == null || points.Count == 0)
                return true;
            if (points.Count >= 2)
            {
                float minX = points.Min(p => p.X);
                float maxX = points.Max(p => p.X);
                float minY = points.Min(p => p.Y);
                float maxY = points.Max(p => p.Y);
                float area = (maxX - minX) * (maxY - minY);
                float minArea = 100;
                if (area < minArea)
                    return true;
            }
            return false;
        }

        
    }
}