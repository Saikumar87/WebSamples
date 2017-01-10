using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebAPI_Lab1.Models;

namespace WebAPI_Lab1.Controllers
{
    public class ValuesController : ApiController
    {
      
            // GET api/<controller>
            public IEnumerable<string> Get()
            {
                return new string[] { "value1", "value2" };
            }

            // GET api/<controller>/5
            public HttpResponseMessage Get(int id)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
                //response.Content = new StringContent("hello", Encoding.Unicode);

                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    MaxAge = TimeSpan.FromMinutes(20)
                };
                return response;
            }

            // POST api/<controller>
            //public string Post([FromBody]string value)
            //{
            //    return "Got Value :"+ value;
            //}
            public string Post([FromBody]MyModel value)
            {
                return "Got Value :" + value.Data;
            }

            // PUT api/<controller>/5
            public void Put(int id, [FromBody]string value)
            {
            }

            // DELETE api/<controller>/5
            public void Delete(int id)
            {
            }
        }
}
