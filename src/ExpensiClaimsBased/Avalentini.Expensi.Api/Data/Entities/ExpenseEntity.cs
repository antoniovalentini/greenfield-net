using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avalentini.Expensi.Api.Data.Entities
{
    public class ExpenseEntity
    {
        public string Id { get; set; }
        public UserEntity User { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime When { get; set; }
        public string Where { get; set; }
        public string What { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
