using LibraryData.Models;

namespace Library.Models.Catalog
{
    public class AssetDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorOrDirector { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string DeweyCallNumber { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
        public string CurrentLocation { get; set; }
        public string ImageUrl { get; set; }
        public string PatronName { get; set; }
        public Checkout LatestCheckout { get; set; }
        public IEnumerable<CheckoutHistory> CheckoutHistroy { get; set; }
        public IEnumerable<AssetHoldModel> CurrentHolds { get; set; }
    }
}
