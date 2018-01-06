using System.Runtime.Serialization;
using System;
using System.Globalization;

namespace test_app.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}