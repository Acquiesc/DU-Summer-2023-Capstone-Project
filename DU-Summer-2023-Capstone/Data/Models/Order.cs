using DU_Summer_2023_Capstone.Data.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DU_Summer_2023_Capstone.Data.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId
        {
            get; set;
        }

        public List<OrderDetail> OrderLines
        {
            get; set;
        }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName
        {
            get; set;
        }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName
        {
            get; set;
        }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string? Email
        {
            get; set;
        }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone")]
        public string? Phone
        {
            get; set;
        }

        [NotMapped]
        [Display(Name = "CarryoutDeliveryToggle")]
        public string IsCarry
        {
            get
            {
                return CarryoutDeliveryToggle ? "on" : "off";
            }
            set => CarryoutDeliveryToggle = value?.ToLower() == "on";
        }

        [Display(Name = "CarryoutDeliveryToggle")]
        public bool CarryoutDeliveryToggle
        {
            get; set;
        }

        [RequiredIf("IsCarry", "on", ErrorMessage = "Street is required for delivery.")]
        [Display(Name = "Street")]
        public string? Street
        {
            get; set;
        }

        [RequiredIf("IsCarry", "on", ErrorMessage = "City is required for delivery.")]
        [Display(Name = "City")]
        public string? City
        {
            get; set;
        }

        [RequiredIf("IsCarry", "on", ErrorMessage = "State is required for delivery.")]
        [Display(Name = "State")]
        public string? State
        {
            get; set;
        }

        [RequiredIf("CarryoutDeliveryToggle", false, ErrorMessage = "Country is required for delivery.")]
        [Display(Name = "Country")]
        public string? Country
        {
            get; set;
        }

        [RequiredIf("CarryoutDeliveryToggle", false, ErrorMessage = "Zipcode is required for delivery.")]
        [Display(Name = "Zipcode")]
        public int Zipcode
        {
            get; set;
        }


        [NotMapped]
        [Display(Name = "Payment Method")]
        public string IsCash
        {
            get
            {
                return PaymentToggle ? "on" : "off";
            }
            set
            {
                PaymentToggle = value?.ToLower() == "on";
            }
        }

        [Display(Name = "Payment Method")]
        public bool PaymentToggle
        {
            get; set;
        }

        [RequiredIf("PaymentToggle", true, ErrorMessage = "Card number is required for card payment.")]
        [Display(Name = "Card Number")]
        public long? CardNumber
        {
            get; set;
        }

        [RequiredIf("PaymentToggle", true, ErrorMessage = "Name on card is required for card payment.")]
        [Display(Name = "Name on Card")]
        public string? CardName
        {
            get; set;
        }

        [RequiredIf("PaymentToggle", true, ErrorMessage = "Expiration date is required for card payment.")]
        [Display(Name = "Expiration")]
        public DateTime? Expiration
        {
            get; set;
        }

        [RequiredIf("PaymentToggle", true, ErrorMessage = "CVV is required for card payment.")]
        [Range(100, 999, ErrorMessage = "Invalid CVV.")]
        [Display(Name = "CVV")]
        public int? CVV
        {
            get; set;
        }

        [DefaultValue(false)]
        public bool Cancelled
        {
            get;
            set;
        }

        [BindNever]
        [ScaffoldColumn(false)]
        public decimal OrderTotal
        {
            get; set;
        }



        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime OrderPlaced
        {
            get; set;
        }

        public bool Completed
        {
            get; set;
        }
        public string AspNetUserId
        {
            get; set;
        }

        public virtual IdentityUser AspNetUser
        {
            get; set;
        }

        public int CalculateCompletedPercentage()
        {
            int timeToComplete = CalculateTimeToComplete();
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - OrderPlaced;
            int completedPercentage = (int)((elapsedTime.TotalMinutes / timeToComplete) * 100);
            return completedPercentage > 100 ? 100 : completedPercentage;
        }

        public int CalculateTimeRemaining()
        {
            int timeToComplete = CalculateTimeToComplete();
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - OrderPlaced;
            int timeRemaining = (timeToComplete - (int)elapsedTime.TotalMinutes);
            return timeRemaining < 0 ? 0 : timeRemaining;
        }

        public int CalculateTimeToComplete()
        {

            int totalCookingTime = 0;
            foreach (var line in OrderLines)
            {
                totalCookingTime += line.Pizza.WaitingTime;
            }
            totalCookingTime = totalCookingTime / OrderLines.Count;
            totalCookingTime = totalCookingTime + (OrderLines.Count * 5);

            if (!CarryoutDeliveryToggle)
            {
                totalCookingTime = totalCookingTime + 30;
            }


            return totalCookingTime;
        }




        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", FirstName, LastName, Street, City, State, Country);
        }

        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var item in OrderLines)
            {
                total = total + item.GetTotal();
            }

            return total;
        }

        public string FormatAddress()
        {
            string address = "";

            // Concatenate the address details
            address += Street + ", ";
            address += City + " ";
            address += State + " ";
            address += Country + ", ";
            address += Zipcode;

            return address;
        }

        public string FormartAccNo()
        {
            return String.Format("**** **** **** {0}", CardNumber?.ToString().Substring(CardNumber.ToString().Length - 4));
        }

        public decimal GetTax()
        {
            decimal tax = 0.16m * GetTotal();
            return tax;
        }

        public decimal GetSubTotal()
        {
            return GetTotal() + GetTax();
        }



    }
}
