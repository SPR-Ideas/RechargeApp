using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RechargeApp.Data;
using RechargeApp.Models;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace RechargeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public readonly CRUD _crud;
        public User _User;

        public HomeController(ApplicationDbContext context,IConfiguration configuration)
        {
            _context = context;
            _crud = new CRUD(_context);
            _configuration = configuration;
        }

        private User  getCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            Console.WriteLine(identity);

            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();
                // Do something with the token
                Console.WriteLine(token);
            }
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new User
                {
                    PhoneNumber = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    id = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value),
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                    Name = userClaims.FirstOrDefault(o=>o.Type == ClaimTypes.MobilePhone)?.Value
                };
            }
            return null;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignUp([Bind("id,Name,PhoneNumber,password,Role")] User user)
		{
			if (ModelState.IsValid)
			{
				_context.Add(user);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(login));
			}
			return View(user);
		}

		[HttpGet]
        public IActionResult login() {
            return View();
        }

		public string CreateToken()
		{
			List<Claim> claim = new List<Claim>() {
			new Claim(ClaimTypes.Name, _User.PhoneNumber),
			new Claim(ClaimTypes.Sid,Convert.ToString(_User.id)),
            new Claim(ClaimTypes.Role, _User.Role),
            new Claim(ClaimTypes.MobilePhone,_User.Name)
			};
			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
				_configuration.GetSection("AppSettings:Token").Value
				));
			var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claim,
				expires: DateTime.Now.AddHours(2),
				signingCredentials: cred
				);
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}

		[HttpPost]
        public async Task<IActionResult >login(User user) 
        {
            user = await _crud.VerifyLogin(user);
            if (user!=null) {
                _User = user;
				string token = CreateToken();
				//Response.Headers.Add("Authorization", "Bearer "+ token);
				Response.Cookies.Append("auth_token", token);
                if (user.Role =="User")
				    return RedirectToAction("dashboard");
                if (user.Role == "Admin")
                    return RedirectToAction("dashboardadmin");
            }
            return Ok("NotDone");
        }

        [Authorize(Roles ="User" ,AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> dashboard() 
        {
            User _user =  getCurrentUser();
            List<PlanTable> planTable = await _crud.getAllPlans();

            ViewBag.user = _user;
            ViewBag.planTables = planTable;

            return View();

        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> dashboardadmin()
        {
            User _user = getCurrentUser();
            
            ViewBag.user = _user;

            return View();

        }

        [HttpPost]
        [Authorize(Roles = "User", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> dashboard( string planId)
        {
            User _user = getCurrentUser();
            await _crud.addPlanToUser(_user.id,Convert.ToInt32( planId));
            

            List<PlanTable> planTable = await _crud.getAllPlans();
            ViewBag.user = _user;
            ViewBag.planTables = planTable;


            return View();

        }


        public IActionResult Index()
        {
            return View();
        }

        
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}