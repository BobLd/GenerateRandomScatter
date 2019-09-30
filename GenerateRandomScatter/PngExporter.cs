using OxyPlot;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GenerateRandomScatter
{
    /// <summary>
    /// The ad-hoc png exporter.
    /// </summary>
    public class PngExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PngExporter" /> class.
        /// </summary>
        public PngExporter()
        {
            this.Width = 700;
            this.Height = 400;
            this.Resolution = 96;
            this.Background = OxyColors.White;
        }

        public List<Tuple<string, RectangleF>> GroundTruthText { get; private set; }
        public Dictionary<string, List<RectangleF>> GroundTruth { get; private set; }

        /// <summary>
        /// Gets or sets the width of the output image.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the output image.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the resolution (dpi) of the output image.
        /// </summary>
        public int Resolution { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Exports the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        public static void Export(IPlotModel model, string fileName, int width, int height, Brush background = null)
        {
            var exporter = new PngExporter { Width = width, Height = height, Background = background.ToOxyColor() };
            using (var stream = File.Create(fileName))
            {
                exporter.Export(model, stream);
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        public void Export(IPlotModel model, Stream stream)
        {
            using (var bm = this.ExportToBitmap(model))
            {
                bm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a <see cref="Bitmap" />.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <returns>A bitmap.</returns>
        public Bitmap ExportToBitmap(IPlotModel model)
        {
            var bm = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);
            bm.SetResolution(this.Resolution, this.Resolution);
            using (var g = Graphics.FromImage(bm))
            {
                if (this.Background.IsVisible())
                {
                    using (var brush = this.Background.ToBrush())
                    {
                        g.FillRectangle(brush, 0, 0, this.Width, this.Height);
                    }
                }

                using (var rc = new GroundTruthGraphicsRenderContext(g) { RendersToScreen = false })
                {
                    model.Update(true);
                    model.Render(rc, this.Width, this.Height);
                    this.GroundTruth = rc.GroundTruth;
                    this.GroundTruthText = rc.GroundTruthText;
                }
                return bm;
            }
        }
    }
}
