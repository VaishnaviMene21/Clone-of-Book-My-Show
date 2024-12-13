using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace book_my_show.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        private readonly EmailService _emailService;

        public SMSController()
        {
            _emailService = new EmailService();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null || string.IsNullOrEmpty(emailRequest.ToEmail) ||
                string.IsNullOrEmpty(emailRequest.Subject) || string.IsNullOrEmpty(emailRequest.Message))
            {
                return BadRequest("Invalid email request.");
            }

            try
            {
                await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Message);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
} 

