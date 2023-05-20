using EasyCashIdentityProject.DtoLayer.Dtos.AppUserRegisterDto;
using EasyCashIdentityProject.EntityLayer.Concrete;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EasyCashIdentityProject.Presentation.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AppUserRegisterDto appUserRegisterDto)
        {
            Random random = new Random();
            if (ModelState.IsValid)
            {
                int code;
                code = random.Next(100000, 1000000);

				AppUser appUser = new AppUser()
                {
                Name= appUserRegisterDto.Name,
                Email= appUserRegisterDto.Email,
                Surname= appUserRegisterDto.Surname,
                UserName= appUserRegisterDto.Username,
                City="istanbul",
                District="AVRUPA",
                ImageUrl="IMAGE",
                ConfirmCode=code,
                };
                var result =await _userManager.CreateAsync(appUser,appUserRegisterDto.Password);
                if(result.Succeeded)
                {

					MimeMessage mimeMessage = new MimeMessage();
                    MailboxAddress mailboxAdressFrom = new MailboxAddress("Easy Cash Manager", "jarvis.tony.34@gmail.com");
                    MailboxAddress mailboxAdressTo = new MailboxAddress("User", appUser.Email);
                    BodyBuilder bodyBuilder = new BodyBuilder();

                    mimeMessage.From.Add(mailboxAdressFrom);
                    mimeMessage.To.Add(mailboxAdressTo);
                    bodyBuilder.TextBody = "Kayıt olmak için onay kodunuz : " + code;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();
                    mimeMessage.Subject = "Easy Cash Onay Kodu";
                    
                    SmtpClient client = new SmtpClient();
                    client.Connect("smtP.gmail.com", 587, false);
                    client.Authenticate("jarvis.tony.34@gmail.com", "pqabajejwxfnsiau");
                    client.Send(mimeMessage);
                    client.Disconnect(true);

                    TempData["Mail"] = appUserRegisterDto.Email;
                    return RedirectToAction("Index","ConfirmMail");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View();
        }
    }
}
