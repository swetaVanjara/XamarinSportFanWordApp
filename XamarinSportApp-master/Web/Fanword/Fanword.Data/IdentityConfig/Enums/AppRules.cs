using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Enums {
    public static class AppRules {
        public static string Join(string qualifier, string suffix) {
            return qualifier + "_" + suffix;
        }
        #region Qualifiers

        #endregion

        #region Suffixes
        public const string SuffixRead = "Read";
        public const string SuffixEdit = "Edit";
        #endregion
    }
}
