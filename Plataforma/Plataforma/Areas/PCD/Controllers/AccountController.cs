using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Plataforma.Areas.PCD.Models;
using System.Security.Principal;
using System.Web.Security;
using System.Collections.Generic;
using System.Web.SessionState;

namespace Plataforma.Areas.PCD.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private pimasEntities db = new pimasEntities();
        private ApplicationDbContext dbIdentity = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Session["usuario"] == null)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                return RedirectToAction("../");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var pass = Utilitarios.EncodePassword(model.Email + model.Password);
            usuario user = db.usuarios.Where(u => u.username.Equals(model.Email) && u.password.Equals(pass)).FirstOrDefault();
            if (user != null)
            {
                //FormsAuthentication.SetAuthCookie(user.nombre + " " + user.apellidos, model.RememberMe);
                if (user.bloqueado == true)
                {
                    ModelState.Remove("Password");
                    ModelState.AddModelError("", "Este usuario esta bloqueado");
                    return View();
                }
                if (user.logueado == true)
                {
                    int dia = user.log_visitas.Last().fecha_hora.DayOfYear;
                    int hora = user.log_visitas.Last().fecha_hora.Hour;
                    //int minuto = hora*60 + user.log_visitas.Last().fecha_hora.Minute;

                    if (dia == DateTime.Now.DayOfYear && (DateTime.Now.Hour - hora <= 1))
                    {
                        ModelState.Remove("Password");
                        ModelState.AddModelError("", "Esta cuenta ya se encuentra en uso");
                        return View();
                    }
                }
                bool isVigente = new usuariosController().VerificarVigencia(user.id);
                if (isVigente)
                {
                    user.cursos = user.cursos.OrderBy(c => c.curso1).ToList();
                    this.SignInUser(user.nombre + " " + user.apellidos, user.id, model.RememberMe);
                    //System.Web.HttpContext.Current.Session["usuario"] = user;
                    Session["usuario"] = user;
                    db.usuarios.Find(user.id).logueado = true;
                    log_visitas log = new log_visitas();
                    log.usuario = user;
                    log.id_usuario = user.id;
                    log.fecha_hora = DateTime.Now;
                    db.log_visitas.Add(log);
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(user.nombre + " " + user.apellidos, model.RememberMe);

                    if (returnUrl == null || returnUrl == "")
                    {
                        return Redirect("../");
                    }
                    else
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return Redirect(returnUrl);
                        }
                    }
                }
                else
                {
                    ModelState.Remove("Password");
                    ModelState.AddModelError("", "Se acabo el tiempo de tu subscripción, contacta al equipo de PIMAS para renovar tu subscripcion");
                    return View();
                }
            }
            ModelState.Remove("Password");
            ModelState.AddModelError("", "Los datos insertados son incorrectos");
            return View();
        }
        private void SignInUser(string username, int id, bool isPersistent)
        {
            // Initialization.    
            var claims = new List<Claim>();
            try
            {
                // Setting    
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.Sid, id.ToString()));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.    
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult LogOff()
        {
            usuario user = (usuario)HttpContext.Session["usuario"];
            if (user != null)
            {
                try
                {
                    db.usuarios.Find(user.id).logueado = false;
                    db.SaveChanges();
                    // Setting.    
                    var ctx = Request.GetOwinContext();
                    var authenticationManager = ctx.Authentication;
                    // Sign Out.    
                    authenticationManager.SignOut();
                    Session["usuario"] = null;
                    Session.RemoveAll();
                    Session.Abandon();
                }
                catch (Exception ex)
                {
                    // Info    
                    throw ex;
                }
            }
            // Info.    
            return this.RedirectToAction("Login", "Account");
        }

        public ActionResult CerrarSesion(int id)
        {
            try
            {
                db.usuarios.Find(id).logueado = false;
                db.SaveChanges();
                //Session.RemoveAll();
                //Session.Abandon();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this.RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult LogOffAutomatico()
        {
            if (Session["usuario"] != null)
            {
                usuario usuarioSesion = (usuario)HttpContext.Session["usuario"];
                db.usuarios.Find(usuarioSesion.id).logueado = false;
                db.SaveChanges();
                Session.RemoveAll();
                Session.Abandon();
                return Json("Exito", JsonRequestBehavior.AllowGet);
            }
            return Json("Usuario no autenticado o sin permisos para utilizar esta función", JsonRequestBehavior.AllowGet);
        }





        ////
        //// GET: /Account/Login
        //[AllowAnonymous]
        //public ActionResult Login(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}

        ////
        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    string query = "SELECT * from usuarios where username = '"+model.Email+"' and password = '"+Utilitarios.EncodePassword(string.Concat(model.Email, model.Password))+"'";
        //    if (db.usuarios.SqlQuery(query).ToList().Count == 1)
        //    {
        //        usuario usuario = db.usuarios.SqlQuery(query).First();
        //        System.Web.HttpContext.Current.Session["usuario"] = usuario;
        //        FormsAuthentication.SetAuthCookie(usuario.username, model.RememberMe);
        //        System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
        //          new System.Security.Principal.GenericIdentity(usuario.username, "Forms"), usuario.roles.FirstOrDefault().rol.Split(';'));
        //        return RedirectToAction("../usuarios");
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //    // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
        //    // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
        //    //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        //    //switch (result)
        //    //{
        //    //    case SignInStatus.Success:
        //    //        return RedirectToLocal(returnUrl);
        //    //    case SignInStatus.LockedOut:
        //    //        return View("Lockout");
        //    //    case SignInStatus.RequiresVerification:
        //    //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //    //    case SignInStatus.Failure:
        //    //    default:
        //    //        ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
        //    //        return RedirectToAction("../");
        //    //}
        //}

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Requerir que el usuario haya iniciado sesión con nombre de usuario y contraseña o inicio de sesión externo
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // El código siguiente protege de los ataques por fuerza bruta a los códigos de dos factores. 
            // Si un usuario introduce códigos incorrectos durante un intervalo especificado de tiempo, la cuenta del usuario 
            // se bloqueará durante un período de tiempo especificado. 
            // Puede configurar el bloqueo de la cuenta en IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código no válido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //            // Para obtener más información sobre cómo habilitar la confirmación de cuenta y el restablecimiento de contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
        //            // Enviar correo electrónico con este vínculo
        //            // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            // await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");

        //            return RedirectToAction("Index", "Home");
        //        }
        //        AddErrors(result);
        //    }

        //    // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        //    return View(model);
        //}

        ////
        //// GET: /Account/ConfirmEmail
        //[AllowAnonymous]
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return View("Error");
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(userId, code);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}

        ////
        //// GET: /Account/ForgotPassword
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // No revelar que el usuario no existe o que no está confirmado
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // Para obtener más información sobre cómo habilitar la confirmación de cuenta y el restablecimiento de contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Enviar correo electrónico con este vínculo
        //        // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
        //        // await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", "Para restablecer la contraseña, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");
        //        // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        //    return View(model);
        //}

        ////
        //// GET: /Account/ForgotPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        ////
        //// GET: /Account/ResetPassword
        //[AllowAnonymous]
        //public ActionResult ResetPassword(string code)
        //{
        //    return code == null ? View("Error") : View();
        //}

        //////
        ////// POST: /Account/ResetPassword
        ////[HttpPost]
        ////[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        ////public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        ////{
        ////    if (!ModelState.IsValid)
        ////    {
        ////        return View(model);
        ////    }
        ////    var user = await UserManager.FindByNameAsync(model.Email);
        ////    if (user == null)
        ////    {
        ////        // No revelar que el usuario no existe
        ////        return RedirectToAction("ResetPasswordConfirmation", "Account");
        ////    }
        ////    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        ////    if (result.Succeeded)
        ////    {
        ////        return RedirectToAction("ResetPasswordConfirmation", "Account");
        ////    }
        ////    AddErrors(result);
        ////    return View();
        ////}

        ////
        //// GET: /Account/ResetPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Solicitar redireccionamiento al proveedor de inicio de sesión externo
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        ////
        //// GET: /Account/SendCode
        //[AllowAnonymous]
        //public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //{
        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
        //    if (userId == null)
        //    {
        //        return View("Error");
        //    }
        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        ////
        //// POST: /Account/SendCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    // Generar el token y enviarlo
        //    if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //    {
        //        return View("Error");
        //    }
        //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        //}

        ////
        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Si el usuario ya tiene un inicio de sesión, iniciar sesión del usuario con este proveedor de inicio de sesión externo
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // Si el usuario no tiene ninguna cuenta, solicitar que cree una
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Obtener datos del usuario del proveedor de inicio de sesión externo
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        ////
        //// POST: /Account/LogOff
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult LogOff()
        ////{
        ////    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        ////    return RedirectToAction("Index", "Home");
        ////}

        ////
        //// GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Aplicaciones auxiliares
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}