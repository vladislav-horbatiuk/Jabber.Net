using System;
using System.Threading;

namespace Jabber.Net.Server.Utils
{
    class ReaderWriterLock
    {
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();


        public IDisposable ReadLock()
        {
            return new ReaderLock(locker);
        }

        public IDisposable WriteLock()
        {
            return new WriterLock(locker);
        }


        private class ReaderLock : IDisposable
        {
            private readonly ReaderWriterLockSlim _locker;

            public ReaderLock(ReaderWriterLockSlim locker)
            {
                _locker = locker;
                _locker.EnterReadLock();
            }

            public void Dispose()
            {
                _locker.ExitReadLock();
            }
        }

        private class WriterLock : IDisposable
        {
            private readonly ReaderWriterLockSlim _locker;

            public WriterLock(ReaderWriterLockSlim locker)
            {
                _locker = locker;
                _locker.EnterWriteLock();
            }

            public void Dispose()
            {
                _locker.ExitWriteLock();
            }
        }
    }
}
