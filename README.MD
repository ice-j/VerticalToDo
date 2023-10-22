# Database
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=bigStrongPwd' -p 1433:1433 --name sqledge -d mcr.microsoft.com/azure-sql-edge