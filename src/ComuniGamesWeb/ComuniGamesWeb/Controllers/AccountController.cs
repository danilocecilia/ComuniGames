using ComuniGamesWeb.Models;
using ComuniGamesWeb.Models.DataContext;
using ComuniGamesWeb.ViewModels;
using FineUploader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using ImageResizer;
using System.Configuration;
using ComuniGamesWeb.Models.Interfaces;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ComuniGamesWeb.Controllers
{
    public class AccountController : Controller
    {
        #region [ Attributes ]
        private IRepository<Cidade> _cityRepository;
        private IRepository<Endereco> _addressRepository;
        private IRepository<Usuario> _userRepository;
        private IRepository<UserProfile> _userProfileRepository;
        private AccountViewModel _accountViewModel;
        private IRepository<Estado> _stateRepository;
        #endregion

        #region [ Constructor ]
        public AccountController(IRepository<UserProfile> userProfile, IRepository<Usuario> user, IRepository<Cidade> city, IRepository<Estado> state, IRepository<Endereco> address)
        {
            _addressRepository = address;
            _userRepository = user;
            _cityRepository = city;
            _userProfileRepository = userProfile;
            _accountViewModel = new AccountViewModel();
            _stateRepository = state;
        }
        #endregion

        #region [ Get Methods ]
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        //GET: /Register/
        public ViewResult Register()
        {
            return View(new AccountViewModel());
        }

        [HttpPost]
        public JsonResult GetCitiesByStateId(int id)
        {
            var cities = _cityRepository.SearchFor(w => w.cod_estado == id).Select(w => new { cod_cidade = w.cod_cidade, nom_cidade = w.nom_cidade });
            return Json(new { Result = true, cities = cities }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListAllStates()
        {
            var states = _stateRepository.GetAll().ToList().Select(w => new { cod_estado = w.cod_estado, nom_estado = w.nom_estado });
            return Json(new { Result = true, states = states }, JsonRequestBehavior.AllowGet);
        }

        //public JsonRequestBehavior GetStateByStateID(short stateId)
        //{
        //    var states = _stateRepository.Get(w => w.cod_estado == stateId).cod_estado;
        //    return Json(new { Result = true, states = states }, JsonRequestBehavior.AllowGet);
        //}

        //
        // GET: /FileUpload/
        public FineUploader.FineUploaderResult UploadFile(FineUploader.FineUpload upload)
        {
            // asp.net mvc will set extraParam1 and extraParam2 from the params object passed by Fine-Uploader
            var dir = ConfigurationManager.AppSettings.Get("ImageUpload");

            var filePath = Path.Combine(dir, upload.Filename);
            try
            {
                upload.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }

            // the anonymous object in the result below will be convert to json and set back to the browser
            return new FineUploaderResult(true, new { fileName = upload.Filename });
        }

        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        #endregion

        #region [ Post Methods ]
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountViewModel accountViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userProfileRepository.Get(w => w.UserName == accountViewModel.UserProfile.UserName);

                    if (null == user)
                    {
                        
                        WebSecurity.CreateUserAndAccount(accountViewModel.UserProfile.UserName, accountViewModel.Password, new { UserName = accountViewModel.UserProfile.UserName });
                        WebSecurity.Login(accountViewModel.UserProfile.UserName, accountViewModel.Password, true);

                        accountViewModel.Users.UserId = WebSecurity.GetUserId(accountViewModel.UserProfile.UserName);
                        accountViewModel.Users.Endereco.CidadeID = int.Parse(accountViewModel.City);
                        //accountViewModel.Users.Endereco.EstadoID = short.Parse(accountViewModel.State);
                        accountViewModel.Users.Endereco.MostraLocalizacao = true;

                        _userRepository.Insert(accountViewModel.Users);
                        _addressRepository.Insert(accountViewModel.Users.Endereco);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(accountViewModel);
        }

        [Authorize]
        public ActionResult Edit(int userId = 0)
        {
            try
            {
                var user = _userRepository.Get(w => w.UserId == userId);
                _accountViewModel.Users = user;

                return View(_accountViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountViewModel acc)
        {
            var userId = Membership.GetUser().ProviderUserKey;

            if (userId != null)
            {
                if (ModelState.IsValid)
                {
                    int _userId = int.Parse(userId.ToString());

                    //acc.Users.UserId = _userRepository.Get(w => w.UserId == _userId).ID;
                    //acc.Users.UserId = _userId;
                    acc.UploadImage = ViewBag.FileName;
                    acc.Users.Endereco.ID = acc.Users.EnderecoId;
                    acc.Users.Endereco.CidadeID = int.Parse(acc.City);
                    //acc.Users.Endereco.EstadoID = short.Parse(acc.State);

                    try
                    {
                        _userRepository.Update(acc.Users);
                        _addressRepository.Update(acc.Users.Endereco);
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            return View(acc);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginVM, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(loginVM.UserName, loginVM.Password, persistCookie: loginVM.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            //ModelState.AddModelError("", "The user name or password provided is incorrect.");
            ViewBag.Msg = "Nome de usuário ou senha inválidos.";
            return View(loginVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel changePass)
        {
            if (ModelState.IsValid)
            {
                // Update the password.
                MembershipUser u = Membership.GetUser(User.Identity.Name);

                try
                {
                    if (u.ChangePassword(changePass.Password, changePass.NewPassword))
                    {
                        ViewBag.Msg = "Senha trocada com sucesso!";
                    }
                    else
                    {
                        ViewBag.Msg = "Erro ao mudar de senha. Verifique a senha digitada.";
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Text = "An exception occurred: " + Server.HtmlEncode(e.Message) + ". Please re-enter your values and try again.";
                }
            }
            return View();
        }
        #endregion

        public PartialViewResult Address()
        {
            return PartialView(_accountViewModel);
        }

        public ActionResult GoogleMap()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}
