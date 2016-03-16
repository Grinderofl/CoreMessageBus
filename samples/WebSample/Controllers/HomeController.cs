using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using CoreMessageBus.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace WebSample.Controllers
{
    public class HomeController : Controller
    {
        private IServiceBus _serviceBus;

        public HomeController(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public IActionResult Index(string id = null)
        {
            if(id != null)
                _serviceBus.Send(new Message()
                {
                    Value = id
                });
            return Content(id);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
