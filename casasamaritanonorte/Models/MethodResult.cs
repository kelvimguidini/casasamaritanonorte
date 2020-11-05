using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace casasamaritanonorte.Models
{
    public class MethodResult
    {
        public string errorMessage;
        public ReturnCode returnCode;

        public enum ReturnCode
        {
            Unknown = 0,
            Success = 1,
            Failure
        }
    }
}