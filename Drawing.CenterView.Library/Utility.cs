using System.ComponentModel;
using System.Reflection;

namespace Drawing.CenterView.Library;

public static class Utility
{
  /// <summary>
  /// Gets a value from the description of an object. This is useful for enumerations.
  /// </summary>
  /// <param name="description"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  /// <exception cref="ArgumentException"></exception>
    public static T GetValueFromDescription<T>(string description)
    {
      var type = typeof (T);
      if (!type.IsEnum)
        throw new InvalidOperationException();
      foreach (var field in type.GetFields())
      {
        if (Attribute.GetCustomAttribute((MemberInfo) field, typeof (DescriptionAttribute)) is DescriptionAttribute customAttribute)
        {
          if (customAttribute.Description == description)
            return (T) field.GetValue((object) null);
        }
        else if (field.Name == description)
          return (T) field.GetValue((object) null);
      }
      throw new ArgumentException("Not found.", "Description");
    }
}