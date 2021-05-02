using System;
using System.ComponentModel.DataAnnotations;

namespace Avalentini.Expensi.Web.Model
{
    public class ExpenseViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "The field Amount must be greater than 0.01")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        [Required]
        public DateTime When { get; set; }
        [Required]
        public string Where { get; set; }
        [Required]
        public string What { get; set; }
    }
}
