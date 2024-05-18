using TestSession.Services;

namespace TestSession.Test.Stubs
{
    public static class InvoiceStubs
    {
        public static Invoice CreateInvoice()
        {
            return new Invoice
            {
                Id = 100,
                Name = "Test",
                Price = 100
            };
        }
    }
}
