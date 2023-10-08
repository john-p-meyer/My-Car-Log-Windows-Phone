using System;
using System.Windows;

namespace CarLog.Core.Helpers
{
    /// <summary>
    /// Converts an image path so that it can change based on the theme selected
    /// Images need to be placed in either the Dark or Light folder to differentiate theme
    /// </summary>
    public static class ThemeImage
    {
        /// <summary>
        /// Converts a string path to use the correct theme by replacing [THEME] with either Dark or Light
        /// </summary>
        /// <param name="sourcePath">The string path containing [THEME] to change</param>
        /// <returns>Returns sourcePath with the correct theme applied</returns>
        public static String Path(String sourcePath)
        {
            Visibility darkBackgroundVisibility = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];
            
            return sourcePath.Replace("[THEME]", darkBackgroundVisibility == Visibility.Visible ? "Dark" : "Light");
        }

        /// <summary>
        /// Converts a string path to use the correct theme by replacing [THEME] with either Dark or Light and returns the URI
        /// </summary>
        /// <param name="sourcePath">Converts a string path to use the correct theme by replacing [THEME] with either Dark or Light and returns the URI</param>
        /// <returns>Returns a URI of the sourcePath with the correct theme applied</returns>
        public static Uri Uri(String sourcePath)
        {
            return new Uri(Path(sourcePath), UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Converts a string path to use the correct theme by replacing [THEME] with either Dark or Light and returns the URI
        /// </summary>
        /// <param name="sourcePath">Converts a string path to use the correct theme by replacing [THEME] with either Dark or Light and returns the URI</param>
        /// <param name="uriKind">The kind of URI to create</param>
        /// <returns>Returns a URI of the sourcePath with the correct theme applied</returns>
        public static Uri Uri(String sourcePath, UriKind uriKind)
        {
            return new Uri(Path(sourcePath), uriKind);
        }
    }
}
