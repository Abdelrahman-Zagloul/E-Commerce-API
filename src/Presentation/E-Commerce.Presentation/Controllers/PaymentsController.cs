using E_Commerce.ServicesAbstraction;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            return HandleResult(result);
        }

        //stripe listen --forward-to https://localhost:7099/api/payments/webhook
        [HttpPost("webhook")]
        public async Task<IActionResult> UpdateOrderStatus()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];

            await _paymentService.UpdateOrderStatus(json, signatureHeader);
            return NoContent();
        }
    }
}
