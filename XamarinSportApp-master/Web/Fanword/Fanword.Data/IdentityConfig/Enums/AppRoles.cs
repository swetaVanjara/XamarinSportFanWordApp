using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Enums {
    public static class AppRoles {
        public const string QualifierSystem = "System";
        public const string SuffixAdmin = "Admin";
        public static string SystemAdmin = AppRules.Join(QualifierSystem, SuffixAdmin);

        public static string ContentSource = "ContentSource";
        public static string TeamAdmin = "TeamAdmin";
        public static string SchoolAdmin = "SchoolAdmin";
        public static string Advertiser = "Advertiser";
        public static string Data = "Data";
        public static string DataAdv = "DataAdv";
    }
}
