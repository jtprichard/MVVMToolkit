﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace PB.MVVMToolkit.Dialogs
{
    [ValueConversion(typeof(bool), typeof(String))]
    /// <summary>
    /// Value Converter for Main WPF Page Navigation
    /// Adjust Page Enumerations to Match Page XMAL documents
    /// </summary>
    public class RowVisibleValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((bool)value)
            {
                case true:
                    return "Auto";
                case false:
                    return "0";
                default:
                    return "0";
            }
        }

        /// <summary>
        /// Convert Back - Unused
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
