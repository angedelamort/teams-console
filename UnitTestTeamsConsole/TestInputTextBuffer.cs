using Microsoft.VisualStudio.TestTools.UnitTesting;
using teams_console.Components;

namespace UnitTestTeamsConsole
{
    [TestClass]
    public class TestInputTextBuffer
    {
        [TestMethod]
        public void TestSingleCharacter()
        {
            var inputTextBuffer = new __StringBufferedLines();
            inputTextBuffer.Insert('a');

            var buffer = inputTextBuffer.GetRenderedText(5, 1);
            Assert.AreEqual(1, buffer.Length);
            CollectionAssert.AreEqual(new[] { 'a', '\0', '\0', '\0', '\0' }, buffer[0]);
        }
    }
}
