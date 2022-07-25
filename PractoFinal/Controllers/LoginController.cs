using PractoFinal.Models;
using PractoFinal.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PractoFinal.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home", new { area = "" });
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register registerDetails)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new PractoEntities3())
                {

                    Signup reglog = new Signup();


                    reglog.FirstName = registerDetails.FirstName;
                    reglog.LastName = registerDetails.LastName;
                    reglog.Email = registerDetails.Email;
                    reglog.Password = registerDetails.Password;


                    databaseContext.Signups.Add(reglog);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("Register");
            }
            else
            {

                return View("Register", registerDetails);
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var isValidUser = IsValidUser(model);


                
                if (isValidUser.Email == "phaniveludurthi@gmail.com")
                {
                    return RedirectToAction("Index", "Doctors", new { area = "" });
                }
                else if (isValidUser != null)
                {
                    
                    
                        FormsAuthentication.SetAuthCookie(model.Email, false);
                        return RedirectToAction("Filter", "Home", new { area = "" });
                    

                }
                
                
                else
                {
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }

        public Signup IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new PractoEntities3())
            {
                /*Signup user1 = dataContext.Signups.Where(x => x.Email == "phaniveludurthi@gmail.com"
                && x.Password == "123456789").SingleOrDefault();*/

                Signup user = dataContext.Signups.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                if(user.Email=="phaniveludurthi@gmail.com" && user.Password == "123456789")
                {
                    return user;
                }
                else
                {
                    if (user == null)
                        return null;



                    else
                        return user;

                }
                
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}