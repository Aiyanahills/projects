using Microsoft.AspNetCore.Mvc;
using EXAM.Data;
using EXAM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // 👈 Добавьте этот using

namespace EXAM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContactController> _logger;  // 👈 Поле для логгера

        public ContactController(ApplicationDbContext context, ILogger<ContactController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] ContactFormDto formData)
        {
            try
            {
                _logger.LogInformation("📥 Получены данные: {@FormData}", formData);

                var contactForm = new ContactForm
                {
                    UserName = formData.UserName?.Trim(),
                    Topic = formData.Topic?.Trim(),
                    Email = formData.Email?.Trim(),
                    Text = formData.Text?.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                if (string.IsNullOrWhiteSpace(contactForm.UserName))
                    return BadRequest(new { success = false, message = "Имя обязательно" });
                if (string.IsNullOrWhiteSpace(contactForm.Email))
                    return BadRequest(new { success = false, message = "Email обязателен" });

                _context.ContactForms.Add(contactForm);
                await _context.SaveChangesAsync();

                _logger.LogInformation("✅ Запись сохранена с Id={Id}", contactForm.Id);
                return Ok(new { success = true, message = "Форма успешно отправлена!", id = contactForm.Id });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "❌ DbUpdateException: {Message}", dbEx.Message);
                if (dbEx.InnerException != null)
                {
                    _logger.LogError("🔍 InnerException: {Inner}", dbEx.InnerException.Message);
                    _logger.LogError("🔍 StackTrace: {Stack}", dbEx.InnerException.StackTrace);
                }
                
                return StatusCode(500, new { 
                    success = false, 
                    message = "Ошибка БД: " + (dbEx.InnerException?.Message ?? dbEx.Message)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Общая ошибка: {Message}", ex.Message);
                return StatusCode(500, new { 
                    success = false, 
                    message = "Ошибка сервера: " + ex.Message 
                });
            }
        }
    }
}