using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset
    {
        private LibraryContext _context;
        public LibraryAssetService(LibraryContext context) => _context = context;
        public void Add(LibraryAsset asset)
        {
            _context.Add(asset);
            _context.SaveChanges();
        }
        public IEnumerable<LibraryAsset> GetAll()
        {
            return _context.LibraryAsset
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }
        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAsset.OfType<Book>().Where(asset => asset.Id == id).Any();

            var isVideo = _context.LibraryAsset.OfType<Video>().Where(asset => asset.Id == id).Any();

            return isBook ? _context.Books.FirstOrDefault(book => book.Id == id).Author : _context.Videos.FirstOrDefault(Video => Video.Id == id).Director ?? "Unknown";
        }
        public LibraryAsset GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(asset => asset.Id == id);
        }
        public LibraryBranch GetCurrentLocation(int id) => GetById(id).Location;
        public string GetDeweyIndex(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).DeweyIndex;
            }
            else
            {
                return String.Empty;
            }
        }
        public string GetIsbn(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).ISBN;
            }
            else
            {
                return String.Empty;
            }
        }
        public string GetTitle(int id) => _context.LibraryAsset.FirstOrDefault(a => a.Id == id).Title;
        public string GetType(int id)
        {
            var book = _context.LibraryAsset.OfType<Book>().Where(b => b.Id == id);

            return book.Any() ? "Book" : "Video";
        }
    }
}
