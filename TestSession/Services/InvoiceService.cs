namespace TestSession.Services
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public Invoice CreateInvoice(Invoice invoice)
        {
            if (string.IsNullOrEmpty(invoice.Name))
            {
                return null;
            }

            if (invoice.Price <= 0)
            {
                return null;
            }
            
            var newInvoice = _invoiceRepository.CreateInvoice(invoice);            

            return newInvoice;
        }
    }

    public interface IInvoiceRepository
    {
        Invoice CreateInvoice(Invoice invoice);
    }

    public class InvoiceRepository : IInvoiceRepository
    {
        public Invoice CreateInvoice(Invoice invoice)
        {
            invoice.Id = 1;
            // call rest api
            return invoice;
        }
    }

    public class Invoice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
