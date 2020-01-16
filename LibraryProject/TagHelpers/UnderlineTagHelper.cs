using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LibraryProject.TagHelpers
{
    [HtmlTargetElement(Attributes = "underline")]
    public class UnderlineTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("underline");
            output.PreContent.SetHtmlContent("<u>");
            output.PostContent.SetHtmlContent("</u>");
        }
    }
}
