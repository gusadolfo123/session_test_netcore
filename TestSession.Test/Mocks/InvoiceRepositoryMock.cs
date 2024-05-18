using TestSession.Services;
using TestSession.Test.Stubs;

namespace TestSession.Test.Mocks
{
    public class InvoiceRepositoryMock : IInvoiceRepository
    {
        public Invoice CreateInvoice(Invoice invoice)
        {
            return InvoiceStubs.CreateInvoice();
        }
    }
}
