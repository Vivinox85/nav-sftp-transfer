# nav-sftp-transfer

A .NET Framework library providing SFTP file transfer capabilities for legacy ERP systems via COM Interop.

It acts as a wrapper around the popular **SSH.NET** library, simplifying the interface for **Dynamics NAV 2009** (Classic Client).

## Features

* **Simple Interface:** Single-method execution (`UploadFile`) to abstract the complexity of SSH connections, authentication, and streams.

## Dependencies

* **Renci.SshNet.dll**: This library **must** be present in the same directory as the registered `SftpTransfer.dll` on the client machine.

## Installation

### 1. File Deployment
Copy both files to the client machine (e.g., `C:\Program Files (x86)\NAV_Addins\ZMTransfer\`):
* `SftpTransfer.dll` (This project)
* `Renci.SshNet.dll` (NuGet dependency)

### 2. Registration
Open a Command Prompt as **Administrator** and register the wrapper:

```cmd
C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe "C:\Path\To\SftpTransfer.dll" /codebase /tlb
```

*Note: You do not need to register Renci.SshNet.dll, only the wrapper.*


## Usage in Dynamics NAV (C/AL)

Define the variable in your C/AL Globals:
* **Variable Name:** `SftpLib`
* **Type:** Automation
* **Subtype:** `'ZMTransfer'.SftpManager`

```pascal
VAR
  UploadSuccess : Boolean;
  LogText : Text[1024];
BEGIN
  IF ISCLEAR(SftpLib) THEN
    CREATE(SftpLib);

  // 1. Execute Upload
  // Arguments: Host, Port, Username, Password, LocalFilePath, RemoteDirectory
  UploadSuccess := SftpLib.UploadFile(
    'sftp.bzst.de',       // Host
    22,                   // Port
    'myUser',             // Username
    'myPassword',         // Password
    'C:\Temp\ZM_Export.xml', // Local File
    '/incoming/'          // Remote Path
  );

  // 2. Retrieve Log (Must be done immediately after upload to capture details)
  LogText := SftpLib.GetLastError();

  // 3. Handle Result
  IF UploadSuccess THEN BEGIN
    MESSAGE('Success! Log: ' + LogText);
    // Recommended: Save LogText to a NAV Log Table
  END ELSE BEGIN
    ERROR('Upload failed. Error: %1', SftpLib.GetLastError());
  END;
END;
```

## Troubleshooting

### "Could not create an instance of the control"
* **Cause:** The .NET runtime cannot find `Renci.SshNet.dll`.
* **Fix:** Ensure `Renci.SshNet.dll` is in the **exact same folder** as the registered `SftpTransfer.dll`.

### "RegAsm warning RA0000"
* **Cause:** The assembly is not signed with a Strong Name Key.
* **Fix:** This warning can be safely ignored for internal deployments using `/codebase`. For a cleaner build, sign the assembly in Visual Studio project settings.
