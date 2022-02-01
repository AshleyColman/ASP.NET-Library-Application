using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryData.Models
{
    public sealed class Video : LibraryAsset
    {
        [Required]
        public string Director { get; set; }
    }
}
