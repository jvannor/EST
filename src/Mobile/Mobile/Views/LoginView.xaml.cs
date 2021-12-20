using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Mobile.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
