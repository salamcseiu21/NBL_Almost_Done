﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Orders;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;

namespace NBL.BLL
{
    public class ClientManager:IClientManager
    {
        private readonly IClientGateway _iClientGateway;
        private readonly IOrderManager _iOrderManager;
        private readonly IDeliveryGateway _iDeliveryGateway;
        private readonly IReportManager _iReportManager;
       
        public ClientManager(IClientGateway iClientGateway,IOrderManager iOrderManager,IDeliveryGateway iDeliveryGateway,IReportManager iReportManager)
        {
            _iClientGateway = iClientGateway;
            _iOrderManager = iOrderManager;
            _iDeliveryGateway = iDeliveryGateway;
            _iReportManager = iReportManager;
        }
        
        public bool Add(Client client)
        {

            if (client.Email !=null)
            {
                bool isEmailValid = CheckEmail(client.Email);
                if (!isEmailValid) return false;
                bool isUnique = IsEmailAddressUnique(client.Email);
                if (!isUnique) return false;
               
            }
            else
            {
                string acountPrefix = "31020";
                switch (client.ClientTypeId)
                {
                    case 1:
                        acountPrefix += "3";
                        break;
                    case 2:
                        acountPrefix += "2";
                        break;
                    case 3:
                        acountPrefix += "1";
                        break;
                }
                var lastClientNo = _iClientGateway.GetMaxSerialNoOfClientByAccountPrefix(acountPrefix);
                var accountCode = Generator.GenerateAccountCode(acountPrefix, lastClientNo);
                client.SubSubAccountCode = acountPrefix;
                client.SubSubSubAccountCode = accountCode;
            }
            return _iClientGateway.Add(client)>0;
            
        }

        public bool ApproveClient(Client aClient, ViewUser anUser)
        {
           
            int rowAffected= _iClientGateway.ApproveClient(aClient,anUser);
            return rowAffected != 0;
        }

        public List<Client> GetPendingClients()
        {
            return _iClientGateway.GetPendingClients();
        }

        private bool IsEmailAddressUnique(string email)
        {
            var result = _iClientGateway.GetClientByEmailAddress(email);
            return result.Email == null;
        }

        public Client GetClientByEmailAddress(string email)
        {
            return _iClientGateway.GetClientByEmailAddress(email);
        }

        private bool CheckEmail(string email)
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }


        public ICollection<Client> GetAll()
        {
            var clients = _iClientGateway.GetAll();
            foreach (Client client in clients)
            {
                client.Orders = _iOrderManager.GetOrdersByClientId(client.ClientId).ToList();
            }
            return clients;
        }
        public IEnumerable<ViewClient> GetClientByBranchId(int branchId)
        {
            var clients = _iClientGateway.GetClientByBranchId(branchId);
            return clients;
        }
        public ICollection<ViewClient> GetClientByOrderCountAndBranchId(int branchId)
        {
            return _iClientGateway.GetClientByOrderCountAndBranchId(branchId);
        }

        public ICollection<Client> GetActiveClient()
        {
            return _iClientGateway.GetActiveClient();
        }

        public bool SetCreditLimit(int clientId, decimal creditLimit)
        {
            int rowAffected = _iClientGateway.SetCreditLimit(clientId,creditLimit);
            return rowAffected > 0;
        }

        public ICollection<ViewClientSummaryModel> GetClientSummaryByBranchId(int branchId)
        {
            return _iClientGateway.GetClientSummaryByBranchId(branchId);
        }

        public ViewClient GetClientById(int clientId)
        {
            return _iClientGateway.GetClientDeailsById(clientId);
        }

        public bool UpdateCreditLimitConsideationStatus(int clientId,int status)
        {
            int rowAffected = _iClientGateway.UpdateCreditLimitConsideationStatus(clientId,status);
            return rowAffected > 0;
        }

        public ICollection<ViewClient> GetClientByOrderCountBranchAndClientTypeId(int branchId, int clientTypeId)
        {
            return _iClientGateway.GetClientByOrderCountBranchAndClientTypeId(branchId, clientTypeId);
        }

        public bool Delete(Client model)
        {
            throw new System.NotImplementedException();
        }

        public Client GetById(int clientId)
        {
            var client= _iClientGateway.GetById(clientId);
            return client;

        }
        public ViewClient GetClientDeailsById(int clientId)
        {
            var client = _iClientGateway.GetClientDeailsById(clientId);
            SearchCriteria aCriteria = new SearchCriteria
            {
                ClientId = clientId,
                MonthNo = DateTime.Now.Month
            };
            ICollection<Order> orders=_iOrderManager.GetOrdersBySearchCriteria(aCriteria);
              client.Orders = orders.ToList();
             // client.DeliveredOrderModels = _iDeliveryGateway.GetDeliveredOrderByClientId(clientId);
             ICollection<ViewDeliveredOrderModel> deliveredOrderModels = _iDeliveryGateway.GetDeliveredOrderBySearchCriteria(aCriteria);
             client.DeliveredOrderModels = deliveredOrderModels;
              client.StockProducts = _iReportManager.GetStockProductToclientByClientIdWithBarcode(clientId);

             return client;

        }
        public bool Update(Client client)
        {
            return _iClientGateway.Update(client)>0;
        }

        public IEnumerable<ViewClient> GetAllClientDetails()
        {
            var clients = _iClientGateway.GetAllClientDetails();
            //foreach (var client in clients)
            //{
            //    client.Orders = _iOrderManager.GetOrdersByClientId(client.ClientId).ToList();
            //}
            return clients;
        }

        public IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId)
        {
            return _iClientGateway.GetAllClientDetailsByBranchId(branchId);
        }

        public decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode)
        {
            return _iClientGateway.GetClientOustandingBalanceBySubSubSubAccountCode(subsubsubAccountCode);
        }

        public bool UploadClientDocument(ClientAttachment clientAttachment)
        {
            int rowAffected = _iClientGateway.UploadClientDocument(clientAttachment);
            return rowAffected > 0;
        }

        public IEnumerable<ClientAttachment> GetClientAttachments()
        {
            return _iClientGateway.GetClientAttachments();
        }

        public IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId)
        {
            return _iClientGateway.GetClientAttachmentsByClientId(clientId);
        }

        public IEnumerable<ViewClientSummaryModel> GetClientSummary()
        {
            return _iClientGateway.GetClientSummary();
        }

        public ICollection<object> GetClientByBranchIdAndSearchTerm(int branchId, string searchTerm)
        {
            return _iClientGateway.GetClientByBranchIdAndSearchTerm(branchId,searchTerm);
        }

        public ICollection<object> GetAllClientBySearchTerm(string searchTerm)
        {
            return _iClientGateway.GetAllClientBySearchTerm(searchTerm);
        }

        public ViewClient GetClientInfoBySubSubSubAccountCode(string accountCode)
        {
            return _iClientGateway.GetClientInfoBySubSubSubAccountCode(accountCode);
        }

        
    }
}