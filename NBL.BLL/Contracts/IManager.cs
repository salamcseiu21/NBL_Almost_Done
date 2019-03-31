using System;
using System.Collections.Generic;
namespace NBL.BLL.Contracts
{
    public interface IManager<T> where T:class
    {
        bool Add(T model);
        bool Update(T model);
        bool Delete(T model);
        T GetById(int id);
        ICollection<T> GetAll();
        
    }
}
