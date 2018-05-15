using Microsoft.AspNetCore.Mvc;

namespace TeamCityTheatre.Web.Controllers {
  [Route("")]
  public class HomeController : Controller {

    [HttpGet("")]
    public IActionResult Index() {
      return RedirectToAction("Dashboard");
    }

    [HttpGet("dashboard/{*ignored}")]
    public IActionResult Dashboard() {
      return View("dashboard");
    }

    [HttpGet("settings")]
    public IActionResult Settings() {
      return View("settings");
    }
  }
}