using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fanword.Api.Models
{
    public class RebuildFile
    {
        public List<string> Guids { get; set; }
        public string FileExtension { get; set; }
        public string OriginalFileName { get; set; }
    }
}