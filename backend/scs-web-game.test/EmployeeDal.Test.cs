using scs_web_game.DataAccessLayer;
using scs_web_game.Models;
using Moq;
using Serilog;

namespace scs_web_game.test
{
    public class EmployeeDalTests
    {
        private const string TestImageFolder = @"TestImages";
        private Mock<WebGameContext> _mockContext;
        private Mock<ILogger> _mockLogger;
        private EmployeeDal _employeeDal;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<WebGameContext>();
            _mockLogger = new Mock<ILogger>();
            _employeeDal = new EmployeeDal(_mockContext.Object, _mockLogger.Object);

            if (!Directory.Exists(TestImageFolder))
            {
                Directory.CreateDirectory(TestImageFolder);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(TestImageFolder))
            {
                Directory.Delete(TestImageFolder, true);
            }
        }

        [TestCase("validImage.jpg", true)]
        [TestCase("nonexistentImage.jpg", false)]
        [TestCase("", false)]
        [TestCase("   ", false)]
        [TestCase("", false)]
        public void CompileAndValidateEmployeeImagePath_ValidateImagePath_ReturnsCorrectResult(string imgFileName, bool expected)
        {
            // Arrange
            var employee = new Employee { ImgFileName = imgFileName };
            var imagePath = Path.Combine(TestImageFolder, imgFileName);

            if (expected && !string.IsNullOrWhiteSpace(imgFileName))
            {
                File.Create(imagePath).Dispose();
            }

            try
            {
                var actual = _employeeDal.CompileAndValidateEmployeeImagePath(employee);

                Assert.That(actual, Is.EqualTo(imagePath), $"Image path should be valid for: {imgFileName}");
            }
            catch (FileNotFoundException)
            {
                Assert.That(expected, Is.False, $"Image path should be invalid for: {imgFileName}");
            }
        }
    }
}
