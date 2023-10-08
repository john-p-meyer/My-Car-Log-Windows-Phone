using CarLog.Core.Helpers;

namespace CarLog.Core
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedVariableStrings
    {
        private static LocalizedVariableSettingsStrings _localizedVariableSettingsStrings = new LocalizedVariableSettingsStrings();

        public static LocalizedVariableSettingsStrings LocalizedVariableSettingsStrings { get { return _localizedVariableSettingsStrings; } }
    }
}
