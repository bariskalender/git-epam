using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Infrastructure;

[HtmlTargetElement("div", Attributes = "page-model")]
public class PageLinkTagHelper : TagHelper
{
    private readonly IUrlHelperFactory urlHelperFactory;

    public PageLinkTagHelper(IUrlHelperFactory helperFactory)
    {
        urlHelperFactory = helperFactory;
    }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public PagingInfo PageModel { get; set; } = new();

    public string PageAction { get; set; } = string.Empty;

    public override void Process(
        TagHelperContext context,
        TagHelperOutput output)
    {
        IUrlHelper urlHelper =
            urlHelperFactory.GetUrlHelper(ViewContext);

        TagBuilder result = new("div");

        for (int i = 1; i <= PageModel.TotalPages; i++)
        {
            TagBuilder tag = new("a");

            tag.Attributes["href"] =
                urlHelper.Action(
                    PageAction,
                    new { productPage = i });

            tag.InnerHtml.Append(
                i.ToString(
                    System.Globalization.CultureInfo.InvariantCulture));

            tag.AddCssClass("btn");

            tag.AddCssClass(
                i == PageModel.CurrentPage
                    ? "btn-primary"
                    : "btn-outline-secondary");

            result.InnerHtml.AppendHtml(tag);
        }

        output.Content.AppendHtml(result.InnerHtml);
    }
}
