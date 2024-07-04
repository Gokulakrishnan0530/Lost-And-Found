using Lost_And_Found.Admin;
using Lost_And_Found.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;


namespace Lost_And_Found.Admin
{
    public class AdminController : Controller
    {
        private readonly CoreProjectContext _projectContext;
        public AdminController(CoreProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminView()
        {
            return View(new Admin());
        }

        [HttpPost]
        public IActionResult AdminView(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return View(admin);
            }

           
            var existingAdmin = _projectContext.Admin
                .FirstOrDefault(a => a.EmailId == admin.EmailId && a.Password == admin.Password);

            if (existingAdmin != null)
            {
                
                return Redirect("~/AdminUi/index.html");
            }

            else
            {
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(admin);
            }
        }
    }
}
