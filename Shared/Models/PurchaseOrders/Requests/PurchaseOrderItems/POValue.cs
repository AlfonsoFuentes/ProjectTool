using Shared.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems
{
    public class POValue
    {
        public double CurrencyValue { get; set; }
        public CurrencyEnum Currency { get; set; } = CurrencyEnum.None;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public double USDValue => Currency.Id == CurrencyEnum.USD.Id ? CurrencyValue :
            Currency.Id == CurrencyEnum.COP.Id ? CurrencyValue / USDCOP :
            CurrencyValue / USDEUR;

    }
}
