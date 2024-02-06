using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SyncFusionClient.Data
{
    public class OrderDetails
    {
        public OrderDetails()
        {

        }

        // Group 1
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Order Date is required")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "You must agree to the Terms and Conditions")]
        public bool AcceptTerms { get; set; }

        //Group-2 Shipping Address
        [Required(ErrorMessage = "Shipped Date is required")]
        public DateTime? ShippedDate { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Address Line is required")]
        public string AddressLine { get; set; }

        public string AddressLine2 { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }

        public int Quantity { get; set; }
    }

}
