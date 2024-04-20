#!/bin/sh

/opt/mssql-tools/bin/sqlcmd -S sql-server-microsservicosv2 -U sa -P SqlServer2019! -d master -i /tmp/banco-dados/db.sql