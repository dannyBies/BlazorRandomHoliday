using Microsoft.AspNetCore.Components;

namespace RandomHoliday.Components
{
    public partial class BackgroundImage
    {
        [Parameter]
        public string ImageLocation { get; set; }
    }
}