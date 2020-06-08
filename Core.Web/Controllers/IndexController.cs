using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Web.Services;
using DataLib;
using DataLib.Domain;
using DataLib.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Core.Web.Controllers
{
    public class IndexController : Controller
    {
        private readonly IHubContext<ChatHouService> _hubContext;
        private readonly SqlServerDbContext _dbContext;

        //private readonly IRepository<faceTab> _repository;

        public IndexController(IHubContext<ChatHouService> hubContext,
            SqlServerDbContext dbContext
            //IRepository<faceTab> repository
            )
        {
            this._hubContext = hubContext;
            this._dbContext = dbContext;
            //this._repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            //var data = await sqlDb.FaceTab.ToListAsync();
            //var data = await _repository.Query().Include(X => X.FaceUser).ToListAsync();
            var data = await _dbContext.faceUser.Include(x=>x.faceTab).ToListAsync();

            return View();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> PostData()
        {
            var userId = Request.Form["userid"];
            var message = Request.Form["message"];

            //var db = new SqlDb();
            //var data = await db.User.AsQueryable().Include(x => x.UserType).ToListAsync();
            //db

            if (!string.IsNullOrWhiteSpace(userId))
                await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", new { message = message });

            return Json(new { code = "success", msg = "发送成功" });
        }

        [HttpPost]
        public async Task<JsonResult> LoginPost()
        {
            var username = Request.Form["username"];
            var userpwd = Request.Form["userpwd"];

            //本demo没连接数据库，就不做用户验证了，用户id就用Guid生成了

            //登陆授权
            string userId = Guid.NewGuid().ToString().Replace("-", "");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,username),   //储存用户name
                new Claim(ClaimTypes.NameIdentifier,userId)  //储存用户id
            };

            var indentity = new ClaimsIdentity(claims, "formlogin");
            var principal = new ClaimsPrincipal(indentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //验证是否授权成功
            if (principal.Identity.IsAuthenticated)
            {
                return Json(new { code = "success", msg = "登陆成功" });
            }
            else
                return Json(new { code = "failed", msg = "登陆失败" });
        }

    }
}