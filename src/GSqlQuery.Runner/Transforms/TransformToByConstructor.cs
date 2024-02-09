using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace GSqlQuery.Runner.Transforms
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TransformToByConstructor<T> : TransformTo<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numColumns"></param>
        public TransformToByConstructor(int numColumns) : base(numColumns)
        {}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override T Generate(IEnumerable<PropertyOptionsInEntity> columns, DbDataReader reader)
        {
            object[] fields = new object[_numColumns];

            foreach (PropertyOptionsInEntity item in columns)
            {
                if (item.Ordinal.HasValue)
                {
                    object value = reader.GetValue(item.Ordinal.Value);
                    value = TransformTo.SwitchTypeValue(item.Type, value);
                    fields[item.Property.PositionConstructor] = value;
                }
                else
                {
                    fields[item.Property.PositionConstructor] = item.DefaultValue;
                }
            }

            return (T)_classOptions.ConstructorInfo.Invoke(fields);
        }

        public override Task<T> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, DbDataReader reader)
        {
            return Task.FromResult(Generate(columns, reader));
        }
    }
}