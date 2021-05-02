using System;

namespace Avalentini.Expensi.Api.Contracts.Models
{
    public class Expense
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime When { get; set; }
        public string Where { get; set; }
        public string What { get; set; }
    }
}
