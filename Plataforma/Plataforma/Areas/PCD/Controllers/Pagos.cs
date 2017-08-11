using GreenPay.Core.Api;
using GreenPay.Core.Client;
using GreenPay.Core.Model;
using Plataforma.Areas.PCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plataforma.Areas.PCD.Controllers
{
    public class Pagos
    {

        public string Login()
        {
            ApiClient client = new ApiClient();
            UserApi userApi = new UserApi();
            GPLoginRequest loginRequest = new GPLoginRequest("pimas", "pimas5");
            GPLoginResponse loginResponse = userApi.Login(loginRequest);
            String completAccessToken = "Bearer " + loginResponse.AccessToken;
            return completAccessToken;
        }

        public long CreateCustomer(String accessToken, usuario usuario, string identficacion, int tipoIdentificacion, int pin )
        {
            CustomerApi customerApi = new CustomerApi();
            GPCompany company = new GPCompany(9);
            GPCustomer newCustomer = new GPCustomer(Company: company, 
                CustomerProfile: GPCustomer.CustomerProfileEnum.PROFILE_LEVEL_1, 
                CustomerType: GPCustomer.CustomerTypeEnum.PERSONAL_ACCOUNT, 
                Email: usuario.correo, Identification: identficacion, 
                IdentificationType: (GPCustomer.IdentificationTypeEnum)tipoIdentificacion, LastName: usuario.apellidos, 
                Name: usuario.nombre, PhoneNumber: usuario.telefono.ToString(), Pin: pin.ToString());
            GPCustomer customer = customerApi.SaveCustomer(accessToken, newCustomer);
            if (customer != null)
            {
                return customer.Id.Value;
            }
            else
            {
                return 0;
            }
        }

        public GPCreditCardList ListCreditCards(String accessToken, long customerId)
        {
            CreditcardApi creditCardApi = new CreditcardApi();
            GPCreditCardList creditCardList = creditCardApi.ListCreditCards(accessToken, customerId);
            if (creditCardList != null)
            {
                return creditCardList;
            }
            return null;
        }

        public void CreateCreditCard(String accessToken, long customerId, string cardHolderName, 
                        string cardNumber, int cardVendor, string ccv, string expirationDate, Boolean favorite, string nickname)
        {
            CreditcardApi creditCardApi = new CreditcardApi();
            GPCreditCard newCreditCard = new GPCreditCard(CardHolderName: cardHolderName, CardNumber: cardNumber, CardVendor: (GPCreditCard.CardVendorEnum)cardVendor,
                                        Ccv: ccv, ExpirationDate: expirationDate, Favorite: favorite, Nickname: nickname);
            GPCreditCard creditCard = creditCardApi.SaveCreditCard(accessToken, customerId, newCreditCard);
        }

        public GPTransactionList ListTransactions(String accessToken, long customerId, int numberOfTransactions = 0)
        {
            TransactionApi transactionApi = new TransactionApi();
            GPTransactionList transactionsList;
            if (numberOfTransactions == 0)
            {
                transactionsList = transactionApi.ListTransactions(accessToken, customerId: customerId, max: numberOfTransactions);
            }
            else
            {
                transactionsList = transactionApi.ListTransactions(accessToken, customerId: customerId, max: numberOfTransactions);
            }
            if (transactionsList != null)
            {
                return transactionsList;
            }
            return null;
        }

        public GPTransaction CreateTrasaction(String accessToken, venta venta, int creditCardID)
        {
            TransactionApi transactionApi = new TransactionApi();

            List<GPStatus> statusList = new List<GPStatus>
            {
                new GPStatus(Code: GPStatus.CodeEnum.NUMBER_1, Timestamp: DateTime.Now.ToString())
            };
            GPTransaction newTransaction = new GPTransaction(Amount: (double)venta.total, AuthorizationCode: 12345, 
                Channel: GPTransaction.ChannelEnum.WEB, CreditCard: creditCardID, Description: "Compra en PIMÁS", 
                Status: statusList);
            GPTransaction transaction = transactionApi.SaveTransaction(accessToken, newTransaction);
            if (transaction != null)
            {
                return transaction;
            }
            return null;
        }

    }
}