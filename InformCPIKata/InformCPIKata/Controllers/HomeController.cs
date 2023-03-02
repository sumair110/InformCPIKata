using InformCPIKata.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InformCPIKata.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Error()
		{
			var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
			return View(errorViewModel);
		}
	}
}
