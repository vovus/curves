using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Markup;

namespace YieldCurveSL
{
    public static class ColorFromString
    {
#if __VC10__
        public static Color ToColor(this string strColor)        
        {
            Color color;
            string xaml = string.Format
                ("<Color xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">{0}</Color>", strColor);
            try
            {
                object obj = XamlReader.Load(xaml);
                if (obj != null && obj is Color)
                {
                    color = (Color) obj;
                    return color;
                }
            }
            catch (Exception)
            {
                //Swallow useless exception
            }
            return Colors.Black;
        }
#else
		public static Color ToColor(this string strColor) 
		{
			String xamlString = String.Format(
				"<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Background=\"{0}\"/>",
				strColor);
			try
			{
				Canvas c = (Canvas)System.Windows.Markup.XamlReader.Load(xamlString);
				SolidColorBrush brush = (SolidColorBrush)c.Background;
				return brush.Color;
			}
			catch 
            {}
			return Colors.Black;
		}
#endif
    }
}
