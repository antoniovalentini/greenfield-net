using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorSample.Models;

namespace RazorSample.Pages.Payments
{
    public class List : PageModel
    {
        public List<PaymentModel> Payments { get; private set; }
        
        public void OnGet()
        {
            var random = new Random();
            var rng = random.Next(3, 6);
            Payments = new List<PaymentModel>();
            for (var i = 0; i < rng; i++)
            {
                Payments.Add(new PaymentModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = random.Next(100, 1001).ToString(),
                    Date = DateTime.UtcNow.AddDays(-(random.Next(100, 1000))).ToString("s"),
                    From = $"User {i}",
                });
            }
        }
    }
}
