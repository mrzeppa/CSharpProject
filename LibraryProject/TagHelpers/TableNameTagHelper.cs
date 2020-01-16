using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LibraryProject.TagHelpers
{
    public class TableNameTagHelper : TagHelper
    {
        public string Name { get; set; }

        private const string Class = "titleFormat float-left";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "h1";
            output.Attributes.SetAttribute("class", Class);
            output.Content.SetContent(Name);
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}