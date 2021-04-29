using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DH.Core
{
    public class UserEndpoint : IDisposable
    {
        public byte[] PublicKey;
        private byte[] _exchangeKey;
        private readonly ECDiffieHellmanCng _dh;
        public bool KeyExchanged { get; private set; }

        public UserEndpoint(string name)
        {
            _dh = new ECDiffieHellmanCng
            {
                KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
                HashAlgorithm = CngAlgorithm.Sha256
            };
            PublicKey = _dh.PublicKey.ToByteArray();
        }

        public void CalculateExchangeKey(byte[] otherPublicKey)
        {
            _exchangeKey = _dh.DeriveKeyMaterial(CngKey.Import(otherPublicKey, CngKeyBlobFormat.EccPublicBlob));
            KeyExchanged = true;
        }

        public void Send(string secretMessage, out byte[] encryptedMessage, out byte[] iv)
        {
            if (_exchangeKey == null)
                throw new ArgumentNullException(nameof(_exchangeKey));

            using Aes aes = new AesCryptoServiceProvider
            {
                Key = _exchangeKey
            };
            iv = aes.IV;

            // Encrypt the message
            using var ciphertext = new MemoryStream();
            using var cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write);
            var plaintextMessage = Encoding.UTF8.GetBytes(secretMessage);
            cs.Write(plaintextMessage, 0, plaintextMessage.Length);
            cs.Close();
            encryptedMessage = ciphertext.ToArray();
        }

        public string Receive(byte[] encryptedMessage, byte[] iv)
        {
            if (_exchangeKey == null)
                throw new ArgumentNullException(nameof(_exchangeKey));

            using Aes aes = new AesCryptoServiceProvider
            {
                Key = _exchangeKey,
                IV = iv
            };

            // Decrypt the message
            using var plaintext = new MemoryStream();
            using var cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(encryptedMessage, 0, encryptedMessage.Length);
            cs.Close();
            return Encoding.UTF8.GetString(plaintext.ToArray());
        }

        public void Dispose()
        {
            _dh?.Dispose();
        }
    }
}
