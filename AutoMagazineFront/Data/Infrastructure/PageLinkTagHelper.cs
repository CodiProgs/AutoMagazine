using AutoMagazineFront.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AutoMagazineFront.Data.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper(IUrlHelperFactory factory) : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory = factory;

        [ViewContext]
        [HtmlAttributeNotBound]
        public required ViewContext ViewContext { get; set; }
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = [];
        public required string PageAction { get; set; }
        public required PagingInfo PageModel { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public required string Class { get; set; }
        public required string PageClass { get; set; }
        public required string PageClassNormal { get; set; }
        public required string PageClassSelected { get; set; }
        public Guid? PageCurrentCategoryId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new("div");

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder aTag = new("a");

                PageUrlValues["page"] = i;
                PageUrlValues["catId"] = PageCurrentCategoryId == null ? 0 : PageCurrentCategoryId;

                aTag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                if (PageClassesEnabled)
                {
                    aTag.AddCssClass(PageClass);
                    aTag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                }
                aTag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(aTag);
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
