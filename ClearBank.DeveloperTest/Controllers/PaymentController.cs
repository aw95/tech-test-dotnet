using ClearBank.DeveloperTest.Common;
using ClearBank.DeveloperTest.Services.Interfaces;
using ClearBank.DeveloperTest.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("make-payment")]
        public ActionResult<MakePaymentResult> MakePayment([FromBody] MakePaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request received.");
                return BadRequest(ModelState);
            }

            var result = _paymentService.MakePayment(request);

            if (!result.Success)
            {
                _logger.LogInformation(
                    "Payment failed for debtor account ending in {LastFourDigits}",
                    AccountNumberMasker.Mask(request.DebtorAccountNumber));
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
