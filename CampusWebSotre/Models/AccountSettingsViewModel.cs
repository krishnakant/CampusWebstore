﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusWebStore.Models
{
    public class AccountSettingsViewModel
    {

        #region "User Account Details"

        [Required]
        [Display(Name = "User login")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        #endregion

        #region "User Credit Card Billing Info"

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Required]
        [Display(Name = "Country")]
        [UIHint("CountryDropDown")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Day Phone")]
        public string DayPhone { get; set; }


        [Display(Name = "Evening")]
        public string Evening { get; set; }

        #endregion

        #region "User Shipping Info"

        [Required]
        [Display(Name = "Ship To Name")]
        public string ShipToName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string ShipAddress { get; set; }

        [Display(Name = "Address2")]
        public string ShipAddress2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string ShipCity { get; set; }

        [Required]
        [Display(Name = "State")]
        public string ShipState { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public string ShipZip { get; set; }

        [Required]
        [Display(Name = "Country")]
        [UIHint("CountryDropDown")]
        public string ShipCountry { get; set; }

        [Display(Name = "Ship Instruc")]
        public string ShipInstruc { get; set; }

        [Display(Name = "Keep me updated on sales, buybacks, and other promotions")]
        public bool IsEmailOptIn { get; set; }

        public string DropDownAccountType { get; set; }

        #endregion
    }
}