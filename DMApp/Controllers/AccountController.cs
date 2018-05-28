using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UserManager.Models;

namespace UserManager.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ExtendedUser> UserManager => HttpContext.GetOwinContext().Get<UserManager<ExtendedUser>>();
        public SignInManager<ExtendedUser, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<ExtendedUser, string>>();

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var SignInStatus = await SignInManager.PasswordSignInAsync(model.Username, model.password, true, true);

                switch (SignInStatus)
                {
                    case SignInStatus.Success:
                        {
                            HttpClient client = new HttpClient();
                            var nvc = new List<KeyValuePair<string, string>>();
                            nvc.Add(new KeyValuePair<string, string>("username", model.Username));
                            nvc.Add(new KeyValuePair<string, string>("password", model.password));
                            nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
                            var req = new HttpRequestMessage(HttpMethod.Post, new Uri("http://localhost:57853/token")) { Content = new FormUrlEncodedContent(nvc) };

                            var res = await client.SendAsync(req);
                            var con = await res.Content.ReadAsStringAsync();

                            return RedirectToAction("Index", "Home");
                        }

                    default:
                        ModelState.AddModelError("", "Invalid Credentials");
                        return View(model);
                }
            }
            else
                return View(model);
        }

        public ActionResult Register()
        {
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            try
            {
                var identityUser = await UserManager.FindByNameAsync(model.Username);
                if (identityUser != null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var user = new ExtendedUser()
                {
                    UserName = model.Username,
                    Name = model.Name,
                    Gender = model.Gender,



                };
                var identityresult = await UserManager.CreateAsync(user, model.password);
                if (identityresult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", identityresult.Errors.FirstOrDefault());

                return View(model);
            }
            catch (Exception e)
            {
                return View(model);
            }
        }
    }

    public class RegisterModel
    {
        public string Username { get; set; }

        public string password { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string  Country { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }

        public string password { get; set; }
    }
}
