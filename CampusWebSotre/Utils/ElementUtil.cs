using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Web.Security;
using System.Text;
using System.Web.Mvc;
using CampusWebStore.Models;
using CampusWebStore.Element;
using CampusWebStore.Shared.Models;

// Debug Use Statements
using CampusWebStore.Utils;

namespace CampusWebStore.Utils
{
    public class ElementUtil
    {
        #region Properties
        public string TRANSACTION_PATH = "/Account/ProcessTransaction";
        private ExpressSoapClient express;
        private Application application;
        private Credentials credentials;
        private Card card;
        private Terminal terminal;
        private Transaction transaction;
        private Response response;
        private Address address;
        private TransactionSetup transactionSetup;
        private PaymentAccount paymentAccount;
        private ElementResponseModel responseCollection;
        #endregion

        #region Public Methods

        // <summary>
        //      Function for setting up an Element Transaction environment
        // </summary>
        // <returns>
        //      NameValueCollection 
        // </returns>
        public ElementResponseModel SetupTransaction(UserModel user, string amount, string redirectUrl)
        {
            // Create Client and Response objects
            ExpressSoapClient express = new ExpressSoapClient();
            Response response = new Response();

            // Call Element Setup classes/functions
            GetSetupConfig(user, amount, redirectUrl);

            // Generate and interpret response
            response = express.TransactionSetup(credentials, application, terminal, transaction, transactionSetup, address, paymentAccount, null);
            string tsi = response.TransactionSetup.TransactionSetupID;
            string tempvalcode = response.TransactionSetup.ValidationCode;

            // Store response data
            responseCollection = new ElementResponseModel();
            responseCollection.TransactionSetupId = tsi.ToString();
            responseCollection.ValidationCode = tempvalcode.ToString();

            // Return store response data
            return responseCollection;
        }

        // <summary>
        //      Function for performing and AVS check
        // </summary>
        // <returns>
        //      string 
        // </returns>
        public string PerformAVSCheck(UserModel user, string token)
        {
            // Create Client and Response objects
            ExpressSoapClient express = new ExpressSoapClient();
            Response response = new Response();

            // Call Element AVS Setup classes/functions
            GetAVSConfig(user, token);
            ExtendedParameters[] arExt = GetExtendedParameters(token);

            System.Diagnostics.Debug.Write(token);

            // Generate response
            response = express.CreditCardAVSOnly(credentials, application, terminal, card, transaction, address, arExt);

            // Return response
            return response.Card.AVSResponseCode;
        }

        // <summary>
        //      Method for getting the path of transaction processing
        // </summary>
        public string TransactionPath(string url)
        {
            string path = url + TRANSACTION_PATH;
            return path;
        }
        #endregion

        #region Private Methods
        private void GetSetupConfig(UserModel user, string amount, string redirectUrl)
        {
            GetApplication();
            GetCredentials();
            GetCard();
            GetTransactionTerminal();
            GetTransaction(amount);
            GetAddress(user);
            GetTransactionSetup(redirectUrl);
            GetPaymentAccount(user);
        }

        private void GetAVSConfig(UserModel user, string token)
        {
            GetApplication();
            GetCredentials();
            GetCard();
            GetAVSTerminal();
            GetTransaction("0.00");
            GetAddress(user);
            GetPaymentAccount(user);
        }

        private ExtendedParameters[] GetExtendedParameters(string token)
        {
            PaymentAccount pa = new PaymentAccount();
            ExtendedParameters[] arExt = new ExtendedParameters[1];
            ExtendedParameters ext = new ExtendedParameters();

            pa.PaymentAccountID = token;
            ext.Key = "PaymentAccount";
            ext.Value = pa;
            arExt[0] = ext;

            return arExt;
        }

        private void GetApplication()
        {
            application = new Application();
            application.ApplicationID = "930";
            application.ApplicationName = "TCS";
            application.ApplicationVersion = "2.0.1";
        }

        private void GetCredentials()
        {
            credentials = new Credentials();

            //LIVE
            credentials.AccountToken = System.Configuration.ConfigurationManager.AppSettings["elementpsaccounttoken"] as string;
            credentials.AccountID = System.Configuration.ConfigurationManager.AppSettings["elementpsaccountid"] as string;
            credentials.AcceptorID = System.Configuration.ConfigurationManager.AppSettings["elementpsacceptorid"] as string;
        }

        private void GetCard()
        {
            card = new Card();
        }

        private void GetTransactionTerminal()
        {
            terminal = new Terminal();
            terminal.TerminalID = System.Configuration.ConfigurationManager.AppSettings["elementpsterminalid"] as string;
            terminal.CardholderPresentCode = CardholderPresentCode.Present;
            terminal.CardInputCode = CardInputCode.ManualKeyedMagstripeFailure;
            terminal.TerminalCapabilityCode = TerminalCapabilityCode.MagstripeReader;
            terminal.TerminalEnvironmentCode = TerminalEnvironmentCode.LocalAttended;
            terminal.CardPresentCode = CardPresentCode.Present;
            terminal.MotoECICode = MotoECICode.NotUsed;
        }

        private void GetAVSTerminal()
        {
            terminal = new Terminal();
            terminal.TerminalID = System.Configuration.ConfigurationManager.AppSettings["elementpsterminalid"] as string;
            terminal.CardholderPresentCode = CardholderPresentCode.ECommerce;
            terminal.CardInputCode = CardInputCode.ManualKeyed;
            terminal.TerminalCapabilityCode = TerminalCapabilityCode.MagstripeReader;
            terminal.TerminalEnvironmentCode = TerminalEnvironmentCode.LocalAttended;
            terminal.CardPresentCode = CardPresentCode.NotPresent;
            terminal.CVVPresenceCode = CVVPresenceCode.NotProvided;
            terminal.MotoECICode = MotoECICode.NonAuthenticatedSecureECommerceTransaction;
            terminal.TerminalCapabilityCode = TerminalCapabilityCode.KeyEntered;
            terminal.TerminalEnvironmentCode = TerminalEnvironmentCode.ECommerce;
            terminal.TerminalType = TerminalType.ECommerce;
        }

        private void GetTransaction(string amount)
        {
            transaction = new Transaction();
            transaction.MarketCode = MarketCode.ECommerce;
            transaction.TransactionAmount = amount;
        }

        private void GetAddress(UserModel user)
        {
            address = new Address();

            address.BillingName = user.FirstName + " " + user.LastName;
            address.BillingAddress1 = user.Address;
            address.BillingAddress2 = user.Address2;
            address.BillingCity = user.City;
            address.BillingState = user.State;
            address.BillingZipcode = user.Zip;
            address.BillingPhone = user.DayPhone;
            address.BillingEmail = user.Email;
        }

        private void GetTransactionSetup(string redirectUrl)
        {
            transactionSetup = new TransactionSetup();

            transactionSetup.ReturnURL = redirectUrl;
            transactionSetup.TransactionSetupMethod = TransactionSetupMethod.PaymentAccountCreate;
            transactionSetup.Embedded = BooleanType.False;
            transactionSetup.AutoReturn = BooleanType.True;
            transactionSetup.CVVRequired = BooleanType.False;
        }

        private void GetPaymentAccount(UserModel user)
        {
            paymentAccount = new PaymentAccount();

            // ##### TODO: Please change this to an actual account reference number attributed to the user #####
            string referenceId = GetSha1Hash(user.UserName + "{" + user.Email + "}");
            paymentAccount.PaymentAccountReferenceNumber = referenceId;
        }

        private string GetSha1Hash(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;

            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }

            return hash;
        }
        #endregion
    }
}