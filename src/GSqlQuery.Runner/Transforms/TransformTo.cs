using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace GSqlQuery.Runner.Transforms
{
    public static class TransformTo 
    {
        internal static Type charType = typeof(char[]);

        public static object SwitchTypeValue(Type type, object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return null;
            }
            else if (charType == type && value is string tmp)
            {
                return tmp.ToCharArray();
            }
            else
            {
                return Convert.ChangeType(value, type);
            }
        }
    }

    public abstract class TransformTo<T, TDbDataReader>(int numColumns) : ITransformTo<T, TDbDataReader> 
        where T : class
        where TDbDataReader : DbDataReader
    {
        protected readonly int _numColumns = numColumns;
        protected readonly ClassOptions _classOptions = ClassOptionsFactory.GetClassOptions(typeof(T));

        protected IEnumerable<PropertyOptionsInEntity> GetProperties(IEnumerable<PropertyOptions> propertyOptions, IEnumerable<PropertyOptions> propertyOptionsColumns, TDbDataReader reader)
        {
            return (from pro in propertyOptions
                    join ca in propertyOptionsColumns on pro.ColumnAttribute.Name equals ca.ColumnAttribute.Name into leftJoin
                    from left in leftJoin.DefaultIfEmpty()
                    select
                        new PropertyOptionsInEntity(pro,
                        Nullable.GetUnderlyingType(pro.PropertyInfo.PropertyType) ?? pro.PropertyInfo.PropertyType,
                        pro.PropertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(pro.PropertyInfo.PropertyType) : null,
                        left != null ? reader.GetOrdinal(pro.ColumnAttribute.Name) : null)).ToArray();
        }

        public virtual IEnumerable<PropertyOptionsInEntity> GetOrdinalPropertiesInEntity
            (IEnumerable<PropertyOptions> propertyOptions, IQuery<T> query, TDbDataReader reader)
        {
            return GetProperties(propertyOptions, query.Columns, reader);
        }

        public abstract T Generate(IEnumerable<PropertyOptionsInEntity> columns, TDbDataReader reader);

        public abstract Task<T> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, TDbDataReader reader);
    }
}