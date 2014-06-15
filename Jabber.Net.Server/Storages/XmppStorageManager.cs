using Jabber.Net.Server.Collections;

namespace Jabber.Net.Server.Storages
{
    public class XmppStorageManager
    {
        private readonly ReaderWriterLockDictionary<string, object> storages = new ReaderWriterLockDictionary<string, object>();
        
        public IXmppUserStorage Users
        {
            get { return GetStorage<IXmppUserStorage>("users"); }
        }

        public IXmppElementStorage Elements
        {
            get { return GetStorage<IXmppElementStorage>("elements"); }
        }


        public void AddStorage(string name, object storage)
        {
            Args.NotNull(name, "name");
            Args.NotNull(storage, "storage");

            storages.Add(name, storage);
        }

        public void RemoveStorage(string name)
        {
            Args.NotNull(name, "name");

            storages.Remove(name);
        }

        public T GetStorage<T>(string name)
        {
            Args.NotNull(name, "name");

            object storage;
            return storages.TryGetValue(name, out storage) ? (T)storage : default(T);
        }
    }
}
