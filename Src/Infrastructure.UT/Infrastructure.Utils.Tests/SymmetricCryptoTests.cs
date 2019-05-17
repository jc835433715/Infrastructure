using NUnit.Framework;

namespace Infrastructure.Utils.Tests
{
    [TestFixture]
    class SymmetricCryptoTests
    {
        [Test]
        public void Test()
        {
            const string aesKey = "Mi9l/+7Zujhy12CE";
            const string aesIV = "Mi9l/+AB";
            SymmetricCrypto symmetricCrypto = new SymmetricCrypto(SymmetricCrypto.AlgorithmName.AesManaged, aesKey, aesIV);
            string encryptedText = symmetricCrypto.Encrypt("201807041500");
            string clearText = symmetricCrypto.Decrypt(encryptedText);

            Assert.AreEqual("201807041500", clearText);
        }
    }
}
