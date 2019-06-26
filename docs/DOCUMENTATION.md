# SQL Update Manager 1.0.0, CLI Documentation
SQL Update Manager CLI is a Command Line interface to SUM.Core. It uses the commands approach to manage SUM (SQL Update Manager).
## Contents
* **[Intro](#intro)**
* **[Getting started](#getting-started)**
* **[SUM Commands](#sql-update-manager-commands)**
  * **[sum help](#sum-help)**
  * **[sum connect](#sum-connect)**
  * **[sum use](#sum-use)**
  * **[sum config](#sum-config)**
  * **[sum register](#sum-register)**
  * **[sum deregister](#sum-deregister)**
  * **[sum update](#sum-update)**
  * **[sum status](#sum-status)**
  * **[sum diff](#sum-diff)**
  * **[sum log](#sum-log)**
## Intro
The CLI has two kinds of commands, which determines by the prefix:
* **environment** - has no prefix;
* **sum** - has "sum" prefix before any command.
### Example:
Environment command:
```
echo -e "Hello world!"
```
SUM command:
```
sum help -s
```
### Note
This approach helps to separate SUM commands from CLI commands.
If you're familiar with Git bash, you'll notice that it uses the same approach: CLI commands run without any prefixes, Git commands run with "git" prefix.
Almost every command has parameters, which affect command execution. There are two types of parameter names: single char and full name. Single char params can be applied as one:
```
// executes sum help command with -h, -f and -a parameters.
sum help -hfa
```
Also, some parameters may have values:
```
sum command --param "some value"
```
## Getting started
### Server configuration
First of all, you need to connect to the server.
```
sum connect
Location: [server address]
User: [user name]
Password: [password]
```
This will connect your current session to the server you've set, and further all your actions will be performed over this server.
### Databases configuration
Before update any procedures you should first use the database from your server.
```
sum use [database name]
```
### Files configuration
After the server configuration is done, you need to point the procedure files location, which contains .sql files. Each server contains its own locations, so you have to make sure you've registered locations for each server.
```
sum connect [server name]
sum use [database name]
sum registr [physical path]
```
### Note
As a default SUM will track files with .sql extension only.
### Updating
SUM will start tracking procedures files after a specified location registered.
So, after you've registered locations you can check its status and update procedures that have changed.
```
sum update
```
### Note
What happened:
1. Connection to the server, so all of the following commands will be executed over this server;
2. Getting server status, which checks if procedure files have been changed;
3. Updating server procedures.
### Go through
```
sum connect
sum use "MyDatabase"
sum register "C:/Procedures"
sum status
sum update
```
The commands above added server to SUM, connected to it, added database, moved the scope to the specified database, registered directory with procedures files, shown status and executed procedures update for the current database.
## SQL Update Manager commands
## Global parameters
#### -h, --help
Shows help information about specified commands.
#### -v, --verbose
Shows additional information during command execution.
## sum help
Shows short information about available SUM commands. Without any parameters shows only commands list.
```
sum help [params]
```
### Parameters
#### -a, --all
Shows all commands with complete information.
### Example
```
sum help
```
## sum connect
Connects to database server. After connection established all next operations will be performed over the connected server.
```
sum connect [params]
```
### Parameters
#### -s, --save
Sets the current connection as a default. At the application start, the default connection will be used to permanently connect to the server.
### Example
```
sum server --check add "MyServer"
sum connect --save "MyServer"
```
## sum use
Same as connect, but for a database. Allows preform database-oriented commands without pointing database name. It can be run if connected to the server.
```
sum use [params] [database name]
```
### Arguments
Commands accept database name, which located at the pointed server.
### Parameters
#### -s, -save
Sets the current database as a default for the current server.
### Example
```
sum use -s "MyDatabase"
```
## sum config
Allows configuring SUM application.
```
sum config [property] [value]
```
### Arguments
All configuration consists of properties and values. Command accepts property to edit and value. Each server and database can contain personal properties and values. If command ran without params changes will be applied over application.
```
sum config [params] [property] [value]
```
### Parameters
#### -s, --server
Applies configuration over the current server. Need to be connected to the server.
#### -d, -database
Applies configuration over the current database. Need to be connected to the database.
#### -l, --list
Shows all available properties.
### Example
```
sum config --server connectionType WindowsAuthentification
sum config --database --list
```
## sum register
Allows to register database procedures to track. Registered procedure will be tracked. It can be run if using the database.
```
sum register [params] [physical path]
```
### Arguments
Command accepts a physical path to procedures directory.
### Parameters
#### -t, --tree
Performs command over all subdirectories in a specified directory.
#### -n, --name [value]
Sets an alias for a path.
### Example
```
sum register -n "default" -t "C:/MyProcedures"
```
## sum deregister
Allow to remove tracking procedure files. It can be run if using the database.
```
sum deregister [params] [physical path][registered alias]
```
### Arguments
Command accpets a physical path procedures directory.
### Parameters
#### -t, --tree
Perform command over all subdirectories in a specified directory.
### Example
```
sum deregister -t "default"
```
## sum update
Runs procedures update. It can be run if using a database.
```
sum update [params] [arguments]
```
### Arguments
Command accepts procedure or directory name to update procedures only from specified location and procedure names to update only specified procedures. Executed with no arguments will update procedures in all locations (including subdirectories). It can be run if using a database.
```
sum update myProcedur.sql myProcedure2.sql
```
### Parameters
#### -i, --ignore
As a default is during updating error appeared updating stops allowing you to choose what to do next. This parameter disables confirmation.
#### -t, --test
Runs a command in a test mode. Displays the output as if that was the real mode.
#### -d, --deploy
Executes all procedures. Even those that didn't change.
### Example
```
sum update -i
```
## sum status
Displays info about the state of current database procedures. It can be run if using a database.
```
sum status
```
### Example
```
sum status
```
## sum diff
Displays the difference between an old procedure file(s) and new ones. It can be run if using a database.
```
sum diff [params] [arguments]
```
### Arguments
Command accepts file names separated with space. Executed with no file names will show the difference for all files related to the database.
```
sum diff [file name] [file name] ...
```
### Parameters
#### -f, --full
As a default command shows only parts of the file that has been changed. This parameter shows the whole file with changed parts highlighted.
### Example
```
sum diff --full
```
## sum log
Displays information about performed actions, related to the SUM interface (logs for environment may be recieved by environment command). Has predefined arguments - event, error and data. Runned with no parameters will display only list of SUM commands executed or list of procedures executed.
```
sum log [params] [event][error][data]
```
### Parameters
#### -d, --detailed
Shows additional info about each command/procedure.
### Example
```
sum log --detailed event
```
