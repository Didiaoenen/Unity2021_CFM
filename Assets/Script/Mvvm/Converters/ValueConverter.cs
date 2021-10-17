using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

namespace Mvvm.converters
{
    [SerializeField]
    public abstract class ItemMapping<TSource, TTarget>
    {
        [SerializeField]
        public TSource SourceValue;

        [SerializeField]
        public TTarget TargetValue;
    }

    public abstract class ValueConverter<ItemMapping, TSource, TTarget> : ScriptableObject, IValueConverter where ItemMapping : ItemMapping<TSource, TTarget>, new()
    {
        [SerializeField]
        [Tooltip("")]
        private List<ItemMapping> itemLookup = null;

        [SerializeField]
        [Tooltip("")]
        private bool allowTwoWayConversion = false;

        [SerializeField]
        [Tooltip("")]
        private TSource defaultSourceValue = default(TSource);

        [SerializeField]
        [Tooltip("")]
        private TTarget defaultTargetValue = default(TTarget);

        private readonly IEqualityComparer<TSource> sourceComparer;
        private readonly IEqualityComparer<TTarget> targetComparer;

        protected ValueConverter() : this(EqualityComparer<TSource>.Default, EqualityComparer<TTarget>.Default)
        {

        }

        protected ValueConverter(IEqualityComparer<TSource> tSourceComparer) : this(tSourceComparer, EqualityComparer<TTarget>.Default)
        {

        }

        protected ValueConverter(IEqualityComparer<TSource> tSourceComparer, IEqualityComparer<TTarget> tTargetComparer)
        {
            this.sourceComparer = tSourceComparer;
            this.targetComparer = tTargetComparer;
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (itemLookup == null)
            {
                throw new ArgumentNullException($"");
            }

            if (value == null)
            {
                return defaultTargetValue;
            }

            if (!(value is TSource))
            {
                throw new InvalidCastException($"");
            }

            var castedValue = (TSource)value;
            var item = itemLookup.FirstOrDefault(x => sourceComparer.Equals(castedValue, x.SourceValue));

            if (item == null)
            {
                return defaultTargetValue;
            }
            else
            {
                return item.TargetValue;
            }
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!allowTwoWayConversion)
            {
                throw new ArgumentNullException($"");
            }

            if (itemLookup == null)
            {
                throw new ArgumentNullException($"");
            }

            if (value == null)
            {
                return defaultSourceValue;
            }

            if (!(value is TTarget))
            {
                throw new InvalidCastException($"");
            }

            var castedValue = (TTarget)value;
            var item = itemLookup.FirstOrDefault(x => targetComparer.Equals(castedValue, x.TargetValue));
            if (item == null)
            {
                return defaultSourceValue;
            }
            else
            {
                return item.SourceValue;
            }
        }
    }
}
