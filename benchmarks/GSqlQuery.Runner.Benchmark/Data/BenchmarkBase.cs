using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Moq;
using System.Data;

namespace GSqlQuery.Runner.Benchmark.Data
{
    [SimpleJob(RuntimeMoniker.Net70, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net60)]
    [SimpleJob(RuntimeMoniker.Net462)]
    public abstract class BenchmarkBase
    {
        protected readonly ConnectionOptions<IDbConnection> _connections;
        Mock<IDatabaseManagement<IDbConnection>> mock = new Mock<IDatabaseManagement<IDbConnection>>();

        public BenchmarkBase()
        {
            _connections = new ConnectionOptions<IDbConnection>(new DefaultFormats(), mock.Object);
        }
    }
}
