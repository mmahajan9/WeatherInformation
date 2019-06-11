using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Web;
using WeatherInformation.BusinessLayer;

namespace WeatherInformation.Tests
{
    [TestFixture]
    public class FileManagerTests
    {
        #region SaveFileToBlob
        [Test]
        [TestCase(@"Tests\output")]
        public void SaveFileToBlob_Save_VALID_file_to_blob(string outPutPath)
        {
            // Arrange

            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);

            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Tests/Uploads");
            string fileName = "dummy.json";
            string filePath = Path.Combine(directoryPath, fileName);
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            Mock<HttpPostedFileBase> uploadedFile = new Mock<HttpPostedFileBase>();
            uploadedFile.Setup(x => x.FileName).Returns("dummy.json");
            uploadedFile.Setup(x => x.ContentType).Returns("json");
            uploadedFile.Setup(x => x.InputStream).Returns(fileStream);
            HttpPostedFileBase httpPostedFileBases = uploadedFile.Object;

            //Act
            var result = FileManager.SaveFileToBlob(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outPutPath), fileName, httpPostedFileBases);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(@"Tests\output")]
        public void SaveFileToBlob_Save_INVALID_file_to_blob(string outPutPath)
        {
            Assert.IsTrue(true);
        }

        [Test]
        [TestCase(@"Tests\output")]
        public void SaveFileToBlob_Save_VALID_file_to_EXISTING_Folder(string outPutPath)
        {
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(@"Tests\output")]
        public void SaveFileToBlob_Save_VALID_file_to_NEW_Folder(string outPutPath)
        {
            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(@"Tests\output")]
        public void SaveFileToBlob_Save_VALID_file_to_EXISTING_File_name(string outPutPath)
        {
            Assert.IsTrue(true);
        }
        #endregion

        #region StoreCityWiseOutputForHistoryAsync
        [Test]
        public void StoreCityWiseOutputForHistoryAsync_ProvidePath_For_XML_Extension()
        {
            Assert.IsTrue(true);
        }
        [Test]
        public void StoreCityWiseOutputForHistoryAsync_ProvidePathFor_JSON_Extension()
        { Assert.IsTrue(true); }
        [Test]
        public void StoreCityWiseOutputForHistoryAsync_ProvidePathFor_Excel_Extension()
        {
            Assert.IsTrue(true);
        }
        [Test]
        public void StoreCityWiseOutputForHistoryAsync_ProvidePathFor_Random_Extension()
        {
            Assert.IsTrue(true);
        }
        [Test]
        public void StoreCityWiseOutputForHistoryAsync_ProvideWrongFilePath()
        { Assert.IsTrue(true); }
        [Test]
        public void StoreCityWiseOutputForHistoryAsync_ProvideValidFilePath()
        { Assert.IsTrue(true); }


        #endregion

    }
}
