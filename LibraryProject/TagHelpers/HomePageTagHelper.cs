using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace LibraryProject.TagHelpers
{
    public class HomePageTagHelper : TagHelper
    {
        private const string Class = "navbar-brand";
        private const string href = "/";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            output.Attributes.SetAttribute("class", Class);
            output.Attributes.SetAttribute("href", href);
            output.Content.SetContent("Library Project");
        }
    }
}