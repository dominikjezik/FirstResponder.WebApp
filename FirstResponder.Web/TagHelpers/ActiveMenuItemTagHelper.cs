using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace FirstResponder.Web.TagHelpers;

/// <summary>
/// Tag helper is mainly for the main navigation menu. It adds the class "active" to the given HTML element
/// if the user is on the given subpage. The subpage is specified via the is-active-controller and possibly
/// is-active-action and one or more is-active-parameter-NAME attributes.
/// </summary>
[HtmlTargetElement(Attributes = "is-active-controller")]
public class ActiveMenuItemTagHelper : TagHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActiveMenuItemTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HtmlAttributeName("is-active-controller")]
    public string? IsActiveController { get; set; }

    [HtmlAttributeName("is-active-action")]
    public string? IsActiveAction { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var currentController = _httpContextAccessor.HttpContext.GetRouteValue("controller").ToString();
        var currentAction = _httpContextAccessor.HttpContext.GetRouteValue("action").ToString();

        if (!string.Equals(currentController, IsActiveController, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        
        if (IsActiveAction is null)
        {
            output.AddClass("selected", HtmlEncoder.Default);
            return;
        }

        if (!string.Equals(currentAction, IsActiveAction, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        
        var dynamicAttributes = output.Attributes
            .Where(attr => attr.Name.StartsWith("is-active-parameter-"))
            .ToList();

        foreach (var attribute in dynamicAttributes)
        {
            var attributeKey = attribute.Name.Replace("is-active-parameter-", "");
            var attributeValue = attribute.Value.ToString();

            if (_httpContextAccessor.HttpContext.GetRouteData().Values.TryGetValue(attributeKey, out var routeKeyValue))
            {
                var routeKeyValueString = routeKeyValue.ToString();

                if (!routeKeyValueString.Equals(attributeValue, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }
        }

        output.AddClass("selected", HtmlEncoder.Default);
    }
}