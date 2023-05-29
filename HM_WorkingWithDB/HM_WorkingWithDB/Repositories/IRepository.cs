using HM_WorkingWithDB.Entities;

namespace HM_WorkingWithDB.Repositories
{
    internal interface IRepository<T> where T : IEntity
    {
        public bool ExistRecord(T record);
        public void AddRecord(T record);
        public int UpdateRecord(T record);
        public int DeleteRecord(long id);
        public T GetRecord(long id);
        public IEnumerable<T> GetAllRecords();
    }
}
