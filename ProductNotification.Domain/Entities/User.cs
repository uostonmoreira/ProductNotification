using System;

namespace ProductNotification.Domain.Entities
{
    public class User : Base
    {
        public int Codigo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public override bool Valido()
        {
            if (string.IsNullOrEmpty(this.UserName) ||
               string.IsNullOrEmpty(this.Password) ||
               string.IsNullOrEmpty(this.Role))
                return false;

            return true;
        }
    }
}
