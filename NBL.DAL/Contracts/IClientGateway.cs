
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
        IEnumerable<Client> GetClientByBranchId(int branchId);
        ViewClient GetClientDeailsById(int clientId);
        IEnumerable<ViewClient> GetAllClientDetails();
        IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId);
        decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode);
        int UploadClientDocument(ClientAttachment clientAttachment);
        IEnumerable<ClientAttachment> GetClientAttachments();
        IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId);
        IEnumerable<ViewClientSummaryModel> GetClientSummary();
        int GetMaxSerialNoOfClientByAccountPrefix(string acountPrefix);


    }
}
