using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateRandomScatter
{
    public static class Resources
    {
        static Resources()
        {
            _markersCount = Markers.Length;
            _lineStylesCount = LineStyles.Length;
            _oxyColorsCount = OxyColorArray.Length;
            _directionTicksCount = DirectionTicks.Length;
            _fontListCount = FontList.Length;
        }

        public static readonly int[] updown = new[] { -1, 1 };
        public static readonly string[] point_dist = new string[] { "uniform", "linear", "quadratic", "binormal" };

        public static readonly MarkerType[] Markers = new MarkerType[]
        {
            MarkerType.Circle,
            MarkerType.Cross,
            MarkerType.Diamond,
            MarkerType.Plus,
            MarkerType.Square,
            MarkerType.Star,
            MarkerType.Triangle
        };
        public static int _markersCount = 0;

        public static readonly LineStyle[] LineStyles = new LineStyle[]
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
        public static int _lineStylesCount = 0;

        public static readonly OxyColor[] OxyColorArray = new OxyColor[]
        {
            OxyPlot.OxyColors.AliceBlue,
            OxyPlot.OxyColors.AntiqueWhite,
            OxyPlot.OxyColors.Aqua,
            OxyPlot.OxyColors.Aquamarine,
            OxyPlot.OxyColors.Azure,
            OxyPlot.OxyColors.Beige,
            OxyPlot.OxyColors.Bisque,
            OxyPlot.OxyColors.Black,
            OxyPlot.OxyColors.BlanchedAlmond,
            OxyPlot.OxyColors.Blue,
            OxyPlot.OxyColors.BlueViolet,
            OxyPlot.OxyColors.Brown,
            OxyPlot.OxyColors.BurlyWood,
            OxyPlot.OxyColors.CadetBlue,
            OxyPlot.OxyColors.Chartreuse,
            OxyPlot.OxyColors.Chocolate,
            OxyPlot.OxyColors.Coral,
            OxyPlot.OxyColors.CornflowerBlue,
            OxyPlot.OxyColors.Cornsilk,
            OxyPlot.OxyColors.Crimson,
            OxyPlot.OxyColors.Cyan,
            OxyPlot.OxyColors.DarkBlue,
            OxyPlot.OxyColors.DarkCyan,
            OxyPlot.OxyColors.DarkGoldenrod,
            OxyPlot.OxyColors.DarkGray,
            OxyPlot.OxyColors.DarkGreen,
            OxyPlot.OxyColors.DarkKhaki,
            OxyPlot.OxyColors.DarkMagenta,
            OxyPlot.OxyColors.DarkOliveGreen,
            OxyPlot.OxyColors.DarkOrange,
            OxyPlot.OxyColors.DarkOrchid,
            OxyPlot.OxyColors.DarkRed,
            OxyPlot.OxyColors.DarkSalmon,
            OxyPlot.OxyColors.DarkSeaGreen,
            OxyPlot.OxyColors.DarkSlateBlue,
            OxyPlot.OxyColors.DarkSlateGray,
            OxyPlot.OxyColors.DarkTurquoise,
            OxyPlot.OxyColors.DarkViolet,
            OxyPlot.OxyColors.DeepPink,
            OxyPlot.OxyColors.DeepSkyBlue,
            OxyPlot.OxyColors.DimGray,
            OxyPlot.OxyColors.DodgerBlue,
            OxyPlot.OxyColors.Firebrick,
            OxyPlot.OxyColors.FloralWhite,
            OxyPlot.OxyColors.ForestGreen,
            OxyPlot.OxyColors.Fuchsia,
            OxyPlot.OxyColors.Gainsboro,
            //OxyColors.GhostWhite,
            OxyPlot.OxyColors.Gold,
            OxyPlot.OxyColors.Goldenrod,
            OxyPlot.OxyColors.Gray,
            OxyPlot.OxyColors.Green,
            OxyPlot.OxyColors.GreenYellow,
            OxyPlot.OxyColors.Honeydew,
            OxyPlot.OxyColors.HotPink,
            OxyPlot.OxyColors.IndianRed,
            OxyPlot.OxyColors.Indigo,
            OxyPlot.OxyColors.Ivory,
            OxyPlot.OxyColors.Khaki,
            OxyPlot.OxyColors.Lavender,
            OxyPlot.OxyColors.LavenderBlush,
            OxyPlot.OxyColors.LawnGreen,
            OxyPlot.OxyColors.LemonChiffon,
            OxyPlot.OxyColors.LightBlue,
            OxyPlot.OxyColors.LightCoral,
            OxyPlot.OxyColors.LightCyan,
            OxyPlot.OxyColors.LightGoldenrodYellow,
            OxyPlot.OxyColors.LightGray,
            OxyPlot.OxyColors.LightGreen,
            OxyPlot.OxyColors.LightPink,
            OxyPlot.OxyColors.LightSalmon,
            OxyPlot.OxyColors.LightSeaGreen,
            OxyPlot.OxyColors.LightSkyBlue,
            OxyPlot.OxyColors.LightSlateGray,
            OxyPlot.OxyColors.LightSteelBlue,
            OxyPlot.OxyColors.LightYellow,
            OxyPlot.OxyColors.Lime,
            OxyPlot.OxyColors.LimeGreen,
            OxyPlot.OxyColors.Linen,
            OxyPlot.OxyColors.Magenta,
            OxyPlot.OxyColors.Maroon,
            OxyPlot.OxyColors.MediumAquamarine,
            OxyPlot.OxyColors.MediumBlue,
            OxyPlot.OxyColors.MediumOrchid,
            OxyPlot.OxyColors.MediumPurple,
            OxyPlot.OxyColors.MediumSeaGreen,
            OxyPlot.OxyColors.MediumSlateBlue,
            OxyPlot.OxyColors.MediumSpringGreen,
            OxyPlot.OxyColors.MediumTurquoise,
            OxyPlot.OxyColors.MediumVioletRed,
            OxyPlot.OxyColors.MidnightBlue,
            OxyPlot.OxyColors.MintCream,
            OxyPlot.OxyColors.MistyRose,
            OxyPlot.OxyColors.Moccasin,
            //OxyColors.NavajoWhite,
            OxyPlot.OxyColors.Navy,
            OxyPlot.OxyColors.OldLace,
            OxyPlot.OxyColors.Olive,
            OxyPlot.OxyColors.OliveDrab,
            OxyPlot.OxyColors.Orange,
            OxyPlot.OxyColors.OrangeRed,
            OxyPlot.OxyColors.Orchid,
            OxyPlot.OxyColors.PaleGoldenrod,
            OxyPlot.OxyColors.PaleGreen,
            OxyPlot.OxyColors.PaleTurquoise,
            OxyPlot.OxyColors.PaleVioletRed,
            OxyPlot.OxyColors.PapayaWhip,
            OxyPlot.OxyColors.PeachPuff,
            OxyPlot.OxyColors.Peru,
            OxyPlot.OxyColors.Pink,
            OxyPlot.OxyColors.Plum,
            OxyPlot.OxyColors.PowderBlue,
            OxyPlot.OxyColors.Purple,
            OxyPlot.OxyColors.Red,
            OxyPlot.OxyColors.RosyBrown,
            OxyPlot.OxyColors.RoyalBlue,
            OxyPlot.OxyColors.SaddleBrown,
            OxyPlot.OxyColors.Salmon,
            OxyPlot.OxyColors.SandyBrown,
            OxyPlot.OxyColors.SeaGreen,
            OxyPlot.OxyColors.SeaGreen,
            OxyPlot.OxyColors.SeaShell,
            OxyPlot.OxyColors.Sienna,
            OxyPlot.OxyColors.Silver,
            OxyPlot.OxyColors.SkyBlue,
            OxyPlot.OxyColors.SlateBlue,
            OxyPlot.OxyColors.SlateGray,
            OxyPlot.OxyColors.Snow,
            OxyPlot.OxyColors.SpringGreen,
            OxyPlot.OxyColors.SteelBlue,
            OxyPlot.OxyColors.Tan,
            OxyPlot.OxyColors.Teal,
            OxyPlot.OxyColors.Thistle,
            OxyPlot.OxyColors.Tomato,
            OxyPlot.OxyColors.Transparent,
            OxyPlot.OxyColors.Turquoise,
            OxyPlot.OxyColors.Violet,
            OxyPlot.OxyColors.Wheat,
            //OxyColors.White,
            //OxyColors.WhiteSmoke,
            OxyPlot.OxyColors.Yellow,
            OxyPlot.OxyColors.YellowGreen
        };
        static int _oxyColorsCount = 0;

        public static readonly TickStyle[] DirectionTicks = new TickStyle[]
        {
            TickStyle.Crossing,
            TickStyle.Inside,
            TickStyle.Outside,
        };
        public static int _directionTicksCount = 0;

        public static readonly string[] FontList = new[]
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
        public static int _fontListCount = 0;
    }
}
