﻿using Microsoft.AspNetCore.Mvc;

namespace TestApp.Controllers
{
	public class SecondController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}