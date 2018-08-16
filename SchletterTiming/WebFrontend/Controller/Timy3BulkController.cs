using ReaderInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Timy3Reader;
using Model;

namespace WebFrontend.Controller {
    public class Timy3BulkController : ApiController {

        private static int counter = 0;
        private ITimy3Reader timy3Reader;


        public IHttpActionResult Get(string name) {
            if (name == "USB") {
                timy3Reader = new Timy3UsbReader();
            } else {
                timy3Reader = new Timy3RS232Reader();
            }

            timy3Reader.Init();

            var results = timy3Reader.WaitForBulk();

            return Ok<IEnumerable<Dummy>>(results);
        }


        public void Post([FromBody]string name) {
            Console.WriteLine($"{name}");
            return;
        }
    }
}
