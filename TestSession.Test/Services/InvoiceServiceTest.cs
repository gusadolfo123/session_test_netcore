using Moq;
using TestSession.Services;
using TestSession.Test.Stubs;

namespace TestSession.Test.Services
{
    public class InvoiceServiceTest
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;

        public InvoiceServiceTest()
        {
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        }

        [Fact]
        public void CreateInvoice_GivenAValidInvoiceModel_ShouldReturnAInvoiceWithId()
        {
            // Arrange
            var expected = InvoiceStubs.CreateInvoice();
            expected.Id = 1;
            expected.Name = "TEST";

            _invoiceRepositoryMock.Setup(x => x.CreateInvoice(It.IsAny<Invoice>()))
                .Returns(expected);

            var invoiceService = new InvoiceService(_invoiceRepositoryMock.Object);
            
            // Act
            var result = invoiceService.CreateInvoice(InvoiceStubs.CreateInvoice());

            // Assert
            Assert.True(result != null);
            Assert.True(expected.Id == result.Id);
        }

        [Fact]
        public void CreateInvoice_GivenAWithNullNameInvoiceModel_ShouldReturnNull()
        { 
            // Arrange
            var invoiceService = new InvoiceService(_invoiceRepositoryMock.Object);
            var invoice = InvoiceStubs.CreateInvoice();
            invoice.Name = null;

            // Act
            var result = invoiceService.CreateInvoice(invoice);

            // Assert
            Assert.Null(result);
        }
    }
}
