using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TestApp.Controllers
{
	public class HomeController : Controller
	{
		public static List<SomethingViewModel> somethings = new List<SomethingViewModel>();

		public ActionResult Index()
		{
			return View("index", somethings);
		}

		public ActionResult Create()
		{
			return View("create", new SomethingViewModel());
		}

		[HttpPost]
		public ActionResult Create(SomethingViewModel vm)
		{
			somethings.Add(vm);
			return RedirectToAction("index");
		}
	}

	public class SomethingViewModel
	{
		public string FullName { get; set; }

		public string FullAddress { get; set; }
	}
}
