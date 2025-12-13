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
    }
}
