using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Utilities
{
    internal class Constants
    {
        // id, name, parent 
        public static readonly (int, string, int)[] SeizureReportClassifications =
        {
            (1, "Focal Onset", 0),
            (2, "Generalized Onset", 0),
            (3, "Unknown Onset", 0),
            (4, "Unclassified", 0),
            (5, "Focal to Bilateral Tonic-Clonic", 0),
            (6, "Motor Onset", 1),
            (7, "Nonmotor Onset", 1),
            (8, "Motor", 2),
            (9, "Nonmotor (absence)", 2),
            (10, "Motor", 3),
            (11, "Nonmotor", 3),
            (12, "Automatisms", 6),
            (13, "Atonic", 6),
            (14, "Clonic", 6),
            (15, "Epileptic Spasms", 6),
            (16, "Hyperkinetic", 6),
            (17, "Myoclonic", 6),
            (18, "Tonic", 6),
            (19, "Autonomic", 7),
            (20, "Behavior Arrest", 7),
            (21, "Cognitive", 7),
            (22, "Emotional", 7),
            (23, "Sensory", 7),
            (24, "Tonic-Clonic", 8),
            (25, "Clonic", 8),
            (26, "Tonic", 8),
            (27, "Myoclonic", 8),
            (28, "Myoclonic-Tonic-Clonic", 8),
            (29, "Myoclonic-Atonic", 8),
            (30, "Epilieptic Spasms", 8),
            (31, "Typical", 9),
            (32, "Atypical", 9),
            (33, "Myclonic", 9),
            (34, "Eyelid Myclonia", 9),
            (35, "Tonic-Clonic", 10),
            (36, "Epilpetic Spsams", 10),
            (37, "Behavior Arrest", 11)
        };

        public const string Authority = "https://identity.dev.detroitcyclecar.com:5001";
        public const string ClientId = "EST.Mobile";
        public const string PostLogoutRedirectUri = "est.mobile://signout-callback-oidc";
        public const string RedirectUri = "est.mobile://signin-oidc";
        public const string Scope = "openid profile email EST.WebApi offline_access";
        public const string Api = "https://api.dev.detroitcyclecar.com:6001";
        public const string ReportsApiEndpoint = "/api/report";
    }
}
