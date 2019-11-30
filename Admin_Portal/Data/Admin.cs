using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Admin_Portal.Data
{
    public class Admin : IPrincipal, IIdentity
    {
        static readonly string _prefix = @"^|#|^";
        string _password = "";
        IPrincipal _principal;


        [Required(ErrorMessage = "Admin ID is required")]
        public string AdminID { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get { return _password; } set { _password = Hash(value); } }

        public string Admin_Type { get; set; }


        [JsonIgnore]
        public string AuthenticationType { get; private set; }
        [JsonIgnore]
        public bool IsAuthenticated { get; private set; }

        [JsonIgnore]
        public IIdentity Identity => _principal.Identity;

        [JsonIgnore]
        public string Name => AdminID; // Identity.Name;

        public void Attach(IPrincipal existingPrincipal)
        {
            IsAuthenticated = existingPrincipal.Identity.IsAuthenticated;
            AuthenticationType = existingPrincipal.Identity.AuthenticationType;
            _principal = existingPrincipal;
        }

        private string Hash(string pwd)
        {
            if (pwd == null)
            {
                pwd = "pass";
            }
            if (pwd.StartsWith(_prefix)) // don't do re-encrypt when instantiating from the database, deserializing
                return pwd;

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]); // generate random bytes for salt
            var key = new Rfc2898DeriveBytes(pwd, salt, 10000);
            byte[] hash = key.GetBytes(20);
            byte[] saltandhash = new byte[36];

            Array.Copy(salt, 0, saltandhash, 0, 16);
            Array.Copy(hash, 0, saltandhash, 16, 20);

            return String.Format("{0}{1}", _prefix, Convert.ToBase64String(saltandhash));
        }

        private bool CheckPassword(string pwd)
        {
            byte[] hash = Convert.FromBase64String(Password.Replace(_prefix, "")); // convert stored pwd to bytes, removing prefix
            byte[] salt = new byte[16];
            Array.Copy(hash, 0, salt, 0, 16); // extract the salt from stored pwd
            var key = new Rfc2898DeriveBytes(pwd, salt, 10000);
            byte[] checkhash = key.GetBytes(20); // hashed test pwd

            return Enumerable.Range(0, 20).All(n => hash[n + 16] == checkhash[n]);
        }

        public bool CheckLogin(LoginAttempt login)
        {
            if (CheckPassword(login.Password))
            {
                login.IsSuccessful = true;
            }
            else
            {
                login.IsSuccessful = false;
                login.ErrorMessage = "Invalid password.";
            }
            return login.IsSuccessful;
        }

        public bool IsInRole(string Roles)
        {
            return this.Admin_Type.ToString() == Roles;
        }
    }
}