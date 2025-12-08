
using System.Runtime.InteropServices;

namespace SftpTransfer
{
    [Guid("E877A41B-2890-4348-BB5E-EA16E456D8F2")]
    [ComVisible(true)]
    public interface ISFTPClient
    {
        bool TransferFileViaSFTP(string sourcePath, string destPath, string host, string username, string password);
        bool TransferFileViaSFTPKey(string sourcePath, string destPath, string host, string username, string privateKeyFilePath, string keyPass);
        string GetLastError();
    }
}
