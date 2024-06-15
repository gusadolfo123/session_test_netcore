using Moq;
using System.Linq.Expressions;
using TestSession.Services;
using TestSession.Test.Mocks;
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

        [Fact]
        public void CreateInvoice_WhenCreateInvoiceRepositoryReturnException_ShouldReturnException()
        {
            // Arrange
            _invoiceRepositoryMock.Setup(x => x.CreateInvoice(It.IsAny<Invoice>()))
                .Throws(new Exception("Error"));

            var invoiceService = new InvoiceService(_invoiceRepositoryMock.Object);

            // Act
            var exception = Record.Exception(() => invoiceService.CreateInvoice(InvoiceStubs.CreateInvoice()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
        }

        [Theory]
        [InlineData("name1", 1, false)]
        [InlineData("", 1, true)]
        public void CreateInvoice_GivenAWithInvalidPriceInvoiceModel_ShouldReturnNull(string name, decimal price, bool isError)
        {
            // Arrange
            var invoiceService = new InvoiceService(_invoiceRepositoryMock.Object);
            var input = new Invoice
            {
                Name = name,
                Price = price
            };

            var expected = new Invoice
            {                
                Name = name,
                Price = price
            };

            _invoiceRepositoryMock.Setup(x => x.CreateInvoice(It.IsAny<Invoice>()))
                .Returns(expected);

            // Act
            var result = invoiceService.CreateInvoice(input);

            // Assert
            if (isError)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
                Assert.True(expected.Name == result.Name);
                Assert.True(expected.Price == result.Price);
            }
        }

        [Theory]
        [MemberData(nameof(CreateInvoiceData))]        

        public void CreateInvoice_GivenAWithInvalidPriceInvoiceModel_ShouldReturnNullw(InvoiceData data)
        {
            // Arrange
            var invoiceService = new InvoiceService(_invoiceRepositoryMock.Object);
            var input = new Invoice
            {
                Name = data.Name,
                Price = data.Price
            };
        
            if (data.IsError)
            {
                _invoiceRepositoryMock.Setup(data.Mock)
                    .Throws(data.ExpectedError);
            }
            else 
            {
                _invoiceRepositoryMock.Setup(data.Mock)
                 .Returns(data.Expected);
            }

            // Act
            if (data.IsError)
            {
                var result = Record.Exception(() => invoiceService.CreateInvoice(input));

                Assert.NotNull(result);
                Assert.IsType<Exception>(data.ExpectedError);
            }
            else
            {
                var result = invoiceService.CreateInvoice(input);
                Assert.NotNull(result);
                Assert.True(data.Expected == result);
            }
        }

        public static IEnumerable<object[]> CreateInvoiceData()
        {
            return new List<InvoiceData[]>
            {
                new InvoiceData[]
                {
                    new InvoiceData
                    {
                        NameTest = "when repository return exception, should return an exception",
                        Name = "name1",
                        Price = 1,
                        ExpectedError = new Exception("error"),
                        IsError = true,
                        Mock = x => x.CreateInvoice(It.IsAny<Invoice>())
                    }
                },
                new InvoiceData[]
                {
                    new InvoiceData
                    {
                        NameTest = "when repository return exception, should return an exception",
                        Name = "name1",
                        Price = 1,
                        Expected = new Invoice
                        {
                            Name = "name1",
                            Price = 1
                        },
                        IsError = false,
                        Mock = x => x.CreateInvoice(It.IsAny<Invoice>())
                    }
                }
            };
        }

    }

    public class InvoiceData
    {
        public string NameTest { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Exception ExpectedError { get; set; }
        public Invoice Expected { get; set; }
        public bool IsError { get; set; }
        public Expression<Func<IInvoiceRepository, Invoice>> Mock { get; set; }
    }
}
