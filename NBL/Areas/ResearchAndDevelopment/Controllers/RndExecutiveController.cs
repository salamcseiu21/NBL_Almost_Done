using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Models.Logs;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize(Roles = "R&D")]
    public class RndExecutiveController : Controller
    {
        // GET: ResearchAndDevelopment/RndExecutive


    }
}