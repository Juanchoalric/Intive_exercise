using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdController : ControllerBase
    {

        // POST api/id
        [HttpPost]
        public UserInfo PostUserId([FromBody] int id)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://randomuser.me/api/?ud=" + id.ToString());
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream newStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(newStream);
            var result = sr.ReadToEnd();
            var splashInfo = JsonConvert.DeserializeObject<RootObject>(result);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                UserInfo userInfo = new UserInfo();

                User user = new User();

                Address address = new Address();

                user.FirstName = splashInfo.Results.Select(x => x.Name).Select(x => x.First).FirstOrDefault();
                user.LastName = splashInfo.Results.Select(x => x.Name).Select(x => x.Last).FirstOrDefault();
                user.Image = splashInfo.Results.Select(x => x.Picture).Select(x => x.Thumbnail).FirstOrDefault();
                address.City = splashInfo.Results.Select(x => x.Location).Select(x => x.City).FirstOrDefault() + " - " + splashInfo.Results.Select(x => x.Location).Select(x => x.State).FirstOrDefault();
                address.Street = splashInfo.Results.Select(x => x.Location).Select(x => x.Street).FirstOrDefault();

                user.Address = address;

                userInfo.User = user;

                return userInfo;
            }

            return null;
            
        }
         
    }
}
