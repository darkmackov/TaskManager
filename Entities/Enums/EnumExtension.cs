using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TaskManager.Entities.Enums
{
    /// <summary>
    /// Class extending the functionality of enums.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the display name of an enum item using the DisplayAttribute.
        /// </summary>
        public static string GetDisplayName(this Enum item)
        {
            return item.GetType()
                .GetMember(item.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?.Name ?? item.ToString();
        }
    }
}
