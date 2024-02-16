# TlsTest
This is simple test code to determine the TLS protocol version supported by SQL Server. Technically, the code could check any server using TLS by changing the port number. In this case, the port is 1433, which is the main SQL Server TCP port. 

The code iterates through SSL2, SSL3, TLS1.0, TLS 1.1, TLS 1.2 and TLS 1.3 and displays the mututally agreed upon ciphersuite. If an exception is raised, then that protocol version is unsupported by SQL Server.

Also note that the code does not perform any server certificate verification, any and all certificate errors are ignored. The purpose of this tool is to check protocol version support.
