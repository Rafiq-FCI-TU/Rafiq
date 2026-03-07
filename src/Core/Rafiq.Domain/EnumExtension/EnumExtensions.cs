using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rafiq.Domain.EnumExtension;
public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var member = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault();

        return member?
            .GetCustomAttribute<DisplayAttribute>()?
            .Name ?? value.ToString();
    }
}