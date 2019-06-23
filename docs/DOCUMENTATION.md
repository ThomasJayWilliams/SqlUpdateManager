# SQL Update Manager 1.0.0
## CLI Documentation

SQL Update Manager CLI is a Command Line interface to SUM.Core. It uses the commands approach to manage SUM (SQL Update Manager).
This application has two kinds of commands, which determines by the prefix:
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
Name: Test_SQL_Server
Location: 127.0.0.1
User: admin
Password: 
```
This command will save server credits. Further, you will be able to update procedures by 
simply pointing the name of the server you've added.
Second way:
```shell
sum connect
Location: 127.0.0.1
User: admin
Password:
```
Or:
```shell
sum connect Test_SQL_Server
```
This will connect your current session to the server you've set, and further all your actions will be performed over this server.
For example:
the following command shows server status - procedures that can be updated.
```shell
sum status
```
With the first approach used you will have to pass the server name to the command:
```shell
sum status -s Test_SQL_Server
```
### Note
The second approach is better if you're working with a low number of servers. The first approach is preferable over the second if you're working will many servers. This approach allows working with any number of servers without performing a connection to them every time.

### Files configuration
After the server configuration is done, you need to point the procedure files location, which contains .sql files. Each server contains its own locations, so you have to make sure you've added locations for each server.
```shell
sum connect Test_SQL_Server
sum location add "C:/MyRepository/Procedures"
```
Or:
```shell
sum location add -s Test_SQL_Server "C:/MyRepository/Procedures"
```
### Note
As a default SUM will track files with .sql extension only.

### Updating
SUM will start tracking procedures files after a specified location added.
So, after you've added locations you can check its status and update procedures that have changed.
```shell
sum connect Test_SQL_Server
sum status
// some output here that shows obsolete procedures
sum update
```
### Note
What happened:
1. Connection to the server, so all of the following commands will be executed over this server;
2. Getting server status, which checks if procedure files have been changed;
3. Updating server procedures.
