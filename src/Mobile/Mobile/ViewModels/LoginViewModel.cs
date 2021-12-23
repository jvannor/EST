﻿using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Mobile.ServiceContracts;

namespace Mobile.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        public Command LoginCommand => new Command(OnLogin);

        public LoginViewModel(IAuthenticationService authentication) 
        {
            authenticationService = authentication;
        }

        public async void OnLogin()
        {
            try
            {
                var success = await authenticationService.Login();
                if (success)
                {
                    await Shell.Current.GoToAsync("//home");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"LoginViewModel::OnLogin() experienced an unexpected exception, {ex.GetType().Name}; {ex.Message}");
            }
        }

        private IAuthenticationService authenticationService;
    }
}