using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using WeatherInformation;
using WeatherInformation.Controllers;

namespace WeatherInformation.Tests.Controllers
{
    
    public class HomeControllerTest
    {
        [Test]
        public void UploadFile_Accepts_VALID_file_and_Savesto_Server_Fetch_CityId_and_Store_result_In_Individual_file()
        {
            //Arrange
            HomeController homeController = new HomeController();
            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            serverMock.Setup(x => x.MapPath("~/Tests/Uploads/")).Returns(Path.GetFullPath(@"\testfile"));
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);
            //httpContextMock.Setup(x => x.User.Identity.Name).Returns("testcase@user.net");
            homeController.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), homeController);

            //string filePath = Path.GetFullPath(@"~/testfile/dummy.json");
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Tests/Uploads", "dummy.json");
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            Mock<HttpPostedFileBase> uploadedFile = new Mock<HttpPostedFileBase>();
            uploadedFile.Setup(x => x.FileName).Returns("dummy.json");
            uploadedFile.Setup(x => x.ContentType).Returns("json");
            uploadedFile.Setup(x => x.InputStream).Returns(fileStream);
            HttpPostedFileBase httpPostedFileBases = uploadedFile.Object;

            //var exisitngFolder = db.Contents.Where(c => c.ContentType == ContentType.Folder).FirstOrDefault();

            //Act
            var result = (homeController.UploadFile(true) as Task<bool>).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void UploadFile_upload_INVALID_file_and_should_give_Error_Message()
        {
        }
        
    }
}
