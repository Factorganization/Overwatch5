using System.Collections.Generic;

namespace Systems.Persistence
{
    public interface IDataService
    {
        void Save(GameData data, bool overwrite = true);
        GameData Load(string key);
        void Delete(string key);
        void DeleteAll();
        IEnumerable<string> ListSaves();
    }
}
