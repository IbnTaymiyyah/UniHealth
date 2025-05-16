using System;

namespace UniHealth.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}