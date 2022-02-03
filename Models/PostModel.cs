using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IrsSubmission.Models
{
    public class PostModel
    {
       
    }
    public class InfoModel
    {
        public string etin { get; set; }
        public string appSysId { get; set; }
    }
    public class submissionModel
    {
        public string etin { get; set; }
        public string appSysId { get; set; }
        public string submissionId { get; set; }
        public string Manifest { get; set; }
        public string XML { get; set; }
        public string PDF { get; set; }
        public string environment { get; set; }
    }
    public class PostAck
    {
        public string etin { get; set; }
        public string appSysId { get; set; }
        public string submissionId { get; set; }
        public string environment { get; set; }
    }
}