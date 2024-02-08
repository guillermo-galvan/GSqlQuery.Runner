using System.Collections.Generic;
using System.Data.Common;

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
                fields[item.Property.PositionConstructor] = item.Ordinal.HasValue ? TransformTo.SwitchTypeValue(item.Type, reader.GetValue(item.Ordinal.Value)) : item.DefaultValue;
            }

            return (T)_classOptions.ConstructorInfo.Invoke(fields);
        }
    }
}