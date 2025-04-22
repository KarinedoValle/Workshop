using System.ComponentModel;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

        return attribute == null ? value.ToString() : attribute.Description;
    }


    public static T GetEnumByDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            DescriptionAttribute? attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (attribute != null && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
            {
                return (T)field.GetValue(null);
            }
        }
        throw new ArgumentException($"No enum value found for description '{description}'", nameof(description));
    }


    public static List<string> GetEnumDescriptions<T>() where T : Enum
    {
        return typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(f => f.GetCustomAttribute<DescriptionAttribute>()?.Description ?? f.Name)
        .ToList();
    }


}
