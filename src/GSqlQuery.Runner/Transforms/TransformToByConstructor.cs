using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace GSqlQuery.Runner.Transforms
{
    internal class TransformToByConstructor<T, TDbDataReader>(int numColumns) : TransformTo<T, TDbDataReader>(numColumns) 
        where T : class
        where TDbDataReader : DbDataReader
    {
        public override T Generate(IEnumerable<PropertyOptionsInEntity> columns, TDbDataReader reader)
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

        public override Task<T> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, TDbDataReader reader)
        {
            return Task.FromResult(Generate(columns, reader));
        }
    }
}