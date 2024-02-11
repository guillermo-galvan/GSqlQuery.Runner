using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace GSqlQuery.Runner.Transforms
{
    internal class TransformToByField<T, TDbDataReader>(int numColumns) : TransformTo<T, TDbDataReader>(numColumns) 
        where T : class
        where TDbDataReader : DbDataReader
    {
        public override T Generate(IEnumerable<PropertyOptionsInEntity> columns, TDbDataReader reader)
        {
            object result = _classOptions.ConstructorInfo.Invoke(null);

            foreach (PropertyOptionsInEntity item in columns)
            {
                object value;
                if (item.Ordinal.HasValue)
                {
                    value = TransformTo.SwitchTypeValue(item.Type, reader.GetValue(item.Ordinal.Value));
                } 
                else
                {
                    value = item.DefaultValue;
                }

                if (value != null)
                {
                    item.Property.PropertyInfo.SetValue(result, value);
                }
            }

            return (T)result;
        }

        public override Task<T> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, TDbDataReader reader)
        {
            return Task.FromResult(Generate(columns, reader));
        }
    }
}