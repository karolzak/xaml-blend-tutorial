using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Xaml.Blend.Tutorial.Shared.Microsoft;
using Xaml.Blend.Tutorial.Shared.Models;
using Xaml.Blend.Tutorial.Shared.ViewModels;
using System.Linq;      
using Windows.Security.Credentials;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Xaml.Blend.Tutorial.W81.ViewModels
{
    public class MainPageViewModel : MainPageViewModelBase
    {
        // Define a member variable for storing the signed-in user. 
        private MobileServiceUser user;

        // Define a method that performs the authentication process
        // using a Facebook sign-in. 
        //private async System.Threading.Tasks.Task AuthenticateAsync()
        //{
        //    while (user == null)
        //    {
        //        string message;
        //        try
        //        {
        //            // Change 'MobileService' to the name of your MobileServiceClient instance.
        //            // Sign-in using Facebook authentication.
        //            user = await App.MobileService
        //                .LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
        //            message =
        //                string.Format("You are now signed in - {0}", user.UserId);
        //        }
        //        catch (InvalidOperationException)
        //        {
        //            message = "You must log in. Login Required";
        //        }

        //        var dialog = new MessageDialog(message);
        //        dialog.Commands.Add(new UICommand("OK"));
        //        await dialog.ShowAsync();
        //    }
        //}

        public override async void LogoutAsync()
        {
            App.MobileService.Logout();

            string message;
            // This sample uses the Facebook provider.
            var provider = "AAD";

            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;
            try
            {
                // Try to get an existing credential from the vault.
                credential = vault.FindAllByResource(provider).FirstOrDefault();
                vault.Remove(credential);
            }
            catch (Exception)
            {
                // When there is no matching resource an error occurs, which we ignore.
            }
            message = string.Format("You are now logged out!");
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
            IsLoggedIn = false;

        }

        public override async void LoginAsync()
        {
            string message;
            // This sample uses the Facebook provider.
            var provider = "AAD";

            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            while (credential == null)
            {
                try
                {
                    // Try to get an existing credential from the vault.
                    credential = vault.FindAllByResource(provider).FirstOrDefault();
                }
                catch (Exception)
                {
                    // When there is no matching resource an error occurs, which we ignore.
                }

                if (credential != null)
                {
                    // Create a user from the stored credentials.
                    user = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    user.MobileServiceAuthenticationToken = credential.Password;

                    // Set the user from the stored credentials.
                    App.MobileService.CurrentUser = user;

                    try
                    {
                        // Try to return an item now to determine if the cached credential has expired.
                        await App.MobileService.InvokeApiAsync("custom", HttpMethod.Get, new Dictionary<string,string>() );                        
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            // Remove the credential with the expired token.
                            vault.Remove(credential);
                            credential = null;
                            continue;
                        }
                    }
                }
                else
                {
                    try
                    {
                        // Login with the identity provider.
                        user = await App.MobileService
                            .LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);

                        // Create and store the user credentials.
                        credential = new PasswordCredential(provider,
                            user.UserId, user.MobileServiceAuthenticationToken);
                        vault.Add(credential);
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        message = "You must log in. Login Required";
                    }
                }
                message = string.Format("You are now logged in - {0}", user.UserId);
                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
                IsLoggedIn = true;
            }
        }


        public async override void BroadcasterAsync()
        {
            await App.MobileService.InvokeApiAsync("broadcaster", new JObject(new JProperty("message", "Witaj świecie z Windows 8.1!")));

        }

        public async override void SingleUserPushAsync()
        {
            await App.MobileService.InvokeApiAsync("sendpush", new JObject(new JProperty("message", "Witaj użytkowniku z Windows 8.1!")));
        }

        public override void PushRegisterAsync()
        {
            karolzakpipPush.UploadChannel();
            IsPushRegistered = true;
        }
    }
}
