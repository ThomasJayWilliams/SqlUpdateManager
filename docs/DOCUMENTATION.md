# SQL Update Manager 1.0.0, CLI Documentation
SQL Update Manager CLI is a Command Line interface to SUM.Core. It uses the commands approach to manage SUM (SQL Update Manager).
## [Intro](#intro)
## [Getting started](#getting-started)
## [Commands](#commands)
* ### [sum connect](#sum-connect)
* ### [sum server](#sum-server)
* ### [sum database](#sum-database)
## Intro
The CLI has two kinds of commands, which determines by the prefix:
* **environment** - has no prefix;
* **sum** - has "sum" prefix before any command.
### Example:
Environment command:
```shell
echo -e "Hello world!"
```
SUM command:
```shell
sum help -s
```
### Note
This approach helps to separate SUM commands from CLI commands.
If you're familiar with Git bash, you'll notice that it uses the same approach: CLI commands run without any prefixes, Git commands run with "git" prefix.
## Getting started
### Server configuration
First of all, you need to configure the server connection. There are to two ways to do that:
```shell
sum server add
Name: [server name]
Location: [server address]
User: admin
Password: 
```
This command will save server credits. Further, you will be able to update procedures by 
simply pointing the name of the server you've added.
Second way:
```shell
sum connect
Location: [server address]
User: [user name]
Password: [password]
```
Or:
```shell
sum connect [server name]
```
This will connect your current session to the server you've set, and further all your actions will be performed over this server.
For example:
the following command shows server status - procedures that can be updated.
```shell
sum status
```
With the first approach used you will have to pass the server name to the command:
```shell
sum status -s [server name]
```
### Note
The second approach is better if you're working with a low number of servers. The first approach is preferable over the second if you're working will many servers. This approach allows working with any number of servers without performing a connection to them every time.
### Files configuration
After the server configuration is done, you need to point the procedure files location, which contains .sql files. Each server contains its own locations, so you have to make sure you've added locations for each server.
```shell
sum connect [server name]
sum location add [physical path]
```
Or:
```shell
sum location add -s [server name] [physical path]
```
### Note
As a default SUM will track files with .sql extension only.

### Databases configuration
Before update any procedures you should first add the database to your server.
```shell
sum database add [database name]
```
### Updating
SUM will start tracking procedures files after a specified location added.
So, after you've added locations you can check its status and update procedures that have changed.
```shell
sum update
```
### Note
What happened:
1. Connection to the server, so all of the following commands will be executed over this server;
2. Getting server status, which checks if procedure files have been changed;
3. Updating server procedures.
### Go through
```shell
sum server add "MyServer"
sum connect "MyServer"
sum status
sum update
```
## Commands
## sum connect
Connects to database server. After connection established all next operations will be performed over the connected server.
```shell
sum connect [params] [params arguments] [argument]
```
### Arguments
Connect command has an optional argument - server name, that added before.
```shell
sum connect [server name]
```
### Parameters
#### -s, --save
Sets the current connection as a default. At the application start, the default connection will be used to permanently connect to the server.
## sum server
Manages database servers.
```shell
sum server [params] [add][remove] [argument]
```
### Arguments
Command excepts only predefined arguments, that are add or remove.
```shell
sum server add
```
### Parameters
#### -c, --check
Performs connection query to make sure data is valid and server is up.
#### --retry-count [count]
Sets retry count for the current server. The default is 0.
#### --retry-delay [milliseconds]
Sets delay between retries. The default is 0.
## sum database
Manages the server databases.
```shell
sum database [params] [add][remove][show]
```
### Arguments
Command excepts predefined arguments, that are add, remove and show and database name.
```shell
sum database add [database name]
```
### Parameters
#### -a, --all
Performs set operation (add or remove) overall server databases.
#### --system
Performs set operation (add or remove) overall, including system server databases.
#### -v, --verbose
Shows additional info, like stored procedures.
