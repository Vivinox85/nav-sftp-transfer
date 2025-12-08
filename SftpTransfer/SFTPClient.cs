

using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SftpTransfer
{
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SFTPClient")]
    [ComVisible(true)]
    public class SFTPClient : ISFTPClient
    {
        private string lastError;

        public SFTPClient() 
        {

        }

        public string GetLastError()
        {
            return lastError;
        }

        public bool TransferFileViaSFTP(string sourcePath, string destPath, string host, string username, string password)
        {
            bool transferSuccess = false;
            try
            {
                using (var client = new SftpClient(host, username, password))
                {
                    client.Connect();

                    using (FileStream fs = File.OpenRead(@sourcePath))
                    {
                        client.UploadFile(fs, destPath);                        
                    }
                    transferSuccess = true;
                    client.Disconnect();
                }
            } catch (Exception ex)
            {
                lastError = ex.Message;
                transferSuccess = false;
            }

            return transferSuccess;
        }

        public bool TransferFileViaSFTPKey(string sourcePath, string destPath, string host, string username, string privateKeyFilePath, string keyPass)
        {
            bool transferSuccess = false;
            try
            {
                using (var privateKeyFile = new PrivateKeyFile(privateKeyFilePath, passPhrase: keyPass))
                {
                    using (var client = new SftpClient(host, username, privateKeyFile))
                    {
                        client.Connect();

                        using (FileStream fs = File.OpenRead(@sourcePath))
                        {
                            client.UploadFile(fs, destPath);
                        }
                        transferSuccess = true;
                        client.Disconnect();
                    }
                }                
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
                transferSuccess = false;
            }

            return transferSuccess;
        }
    }
}
