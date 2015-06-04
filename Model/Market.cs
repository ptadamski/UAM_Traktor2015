using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Model
{
    private class MarketPrice
    {
        public decimal Buy { get; set; }
        public decimal Sell { get; set; }
    }

    public class MarketItem
    {
        public MarketPrice Price { get; set; }
        public string ItemName { get; set; }

    }

    public class Market
    {                        
        Dictionary<string, MarketPrice> stock;

        void TryBuy(IList<string> items, IList<int> amounts, out decimal price)
        {
            price = 0;
            if (items.Count != amounts.Count)
                return;

            for (int i = 0; i < items.Count; i++)
                price += stock[items[i]].Buy * amounts[i];
        }

        void TrySell(IList<string> items, IList<int> amounts, out decimal price)
        {
            price = 0;
            if (items.Count != amounts.Count)
                return;

            for (int i = 0; i < items.Count; i++)
                price += stock[items[i]].Sell * amounts[i];
        }

    }
    

}
