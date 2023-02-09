﻿using System;
using System.Threading.Tasks;
using System.Linq;

namespace TastyBot
{
    public class Program
    {
        private const string BaseUrl = "https://api.tastyworks.com";
        private const string SecretName = "<YOUR_EMAIL>";
        private const string SecretSauce = "<YOUR_PASSWORD>";
        private const int TimeOut = 10;

        public static async Task Main(string[] args)
        {
            var tastyBot = Library.TastyBot.CreateInstance(SecretName, SecretSauce, BaseUrl, TimeOut);

            try
            {
                var sessionInfo = await tastyBot.getAuthorization();

                Console.WriteLine("Welcome: " + sessionInfo.user.username);

                var accounts = await tastyBot.getAccounts();

                accounts.items.ToList().ForEach(account =>
                {
                    Console.WriteLine("Account: " + account.account.accountnumber);
                });

                var balance = await tastyBot.getBalance(accounts.items.First().account.accountnumber);

                Console.WriteLine("Cash: " + balance.cashbalance + ", Maintenance: " + balance.maintenanceexcess + ", Reg-T Margin: " + balance.regtmarginrequirement + ", Futures Margin; " + balance.futuresmarginrequirement);

                var result = await tastyBot.processRules();

                if (result.Any() && result.All(x => x.answer == true))
                {
                    // TODO: Implement
                    // Sell a $25 wide SPX put spread at the 10 delta
                } else
                {
                    Console.WriteLine("Nothing to do at this time.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                tastyBot.Terminate();

                Console.WriteLine("Done and done.");
            }
        }
    }
}