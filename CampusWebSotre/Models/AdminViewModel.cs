using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampusWebStore.Models
{
    public class AdminViewModel
    {
        [Display(Name = "Enable Buyback ?")]
        public bool IsBuyBackEnable { get; set; }

        [Display(Name = "Show ISBN?")]
        public bool IsShowIsbnTr { get; set; }

        [Display(Name = "Show ISBN?")]
        public bool IsShowIsbn { get; set; }

        [Display(Name = "Show ISBN for Class Integration?")]
        public bool IsShowIsbnIntegration { get; set; }

        [Display(Name = "Use Live Rates?")]
        public bool IsLiveRates { get; set; }

        [Display(Name = "Display Substitution Option ?")]
        public bool IsShowSubs { get; set; }

        [Display(Name = "Use Catalogs?")]
        public bool IsUseCats { get; set; }

        [Display(Name = "Use Quicklinks?")]
        public bool IsUseQuickLinks { get; set; }

        [Display(Name = "Send email copy of order to admins?")]
        public bool IsEmailAdmins { get; set; }

        [Display(Name = "Allow shopping as guest?")]
        public bool IsAsGuest { get; set; }

        [Display(Name = "Enable Buyback ?")]
        public bool IsUseBuyBack { get; set; }

        [Display(Name = "Display Prices?")]
        public bool IsBuyBackDisplayPrices { get; set; }

        [Display(Name = "Show Retail?")]
        public bool IsBuyBackShowRetail { get; set; }

        [Display(Name = "Show BuyBack Need?")]
        public bool IsBuyBackShowNeed { get; set; }

        [Display(Name = "Use Store Credit?")]
        public bool IsBuyBackUseStoreCredit { get; set; }

        [Display(Name = "Show WholeSale?")]
        public bool IsBuyBackShowSholeSale { get; set; }

        [Display(Name = "Combine QOO and QOH?")]
        public bool IsTrUseQoo { get; set; }

        [Display(Name = "Combine QOO and QOH?")]
        public bool IsGmUseQoo { get; set; }

        [Display(Name = "Combine QOO and QOH?")]
        public bool IsQootx { get; set; }

        [Display(Name = "Allow Pick-up instore?")]
        public bool IsPickupValid { get; set; }

        [Display(Name = "Display Out of Stock Message for GM?")]
        public bool IsoosGm { get; set; }

        [Display(Name = "Display Out of Stock Message for TR?")]
        public bool IsoosTr { get; set; }

        [Display(Name = "Display Out of Stock Message for TX?")]
        public bool IsoosTx { get; set; }

        [Display(Name = "NOT Buying Wording:")]
        public string BuyBackNotBuyingWording { get; set; }

        [Display(Name = "Buying Wording:")]
        public string BuyBackBuyingWording { get; set; }

        [Display(Name = "BuyBack Need Wording:")]
        public string BuyBackNeedWording { get; set; }

        [Display(Name = "Wholesale Price Wording:")]
        public string BuyBackWholesalePriceWording { get; set; }

        [Display(Name = "Retail Price Wording:")]
        public string BuyBackRetailPriceWording { get; set; }

        [Display(Name = "Wholesale Coin:")]
        public string BuyBackWholeSaleCoin { get; set; }

        [Display(Name = "Wholesale Rounding:")]
        public string BuyBackWholeSaleRounding { get; set; }

        [Display(Name = "Wholesale Price Percentage:")]
        public string BuyBackWholesalePrice { get; set; }

        [Display(Name = "Retail Coin:")]
        public string BuyBackRetailCoin { get; set; }

        [Display(Name = "Retail Rounding:")]
        public string BuyBackRetailRounding { get; set; }

        [Display(Name = "Retail Price Percentage:")]
        public string BuyBackRetailPrice { get; set; }

        [Display(Name = "Store Credit Percentage:")]
        public string BuyBackStoreCredit { get; set; }

        [Display(Name = "Out of Stock Message:")]
        public string GMoosmsg { get; set; }

        [Display(Name = "Low Stock Message:")]
        public string GMlowstockmsg { get; set; }

        [Display(Name = "Low Stock Message:")]
        public string TRlowstockmsg { get; set; }

        [Display(Name = "Out of Stock Message:")]
        public string TRoosmsg { get; set; }

        [Display(Name = "TX Used Out of Stock Message:")]
        public string UsedOosMsg { get; set; }

        [Display(Name = "TX Used Out of Stock Message:")]
        public string UsedLowStockMsg { get; set; }

        [Display(Name = "TX New Low Stock Message:")]
        public string NewLowStockMsg { get; set; }

        [Display(Name = "TX New Low Stock Message:")]
        public string NewOosMsg { get; set; }

        [Display(Name = "TX Used Low Inventory QTY:")]
        public string UsedTxLowQty { get; set; }

        [Display(Name = "TX Used Out of Stock QTY:")]
        public string UsedTXoosQty { get; set; }

        [Display(Name = "GM Low Inventory QTY:")]
        public string GmLowQty { get; set; }

        [Display(Name = "TR Low Inventory QTY:")]
        public string TrLowQty { get; set; }

        [Display(Name = "TR Out of Stock QTY:")]
        public string TRoosQTY { get; set; }

        [Display(Name = "TX New Out of Stock QTY:")]
        public string NewTXoosQty { get; set; }

        [Display(Name = "TX New Low Inventory QTY:")]
        public string NewTxLowQty { get; set; }

        [Display(Name = "TX Image path:")]
        public string TxImagePath { get; set; }

        [Display(Name = "TR Image path:")]
        public string TrImagePath { get; set; }

        [Display(Name = "GM Image path:")]
        public string GmImagePath { get; set; }

        [Display(Name = "BuyBack Disclaimer:")]
        public string BuyBackDisclaimer { get; set; }

    }
}