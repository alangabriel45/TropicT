using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TropicTrail.Utils;

namespace TropicTrail.Models
{
    public class Response
    {
        public int code { get; set; }
        public String message { get; set; }
        public ErrorCode codes { get; set; }
    }
}