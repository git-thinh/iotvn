SixLabors.ImageSharp

AngleSharp
AngleSharp.Io
Microsoft.ClearScript.V8
Microsoft.ClearScript.V8.Native.win-x64
PuppeteerSharp

StackExchange.Redis


Swashbuckle.AspNetCore.SwaggerGen -> Only ReDoc

Swashbuckle.AspNetCore.SwaggerUI
Swashbuckle.AspNetCore



Microsoft.AspNetCore.SignalR
Microsoft.AspNetCore.SignalR.Protocols.MessagePack

Hangfire.AspNetCore
Hangfire.MemoryStorage

Google.Apis.Drive.v3
Google.Apis.Blogger.v3

Google.Protobuf
Grpc.Core
Grpc.Tools

NLog.Web;
NLog.Web.AspNetCore;

FluentFTP

services.msc

https://redis.io/topics/notifications
$ redis-cli      > config set notify-keyspace-events KEA
$ redis-cli--csv > psubscribe '__key*__:*' "__key*__:*" "__keyevent@*__:*"

redis-server.exe --service-install --service-name r5.1000 --bind 127.0.0.1 --port 1000 --dbfilename 1000.rdb --loglevel verbose --logfile 1000.log --appendonly no --protected-mode no
redis-server.exe --service-install --service-name r5.1001 --bind 127.0.0.1 --port 1001 --dbfilename 1001.rdb --loglevel verbose --logfile 1001.log --appendonly no --protected-mode no --replicaof 127.0.0.1 1000
redis-server.exe --service-install --service-name r5.1002 --bind 127.0.0.1 --port 1002 --dbfilename 1002.rdb --loglevel verbose --logfile 1002.log --appendonly no --protected-mode no --replicaof 127.0.0.1 1000
redis-server.exe --service-install --service-name r5.1003 --bind 127.0.0.1 --port 1003 --dbfilename 1003.rdb --loglevel verbose --logfile 1003.log --appendonly no --protected-mode no --replicaof 127.0.0.1 1000

https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier
cdnjs, jsdelivr, unpkg, filesystem

---------------------------------------------

* If you get the error ERR_SPDY_INADEQUATE_TRANSPORT_SECURITY in Chrome, 
run these commands to update your development certificate:

dotnet dev-certs https --clean
dotnet dev-certs https --trust


