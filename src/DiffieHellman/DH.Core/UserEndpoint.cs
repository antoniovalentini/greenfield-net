using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DH.Core
{
    public class UserEndpoint : IDisposable
    {
        public readonly ECDiffieHellmanPublicKey PublicKey;
        private readonly ECDiffieHellman _dh;

        private byte[] _exchangeKey;

        public bool KeyExchanged { get; private set; }

        public UserEndpoint()
        {
            _dh = ECDiffieHellman.Create();
            PublicKey = _dh.PublicKey;
        }

        public void CalculateExchangeKey(ECDiffieHellmanPublicKey otherPublicKey)
        {
            _exchangeKey = _dh.DeriveKeyFromHash(otherPublicKey, HashAlgorithmName.SHA256);
            KeyExchanged = true;
        }

        public void CalculateExchangeKey(byte[] otherPublicKey)
        {
            using var dh2 = ECDiffieHellman.Create();
            dh2.ImportSubjectPublicKeyInfo(otherPublicKey, out _);
            CalculateExchangeKey(dh2.PublicKey);
        }

        public void Send(string secretMessage, out byte[] encryptedMessage, out byte[] iv)
        {
            if (_exchangeKey == null)
                throw new ArgumentNullException(nameof(_exchangeKey));

            using var aes = Aes.Create();
            aes.Key = _exchangeKey;
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

            using var aes = Aes.Create();
            aes.Key = _exchangeKey;
            aes.IV = iv;

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
