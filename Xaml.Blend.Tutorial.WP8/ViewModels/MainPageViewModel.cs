using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xaml.Blend.Tutorial.Shared.Microsoft;
using Xaml.Blend.Tutorial.Shared.Models;
using Xaml.Blend.Tutorial.Shared.ViewModels;

namespace Xaml.Blend.Tutorial.WP8.ViewModels
{
    public class MainPageViewModel : MainPageViewModelBase
    {
        private MobileServiceUser user;
        public override void LogoutAsync()
        {
            App.MobileService.Logout();

            string message;
            // This sample uses the Facebook provider.
            var provider = "AAD";

            // Isolated storage for the app.
            IsolatedStorageSettings settings =
                IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(provider))
                settings.Remove(provider);
            settings.Save();
            message = string.Format("You are now logged out!");
            MessageBox.Show(message);
            IsLoggedIn = false;
        }

        public async override void LoginAsync()
        {
            string message;
            // This sample uses the Facebook provider.
            var provider = "AAD";

            // Provide some additional app-specific security for the encryption.
            byte[] entropy = { 1, 8, 3, 6, 5 };

            // Authorization credential.
            MobileServiceUser user = null;

            // Isolated storage for the app.
            IsolatedStorageSettings settings =
                IsolatedStorageSettings.ApplicationSettings;

            while (user == null)
            {
                // Try to get an existing encrypted credential from isolated storage.                    
                if (settings.Contains(provider))
                {
                    // Get the encrypted byte array, decrypt and deserialize the user.
                    var encryptedUser = settings[provider] as byte[];
                    var userBytes = ProtectedData.Unprotect(encryptedUser, entropy);
                    user = JsonConvert.DeserializeObject<MobileServiceUser>(
                        System.Text.Encoding.Unicode.GetString(userBytes, 0, userBytes.Length));
                }
                if (user != null)
                {
                    // Set the user from the stored credentials.
                    App.MobileService.CurrentUser = user;

                    try
                    {
                        // Try to return an item now to determine if the cached credential has expired.
                        await App.MobileService.InvokeApiAsync("custom", HttpMethod.Get, new Dictionary<string, string>());
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            // Remove the credential with the expired token.
                            settings.Remove(provider);
                            settings.Save();
                            user = null;
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
                            .LoginAsync(provider);

                        // Serialize the user into an array of bytes and encrypt with DPAPI.
                        var userBytes = System.Text.Encoding.Unicode
                            .GetBytes(JsonConvert.SerializeObject(user));
                        byte[] encryptedUser = ProtectedData.Protect(userBytes, entropy);

                        // Store the encrypted user credentials in local settings.
                        settings.Add(provider, encryptedUser);
                        settings.Save();
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        message = "You must log in. Login Required";
                    }
                }
                message = string.Format("You are now logged in - {0}", user.UserId);
                MessageBox.Show(message);
                IsLoggedIn = true;
            }
        }

        public async override void BroadcasterAsync()
        {
            await App.MobileService.InvokeApiAsync("broadcaster", new JObject(new JProperty("message", "Witaj swiecie z Windows Phone 8.0!")));

        }

        public async override void SingleUserPushAsync()
        {
            await App.MobileService.InvokeApiAsync("sendpush", new JObject(new JProperty("message", "Witaj uzytkowniku z Windows Phone 8.0!")));
        }
        public override void PushRegisterAsync()
        {
            App.RegisterPush();
            IsPushRegistered = true;
        }
    }
}
