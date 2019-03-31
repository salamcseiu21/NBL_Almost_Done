
using System.Collections.Generic;
namespace NBL.DAL.Contracts
{
    public interface IGateway<T> where T:class 
    {
        int Add(T model);
        int Update(T model);
        int Delete(T model);
        T GetById(int id);
        ICollection<T> GetAll();
    }
}
