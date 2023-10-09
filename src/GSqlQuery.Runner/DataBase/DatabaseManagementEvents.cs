using GSqlQuery.Runner;
using GSqlQuery.Runner.Transforms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GSqlQuery
{
    public abstract class DatabaseManagementEvents
    {
        public bool IsTraceActive { get; set; } = false;

        public abstract IEnumerable<IDataParameter> GetParameter<T>(IEnumerable<ParameterDetail> parameters);

        public virtual void WriteTrace(string message, object[] param)
        {
            if (IsTraceActive)
            {
                Console.WriteLine("Message: {0}, Param {1}", message, param);
            }
        }

        public virtual ITransformTo<T> GetTransformTo<T>(ClassOptions classOptions, IQuery<T> query)
           where T : class
        {
            if (query is JoinQuery<T> joinQuery)
            {
                return new JoinTransformTo<T>(classOptions.PropertyOptions.Count());
            }
            else if (!classOptions.IsConstructorByParam)
            {
                return new TransformToByField<T>(classOptions.PropertyOptions.Count());
            }
            else
            {
                return new TransformToByConstructor<T>(classOptions.PropertyOptions.Count());
            }
        }
    }
}