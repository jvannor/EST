using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Utilities
{
    internal class Constants
    {
        public static readonly Tree<string> SeizureClassifications = new Tree<string> { Value = "root" }.AddMultiple(
            new Tree<string> { Value = "Unclassified" }.AddMultiple(
                new Tree<string> { Value = "-" }.AddMultiple (
                    new Tree<string> { Value = "-" })),
            new Tree<string> { Value = "Unknown Onset" }.AddMultiple(
                new Tree<string> { Value = "Motor" }.AddMultiple(
                    new Tree<string> { Value = "Tonic-Clonic" },
                    new Tree<string> { Value = "Epileptic Spasms" }),
                new Tree<string> { Value = "Nonmotor" }.AddMultiple(
                    new Tree<string> { Value = "Behavior Arrest" })),
            new Tree<string> { Value = "Focal Onset" }.AddMultiple(
                new Tree<string> { Value = "Motor Onset" }.AddMultiple(
                    new Tree<string> { Value = "Automatisms" },
                    new Tree<string> { Value = "Atonic" },
                    new Tree<string> { Value = "Clonic" },
                    new Tree<string> { Value = "Epileptic Spasms" },
                    new Tree<string> { Value = "Hyperkinetic" },
                    new Tree<string> { Value = "Myoclonic" },
                    new Tree<string> { Value = "Tonic" }),
                new Tree<string> { Value = "Nonmotor Onset" }.AddMultiple(
                    new Tree<string> { Value = "Autonomic" },
                    new Tree<string> { Value = "Behavior Arrest" },
                    new Tree<string> { Value = "Cognitive" },
                    new Tree<string> { Value = "Emotional" },
                    new Tree<string> { Value = "Sensory" })),
            new Tree<string> { Value = "Generalized Onset" }.AddMultiple(
                new Tree<string> { Value = "Motor" }.AddMultiple(
                    new Tree<string> { Value = "Tonic-Clonic" },
                    new Tree<string> { Value = "Clonic" },
                    new Tree<string> { Value = "Tonic" },
                    new Tree<string> { Value = "Myoclonic" },
                    new Tree<string> { Value = "Myoclonic-Tonic-Clonic" },
                    new Tree<string> { Value = "Myoclonic-Atonic" },
                    new Tree<string> { Value = "Atonic" },
                    new Tree<string> { Value = "Epileptic Spasms" }),
                new Tree<string> { Value = "Nonmotor (Absence)" }.AddMultiple(
                    new Tree<string> { Value = "Typical" },
                    new Tree<string> { Value = "Atypical" },
                    new Tree<string> { Value = "Myclonic" },
                    new Tree<string> { Value = "Eyelid Myoclonia" })));

        public const string Authority = "https://identity.dev.detroitcyclecar.com:5001";
        public const string ClientId = "EST.Mobile";
        public const string PostLogoutRedirectUri = "est.mobile://signout-callback-oidc";
        public const string RedirectUri = "est.mobile://signin-oidc";
        public const string Scope = "openid profile email EST.WebApi offline_access";
        public const string Api = "https://api.dev.detroitcyclecar.com:6001";
        public const string ReportsApiEndpoint = "/api/report";
        public const string SettingsApiEndpoint = "/api/settings";
        public const string SettingsDocument = "SettingsDocument";
    }
}
