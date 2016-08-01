using System.Threading;

namespace Norma.Delta.Services
{
    // Unity Container で管理されるので、単一インスタンス
    public class DatabaseService
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        // DbContext 複数や、長時間使い回しは良くないので、使う時に Connect
        // 使い終わったら Disconnect
        // using(var connection = databaseService.Connect())
        //     Work
        // で良いと思う
        public DbConnection Connect()
        {
            _semaphoreSlim.Wait();
            return new DbConnection(this);
        }

        public void Disconnect()
        {
            _semaphoreSlim.Release();
        }
    }
}