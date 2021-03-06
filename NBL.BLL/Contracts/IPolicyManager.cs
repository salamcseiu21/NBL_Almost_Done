﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.ProductWarranty;
using NBL.Models.EntityModels.Services;
using NBL.Models.ViewModels.ProductWarranty;
using NBL.Models.ViewModels.Services;

namespace NBL.BLL.Contracts
{
   public interface IPolicyManager:IManager<WarrantyPolicy>
   {
       bool AddProductWarrentPolicy(WarrantyPolicy model);
       ICollection<ViewWarrantyPolicy> GetAllWarrantyPolicy();
       bool SaveTestPolicy(TestPolicyModel modelPolicy);
       ICollection<ViewTestPolicy> GetAllTestPolicy();
       ViewTestPolicy DischargeTestPolicyByProductIdCategoryIdAndMonth(int productId, int categoryId, int month); 
   }
}
