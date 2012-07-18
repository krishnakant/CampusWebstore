using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared.Models
{
    public class AdminModel
    {
        public bool IsBuyBackEnable { get; set; }

        public bool IsShowIsbnTr { get; set; }

        public bool IsShowIsbn { get; set; }

        public bool IsShowIsbnIntegration { get; set; }

        public bool IsLiveRates { get; set; }

        public bool IsShowSubs { get; set; }

        public bool IsUseCats { get; set; }

        public bool IsUseQuickLinks { get; set; }

        public bool IsEmailAdmins { get; set; }

        public bool IsAsGuest { get; set; }

        public bool IsUseBuyBack { get; set; }
        
        public bool IsBuyBackDisplayPrices { get; set; }

        public bool IsBuyBackShowRetail { get; set; }

        public bool IsBuyBackShowNeed { get; set; }

        public bool IsBuyBackUseStoreCredit { get; set; }

        public bool IsBuyBackShowSholeSale { get; set; }

        public bool IsTrUseQoo { get; set; }

        public bool IsGmUseQoo { get; set; }

        public bool IsQootx { get; set; }

        public bool IsPickupValid { get; set; }

        public bool IsoosGm { get; set; }

        public bool IsoosTr { get; set; }

        public bool IsoosTx { get; set; }

        public string BuyBackNotBuyingWording { get; set; }

        public string BuyBackBuyingWording { get; set; }

        public string BuyBackNeedWording { get; set; }

        public string BuyBackWholesalePriceWording { get; set; }

        public string BuyBackRetailPriceWording { get; set; }

        public string BuyBackWholeSaleCoin { get; set; }

        public string BuyBackWholeSaleRounding { get; set; }

        public string BuyBackWholesalePrice { get; set; }

        public string BuyBackRetailCoin { get; set; }

        public string BuyBackRetailRounding { get; set; }

        public string BuyBackRetailPrice { get; set; }

        public string BuyBackStoreCredit { get; set; }

        public string GMoosmsg { get; set; }

        public string GMlowstockmsg { get; set; }

        public string TRlowstockmsg { get; set; }

        public string TRoosmsg { get; set; }

        public string UsedOosMsg { get; set; }

        public string UsedLowStockMsg { get; set; }

        public string NewLowStockMsg { get; set; }

        public string NewOosMsg { get; set; }

        public string UsedTxLowQty { get; set; }

        public string UsedTXoosQty { get; set; }

        public string GmLowQty { get; set; }

        public string TrLowQty { get; set; }

        public string TRoosQTY { get; set; }

        public string NewTXoosQty { get; set; }

        public string NewTxLowQty { get; set; }

        public string TxImagePath { get; set; }

        public string TrImagePath { get; set; }

        public string GmImagePath { get; set; }

        public string BuyBackDisclaimer { get; set; }

    }
}
