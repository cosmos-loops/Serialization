﻿using Cosmos.Reflection;
using MessagePack;
using MessagePack.Formatters;

namespace Cosmos.Dynamic.DynamicEnums
{
    public class DynamicFlagEnumNameResolver : IFormatterResolver
    {
        public static readonly DynamicFlagEnumNameResolver Instance = new DynamicFlagEnumNameResolver();

        private DynamicFlagEnumNameResolver() { }

        public IMessagePackFormatter<T> GetFormatter<T>() =>
            FormatterCache<T>.Formatter;

        private static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter;

            static FormatterCache()
            {
                if (Enums.IsDynamicEnum(typeof(T), out var genericArguments))
                {
                    var formatterType = typeof(DynamicFlagEnumNameFormatter<,>).MakeGenericType(genericArguments);
                    Formatter = (IMessagePackFormatter<T>) Types.CreateInstance(formatterType);
                }
            }
        }
    }
}