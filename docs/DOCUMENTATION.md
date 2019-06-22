## SQL Update Manager 1.0.0
**CLI Documentation**

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
If you're familiar with Git bash, you'll notice that it uses the same approach: CLI commands runs without any prefixes, Git commands run with "git" prefix.
