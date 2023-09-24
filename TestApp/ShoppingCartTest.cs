using System;
using ToTestApp.Functionality;

namespace TestApp
{
	public class ShoppingCartTest
	{
        public class DbServiceMock : IDbService
        {
            public bool ProcessResult { get; set; }
            public Product? ProductBeingProcessed { get; set; }
            public int ProductIdBeingProcessed { get; set; }

            public bool RemoveShoppingCartItem(int? id)
            {
                if (id != null)
                    ProductIdBeingProcessed = Convert.ToInt32(id);
                return ProcessResult;
            }

            public bool SaveShoppingCartItem(Product prod)
            {
                ProductBeingProcessed = prod;
                return ProcessResult;
            }
        }

        [Fact]
        public void AddProduct_Success()
        {
            var dbMock = new DbServiceMock();
            dbMock.ProcessResult = true;
            // Arrange
            ShoppingCart shoppingCart = new(dbMock);

            // Act
            var product = new Product(1, "Shoes", 200);
            var result = shoppingCart.AddProduct(product);

            // Assert
            Assert.True(result);
            Assert.Equal(product, dbMock.ProductBeingProcessed);
        }

        [Fact]
        public void AddProduct_Failure_InvalidPayload()
        {
            var dbMock = new DbServiceMock();
            dbMock.ProcessResult = false;

            // Arrange
            ShoppingCart shoppingCart = new(dbMock);

            // Act
            var result = shoppingCart.AddProduct(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveProduct_Success()
        {
            var dbMock = new DbServiceMock();
            dbMock.ProcessResult = true;
            // Arrange
            ShoppingCart shoppingCart = new(dbMock);

            // Act
            var product = new Product(1, "Shoes", 200);
            var result = shoppingCart.DeleteProduct(product.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(product.Id, dbMock.ProductIdBeingProcessed);
        }

        [Fact]
        public void RemoveProduct_Failed()
        {
            var dbMock = new DbServiceMock();
            dbMock.ProcessResult = false;
            // Arrange
            ShoppingCart shoppingCart = new(dbMock);

            // Act
            var result = shoppingCart.DeleteProduct(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveProduct_Failed_InvalidId()
        {
            var dbMock = new DbServiceMock();
            dbMock.ProcessResult = false;
            // Arrange
            ShoppingCart shoppingCart = new(dbMock);

            // Act
            var result = shoppingCart.DeleteProduct(0);

            // Assert
            Assert.False(result);
        }
    }
}

