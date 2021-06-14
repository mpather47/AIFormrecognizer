using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIFormrecognizer.Models
{
    public class ViewModels
    {
        public Files File { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
    }
}
