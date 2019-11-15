using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationJWT.Models;
using WebApplicationJWT.Repository;

namespace WebApplicationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IUsuario _usuario;

        public UserProfileController(UserManager<ApplicationUser> userManager
         , IUsuario usuario  )
        {
            _userManager = userManager;
            _usuario = usuario;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;


            var users = _usuario.ObterUsuariosFake();

            var user = new ApplicationUser();
            user.Id = userId;
            return users.FirstOrDefault();
            //var user = await _userManager.FindByIdAsync(userId);
            //return new
            //{
            //    user.FullName,
            //    user.Email,
            //    user.UserName
            //};
        }
    }
}