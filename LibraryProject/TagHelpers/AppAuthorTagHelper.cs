using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LibraryProject.TagHelpers
{
    public class AppAuthorTagHelper : TagHelper
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IndexNumber { get; set; }

        private const string ulClass = "list-group list-group-flush selectSize";
        private const string liClass = "list-group-item fontBlack selectSize";


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.SetHtmlContent($@"
                <ul class={ulClass}>
                    <li class={liClass}><b>Name:</b> {Name}</li>
                    <li class={liClass}><b>Surname:</b> {Surname}</li>
                    <li class={liClass}><b>Index number:</b> {IndexNumber}</li>
                </ul>");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
