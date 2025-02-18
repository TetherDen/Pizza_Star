using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizza_Star.Data;
using Pizza_Star.Data.Helpers;
using Pizza_Star.Models;
using Pizza_Star.VIewModel;

namespace Pizza_Star.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly EmailSender _emailSender;
        public ContactController(ApplicationContext context, EmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(EmailContactFormViewModel model)
        {
            if(ModelState.IsValid)
            {
                var settings = await _context.ContactFormSettings.FirstOrDefaultAsync();
                if(settings !=  null)
                {
                    bool emailSent = _emailSender.SendContactMessage(model, settings.ContactEmail);
                    if(emailSent)
                    {
                        TempData["SuccessMessage"] = "Ваше сообщение успешно отправлено!";
                        return RedirectToAction("Contact");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка отправки. Пожалуйста, попробуйте позже.");
                    }
                }
                ModelState.AddModelError("", "Ошибка отправки. Администратор не настроил контактный имейл.");               
            }
            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ContactSettings()
        {
            var settings = await _context.ContactFormSettings.FirstOrDefaultAsync();
            if(settings == null)
            {
                settings = new ContactFormSettings { ContactEmail = "default@example.com" };
                await _context.ContactFormSettings.AddAsync(settings);
                await _context.SaveChangesAsync();
            }
            return View(settings);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactSettings(ContactFormSettings model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var settings = await _context.ContactFormSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                _context.ContactFormSettings.Add(model);
            }
            else
            {
                settings.ContactEmail = model.ContactEmail;
                _context.ContactFormSettings.Update(settings);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Настройки успешно сохранены";
            return RedirectToAction(nameof(ContactSettings));
        }


    }
}
