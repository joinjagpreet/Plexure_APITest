using System;
using System.Net;
using NUnit.Framework;

namespace PlexureAPITest
{
    [TestFixture]
    public class PurchaseTest
    {
        Service service;

        [OneTimeSetUp]
        public void Setup()
        {
            service = new Service();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (service != null)
            {
                service.Dispose();
                service = null;
            }
        }

        [Test]
        public void TEST_001PurchaseWithValidProductId()
        {
           
            int productId = 1;
            //Act
            var response = service.Purchase(productId);
            string message = response.Entity.Message;
            int points = response.Entity.Points;

            //Assert that points earned are 100
            Assert.AreEqual(100, points);
            response.Expect(HttpStatusCode.Accepted);
        }

        [Test]
        public void test_002purchasewithinvalidproductid()
        {
            
            int productid = 0;
            //act
            var response = service.Purchase(productid);
            response.Expect(HttpStatusCode.BadRequest);

            //assert
            Assert.AreEqual("\"Error: Invalid product id\"", response.Error);
        }


        [Test]
        public void TEST_003GetPointsForLoggedInUser()
        {
            
            var points = service.GetPoints();
            int userId = points.Entity.UserId;
            int point = points.Entity.Value;
            //Assert
            points.Expect(HttpStatusCode.Accepted);
            //Verifying points earned are not null as points are added after each purchase
            Assert.IsNotNull(point);
            Assert.IsNotNull(userId);
            Assert.AreEqual(1, userId);
        }

        [Test]
        public void TEST_004GetPointsForInvalidToken()
        {
            //Act -- Calling Login to generate another token 
            service.Login("Tester", "Plexure123");
            var response = service.GetPoints();
            //Assert
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);

        }
    }
}
