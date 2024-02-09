﻿using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace GSqlQuery.Runner
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITransformTo<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Generate(IEnumerable<PropertyOptionsInEntity> columns, DbDataReader reader);

        Task<T> GenerateAsync(IEnumerable<PropertyOptionsInEntity> columns, DbDataReader reader);

        IEnumerable<PropertyOptionsInEntity> GetOrdinalPropertiesInEntity(IEnumerable<PropertyOptions> propertyOptions, IQuery<T> query, DbDataReader reader);
    }
}