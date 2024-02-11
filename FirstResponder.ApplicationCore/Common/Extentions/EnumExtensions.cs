using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FirstResponder.ApplicationCore.Common.Extentions;

public static class EnumExtensions
{
    public static string? GetDisplayAttributeValue(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName();
    }
}