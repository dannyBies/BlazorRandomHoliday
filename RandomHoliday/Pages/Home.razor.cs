using System.Collections.Generic;

namespace RandomHoliday.Pages
{
    public partial class Home
    {
        public IList<string> BackgroundImages { get; set; } = new List<string>
        {
            "images/image1.jpg",
            "images/image2.jpg",
            "images/image3.jpg",
        };
    }
}