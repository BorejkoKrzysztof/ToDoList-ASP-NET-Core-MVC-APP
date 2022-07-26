using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ToDoListInfrastructure.Models.ViewModels;

namespace ToDoList.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _urlHelperFactory = helperFactory ?? throw new ArgumentNullException(nameof(helperFactory));
        }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();


        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }

        public string PageAction { get; set; }

        public bool PageClassesEnabled { get; set; } = false;

        public string WrapperClass { get; set; }

        public string PageClass { get; set; }

        public string PageClassNormal { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new TagBuilder("div");
            result.AddCssClass(WrapperClass);

            TagBuilder first = new TagBuilder("a");
            first.Attributes["href"] = urlHelper.Action(PageAction, new { listPage = 1 });

            first.AddCssClass(PageClass);
            first.AddCssClass(PageClassNormal);
            first.Attributes["style"] = "margin: 0 2px";

            first.InnerHtml.Append("First");
            result.InnerHtml.AppendHtml(first);

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");

                tag.Attributes["href"] = urlHelper.Action(PageAction, new { listPage = i });

                if (PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(PageClassNormal);
                    tag.Attributes["style"] = "margin: 0 2px";
                }

                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);
            }

            TagBuilder last = new TagBuilder("a");
            last.Attributes["href"] = urlHelper.Action(PageAction, new { listPage = PageModel.TotalPages !=0 ?
                                                                                      PageModel.TotalPages
                                                                                      :
                                                                                      1});
            last.Attributes["style"] = "margin: 0 2px";

            last.AddCssClass(PageClass);
            last.AddCssClass(PageClassNormal);
            last.InnerHtml.Append("Last");
            result.InnerHtml.AppendHtml(last);

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
