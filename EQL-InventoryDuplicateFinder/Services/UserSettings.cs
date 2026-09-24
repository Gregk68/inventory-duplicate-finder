using System.Configuration;

namespace InventoryDuplicateFinder.Services;

internal sealed class UserSettings : ApplicationSettingsBase
{
    private static readonly UserSettings _defaultInstance = (UserSettings)Synchronized(new UserSettings());

    public static UserSettings Default => _defaultInstance;

    [UserScopedSetting]
    [DefaultSettingValue("")]
    public string IgnoredIds
    {
        get => (string)(this[nameof(IgnoredIds)] ?? string.Empty);
        set => this[nameof(IgnoredIds)] = value;
    }
}
