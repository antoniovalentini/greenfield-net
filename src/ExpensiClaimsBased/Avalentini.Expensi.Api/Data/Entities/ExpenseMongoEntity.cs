using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avalentini.Expensi.Api.Data.Entities
{
    public class ExpenseMongoEntity
    {
        [BsonElement("expense_id")]
        public string ExpenseId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("what")]
        public string What { get; set; }
        [BsonElement("where")]
        public string Where { get; set; }
        [BsonElement("when")]
        public DateTime When { get; set; }
        [BsonElement("created_at")]
        public DateTime CreationDate { get; set; }
    }

    public class ExpensesPerUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        [BsonElement("user_id")]
        public int UserId { get; set; }

        [BsonElement("expenses")]
        public List<ExpenseMongoEntity> Expenses { get; set; } = new List<ExpenseMongoEntity>();
    }
}
