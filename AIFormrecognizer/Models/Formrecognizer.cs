using AIFormrecognizer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;
using System.Text;

namespace AIFormrecognizer.Models
{
    public class Formrecognizer
    {

        private readonly string endpoint = "";
        private static readonly string apiKey = "";
        private readonly AzureKeyCredential credential = new AzureKeyCredential(apiKey);

        public FormRecognizerClient AuthenticateClient()
        {
            var credential = new AzureKeyCredential(apiKey);
            var client = new FormRecognizerClient(new Uri(endpoint), credential);
            return client;
        }

        public async Task<String> RecognizeContent(FormRecognizerClient recognizerClient, string filename)
        {
            FileStream filePath = new FileStream("C:\\Users\\Marcell\\Downloads\\" + filename, FileMode.Open);

            StringBuilder sb = new StringBuilder();
            String s = "";
            FormPageCollection formPages = await recognizerClient
                .StartRecognizeContent(filePath)
                .WaitForCompletionAsync();

            foreach (FormPage page in formPages)
            {
                s = String.Format($"Form Page {page.PageNumber} has {page.Lines.Count} lines.");
                sb.AppendLine(s);

                for (int i = 0; i < page.Lines.Count; i++)
                {
                    FormLine line = page.Lines[i];

                    s = String.Format($"    Line {i} has {line.Words.Count} word{(line.Words.Count > 1 ? "s" : "")}, and text: '{line.Text}'.");
                    sb.AppendLine(s);
                }

                for (int i = 0; i < page.Tables.Count; i++)
                {
                    FormTable table = page.Tables[i];
                    s = String.Format($"Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");
                    sb.AppendLine(s);
                    foreach (FormTableCell cell in table.Cells)
                    {
                        s = String.Format($"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) contains text: '{cell.Text}'.");
                        sb.AppendLine(s);
                    }
                }
            }
            filePath.Close();

            return sb.ToString();
        }

        public async Task<Invoice> AnalyzeInvoice(FormRecognizerClient recognizerClient, string pathName)
        {

            FileStream filePath = new FileStream("C:\\Users\\Marcell\\Downloads\\" + pathName, FileMode.Open);
            StringBuilder sb = new StringBuilder();
            String s = "";
            Invoice invoice1 = new Invoice();

            var options = new RecognizeInvoicesOptions()
            {
                Locale = "en-US"
            };

            RecognizedFormCollection invoices = await recognizerClient.StartRecognizeInvoicesAsync(filePath, options).WaitForCompletionAsync();

            RecognizedForm invoice = invoices.Single();

            FormField invoiceIdField;

            if (invoice.Fields.TryGetValue("InvoiceId", out invoiceIdField))
            {
                if (invoiceIdField.Value.ValueType == FieldValueType.String)
                {
                    string invoiceId = invoiceIdField.Value.AsString();
                    invoice1.InvoiceId = invoiceId;
                }
            }

            FormField invoiceDateField;
            if (invoice.Fields.TryGetValue("InvoiceDate", out invoiceDateField))
            {
                if (invoiceDateField.Value.ValueType == FieldValueType.Date)
                {
                    DateTime invoiceDate = invoiceDateField.Value.AsDate();
                    invoice1.InvoiceDate = invoiceDate;
                }
            }

            FormField dueDateField;
            if (invoice.Fields.TryGetValue("DueDate", out dueDateField))
            {
                if (dueDateField.Value.ValueType == FieldValueType.Date)
                {
                    DateTime dueDate = dueDateField.Value.AsDate();
                    invoice1.DueDate = dueDate;
                }
            }

            FormField vendorNameField;
            if (invoice.Fields.TryGetValue("VendorName", out vendorNameField))
            {
                if (vendorNameField.Value.ValueType == FieldValueType.String)
                {
                    string vendorName = vendorNameField.Value.AsString();
                    invoice1.VendorName = vendorName;
                }
            }

            FormField vendorAddressField;
            if (invoice.Fields.TryGetValue("VendorAddress", out vendorAddressField))
            {
                if (vendorAddressField.Value.ValueType == FieldValueType.String)
                {
                    string vendorAddress = vendorAddressField.Value.AsString();
                    invoice1.Address = vendorAddress;
                }
            }

            FormField customerNameField;
            if (invoice.Fields.TryGetValue("CustomerName", out customerNameField))
            {
                if (customerNameField.Value.ValueType == FieldValueType.String)
                {
                    string customerName = customerNameField.Value.AsString();
                    invoice1.CustomerName = customerName;
                }
            }

            FormField customerAddressField;
            if (invoice.Fields.TryGetValue("CustomerAddress", out customerAddressField))
            {
                if (customerAddressField.Value.ValueType == FieldValueType.String)
                {
                    string customerAddress = customerAddressField.Value.AsString();
                    invoice1.CustomerAddress = customerAddress;
                }
            }

            FormField customerAddressRecipientField;
            if (invoice.Fields.TryGetValue("CustomerAddressRecipient", out customerAddressRecipientField))
            {
                if (customerAddressRecipientField.Value.ValueType == FieldValueType.String)
                {
                    string customerAddressRecipient = customerAddressRecipientField.Value.AsString();
                    invoice1.CustomerAddressRecipient = customerAddressRecipient;
                }
            }

            FormField invoiceTotalField;
            if (invoice.Fields.TryGetValue("InvoiceTotal", out invoiceTotalField))
            {
                if (invoiceTotalField.Value.ValueType == FieldValueType.Float)
                {
                    float invoiceTotal = invoiceTotalField.Value.AsFloat();
                    invoice1.InvoiceTotal = invoiceTotal;
                }
            }

            filePath.Close();
            return invoice1;

        }
    }
}
