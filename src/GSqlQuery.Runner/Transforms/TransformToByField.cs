using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace GSqlQuery.Runner.Transforms
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TransformToByField<T> : TransformTo<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numColumns"></param>
        public TransformToByField(int numColumns) : base(numColumns)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override T Generate(IEnumerable<PropertyOptionsInEntity> columns, DbDataReader reader)
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

        public override Task<T> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, DbDataReader reader)
        {
            return Task.FromResult(Generate(columns, reader));
        }
    }
}