using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIFormrecognizer.Models
{
    public class Invoice
    {
        public string InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string VendorName { get; set; }
        public string Address { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerAddressRecipient { get; set; }
        public string CustomerName { get; set; }
        public float InvoiceTotal { get; set; }

    }
}
