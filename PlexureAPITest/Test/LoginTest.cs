using System;
using System.Net;
using NUnit.Framework;

namespace PlexureAPITest
{
    [TestFixture]
    class LoginTest
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
        public void TEST_001LoginWithValidUser()
        {
            //Act
            var response = service.Login("Tester", "Plexure123");
            String userName = response.Entity.UserName;

            int userId = response.Entity.UserId;
            String token = response.Entity.AccessToken;
            //Assert
            response.Expect(HttpStatusCode.OK);
            Assert.AreEqual("Tester", response.Entity.UserName);
            Assert.AreEqual(1, response.Entity.UserId);
            Assert.AreEqual("37cb9e58-99db-423c-9da5-42d5627614c5", response.Entity.AccessToken);

        }

        [Test]
        public void TEST_002LoginWithInValidUsername()
        {
            //Act
            var response = service.Login("Testar", "Plexure123");
            //Assert
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);


        }

        [Test]
        public void TEST_003LoginWithInValidPassword()
        {
            //Act
            var response = service.Login("Tester", "Qwerty@123");
            //Assert
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);

        }

        [Test]
        public void TEST_004LoginwithUpperCaseUserIdPAssword()
        {
            var response = service.Login("TESTER", "PLEXURE123");
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);

        }

        [Test]
        public void TEST_005LoginwithLowerCaseUserIdPAssword()
        {
            var response = service.Login("tester", "plexure123");
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);

        }

        [Test]
        public void TEST_006LoginwithUserIdCaseSesitive()
        {
            var response = service.Login("TEsTer", "plexure123");
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);

        }

        [Test]
        public void TEST_006aLoginwithPasswordCaseSesitive()
        {
            var response = service.Login("TesTer", "pLeXurE123");
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);

        }

        [Test]
        public void TEST_007LoginWithOutPassword()
        {
            //Act
            var response = service.Login("Tester", "");
            //Assert
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);
        }

        [Test]
        public void TEST_008LoginWithOutUsername()
        {
            //Act
            var response = service.Login("", "Plexure123");
            //Assert
            response.Expect(HttpStatusCode.Unauthorized);
            Assert.AreEqual("\"Error: Unauthorized\"", response.Error);
        }

        [Test]
        public void TEST_009LoginWithOutCredentials()
        {
            //Act
            var response = service.Login("", "");
            //Assert
            response.Expect(HttpStatusCode.BadRequest);
            Assert.AreEqual("\"Error: Username and password required.\"", response.Error);
        }


    }
}
