using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateRandomScatter.Coco
{
    public class CocoFile
    {
        public CocoInfo info { get; set; }
        public CocoLicense[] licenses { get; set; }
        public CocoImage[] images { get; set; }
        public CocoAnnotation[] annotations { get; set; }
        public CocoCategory[] categories { get; set; }
    }

    public class CocoInfo
    {
        public string description { get; set; }
        public string url { get; set; }
        public string version { get; set; }
        public int year { get; set; }
        public string contributor { get; set; }
        public string date_created { get; set; }
    }

    public class CocoLicense
    {
        public string url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CocoImage
    {
        public int license { get; set; }
        public string file_name { get; set; }
        public int height { get; set; }
        public int width { get; set; }

        /// <summary>
        /// Format '2013-11-14 22:32:02'
        /// </summary>
        public DateTime date_captured { get; set; }
        public int id { get; set; }
    }

    public class CocoAnnotation
    {
        public float[][] segmentation { get; set; }
        public float area { get; set; }
        public int iscrowd { get; set; }
        public int image_id { get; set; }

        /// <summary>
        /// [x,y,width,height]
        /// </summary>
        public float[] bbox { get; set; }
        public int category_id { get; set; }
        public long id { get; set; }
    }

    public class CocoCategory
    {
        public string supercategory { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

}
