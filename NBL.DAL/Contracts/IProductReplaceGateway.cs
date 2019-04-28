using NBL.Models.EntityModels;

namespace NBL.DAL.Contracts
{
    public interface IProductReplaceGateway:IGateway<ReplaceModel>
    {
        int SaveReplacementInfo(ReplaceModel model);
        int GetMaxReplaceSerialNoByYear(int year);
    }
}
