using System;
using System.Collections.Generic;

namespace _2c2p.Assignment.WebApp.Dto
{
    public class TransactionUploadResponseModel
    {
        public bool IsSuccessful { get; set; } = true;
        public bool HasValiationErrors { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
   
}
