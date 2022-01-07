using System;
using ApiDemo;
using ApiDemo.DTO;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APITests
{
    [TestClass]
    public class RegressionTests
    {
        public TestContext TestContext { get; set; }

        [ClassInitialize]

        public static void Setup(TestContext testContext)
        {
            var dir = testContext.TestRunDirectory;
            Reporter.SetupExtentReport("API Regression Test", "API Regression Test Report", dir);
        }

        [TestInitialize]
        public void SetupTest()
        {
            Reporter.CreateTest(TestContext.TestName);
        }

        [TestCleanup]
        public void ClenaupTest()
        {
            var testStatus = TestContext.CurrentTestOutcome;
            Status logStatus;

            switch (testStatus)
            {
                case UnitTestOutcome.Failed:
                    logStatus = Status.Fail;
                    Reporter.TestStatus(logStatus.ToString());
                    break;
                case UnitTestOutcome.Inconclusive:
                    break;
                case UnitTestOutcome.Passed:
                    break;
                case UnitTestOutcome.InProgress:
                    break;
                case UnitTestOutcome.Error:
                    break;
                case UnitTestOutcome.Timeout:
                    break;
                case UnitTestOutcome.Aborted:
                    break;
                case UnitTestOutcome.Unknown:
                    break;
                case UnitTestOutcome.NotRunnable:
                    break;
                default:
                    break;
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Reporter.FlushReport();
        }


        [TestMethod]
        public void VerifyUserList()
        {
            var demo = new Demo<ListOfUsersDTO>();
            var response = demo.GetUsers01();
            Assert.AreEqual(2, response.Page);
            Assert.AreEqual("Michael", response.Data[0].first_name);
        }

        [TestMethod]
        public void VerifyListOfUsers()
        {
            var demo = new Demo<ListOfUsersDTO>();
            var user = demo.GetUsers("api/users?page=2");
            Assert.AreEqual(2, user.Page);
            Reporter.LogToReport(Status.Pass, "Page number does match");
            Assert.AreEqual("Michael", user.Data[0].first_name);
            Reporter.LogToReport(Status.Fail, "User first name displayed does not matched");
        }

        [TestMethod]
        public void CreateNewUser()
        {
            string payload = @"{
                                    ""name"": ""Dean"",
                                    ""job"": ""Sub Team Leader""
                               }";

            var user = new APIHelper<CreateUserDTO>();
            var url = user.SetUrl("api/users");
            var request = user.CreatePostRequest(payload);
            var response = user.GetResponse(url, request);
            CreateUserDTO content = user.GetContent<CreateUserDTO>(response);

            Assert.AreEqual("Dean", content.Name);
            Assert.AreEqual("Sub Team Leader", content.Job);
        }

        [TestMethod]
        public void CreateUser0()
        {
            string payload = @"{
                                    ""name"": ""Tosin"",
                                    ""job"": ""2nd Team Leader""
                               }";

            var demo = new Demo<CreateUserDTO>();
            var user = demo.CreateUser("api/users", payload);

            Assert.AreEqual("Tosin", user.Name);
            Assert.AreEqual("2nd Team Leader", user.Job);


        }

        [TestMethod]
        public void CreateUser1()
        {
            string payload = @"{
                                    ""name"": ""Jide"",
                                    ""job"": ""Advisor""
                               }";

            var demo = new Demo<CreateUserDTO>();
            var user = demo.CreateUser("api/users", payload);

            Assert.AreEqual("Jide", user.Name);
            Assert.AreEqual("Advisor", user.Job);

            var demo1 = new Demo<ListOfUsersDTO>();
            var user1 = demo1.GetUsers("api/users?page=2");
            Assert.AreEqual(2, user1.Page);
            Assert.AreEqual("Michael", user1.Data[0].first_name);
        }
    }
}
