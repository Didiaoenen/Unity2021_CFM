using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Assertions;

namespace Mvvm.converters
{
    public abstract class IndexToObjectConverter<T> : ScriptableObject, IValueConverter
    {
        [SerializeField]
        [Tooltip("")]
        private T defaultValue = default(T);

        [SerializeField]
        [Tooltip("")]
        private T[] convertToValue = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return defaultValue;
            }

            int realValue = (int)value;

            Assert.IsTrue(convertToValue.Length >= realValue, $"");

            return convertToValue[realValue];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
