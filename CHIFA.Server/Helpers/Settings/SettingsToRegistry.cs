using System.ComponentModel;
using CHIFA.Server.Helpers;
using Microsoft.Win32;

namespace CHIFA.Pro.Helpers.Settings;

public class SettingsToRegistry<T> where T : class, new()
{
    private static string KeyName =>
        $"SOFTWARE\\{typeof(SettingsToRegistry<>).Assembly.GetName().Name}\\{typeof(T).Name}";

    private static List<string> Properties =>
        typeof(T).GetProperties().Where(x => x.CanWrite).Select(x => x.Name).ToList();

    public static T Default { get; private set; } = Deserialize();

    private static T Deserialize()
    {
        T settings = new();
        foreach (var name in Properties)
            try
            {
                if (Registry.CurrentUser.CreateSubKey(KeyName)?.GetValue(name) is not { } value) continue;
                var prop = typeof(T).GetProperty(name);
                var newVal = TypeDescriptor.GetConverter(prop!.PropertyType).ConvertFromString(value.ToString()!);
                prop.SetValue(settings, newVal);
            }
            catch (Exception ex)
            {
                ex.Log();
            }

        return settings;
    }

    public void Reset()
    {
        Default = Deserialize();
    }

    public void Save()
    {
        try
        {
            foreach (var name in Properties)
            {
                object? value = typeof(T).GetProperty(name)?.GetValue(this)?.ToString();
                if (value is null) continue;
                Registry.CurrentUser.CreateSubKey(KeyName)?.SetValue(name, value!);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}