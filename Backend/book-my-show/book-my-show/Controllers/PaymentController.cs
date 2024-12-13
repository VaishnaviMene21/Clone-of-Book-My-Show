using book_my_show.Data;
using book_my_show.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace book_my_show.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext context;

        public PaymentController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> StorePayment(PaymentDetail paymentDetail)
        {
            try
            {
                if (paymentDetail == null)
                {
                    return BadRequest("Invalid payment details.");
                }

                context.PaymentDetails.Add(paymentDetail);
                await context.SaveChangesAsync();

                return Ok(paymentDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while processing the payment.",
                    error = ex.Message
                });
            }
        }
    }
}
