using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using JWT.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Bimswarm.Models;
using Bimswarm.Models.MiscModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Bimswarm.Identity;
using Bimswarm.Services;

namespace Bimswarm.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> SSO(string code)
        {

            var responseContent = await SsoService.GetToken(code);
            if (responseContent.IsSuccessStatusCode)
            {
                var jsonResult = await responseContent.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(jsonResult);
                var accessToken = (string)jObject["access_token"];

                try
                {
                    var key = await SsoService.GetPublicKey();

                    var jToken = SwarmToken.VerifyAndDecode(accessToken, key, out DateTime expires, verify: true);
                    var mail = (string)jToken["user_name"];
                    var id = (string)jToken["swarm-id"];

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, mail),
                        new Claim(ClaimTypes.NameIdentifier, id),
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(ClaimTypeHandler.Token, accessToken),
                        new Claim(ClaimTypeHandler.ID, id),
                        new Claim(ClaimTypeHandler.EMail, mail),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };


                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                    return RedirectToAction("Index", "Home");
                }
                catch (TokenExpiredException e)
                {
                    return View("ErrorHandler", new ErrorViewModel("Fehler " + e.InnerException, "Die Anmeldung konnte auf Grund eines Fehlers nicht mit dem SSO ausgeführt werden.", e.Message));

                }
                catch (SignatureVerificationException e)
                {
                    return View("ErrorHandler", new ErrorViewModel("Fehler " + e.InnerException, "Die Anmeldung konnte auf Grund eines Fehlers nicht mit dem SSO ausgeführt werden.", "Token has invalid signature"));

                }
                catch (JsonReaderException e)
                {
                    return View("ErrorHandler", new ErrorViewModel("Fehler " + e.InnerException, "Die Anmeldung konnte auf Grund eines Fehlers nicht mit dem SSO ausgeführt werden.", e.Message));
                }
            }
            return View("ErrorHandler", new ErrorViewModel("Fehler " + responseContent.StatusCode, "Die Anmeldung konnte auf Grund eines Fehlers nicht mit dem SSO ausgeführt werden.", responseContent.ReasonPhrase));

        }

    }
}
