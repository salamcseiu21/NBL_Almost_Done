
using NBL.Models;
using NBL.Models.EntityModels.Designations;

namespace NBL.BLL.Contracts
{
   public interface IDesignationManager:IManager<Designation>
   {
        Designation GetDesignationByCode(string code);

    }
}
