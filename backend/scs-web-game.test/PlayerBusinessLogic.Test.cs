using System.Reflection;
using scs_web_game.BusinessLogic;

namespace scs_web_game.test
{
    public class PlayerBusinessLogicTests
    {
        private static bool IsValidEmail(string email)
        {
            var method = typeof(PlayerBusinessLogic)
                .GetMethod("ValidateEmail", BindingFlags.NonPublic | BindingFlags.Static);

            try
            {
                method?.Invoke(null, [email]);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [TestCase("max.mustermann@scs.ch", true)]
        [TestCase("max@scs.ch", true)]
        [TestCase("max.mustermann@gmail.com", false)]
        [TestCase("max.mustermann@", false)]
        [TestCase("@scs.ch", false)]
        [TestCase("max!mustermann@scs.ch", false)]
        [TestCase("", false)]
        [TestCase("   ", false)]
        [TestCase("max.mustermann@scs.chhhhh", false)]
        [TestCase(" max.mustermann@scs.ch ", false)]
        public void ValidateEmail_MultipleCases_ReturnsExpected(string email, bool expected)
        {

            var actual = IsValidEmail(email);
            Assert.That(actual, Is.EqualTo(expected), $"Email: {email}");
        }
    }
}