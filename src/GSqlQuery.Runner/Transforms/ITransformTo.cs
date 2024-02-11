using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace GSqlQuery.Runner
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITransformTo<T, TDbDataReader> 
        where T : class
        where TDbDataReader : DbDataReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Generate(IEnumerable<PropertyOptionsInEntity> columns, TDbDataReader reader);

        Task<T> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, TDbDataReader reader);

        IEnumerable<PropertyOptionsInEntity> GetOrdinalPropertiesInEntity(IEnumerable<PropertyOptions> propertyOptions, IQuery<T> query, TDbDataReader reader);
    }
}