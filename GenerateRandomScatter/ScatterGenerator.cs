using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateRandomScatter
{
    public class ScatterGenerator
    {
        const int dpi_min = 85;
        const int dpi_max = 250;

        const int figsize_min = 500;
        const int figsize_max = 1500;

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



        public ScatterGenerator()
        {
            _random = new Random(42);
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

            var distribution = Resources.point_dist[_random.Next(4)];
            if (distribution == "uniform")
            {
                ys = UniformContinuousDistribution.Random(y_min, y_max, points_nb, _random);
            }
            else if (distribution == "linear")
            {
                ys = xs.Multiply((Math.Max(y_max, -y_min) / (Math.Max(x_max, -x_min)))).Multiply((double)Resources.updown[_random.Next(2)])
                    .Add(UniformContinuousDistribution.Random(0, 1, points_nb, _random).Add(y_min).Multiply((y_max - y_min)).Multiply(_random.NextDouble() / 2.0));
            }
            else if (distribution == "quadratic")
            {
                ys = xs.Pow(2).Multiply(1.0 / (Math.Max(x_max, -x_min))).Pow(2).Multiply(Math.Max(y_max, -y_min)).Multiply((double)Resources.updown[_random.Next(2)])
                    .Add(UniformContinuousDistribution.Random(0, 1, points_nb, _random).Add(y_min).Multiply((y_max - y_min)).Multiply(_random.NextDouble() / 2.0));
            }
            else if (distribution == "binormal")
            {
                double stdDev = Math.Max(y_max, -y_min) / Math.Max(x_max, -x_min);
                double correl = (double)Resources.updown[_random.Next(2)] * _random.NextDouble();
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
                markers_subset.Add(Resources.Markers[_random.Next(Resources._markersCount)]);
            }

            var markers_empty = _random.NextDouble() > 0.75;
            var markers_empty_ratio = new[] { 0.0, 0.5, 0.7 }[_random.Next(3)];



            // PAD BETWEEN TICKS AND LABELS
            double pad_x = Math.Max(tick_size[1] + 0.5, (int)(pad_min + _random.NextDouble() * (pad_max - pad_min)));
            double pad_y = Math.Max(tick_size[1] + 0.5, (int)(pad_min + _random.NextDouble() * (pad_max - pad_min)));
            TickStyle direction_ticks_x = Resources.DirectionTicks[_random.Next(Resources._directionTicksCount)];
            TickStyle direction_ticks_y = Resources.DirectionTicks[_random.Next(Resources._directionTicksCount)];

            // FONT AND SIZE FOR LABELS (tick labels, axes labels and title)
            string font = Resources.FontList[_random.Next(Resources._fontListCount)];
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
            LineStyle lineStyle = Resources.LineStyles[_random.Next(Resources._lineStylesCount)];
            var xAxis = new LinearAxis
            {
                Position = _random.NextDouble() > 0.5 ? AxisPosition.Top : AxisPosition.Bottom,
                TickStyle = direction_ticks_x,
                MajorGridlineStyle = lineStyle,
                MajorTickSize = tick_size[1],
                MinorTickSize = (_random.NextDouble() > 77) ? 0.75 * _random.NextDouble() * tick_size[1] : 0,
                Angle = _random.NextDouble() > 0.77 ? _random.NextDouble() * (double)Resources.updown[_random.Next(2)] * 90 : 0,
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

            MarkerType markerType = Resources.Markers[_random.Next(Resources._markersCount)];

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
                var m_ = Resources.Markers[_random.Next(Resources._markersCount)];

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
