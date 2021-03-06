using System;
using System.Collections.Generic;

namespace Paillave.Etl.Core
{
    public class JobPoolDispatcher : IDisposable
    {
        private object _sync = new object();
        private IDictionary<object, JobPool> _jobPoolDictionary = new Dictionary<object, JobPool>();
        public void Invoke(object threadOwner, Action action)
        {
            JobPool jobPool;
            lock (_sync)
            {
                if (!_jobPoolDictionary.TryGetValue(threadOwner, out jobPool))
                {
                    jobPool = new JobPool();
                    _jobPoolDictionary[threadOwner] = jobPool;
                }
            }
            jobPool.Execute(action);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var item in _jobPoolDictionary)
                    {
                        item.Value.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}