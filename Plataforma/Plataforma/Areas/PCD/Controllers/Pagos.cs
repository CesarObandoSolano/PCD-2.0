using GreenPay.Core.Api;
using GreenPay.Core.Client;
using GreenPay.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plataforma.Areas.PCD.Controllers
{
    public class Pagos
    {

        public static string Login()
        {
            ApiClient client = new ApiClient();

            UserApi userApi = new UserApi();
            GPLoginRequest loginRequest = new GPLoginRequest("pimas", "pimas5");
            GPLoginResponse loginResponse = userApi.Login(loginRequest);

            System.Console.WriteLine("Response : " + loginResponse.AccessToken);

            String completAccessToken = "Bearer " + loginResponse.AccessToken;
            return completAccessToken;
        }

        public static void CreateCustomer(String accessToken)
        {
            CustomerApi customerApi = new CustomerApi();
            GPCompany company = new GPCompany(1);
            GPCustomer newCustomer = new GPCustomer(1, company, GPCustomer.CustomerProfileEnum.PROFILE_LEVEL_1, GPCustomer.CustomerTypeEnum.PERSONAL_ACCOUNT, "prueba@prueba.com", "1111111111",
                                                    GPCustomer.IdentificationTypeEnum.NATIONAL_ID, "Apellido", "Nombre", "12345678", "1234");
            GPCustomer customer = customerApi.SaveCustomer(accessToken, newCustomer);
            if (customer != null)
            {
                System.Console.WriteLine("###### NEW CUSTOMER ADDED ######");

                System.Console.WriteLine("\t\t" + customer.ToString());

                System.Console.WriteLine("################################");
            }
            else
            {
                System.Console.WriteLine("Customer NULL");
            }
        }

        public static void ListCreditCards(String accessToken, long customerId)
        {

            CreditcardApi creditCardApi = new CreditcardApi();
            GPCreditCardList creditCardList = creditCardApi.ListCreditCards(accessToken, customerId);
            if (creditCardList != null)
            {
                System.Console.WriteLine("###### CREDIT CARDS LIST ######");
                if (creditCardList.List.Count > 0)
                {
                    foreach (GPCreditCard cc in creditCardList.List)
                    {
                        System.Console.WriteLine("\t**** CREDIT CARD: ****");

                        System.Console.WriteLine("\t\t" + cc.ToString());

                        System.Console.WriteLine("\t**********************");
                    }
                }
                else
                {
                    System.Console.WriteLine("No credit Cards on system");
                }
            }
            else
            {
                System.Console.WriteLine("CreditCardsList NULL");
            }
        }

        public static void CreateCreditCard(String accessToken, long customerId)
        {
            CreditcardApi creditCardApi = new CreditcardApi();

            GPCreditCard newCreditCard = new GPCreditCard(1, "Tarjeta de Prueba", "1234567890123456", GPCreditCard.CardVendorEnum.VISA, "1234", "10/2019", false, "Prueba");
            GPCreditCard creditCard = creditCardApi.SaveCreditCard(accessToken, customerId, newCreditCard);
            if (creditCard != null)
            {
                System.Console.WriteLine("###### NEW CREDIT CARD ADDED ######");

                System.Console.WriteLine("\t\t" + creditCard.ToString());

                System.Console.WriteLine("################################");
            }
            else
            {
                System.Console.WriteLine("CreditCard NULL");
            }
        }

        public static void ListTransactions(String accessToken, long customerId, int numberOfTransactions = 0)
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
                System.Console.WriteLine("###### LAST " + numberOfTransactions + " TRANSACTIONS ######");
                if (transactionsList.List.Count > 0)
                {
                    foreach (GPTransaction transaction in transactionsList.List)
                    {
                        System.Console.WriteLine("\t**** TRANSACTION: ****");

                        System.Console.WriteLine("\t\t" + transaction.ToString());

                        System.Console.WriteLine("\t**********************");
                    }
                }
                else
                {
                    System.Console.WriteLine("No Transactions on system for Customer " + customerId);
                }
            }
            else
            {
                System.Console.WriteLine("TransactionsList NULL");
            }
        }

        public static void CreateTrasaction(String accessToken)
        {
            TransactionApi transactionApi = new TransactionApi();

            List<GPStatus> statusList = new List<GPStatus>
            {
                new GPStatus(Id:1, Code: GPStatus.CodeEnum.NUMBER_1, Timestamp: DateTime.Now.ToString())
            };
            GPTransaction newTransaction = new GPTransaction(Amount: 100, AuthorizationCode: 12345, Channel: GPTransaction.ChannelEnum.MOBILE, CreditCard: 1, Description: "Prueba API", Status: statusList);
            GPTransaction transaction = transactionApi.SaveTransaction(accessToken, newTransaction);
            if (transaction != null)
            {
                System.Console.WriteLine("###### NEW TRANSACTION ADDED ######");

                System.Console.WriteLine("\t\t" + transaction.ToString());

                System.Console.WriteLine("################################");
            }
            else
            {
                System.Console.WriteLine("Transaction NULL");
            }
        }

    }
}