using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Admin_Portal.Data
{
    public class Link
    {

        [Required(ErrorMessage = "Link ID is required")]
        public int LinkID { get; set; }

        [Required(ErrorMessage = "Link Name is required")]
        public string Link_Name { get; set; }

        [Required(ErrorMessage = "Link Type is required")]
        public string Link_Type { get; set; }

        [JsonIgnore]
        public string Name => Link_Name; // Identity.Name;
    }
}