using Accord;
using Accord.Collections;
using Accord.Imaging.Filters;
using Accord.Imaging.Textures;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using GenerateRandomScatter.Coco;
using GenerateRandomScatter.PascalVOC;
//using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GenerateRandomScatter
{
    class Program
    {
        public const string RootFoler = "data";
        public const string ImageFolder = "images";
        public const string ImageBboxFolder = "imagesBbox";
        public const string AnnotationsFolder = "annots";
        public const string PlotsFolder = "plots";

        const int dpi_min = 85;
        const int dpi_max = 250;

        const int figsize_min = 500;        //3;
        const int figsize_max = 1500;       //10;

        const int tick_size_width_min = 0;
        const int tick_size_width_max = 3;
        const int tick_size_length_min = 0;
        const int tick_size_length_max = 12;

        const int points_nb_min = 10;
        const int points_nb_max = 130;
        const int x_min_top = -2;
        const int x_max_top = 5;
        const int y_min_top = -2;
        const int y_max_top = 4;
        const int x_scale_range_max = 4;
        const int y_scale_range_max = 4;

        const int size_points_min = 3;
        const int size_points_max = 12;

        const int max_points_variations = 5;

        const int pad_min = 2;
        const int pad_max = 18;

        const int axes_label_size_min = 10;
        const int axes_label_size_max = 16;
        const int tick_label_size_min = 10;
        const int tick_label_size_max = 16;
        const int title_size_min = 14;
        const int title_size_max = 24;

        const int axes_label_length_min = 5;
        const int axes_label_length_max = 15;
        const int title_length_min = 5;
        const int title_length_max = 25;

        const double colorbg_transparant_max = 0.05;

        static Random _random;

        static readonly int[] updown = new[] { -1, 1 };
        static readonly string[] point_dist = new string[] { "uniform", "linear", "quadratic", "binormal" };

        static readonly MarkerType[] markers = new MarkerType[]
        {
            MarkerType.Circle,
            MarkerType.Cross,
            MarkerType.Diamond,
            MarkerType.Plus,
            MarkerType.Square,
            MarkerType.Star,
            MarkerType.Triangle
        };
        static int _markersCount = 0;

        static readonly LineStyle[] lineStyles = new LineStyle[]
        {
            LineStyle.Dash,
            LineStyle.DashDashDot,
            LineStyle.DashDashDotDot,
            LineStyle.DashDot,
            LineStyle.DashDotDot,
            LineStyle.Dot,
            LineStyle.LongDash,
            LineStyle.LongDashDot,
            LineStyle.LongDashDotDot,
            LineStyle.None,
            LineStyle.Solid
        };
        static int _lineStylesCount = 0;

        static readonly OxyColor[] oxyColors = new OxyColor[]
        {
            OxyColors.AliceBlue,
            OxyColors.AntiqueWhite,
            OxyColors.Aqua,
            OxyColors.Aquamarine,
            OxyColors.Azure,
            OxyColors.Beige,
            OxyColors.Bisque,
            OxyColors.Black,
            OxyColors.BlanchedAlmond,
            OxyColors.Blue,
            OxyColors.BlueViolet,
            OxyColors.Brown,
            OxyColors.BurlyWood,
            OxyColors.CadetBlue,
            OxyColors.Chartreuse,
            OxyColors.Chocolate,
            OxyColors.Coral,
            OxyColors.CornflowerBlue,
            OxyColors.Cornsilk,
            OxyColors.Crimson,
            OxyColors.Cyan,
            OxyColors.DarkBlue,
            OxyColors.DarkCyan,
            OxyColors.DarkGoldenrod,
            OxyColors.DarkGray,
            OxyColors.DarkGreen,
            OxyColors.DarkKhaki,
            OxyColors.DarkMagenta,
            OxyColors.DarkOliveGreen,
            OxyColors.DarkOrange,
            OxyColors.DarkOrchid,
            OxyColors.DarkRed,
            OxyColors.DarkSalmon,
            OxyColors.DarkSeaGreen,
            OxyColors.DarkSlateBlue,
            OxyColors.DarkSlateGray,
            OxyColors.DarkTurquoise,
            OxyColors.DarkViolet,
            OxyColors.DeepPink,
            OxyColors.DeepSkyBlue,
            OxyColors.DimGray,
            OxyColors.DodgerBlue,
            OxyColors.Firebrick,
            OxyColors.FloralWhite,
            OxyColors.ForestGreen,
            OxyColors.Fuchsia,
            OxyColors.Gainsboro,
            //OxyColors.GhostWhite,
            OxyColors.Gold,
            OxyColors.Goldenrod,
            OxyColors.Gray,
            OxyColors.Green,
            OxyColors.GreenYellow,
            OxyColors.Honeydew,
            OxyColors.HotPink,
            OxyColors.IndianRed,
            OxyColors.Indigo,
            OxyColors.Ivory,
            OxyColors.Khaki,
            OxyColors.Lavender,
            OxyColors.LavenderBlush,
            OxyColors.LawnGreen,
            OxyColors.LemonChiffon,
            OxyColors.LightBlue,
            OxyColors.LightCoral,
            OxyColors.LightCyan,
            OxyColors.LightGoldenrodYellow,
            OxyColors.LightGray,
            OxyColors.LightGreen,
            OxyColors.LightPink,
            OxyColors.LightSalmon,
            OxyColors.LightSeaGreen,
            OxyColors.LightSkyBlue,
            OxyColors.LightSlateGray,
            OxyColors.LightSteelBlue,
            OxyColors.LightYellow,
            OxyColors.Lime,
            OxyColors.LimeGreen,
            OxyColors.Linen,
            OxyColors.Magenta,
            OxyColors.Maroon,
            OxyColors.MediumAquamarine,
            OxyColors.MediumBlue,
            OxyColors.MediumOrchid,
            OxyColors.MediumPurple,
            OxyColors.MediumSeaGreen,
            OxyColors.MediumSlateBlue,
            OxyColors.MediumSpringGreen,
            OxyColors.MediumTurquoise,
            OxyColors.MediumVioletRed,
            OxyColors.MidnightBlue,
            OxyColors.MintCream,
            OxyColors.MistyRose,
            OxyColors.Moccasin,
            //OxyColors.NavajoWhite,
            OxyColors.Navy,
            OxyColors.OldLace,
            OxyColors.Olive,
            OxyColors.OliveDrab,
            OxyColors.Orange,
            OxyColors.OrangeRed,
            OxyColors.Orchid,
            OxyColors.PaleGoldenrod,
            OxyColors.PaleGreen,
            OxyColors.PaleTurquoise,
            OxyColors.PaleVioletRed,
            OxyColors.PapayaWhip,
            OxyColors.PeachPuff,
            OxyColors.Peru,
            OxyColors.Pink,
            OxyColors.Plum,
            OxyColors.PowderBlue,
            OxyColors.Purple,
            OxyColors.Red,
            OxyColors.RosyBrown,
            OxyColors.RoyalBlue,
            OxyColors.SaddleBrown,
            OxyColors.Salmon,
            OxyColors.SandyBrown,
            OxyColors.SeaGreen,
            OxyColors.SeaGreen,
            OxyColors.SeaShell,
            OxyColors.Sienna,
            OxyColors.Silver,
            OxyColors.SkyBlue,
            OxyColors.SlateBlue,
            OxyColors.SlateGray,
            OxyColors.Snow,
            OxyColors.SpringGreen,
            OxyColors.SteelBlue,
            OxyColors.Tan,
            OxyColors.Teal,
            OxyColors.Thistle,
            OxyColors.Tomato,
            OxyColors.Transparent,
            OxyColors.Turquoise,
            OxyColors.Violet,
            OxyColors.Wheat,
            //OxyColors.White,
            //OxyColors.WhiteSmoke,
            OxyColors.Yellow,
            OxyColors.YellowGreen
        };
        static int _oxyColorsCount = 0;

        static readonly TickStyle[] directionTicks = new TickStyle[]
        {
            TickStyle.Crossing,
            TickStyle.Inside,
            TickStyle.Outside,
        };
        static int _directionTicksCount = 0;

        static readonly string[] fontList = new[] 
        {
            // https://en.wikipedia.org/wiki/Font_family_(HTML)
            "Agency FB",
            "Albertina",
            "Antiqua",
            "Architect",
            "Arial",
            "BankFuturistic",
            "BankGothic",
            "Blackletter",
            "Blagovest",
            "Calibri",
            "Comic Sans MS",
            "Consolas",
            "Courier",
            "Cursive",
            "Decorative",
            "Fantasy",
            "Fraktur",
            "Frosty",
            "Garamond",
            "Georgia",
            "Helvetica",
            "Impact",
            "Minion",
            "Modern",
            "Monospace",
            "Open Sans",
            "Palatino",
            "Perpetua",
            "Roman",
            "Sans-serif",
            "Serif",
            "Script",
            "Swiss",
            "Times",
            "Times New Roman",
            "Tw Cen MT",
            "Verdana"
        };
        static int _fontListCount = 0;

        static PascalVocXmlSerializer _vocSerializer;

        static void Main(string[] args)
        {
            int plotsCount = 50;

            _vocSerializer = new PascalVocXmlSerializer();
            _random = new Random(42);
            _markersCount = markers.Length;
            _lineStylesCount = lineStyles.Length;
            _oxyColorsCount = oxyColors.Length;
            _directionTicksCount = directionTicks.Length;
            _fontListCount = fontList.Length;

            CheckFolders();

            var data = new ConcurrentDictionary<int, (IEnumerable<RectangleF> points, IEnumerable<RectangleF> ticks, IEnumerable<RectangleF> labels)>();

            //for (int i = 0; i < plotsCount; i++)
            System.Threading.Tasks.Parallel.For(1, plotsCount + 1, i =>
            {
                var plotBoxes = GetRandomPlot(i);
                data.TryAdd(i, plotBoxes);
            });

            CocoFile cocoFile = new CocoFile()
            {
                 categories = new CocoCategory[]
                 {
                     new CocoCategory() { id = 1, name = "points" },
                     new CocoCategory() { id = 2, name = "ticks" },
                     new CocoCategory() { id = 3, name = "labels" }
                 },
                info = new CocoInfo() { contributor = "todo" }
            };

            CocoImage[] cocoImages = new CocoImage[data.Count];

            List<CocoAnnotation> annotations = new List<CocoAnnotation>();

            for (int d = 0; d < data.Count; d++)
            {
                var kvp = data.ElementAt(d);
                //$"plot_{id}.png"
                cocoImages[d] = new CocoImage()
                {
                    id = kvp.Key,
                    file_name = $"plot_{kvp.Key}.png",
                    width = 600,
                    height = 400
                };

                foreach (var point in kvp.Value.points)
                {
                    CocoAnnotation cocoAnnotation = new CocoAnnotation()
                    {
                        bbox = new float[] { point.Left, point.Top, point.Width, point.Height },
                        category_id = 1,
                        image_id = kvp.Key,
                        segmentation = new float[][] { new float[] { point.Left, point.Top, point.Width, point.Height } }
                    };
                    annotations.Add(cocoAnnotation);
                }

                foreach (var tick in kvp.Value.ticks)
                {
                    CocoAnnotation cocoAnnotation = new CocoAnnotation()
                    {
                        bbox = new float[] { tick.Left, tick.Top, tick.Width, tick.Height },
                        category_id = 2,
                        image_id = kvp.Key,
                        segmentation = new float[][] { new float[] { tick.Left, tick.Top, tick.Width, tick.Height } }
                    };
                    annotations.Add(cocoAnnotation);
                }

                foreach (var label in kvp.Value.labels)
                {
                    CocoAnnotation cocoAnnotation = new CocoAnnotation()
                    {
                        bbox = new float[] { label.Left, label.Top, label.Width, label.Height },
                        category_id = 3,
                        image_id = kvp.Key,
                        segmentation = new float[][] { new float[] { label.Left, label.Top, label.Width, label.Height } }
                    };
                    annotations.Add(cocoAnnotation);
                }
            }

            cocoFile.images = cocoImages;
            cocoFile.annotations = annotations.ToArray();

            Console.WriteLine("Done. Press any key.");
            Console.ReadKey();
        }

        private static PlotModel GetPlot(string name)
        {
            // RESOLUTION AND TICK SIZE
            int dpi = (int)(dpi_min + _random.NextDouble() * (dpi_max - dpi_min));
            int[] figsize = new[] { (int)(figsize_min + _random.NextDouble() * (figsize_max - figsize_min)), (int)(figsize_min + _random.NextDouble() * (figsize_max - figsize_min)) };

            List<double> tick_size = new List<double> { (tick_size_width_min + _random.NextDouble() * (tick_size_width_max - tick_size_width_min)), (tick_size_length_min + _random.NextDouble() * (tick_size_length_max - tick_size_length_min)) };
            tick_size.Sort();

            // ACTUAL POINTS
            var points_nb = (int)(points_nb_min + (Math.Pow(_random.NextDouble(), 1.5)) * (points_nb_max - points_nb_min));
            var x_scale = (int)(x_min_top + _random.NextDouble() * (x_max_top - x_min_top));
            var y_scale = (int)(y_min_top + _random.NextDouble() * (y_max_top - y_min_top));
            var x_scale_range = x_scale + (int)(_random.NextDouble() * x_scale_range_max);
            var y_scale_range = y_scale + (int)(_random.NextDouble() * y_scale_range_max);
            var x_min_temp = (-_random.NextDouble() + _random.NextDouble()) * Math.Pow(10, x_scale);
            var x_max_temp = (-_random.NextDouble() + _random.NextDouble()) * Math.Pow(10, x_scale_range);
            var x_min = Math.Min(x_min_temp, x_max_temp);
            var x_max = Math.Max(x_min_temp, x_max_temp);
            var y_min_temp = (-_random.NextDouble() + _random.NextDouble()) * Math.Pow(10, y_scale);
            var y_max_temp = (-_random.NextDouble() + _random.NextDouble()) * Math.Pow(10, y_scale_range);
            var y_min = Math.Min(y_min_temp, y_max_temp);
            var y_max = Math.Max(y_min_temp, y_max_temp);

            var xs = UniformContinuousDistribution.Random(x_min, x_max, points_nb, _random);
            double[] ys = new double[0];

            var distribution = point_dist[_random.Next(4)];
            if (distribution == "uniform")
            {
                ys = UniformContinuousDistribution.Random(y_min, y_max, points_nb, _random);
            }
            else if (distribution == "linear")
            {
                ys = xs.Multiply((Math.Max(y_max, -y_min) / (Math.Max(x_max, -x_min)))).Multiply((double)updown[_random.Next(2)])
                    .Add(UniformContinuousDistribution.Random(0, 1, points_nb, _random).Add(y_min).Multiply((y_max - y_min)).Multiply(_random.NextDouble() / 2.0));
            }
            else if (distribution == "quadratic")
            {
                ys = xs.Pow(2).Multiply(1.0 / (Math.Max(x_max, -x_min))).Pow(2).Multiply(Math.Max(y_max, -y_min)).Multiply((double)updown[_random.Next(2)])
                    .Add(UniformContinuousDistribution.Random(0, 1, points_nb, _random).Add(y_min).Multiply((y_max - y_min)).Multiply(_random.NextDouble() / 2.0));
            }
            else if (distribution == "binormal")
            {
                double stdDev = Math.Max(y_max, -y_min) / Math.Max(x_max, -x_min);
                double correl = (double)updown[_random.Next(2)] * _random.NextDouble();
                xs = NormalDistribution.Random(x_max - x_min, stdDev, points_nb, _random);
                ys = correl.Multiply(xs).Add(NormalDistribution.Random(y_max - y_min, stdDev, points_nb, _random).Multiply(Math.Sqrt(1 - correl * correl)));
            }

            // POINTS VARIATION
            var nb_points_var = 1 + (int)(_random.NextDouble() * max_points_variations);
            var nb_points_var_colors = 1 + (int)(_random.NextDouble() * nb_points_var);
            var nb_points_var_markers = 1 + (int)(_random.NextDouble() * (nb_points_var - nb_points_var_colors));
            var nb_points_var_size = Math.Max(1, 1 + nb_points_var - nb_points_var_colors - nb_points_var_markers);

            var rand_color_number = _random.NextDouble();

            OxyColor[] colors = new OxyColor[nb_points_var_colors];
            OxyColor[] tempPalette;
            if (rand_color_number <= 0.5)
            {
                tempPalette = OxyPalettes.Rainbow(nb_points_var_colors * 20).Colors.ToArray();
            }
            else if (rand_color_number > 0.5 && rand_color_number <= 0.7)
            {
                tempPalette = OxyPalettes.Hue(nb_points_var_colors * 20).Colors.ToArray(); // gnuplot
            }
            else if (rand_color_number > 0.7 && rand_color_number <= 0.8)
            {
                tempPalette = OxyPalettes.Hot(nb_points_var_colors * 20).Colors.ToArray(); // copper
            }
            else
            {
                tempPalette = OxyPalettes.Jet(nb_points_var_colors * 20).Colors.ToArray(); // gray
            }

            for (int i = 0; i < nb_points_var_colors; i++)
            {
                colors[i] = tempPalette[_random.Next(nb_points_var_colors * 20)];
            }

            var s_set = (size_points_min.Add(UniformContinuousDistribution.Random(0, 1, nb_points_var_size, _random).Multiply(size_points_max - size_points_min)));
            var markers_subset = new List<MarkerType>();
            for (int i = 0; i < nb_points_var_markers; i++)
            {
                markers_subset.Add(markers[_random.Next(_markersCount)]);
            }

            var markers_empty = _random.NextDouble() > 0.75;
            var markers_empty_ratio = new[] { 0.0, 0.5, 0.7 }[_random.Next(3)];



            // PAD BETWEEN TICKS AND LABELS
            double pad_x = Math.Max(tick_size[1] + 0.5, (int)(pad_min + _random.NextDouble() * (pad_max - pad_min)));
            double pad_y = Math.Max(tick_size[1] + 0.5, (int)(pad_min + _random.NextDouble() * (pad_max - pad_min)));
            TickStyle direction_ticks_x = directionTicks[_random.Next(_directionTicksCount)];
            TickStyle direction_ticks_y = directionTicks[_random.Next(_directionTicksCount)];

            // FONT AND SIZE FOR LABELS (tick labels, axes labels and title)
            string font = fontList[_random.Next(_fontListCount)];
            int size_ticks = (int)(tick_label_size_min + _random.NextDouble() * (tick_label_size_max - tick_label_size_min));
            int size_axes = (int)(axes_label_size_min + _random.NextDouble() * (axes_label_size_max - axes_label_size_min));
            int size_title = (int)(title_size_min + _random.NextDouble() * (title_size_max - title_size_min));
            //var ticks_font = font_manager.FontProperties(fname = font, style = 'normal', size = size_ticks, weight = 'normal', stretch = 'normal');
            //var axes_font = font_manager.FontProperties(fname = font, style = 'normal', size = size_axes, weight = 'normal', stretch = 'normal');
            //var title_font = font_manager.FontProperties(fname = font, style = 'normal', size = size_title, weight = 'normal', stretch = 'normal');

            // TEXTS FOR AXIS LABELS AND TITLE
            int label_x_length = (int)(axes_label_length_min + _random.NextDouble() * (axes_label_length_max - axes_label_length_min));
            int label_y_length = (int)(axes_label_length_min + _random.NextDouble() * (axes_label_length_max - axes_label_length_min));
            int title_length = (int)(title_length_min + _random.NextDouble() * (title_length_max - title_length_min));
            string x_label = RandomString(label_x_length);
            string y_label = RandomString(label_y_length);
            string title = RandomString(title_length);

            // BUILDING THE PLOT
            PlotModel model = new PlotModel
            {
                Title = title,
                TitleFont = font,
                TitleFontSize = size_title,
                DefaultColors = colors,
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.None, Palette = new OxyPalette(colors), Minimum = 0, Maximum = colors.Length - 1 });

            // TICKS STYLE AND LOCATION (X AXIS)
            LineStyle lineStyle = lineStyles[_random.Next(_lineStylesCount)];
            var xAxis = new LinearAxis
            {
                Position = _random.NextDouble() > 0.5 ? AxisPosition.Top : AxisPosition.Bottom,
                TickStyle = direction_ticks_x,
                MajorGridlineStyle = lineStyle,
                MajorTickSize = tick_size[1],
                MinorTickSize = (_random.NextDouble() > 77) ? 0.75 * _random.NextDouble() * tick_size[1] : 0,
                Angle = _random.NextDouble() > 0.77 ? _random.NextDouble() * (double)updown[_random.Next(2)] * 90 : 0,
                Font = font,
                FontSize = size_ticks,
                TitleFont = font,
                TitleFontSize = size_axes,
                Title = _random.NextDouble() > 0.80 ? "" : x_label,
                PositionAtZeroCrossing = (_random.NextDouble() > 0.77),
                IsAxisVisible = true,
                AxislineColor = OxyColors.Black,
                AxislineStyle = LineStyle.Solid
            };
            model.Axes.Add(xAxis);

            // TICKS STYLE AND LOCATION (Y AXIS)
            var yAxis = new LinearAxis
            {
                Position = _random.NextDouble() > 0.5 ? AxisPosition.Right : AxisPosition.Left,
                TickStyle = direction_ticks_y,
                MajorGridlineStyle = lineStyle,
                MajorTickSize = tick_size[1],
                MinorTickSize = (_random.NextDouble() > 77) ? 0.75 * _random.NextDouble() * tick_size[1] : 0,
                Angle = _random.NextDouble() > 0.77 ? _random.NextDouble() * 90 : 0,
                Font = font,
                FontSize = size_ticks,
                TitleFont = font,
                TitleFontSize = size_axes,
                Title = _random.NextDouble() > 0.80 ? "" : y_label,
                PositionAtZeroCrossing = (_random.NextDouble() > 0.77),
                IsAxisVisible = true,
                AxislineColor = OxyColors.Black,
                AxislineStyle = LineStyle.Solid
            };
            model.Axes.Add(yAxis);

            MarkerType markerType = markers[_random.Next(_markersCount)];

            var scatterSerie = new ScatterSeries()
            {
                MarkerType = markerType,
                MarkerStrokeThickness = _random.Next(10, 31) / 10,
            };

            if (markerType != MarkerType.Cross && markerType != MarkerType.Plus && markerType != MarkerType.Star)
            {
                if (_random.NextDouble() > 0.5)
                {
                    scatterSerie.MarkerStroke = OxyColors.Black;
                }
            }

            List<double> s = new List<double>();
            for (int i = 0; i < points_nb; i++)
            {
                var s_ = s_set[_random.Next(s_set.Length)];
                var c_ = colors[_random.Next(colors.Length)];
                var m_ = markers[_random.Next(_markersCount)];

                bool e_ = false;

                if (markers_empty)
                {
                    e_ = _random.NextDouble() > markers_empty_ratio;
                }

                scatterSerie.Points.Add(new ScatterPoint(xs[i], ys[i], s_, _random.Next(colors.Length)));
            }

            model.Series.Add(scatterSerie);
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        private static (IEnumerable<RectangleF> points, IEnumerable<RectangleF> ticks, IEnumerable<RectangleF> labels) GetRandomPlot(int id)
        {
            // RESOLUTION AND TICK SIZE
            int dpi = (int)(dpi_min + _random.NextDouble() * (dpi_max - dpi_min));
            int[] figsize = new[] { (int)(figsize_min + _random.NextDouble() * (figsize_max - figsize_min)), (int)(figsize_min + _random.NextDouble() * (figsize_max - figsize_min)) };

            List<double> tick_size = new List<double> { (tick_size_width_min + _random.NextDouble() * (tick_size_width_max - tick_size_width_min)), (tick_size_length_min + _random.NextDouble() * (tick_size_length_max - tick_size_length_min)) };
            tick_size.Sort();

            // ACTUAL POINTS
            var points_nb = (int)(points_nb_min + (Math.Pow(_random.NextDouble(), 1.5)) * (points_nb_max - points_nb_min));
            var x_scale = (int)(x_min_top + _random.NextDouble() * (x_max_top - x_min_top));
            var y_scale = (int)(y_min_top + _random.NextDouble() * (y_max_top - y_min_top));
            var x_scale_range = x_scale + (int)(_random.NextDouble() * x_scale_range_max);
            var y_scale_range = y_scale + (int)(_random.NextDouble() * y_scale_range_max);
            var x_min_temp = (-_random.NextDouble() + _random.NextDouble()) * Math.Pow(10, x_scale);
            var x_max_temp = (-_random.NextDouble() + _random.NextDouble()) * Math.Pow(10, x_scale_range);
            var x_min = Math.Min(x_min_temp, x_max_temp);
            var x_max = Math.Max(x_min_temp, x_max_temp);
            var y_min_temp = (-_random.NextDouble() + _random.NextDouble()) * Math.Pow(10, y_scale);
            var y_max_temp = (-_random.NextDouble() + _random.NextDouble()) * Math.Pow(10, y_scale_range);
            var y_min = Math.Min(y_min_temp, y_max_temp);
            var y_max = Math.Max(y_min_temp, y_max_temp);

            var xs = UniformContinuousDistribution.Random(x_min, x_max, points_nb, _random);
            double[] ys = new double[0];

            var distribution = point_dist[_random.Next(4)];
            if (distribution == "uniform")
            {
                ys = UniformContinuousDistribution.Random(y_min, y_max, points_nb, _random);
            }
            else if (distribution == "linear")
            {
                ys = xs.Multiply((Math.Max(y_max, -y_min) / (Math.Max(x_max, -x_min)))).Multiply((double)updown[_random.Next(2)])
                    .Add(UniformContinuousDistribution.Random(0, 1, points_nb, _random).Add(y_min).Multiply((y_max - y_min)).Multiply(_random.NextDouble() / 2.0));
            }
            else if (distribution == "quadratic")
            {
                ys = xs.Pow(2).Multiply(1.0 / (Math.Max(x_max, -x_min))).Pow(2).Multiply(Math.Max(y_max, -y_min)).Multiply((double)updown[_random.Next(2)])
                    .Add(UniformContinuousDistribution.Random(0, 1, points_nb, _random).Add(y_min).Multiply((y_max - y_min)).Multiply(_random.NextDouble() / 2.0));
            }
            else if (distribution == "binormal")
            {
                double stdDev = Math.Max(y_max, -y_min) / Math.Max(x_max, -x_min);
                double correl = (double)updown[_random.Next(2)] * _random.NextDouble();
                xs = NormalDistribution.Random(x_max - x_min, stdDev, points_nb, _random);
                ys = correl.Multiply(xs).Add(NormalDistribution.Random(y_max - y_min, stdDev, points_nb, _random).Multiply(Math.Sqrt(1 - correl * correl)));
            }

            // POINTS VARIATION
            var nb_points_var = 1 + (int)(_random.NextDouble() * max_points_variations);
            var nb_points_var_colors = 1 + (int)(_random.NextDouble() * nb_points_var);
            var nb_points_var_markers = 1 + (int)(_random.NextDouble() * (nb_points_var - nb_points_var_colors));
            var nb_points_var_size = Math.Max(1, 1 + nb_points_var - nb_points_var_colors - nb_points_var_markers);

            var rand_color_number = _random.NextDouble();

            OxyColor[] colors = new OxyColor[nb_points_var_colors];
            OxyColor[] tempPalette;
            if (rand_color_number <= 0.5)
            {
                tempPalette = OxyPalettes.Rainbow(nb_points_var_colors * 20).Colors.ToArray();
            }
            else if (rand_color_number > 0.5 && rand_color_number <= 0.7)
            {
                tempPalette = OxyPalettes.Hue(nb_points_var_colors * 20).Colors.ToArray(); // gnuplot
            }
            else if (rand_color_number > 0.7 && rand_color_number <= 0.8)
            {
                tempPalette = OxyPalettes.Hot(nb_points_var_colors * 20).Colors.ToArray(); // copper
            }
            else
            {
                tempPalette = OxyPalettes.Jet(nb_points_var_colors * 20).Colors.ToArray(); // gray
            }

            for (int i = 0; i < nb_points_var_colors; i++)
            {
                colors[i] = tempPalette[_random.Next(nb_points_var_colors * 20)];
            }

            var s_set = (size_points_min.Add(UniformContinuousDistribution.Random(0, 1, nb_points_var_size, _random).Multiply(size_points_max - size_points_min)));
            var markers_subset = new List<MarkerType>();
            for (int i = 0; i< nb_points_var_markers; i++)
            {
                markers_subset.Add(markers[_random.Next(_markersCount)]);
            }

            var markers_empty = _random.NextDouble() > 0.75;
            var markers_empty_ratio = new[] { 0.0, 0.5, 0.7 }[_random.Next(3)];

            // PAD BETWEEN TICKS AND LABELS
            double pad_x = Math.Max(tick_size[1] + 0.5, (int)(pad_min + _random.NextDouble() * (pad_max - pad_min)));
            double pad_y = Math.Max(tick_size[1] + 0.5, (int)(pad_min + _random.NextDouble() * (pad_max - pad_min)));
            TickStyle direction_ticks_x = directionTicks[_random.Next(_directionTicksCount)];
            TickStyle direction_ticks_y = directionTicks[_random.Next(_directionTicksCount)];

            // FONT AND SIZE FOR LABELS (tick labels, axes labels and title)
            string font = fontList[_random.Next(_fontListCount)];
            int size_ticks = (int)(tick_label_size_min + _random.NextDouble() * (tick_label_size_max - tick_label_size_min));
            int size_axes = (int)(axes_label_size_min + _random.NextDouble() * (axes_label_size_max - axes_label_size_min));
            int size_title = (int)(title_size_min + _random.NextDouble() * (title_size_max - title_size_min));
            //var ticks_font = font_manager.FontProperties(fname = font, style = 'normal', size = size_ticks, weight = 'normal', stretch = 'normal');
            //var axes_font = font_manager.FontProperties(fname = font, style = 'normal', size = size_axes, weight = 'normal', stretch = 'normal');
            //var title_font = font_manager.FontProperties(fname = font, style = 'normal', size = size_title, weight = 'normal', stretch = 'normal');

            // TEXTS FOR AXIS LABELS AND TITLE
            int label_x_length = (int)(axes_label_length_min + _random.NextDouble() * (axes_label_length_max - axes_label_length_min));
            int label_y_length = (int)(axes_label_length_min + _random.NextDouble() * (axes_label_length_max - axes_label_length_min));
            int title_length = (int)(title_length_min + _random.NextDouble() * (title_length_max - title_length_min));
            string x_label = RandomString(label_x_length);
            string y_label = RandomString(label_y_length);
            string title = RandomString(title_length);

            // BUILDING THE PLOT
            PlotModel model = new PlotModel
            {
                Title = title,
                TitleFont = font,
                TitleFontSize = size_title,
                DefaultColors = colors,
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.None, Palette = new OxyPalette(colors), Minimum = 0, Maximum = colors.Length - 1 });

            // TICKS STYLE AND LOCATION (X AXIS)
            LineStyle lineStyle = lineStyles[_random.Next(_lineStylesCount)];
            var xAxis = new LinearAxis
            {
                Position = _random.NextDouble() > 0.5 ? AxisPosition.Top : AxisPosition.Bottom,
                TickStyle = direction_ticks_x,
                MajorGridlineStyle = lineStyle,
                MajorTickSize = tick_size[1],
                MinorTickSize = (_random.NextDouble() > 77) ? 0.75 * _random.NextDouble() * tick_size[1] : 0,
                Angle = _random.NextDouble() > 0.77 ? _random.NextDouble() * (double)updown[_random.Next(2)] * 90 : 0,
                Font = font,
                FontSize = size_ticks,
                TitleFont = font,
                TitleFontSize = size_axes,
                Title = _random.NextDouble() > 0.80 ? "" : x_label,
                PositionAtZeroCrossing = (_random.NextDouble() > 0.77),
                IsAxisVisible = true,
                AxislineColor = OxyColors.Black,
                AxislineStyle = LineStyle.Solid
            };
            model.Axes.Add(xAxis);

            // TICKS STYLE AND LOCATION (Y AXIS)
            var yAxis = new LinearAxis
            {
                Position = _random.NextDouble() > 0.5 ? AxisPosition.Right : AxisPosition.Left,
                TickStyle = direction_ticks_y,
                MajorGridlineStyle = lineStyle,
                MajorTickSize = tick_size[1],
                MinorTickSize = (_random.NextDouble() > 77) ? 0.75 * _random.NextDouble() * tick_size[1] : 0,
                Angle = _random.NextDouble() > 0.77 ? _random.NextDouble() * 90 : 0,
                Font = font,
                FontSize = size_ticks,
                TitleFont = font,
                TitleFontSize = size_axes,
                Title = _random.NextDouble() > 0.80 ? "" : y_label,
                PositionAtZeroCrossing = (_random.NextDouble() > 0.77),
                IsAxisVisible = true,
                AxislineColor = OxyColors.Black,
                AxislineStyle = LineStyle.Solid
            };
            model.Axes.Add(yAxis);

            MarkerType markerType = markers[_random.Next(_markersCount)];

            var scatterSerie = new ScatterSeries()
            {
                MarkerType = markerType,
                MarkerStrokeThickness = _random.Next(10, 31) / 10,
            };

            if (markerType != MarkerType.Cross && markerType != MarkerType.Plus && markerType != MarkerType.Star)
            {
                if (_random.NextDouble() > 0.5)
                {
                    scatterSerie.MarkerStroke = OxyColors.Black;
                }
            }

            List<double> s = new List<double>();
            for (int i = 0; i < points_nb; i++)
            {
                var s_ = s_set[_random.Next(s_set.Length)];
                var c_ = colors[_random.Next(colors.Length)];
                var m_ = markers[_random.Next(_markersCount)];

                bool e_ = false;

                if (markers_empty)
                {
                    e_ = _random.NextDouble() > markers_empty_ratio;
                }

                scatterSerie.Points.Add(new ScatterPoint(xs[i], ys[i], s_, _random.Next(colors.Length)));
            }
            
            model.Series.Add(scatterSerie);
            
            // ##### End plot generation 

            model.InvalidatePlot(true);

            // export as PDF
            using (var stream = File.Create(Path.Combine(RootFoler, ImageFolder, $"plot_{id}.pdf")))
            {
                PdfExporter.Export(model, stream, 600, 400);
            }
            
            // export as PNG
            var pngExporter = new PngExporter()
            {
                Width = figsize[0],
                Height = figsize[1],
                Resolution = dpi
            };

            string imageFileName = $"plot_{id}.png";
            string stimulusFileName = $"plot_{id}_stimulus.png";

            using (Bitmap plotBitmap = pngExporter.ExportToBitmap(model))
            {
                plotBitmap.Save(Path.Combine(RootFoler, ImageFolder, imageFileName), System.Drawing.Imaging.ImageFormat.Png);

                using (var stimulusBitmap = GetStimulusImage(plotBitmap))
                {
                    stimulusBitmap.Save(Path.Combine(RootFoler, ImageFolder, stimulusFileName), System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            var groundTruth = pngExporter.GroundTruth;
            var groundTruthText = pngExporter.GroundTruthText;

            // Save Jsons
            //File.WriteAllText(Path.Combine(RootFoler, PlotsFolder, name + "_model.json"), JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            //File.WriteAllText(Path.Combine(RootFoler, PlotsFolder, name + "_gt.json"), JsonConvert.SerializeObject(groundTruth, Formatting.Indented));
            //File.WriteAllText(Path.Combine(RootFoler, PlotsFolder, name + "_gtt.json"), JsonConvert.SerializeObject(groundTruthText, Formatting.Indented));


            var boxesPoints = get_data_pixel(xAxis, yAxis, dpi, model.Series, groundTruth);

            var boxesTicksTuple = get_tick_pixel(xAxis, yAxis, groundTruth);
            var boxesTicks = boxesTicksTuple.Item1.ToList();
            boxesTicks.AddRange(boxesTicksTuple.Item2);

            var boxesLabelsTuple = get_label_pixel(xAxis, yAxis, figsize[0], figsize[1], dpi, groundTruthText);
            var boxesLabels = boxesLabelsTuple.Item1.ToList();
            boxesLabels.AddRange(boxesLabelsTuple.Item2);

            return (boxesPoints, boxesTicks, boxesLabels);

            /*
            PascalVocAnnotation annotationImage = new PascalVocAnnotation()
            {
                Folder = "ScatterPlot",
                FileName = imageFileName,
                Path = Path.Combine(RootFoler, ImageFolder, imageFileName),
                Source = new PascalVocSource() { Database = "Unknown" },
                Size = new PascalVocSize() { Width = pngExporter.Width, Height = pngExporter.Height, Depth = 3 },
                Segmented = 0,
                Objects = new PascalVocObject[] { }
            };

            PascalVocAnnotation annotationStimulus = new PascalVocAnnotation()
            {
                Folder = "ScatterPlot",
                FileName = stimulusFileName,
                Path = Path.Combine(RootFoler, ImageFolder, stimulusFileName),
                Source = new PascalVocSource() { Database = "Unknown" },
                Size = new PascalVocSize() { Width = pngExporter.Width, Height = pngExporter.Height, Depth = 3 },
                Segmented = 0,
                Objects = new PascalVocObject[] { }
            };

            var vocPoints = boxesPoints.Select(p => new PascalVocObject()
            {
                Name = "point",
                Pose = "Unspecified",
                Truncated = 0,
                Difficult = 0,
                BoundingBox = PascalVocObjectBndBox.FromRectangle(p)
            });

            var vocTicks = boxesTicks.Select(p => new PascalVocObject()
            {
                Name = "tick",
                Pose = "Unspecified",
                Truncated = 0,
                Difficult = 0,
                BoundingBox = PascalVocObjectBndBox.FromRectangle(p)
            });

            var vocLabels = boxesLabels.Select(p => new PascalVocObject()
            {
                Name = "label",
                Pose = "Unspecified",
                Truncated = 0,
                Difficult = 0,
                BoundingBox = PascalVocObjectBndBox.FromRectangle(p)
            });

            List<PascalVocObject> objects = vocPoints.ToList();
            objects.AddRange(vocTicks);
            objects.AddRange(vocLabels);

            // CORRECTIONS & CHECKS
            objects.ForEach(o =>
            {
                if (o.BoundingBox.XMin > pngExporter.Width)
                {
                    o.BoundingBox.XMin = pngExporter.Width;
                }

                if (o.BoundingBox.XMin < 0)
                {
                    o.BoundingBox.XMin = 0;
                }

                if (o.BoundingBox.XMax > pngExporter.Width)
                {
                    o.BoundingBox.XMax = pngExporter.Width;
                }

                if (o.BoundingBox.XMax < 0)
                {
                    o.BoundingBox.XMax = 0;
                }


                if (o.BoundingBox.YMin > pngExporter.Height)
                {
                    o.BoundingBox.YMin = pngExporter.Height;
                }

                if (o.BoundingBox.YMax > pngExporter.Height)
                {
                    o.BoundingBox.YMax = pngExporter.Height;
                }

                if (o.BoundingBox.YMin < 0)
                {
                    o.BoundingBox.YMin = 0;
                }

                if (o.BoundingBox.YMax < 0)
                {
                    o.BoundingBox.YMax = 0;
                }
            });

            var removed = objects.RemoveAll((PascalVocObject o) =>
            {
                if (Math.Abs(o.BoundingBox.XMax - o.BoundingBox.XMin) <= 1) return true;
                if (Math.Abs(o.BoundingBox.YMax - o.BoundingBox.YMin) <= 1) return true;
                return false;
            });
            if (removed > 0) Console.WriteLine(removed.ToString() + " bounding boxes were removed.");
            */

            /*
            annotationImage.Objects = objects.ToArray();
            File.WriteAllText(Path.Combine(RootFoler, AnnotationsFolder, name + ".xml"), _vocSerializer.Serialize(annotationImage));

            annotationStimulus.Objects = objects.ToArray();
            File.WriteAllText(Path.Combine(RootFoler, AnnotationsFolder, name + "_stimulus.xml"), _vocSerializer.Serialize(annotationStimulus));
            */

            // *******************************************************
            // ***************** DRAW BOUNDING BOXES *****************
            // *******************************************************
            /*using (var image = new Bitmap(Path.Combine(RootFoler, ImageFolder, name + ".png")))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    foreach (var box in boxesPoints)
                    {
                        g.DrawRectangle(Pens.YellowGreen, box.X, box.Y, box.Width, box.Height);
                    }

                    foreach (var box in boxesTicksTuple.Item1)
                    {
                        g.DrawRectangle(Pens.Red, box.X, box.Y, box.Width, box.Height);
                    }

                    foreach (var box in boxesTicksTuple.Item2)
                    {
                        g.DrawRectangle(Pens.Red, box.X, box.Y, box.Width, box.Height);
                    }

                    foreach (var box in boxesLabelsTuple.Item1)
                    {
                        g.DrawRectangle(Pens.Orange, box.X, box.Y, box.Width, box.Height);
                    }

                    foreach (var box in boxesLabelsTuple.Item2)
                    {
                        g.DrawRectangle(Pens.DarkGray, box.X, box.Y, box.Width, box.Height);
                    }
                }

                image.Save(Path.Combine(RootFoler, ImageBboxFolder, name + "_bbox.png"));
            }*/
        }

        private static void CheckFolders()
        {
            Directory.CreateDirectory(Path.Combine(RootFoler, ImageFolder));
            Directory.CreateDirectory(Path.Combine(RootFoler, ImageBboxFolder));
            Directory.CreateDirectory(Path.Combine(RootFoler, AnnotationsFolder));
            Directory.CreateDirectory(Path.Combine(RootFoler, PlotsFolder));
        }

        private static Bitmap GetStimulusImage(Bitmap originalBitmap)
        {
            // make sure the image has 24 bpp format
            return getRandomFilter().Apply(originalBitmap);
        }

        static IFilter getRandomFilter()
        {
            var num = _random.Next(10);

            switch (num)
            {
                case 0:
                    double filterLevel = _random.NextDouble();
                    filterLevel = filterLevel < 0.1 ? 0.1 : filterLevel;
                    filterLevel = filterLevel > 0.85 ? 0.85 : filterLevel;

                    double preserveLevel = _random.NextDouble();
                    preserveLevel = preserveLevel < 0.1 ? 0.1 : preserveLevel;
                    preserveLevel = preserveLevel > 0.85 ? 0.85 : preserveLevel;

                    var texture = _textures[_random.Next(_textures.Length)];
                    return new Texturer(texture, filterLevel, preserveLevel);
                case 1:
                    return Grayscale.CommonAlgorithms.BT709;
                case 2:
                    return new Invert();
                case 3:
                    return new RotateChannels();
                case 4:
                    return new HueModifier(_random.Next(180)); // 360 max
                case 5:
                    float adjustValue = _random.Next(-30_000, 30_001) / 100_000f;
                    return new SaturationCorrection(adjustValue);
                case 6:
                    var sigma = _random.Next(40, 401) / 100.0; // 0.5 to 5
                    var kernel = _random.Next(5, 18);
                    return new GaussianBlur(sigma, kernel);
                case 7:
                    return new Sepia();
                case 8:
                    return new Jitter();
                case 9:
                    return new Sharpen();
                default:
                    throw new NotImplementedException();

                    //return new OilPainting(_random.Next(3, 14)); // size 3 to 21
                    //return new ContrastCorrection(_random.Next(-80, 80)); // -127 to 127     
                    //return new BrightnessCorrection(_random.Next(-150, 150)); // -255 to 255
                    //return new SobelEdgeDetector();
                    //return new HomogenityEdgeDetector();
                    //return new DifferenceEdgeDetector();
                    //return new FloydSteinbergDithering();
                    //return new Threshold();
                    //return new OrderedDithering();
                    //return new Sharpen();


                    //var y1 = _random.NextDouble();
                    //var y2 = _random.NextDouble();
                    //var cb1 = _random.Next(-500, 500) / 100.0;
                    //var cb2 = _random.Next(-500, 500) / 100.0;
                    //var cr1 = _random.Next(-500, 500) / 100.0;
                    //var cr2 = _random.Next(-500, 500) / 100.0;
                    //return new YCbCrFiltering(new Range((float)Math.Min(y1, y2), (float)Math.Max(y1, y2)),
                    //    new Range((float)Math.Min(cb1, cb2), (float)Math.Max(cb1, cb2)),
                    //    new Range((float)Math.Min(cr1, cr2), (float)Math.Max(cr1, cr2)));



                    //var sat1 = _random.NextDouble();
                    //var sat2 = _random.NextDouble();
                    //var lum1 = _random.NextDouble();
                    //var lum2 = _random.NextDouble();
                    //return new HSLFiltering(new IntRange(_random.Next(360), _random.Next(360)),
                    //    new Range((float)Math.Min(sat1, sat2), (float)Math.Max(sat1, sat2)),
                    //    new Range((float)Math.Min(lum1, lum2), (float)Math.Max(lum1, lum2)));
            }
        }

        static ITextureGenerator[] _textures = new ITextureGenerator[]
        {
            new TextileTexture(),
            new CloudsTexture(),
            new LabyrinthTexture(),
            //new MarbleTexture(),
            new WoodTexture()
        };


        /// <summary>
        /// Method that return the bouding box of the points.
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="dpi"></param>
        /// <param name="scatterSerie"></param>
        /// <returns>boxes (list) : list of bounding boxes for each points.</returns>
        public static IEnumerable<RectangleF> get_data_pixel(Axis xAxis, Axis yAxis, int dpi, IEnumerable<Series> series, Dictionary<string, List<RectangleF>> groundTruth)
        {
            foreach (var serie in series)
            {
                if (serie is ScatterSeries)
                {
                    var scatterSerie = (ScatterSeries)serie;
                    var screenPoints = scatterSerie.Points.Select(p => xAxis.Transform(p.X, p.Y, yAxis)).ToArray();

                    switch (scatterSerie.MarkerType)
                    {
                        case MarkerType.Circle:
                            var nn = KDTree.FromData(groundTruth["Ellipse"].Select(x => new double[] { x.GetCenter().X, x.GetCenter().Y }).ToArray(),
                                groundTruth["Ellipse"].ToArray(), new Accord.Math.Distances.Euclidean());

                            foreach (var point in screenPoints)
                            {
                                var nearest = nn.Nearest(new double[] { point.X, point.Y });
                                yield return nearest.Value;
                            }
                            break;

                        case MarkerType.Diamond:
                        case MarkerType.Triangle:
                            var nnPolygon = KDTree.FromData(groundTruth["Polygon"].Select(x => new double[] { x.GetCenter().X, x.GetCenter().Y }).ToArray(),
                                groundTruth["Polygon"].ToArray(), new Accord.Math.Distances.Euclidean());

                            foreach (var point in screenPoints)
                            {
                                var nearest = nnPolygon.Nearest(new double[] { point.X, point.Y });
                                yield return nearest.Value;
                            }
                            break;

                        case MarkerType.Square:
                            var nnRectangle = KDTree.FromData(groundTruth["Rectangle"].Select(x => new double[] { x.GetCenter().X, x.GetCenter().Y }).ToArray(),
                                groundTruth["Rectangle"].ToArray(), new Accord.Math.Distances.Euclidean());

                            foreach (var point in screenPoints)
                            {
                                var nearest = nnRectangle.Nearest(new double[] { point.X, point.Y });
                                yield return nearest.Value;
                            }
                            break;

                        case MarkerType.Plus:
                        case MarkerType.Cross:
                        case MarkerType.Star:
                            var nnLines = KDTree.FromData(groundTruth["Line"].Select(x => new double[] { x.GetCenter().X, x.GetCenter().Y }).ToArray(),
                                groundTruth["Line"].ToArray(), new Accord.Math.Distances.Euclidean());
                            int count = (scatterSerie.MarkerType == MarkerType.Plus || scatterSerie.MarkerType == MarkerType.Cross) ? 2 : 4;
                            foreach (var point in screenPoints)
                            {
                                var nearest = nnLines.Nearest(new double[] { point.X, point.Y }, count);
                                var rect = nearest[0].Node.Value;

                                for (int i = 1; i < count; i++)
                                {
                                    rect = rect.Union(nearest[i].Node.Value);
                                }
                                yield return rect;
                            }
                            break;

                        case MarkerType.Custom:
                        case MarkerType.None:
                        default:
                            throw new NotImplementedException("get_data_pixel(): " + scatterSerie.MarkerType);
                    }
                }
                else
                {
                    throw new NotImplementedException("get_data_pixel(): " + serie.GetType().ToString());
                }
            }
        }

        /// <summary>
        /// [OK] Method that return the bouding box of the ticks.
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="dpi"></param>
        /// <returns>boxes_x, boxes_y (list, list) : list of bounding boxes for each ticks.</returns>
        public static Tuple<IEnumerable<RectangleF>, IEnumerable<RectangleF>> get_tick_pixel(Axis xAxis, Axis yAxis, 
            Dictionary<string, List<RectangleF>> groundTruth)
        {
            List<RectangleF> boxes_x = new List<RectangleF>();
            List<RectangleF> boxes_y = new List<RectangleF>();
            xAxis.GetTickValues(out IList<double> labelValuesX, out IList<double> majorTicksX, out IList<double> minorTicksX);
            yAxis.GetTickValues(out IList<double> labelValuesY, out IList<double> majorTicksY, out IList<double> minorTicksY);

            var groundTruthTicksX = groundTruth["Line"].Where(r => r.Width == 0);
            foreach (var tickValue in majorTicksX.Where(t => t > xAxis.ActualMinimum).Where(t => t < xAxis.ActualMaximum))
            {
                float x = (float)xAxis.Transform(tickValue);
                var ticks = groundTruthTicksX.Where(r => r.X == x).ToList();

                if (ticks.Count == 0)
                {
                    ticks = groundTruthTicksX.Where(r => Math.Round(r.X, 3) == Math.Round(x, 3)).ToList();
                    if (ticks.Count == 0)
                    {
                        Console.WriteLine("Missing " + x);
                        continue;
                    }
                }

                float minHeight = ticks.Min(t => t.Height);
                var selected = ticks.Where(t => t.Height == minHeight).First(); // take the smallest
                float newHeight = selected.Height < 10 ? 10 : selected.Height;
                float newWidth = newHeight;
                float y0 = (selected.Top + selected.Bottom) / 2f;

                groundTruth["Line"].Remove(selected);
                boxes_x.Add(new RectangleF(x - newWidth / 2f, y0 - newHeight / 2f, newWidth, newHeight));
            }

            var groundTruthTicksY = groundTruth["Line"].Where(r => r.Height == 0);
            foreach (var tickValue in majorTicksY.Where(t => t > yAxis.ActualMinimum).Where(t => t < yAxis.ActualMaximum))
            {
                float y = (float)yAxis.Transform(tickValue);
                var ticks = groundTruthTicksY.Where(r => r.Y == y).ToList();

                if (ticks.Count == 0)
                {
                    ticks = groundTruthTicksY.Where(r => Math.Round(r.Y, 3) == Math.Round(y, 3)).ToList();
                    if (ticks.Count == 0)
                    {
                        Console.WriteLine("Missing " + y);
                        continue;
                    }
                }

                float minWidth = ticks.Min(t => t.Width);
                var selected = ticks.Where(t => t.Width == minWidth).First(); // take the shortest line, other should be a grid line
                float newWidth = selected.Width < 10 ? 10 : selected.Width;
                float newHeight = newWidth;
                float x0 = (selected.Left + selected.Right) / 2f;

                groundTruth["Line"].Remove(selected);
                boxes_x.Add(new RectangleF(x0 - newWidth / 2f, y - newHeight / 2.0f, newWidth, newHeight));
            }

            return new Tuple<IEnumerable<RectangleF>, IEnumerable<RectangleF>>(boxes_x, boxes_y);
        }

        /// <summary>
        /// Method that return the value of the labels.
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <returns>x_labels, y_labels (list, list) : list of the values for each labels.</returns>
        public static Tuple<IEnumerable<double>, IEnumerable<double>> get_label_value(Axis xAxis, Axis yAxis)
        {
            xAxis.GetTickValues(out IList<double> labelValuesX, out IList<double> majorTicksX, out IList<double> minorTicksX);
            yAxis.GetTickValues(out IList<double> labelValuesY, out IList<double> majorTicksY, out IList<double> minorTicksY);
            return new Tuple<IEnumerable<double>, IEnumerable<double>>(
                labelValuesX.Where(t => t > xAxis.ActualMinimum).Where(t => t < xAxis.ActualMaximum),
                labelValuesY.Where(t => t > yAxis.ActualMinimum).Where(t => t < yAxis.ActualMaximum));
        }

        /// <summary>
        /// Method that return the bouding box of the labels.
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <returns>x_label_bounds, y_label_bounds (list, list) : list of bounding boxes for each labels.</returns>
        public static Tuple<IEnumerable<RectangleF>, IEnumerable<RectangleF>> get_label_pixel(Axis xAxis, Axis yAxis, int Width, int Height, int dpi, List<Tuple<string, RectangleF>> groundTruthText)
        {
            List<RectangleF> boxes_x = new List<RectangleF>();
            List<RectangleF> boxes_y = new List<RectangleF>();
            xAxis.GetTickValues(out IList<double> labelValuesX, out IList<double> majorTicksX, out IList<double> minorTicksX);
            yAxis.GetTickValues(out IList<double> labelValuesY, out IList<double> majorTicksY, out IList<double> minorTicksY);
            labelValuesX = labelValuesX.Where(t => t > xAxis.ActualMinimum).Where(t => t < xAxis.ActualMaximum).ToList();
            labelValuesY = labelValuesY.Where(t => t > yAxis.ActualMinimum).Where(t => t < yAxis.ActualMaximum).ToList();

            foreach (var labelValue in labelValuesX)
            {
                string label = xAxis.FormatValue(labelValue);
                var candidates = groundTruthText.Where(t => t.Item1 == label).ToList();

                if (candidates.Count == 0)
                {
                    if (xAxis.PositionAtZeroCrossing && labelValue == 0)
                    {
                        continue;
                    }
                    throw new ArgumentNullException();
                }

                if (candidates.Count == 1)
                {
                    var pt = candidates.First();
                    boxes_x.Add(pt.Item2);
                    groundTruthText.Remove(pt);
                }
                else
                {
                    var cp = xAxis.Transform(labelValue);
                    var minDist = candidates.Min(x => Math.Abs(x.Item2.GetCenter().X - cp));
                    var pt = candidates.Where(x => Math.Abs(x.Item2.GetCenter().X - cp) == minDist).First();
                    boxes_x.Add(pt.Item2);
                    groundTruthText.Remove(pt);
                }
            }

            foreach (var labelValue in labelValuesY)
            {
                string label = yAxis.FormatValue(labelValue);
                var candidates = groundTruthText.Where(t => t.Item1 == label).ToList();

                if (candidates.Count == 0)
                {
                    if (yAxis.PositionAtZeroCrossing && labelValue == 0)
                    {
                        continue;
                    }
                    throw new ArgumentNullException();
                }

                if (candidates.Count == 1)
                {
                    var pt = candidates.First();
                    boxes_y.Add(pt.Item2);
                    groundTruthText.Remove(pt);
                }
                else
                {
                    var cp = yAxis.Transform(labelValue);
                    var minDist = candidates.Min(x => Math.Abs(x.Item2.GetCenter().Y - cp));
                    var pt = candidates.Where(x => Math.Abs(x.Item2.GetCenter().Y - cp) == minDist).First();
                    boxes_y.Add(pt.Item2);
                    groundTruthText.Remove(pt);
                }
            }

            return new Tuple<IEnumerable<RectangleF>, IEnumerable<RectangleF>>(boxes_x, boxes_y);
        }

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,0123456789       ";

        /// <summary>
        /// https://stackoverflow.com/questions/29859011/changing-title-font-in-wpf-oxyplot
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray()).Trim();
        }
    }
}
