using IrsSubmission.Submission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IrsSubmission.Models;

namespace IrsSubmission.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        SiteFunctions _functions = new SiteFunctions();
        
        [HttpPost]
        [Route("api/values/login")]
        // GET api/values/5
        public dynamic Login([FromBody] submissionModel values)
        {
            dynamic final = new DataTable();
            dynamic obj = new ExpandoObject();
            string etin = "";
            string appSysId = "";
            if (values!=null)
            {
                etin = values.etin;
                appSysId = values.appSysId;
            }
            else
            {
                obj.output = "Failure";
                obj.data = new DataTable();
                obj.reason = "Data supplied is not in valid format. ";
                return obj;
            }
            final = _functions.SendSubmission(values);
            if (final != null)
            {
                obj.output = "success";
                obj.data = final;
                obj.reason = "";
            }
            else
            {
                obj.output = "Failure";
                obj.data = final;
                obj.reason = "No Record Found. ";
            }
            return obj;
        }
        [HttpPost]
        [Route("api/values/ack")]
        // GET api/values/5
        public dynamic GetAck([FromBody] PostAck values)
        {
            dynamic final = new DataTable();
            dynamic obj = new ExpandoObject();
            string etin = "";
            string appSysId = "";
            
            if (values != null)
            {
                etin = values.etin;
                appSysId = values.appSysId;

            }
            else
            {
                obj.output = "Failure";
                obj.data = new DataTable();
                obj.reason = "Data supplied is not in valid format. ";
                return obj;
            }
            final = _functions.GetAck(values);
            if (final != null)
            {
                obj.output = "success";
                obj.data = final;
                obj.reason = "";
            }
            else
            {
                obj.output = "Failure";
                obj.data = final;
                obj.reason = "No Record Found. ";
            }
            return obj;
        }
        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
