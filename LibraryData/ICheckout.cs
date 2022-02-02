using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryData
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        Checkout GetById(int id);
        void Add(Checkout checkout);
        void CheckInItem(int id, int libraryCardId);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        void PlaceHold(int id, int libraryCardId);
        string GetCurrentHoldPatronName(int id);
        DateTime GetCurrentHoldPlaced(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);
        void MarkLost(int id);
        void MarkFound(int id);
        Checkout GetLatestCheckout(int id);
    }
}
