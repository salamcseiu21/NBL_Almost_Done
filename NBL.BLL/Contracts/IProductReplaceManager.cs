﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Replaces;

namespace NBL.BLL.Contracts
{
    public interface IProductReplaceManager:IManager<ReplaceModel>
    {
        bool SaveReplacementInfo(ReplaceModel model);
        ICollection<ViewReplaceModel> GetAllReplaceListByBranchCompanyAndStatus(int branchId, int companyId, int status);
        ICollection<ViewReplaceModel> GetAllPendingReplaceListByBranchAndCompany(int branchId, int companyId);
        ViewReplaceModel GetReplaceById(long id);
        ICollection<ViewReplaceDetailsModel> GetReplaceProductListById(long id);
        ICollection<ViewProduct> GetDeliveredProductsByReplaceRef(string replaceRef);
       
    }
}