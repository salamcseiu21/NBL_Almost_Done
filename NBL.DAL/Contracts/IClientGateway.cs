﻿
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface IClientGateway:IGateway<Client>
    {
        int ApproveClient(Client aClient, ViewUser anUser);
        List<Client> GetPendingClients();
        Client GetClientByEmailAddress(string email);
        IEnumerable<ViewClient> GetClientByBranchId(int branchId);
        ViewClient GetClientDeailsById(int clientId);
        IEnumerable<ViewClient> GetAllClientDetails();
        IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId);
        decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode);
        int UploadClientDocument(ClientAttachment clientAttachment);
        IEnumerable<ClientAttachment> GetClientAttachments();
        IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId);
        IEnumerable<ViewClientSummaryModel> GetClientSummary();
        int GetMaxSerialNoOfClientByAccountPrefix(string acountPrefix);
        ICollection<object> GetClientByBranchIdAndSearchTerm(int branchId, string searchTerm);
        List<ViewProduct> GetStockProductToclient(int clientId);
        ICollection<object> GetAllClientBySearchTerm(string searchTerm);
        ViewClient GetClientInfoBySubSubSubAccountCode(string accountCode);
        ICollection<ViewClient> GetClientByOrderCountAndBranchId(int branchId);
        ICollection<Client> GetActiveClient();
        int SetCreditLimit(int clientId, decimal creditLimit);
        ICollection<ViewClientSummaryModel> GetClientSummaryByBranchId(int branchId);
        int UpdateCreditLimitConsideationStatus(int clientId,int status);
        ICollection<ViewClient> GetClientByOrderCountBranchAndClientTypeId(int branchId, int clientTypeId);
    }
}
