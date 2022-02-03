using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IrsSubmission.Models
{
    public class GetModel
    {

    }
    public class SubmissionResponse
    {
        public string MessageID { get; set; }
        public string Relatesto { get; set; }
        public string Statustxt { get; set; }
    }
    public class AckResponse
    {
        public string MessageID { get; set; }
        public byte[] content { get; set; }
        public string Relatesto { get; set; }
        public string Statustxt { get; set; }
    }
}