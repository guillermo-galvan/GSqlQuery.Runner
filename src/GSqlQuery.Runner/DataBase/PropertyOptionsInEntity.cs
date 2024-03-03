﻿using System;

namespace GSqlQuery.Runner
{
    public class PropertyOptionsInEntity
    {
        public PropertyOptions Property { get; }

        public Type Type { get; }

        public object DefaultValue { get; }

        public int? Ordinal { get; }

        public PropertyOptionsInEntity(PropertyOptions propertyOptions, Type type, object defaultValue, int? ordinal)
        {
            Property = propertyOptions;
            Type = type;
            DefaultValue = defaultValue;
            Ordinal = ordinal;
        }
    }

    public class PropertyValue(PropertyOptions property, object value)
    {
        public PropertyOptions Property { get; } = property;

        public object Value { get; } = value;
    }
}
