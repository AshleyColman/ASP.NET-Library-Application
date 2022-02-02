using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
    public class CheckoutService : ICheckout
    {
        private readonly LibraryContext _context;
        public CheckoutService(LibraryContext context) => this._context = context;
        public void Add(Checkout checkout)
        {
            _context.Add(checkout);
            _context.SaveChanges();
        }
        public void CheckInItem(int id, int libraryCardId)
        {
            var item = _context.LibraryAsset.FirstOrDefault(a => a.Id == id);

            RemoveExistingCheckouts(id);
            CloseExistingCheckoutHistory(id);
            var currentHolds = _context.Holds.Include(h => h.LibraryAsset).Include(h => h.LibraryCard).Where(h => h.LibraryAsset.Id == id);

            if (currentHolds.Any() == true)
            {
                CheckoutToEarlierHold(id, currentHolds);
            }

            UpdateAssetStatus(id, "Available");

            _context.SaveChanges();
        }

        private void CheckoutToEarlierHold(int id, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds.OrderBy(holds => holds.HoldPlaced).FirstOrDefault();

            var card = earliestHold.LibraryCard;

            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckoutItem(id, card.Id);
        }

        private void CheckoutItem(int id, int libraryCardId)
        {
            if (IsCheckedOut(id))
            {
                return;
            }

            var item = _context.LibraryAsset.FirstOrDefault(a => a.Id == id);

            UpdateAssetStatus(id, "Checked Out");
            var libraryCard = _context.LibraryCards.Include(c => c.Checkouts).FirstOrDefault(c => c.Id == libraryCardId);
        }

        private bool IsCheckedOut(int id)
        {
            var isCheckedOut = _context.Checkouts.Where(c => c.LibraryAsset.Id == id).Any();
            return isCheckedOut;
        }

        public IEnumerable<Checkout> GetAll() => _context.Checkouts;
        public Checkout GetById(int id) => GetAll().FirstOrDefault(checkout => checkout.Id == id);
        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id) => _context.CheckoutHistories.Include(history => history.LibraryAsset).Include(history => history.LibraryCard).Where(history => history.LibraryAsset.Id == id);
        public string GetCurrentHoldPatronName(int id)
        {
            throw new NotImplementedException();
        }
        public DateTime GetCurrentHoldPlaced(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds.Include(hold => hold.LibraryAsset).Where(hold => hold.LibraryAsset.Id == id);
        }
        public Checkout GetLatestCheckout(int id) => _context.Checkouts.Where(checkout => checkout.LibraryAsset.Id == id).OrderByDescending(checkout => checkout.Since).FirstOrDefault();
        public void MarkFound(int id)
        {
            UpdateAssetStatus(id, "Available");
            RemoveExistingCheckouts(id);
            CloseExistingCheckoutHistory(id);
            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int id, string status)
        {
            var item = _context.LibraryAsset.FirstOrDefault(a => a.Id == id);

            _context.Update(item);
            item.Status = _context.Statuses.FirstOrDefault(s => s.Name == status);
        }

        private void CloseExistingCheckoutHistory(int id)
        {
            var history = _context.CheckoutHistories.FirstOrDefault(h => h.Id == id && h.CheckedIn == null);

            if (history is not null)
            {
                _context.Update(history);
                history.CheckedIn = DateTime.Now;
            }
        }
        private void RemoveExistingCheckouts(int id)
        {
            var checkout = _context.Checkouts.FirstOrDefault(c => c.LibraryAsset.Id == id);

            if (checkout is not null)
            {
                _context.Remove(checkout);
            }
        }
        public void MarkLost(int id)
        {
            UpdateAssetStatus(id, "Lost");
            _context.SaveChanges();
        }
        public void PlaceHold(int id, int libraryCardId)
        {
            throw new NotImplementedException();
        }
    }
}
