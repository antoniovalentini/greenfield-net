using System;

namespace Avalentini.Expensi.IdentityServer.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreationDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactEmail { get; set; }
    }
}
