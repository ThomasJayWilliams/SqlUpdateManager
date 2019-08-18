# Sql Update Manager Requirements
This file contains all of the requirements for the final version of the application.
Not all of these requirements will be satisfied with the first stable build.  
The following requirements **MAY** be changed during the developing process.  
The following document contains different pointing words, such as **SHOULD**, **MAY** and **MUST**.
* **MUST** means that requirement **MUST** be absolutely satisfied by following all of the points described;
* **SHOULD** means that requirement **SHOULD** be satisfied with some changes and deviations allowed;
* **MAY** means that requirement is optional and **MAY** be satisfied or not.
## Contents
* ### Basic requirements
   * **[Input/Output](#inputoutput)**
   * **[Reports](#reports)**
   * **[Software](#software)**
   * **[Failures](#failures)**
   * **[Extensions](#extensions)**
   * **[Unit tests](#unit-tests)**
   * **[Documentation](#documentation)**
* ### Functionality requirements

## Input/Output
### Input
The final version of the Sql Update Manager (SUM) **MUST** provide the following interfaces:
1. **Command Line Interface (CLI)**
    * Text commands
    * Command arguments
    * Command parameters
    * Embedded files data
2. **Graphic User Interface (GUI)**
    * Buttons
    * Hotkeys
    * Checkboxes
    * Text areas
    * Text boxes
    * Radio buttons
3. **Operation System Background Service (OSBS)**

All of the secret data **MUST** be stored encrypted. The following data will be encrypted:
* **Server address**
* **Username**
* **User password**
* **Database names**
* **Database procedures**

The application work **MUST** be depended on users input, but application **MAY** collect some data automatically.
The application **SHOULD** require a confirmation to collect data automatically from the user.
### Output
All output data of the application should be presented as text information. The format of text **MUST** depended on the interface.

## Reports
The final version of SUM **MUST** provide reports on demand.  
The reports for CLI **SHOULD** be presented as logged information.
The application reports **SHOULD** contain the following information:
* **Errors information**
* **Events information**
All of the logs **MUST** saved to the files, available for the user to read.

## Software
The final version of SUM **MUST** be able to run on the following operation systems:
* **Windows 7**
* **Windows 8**
* **Windows 10**
* **Windows Server 2012**
* **Windows Server 2012 R2**
* **Windows Server 2016**
* **Windows Server 2019**
* **Ubuntu 19.0**
* **Debian 9.9**
* **Fedora 21**

The final version of SUM **MUST** be able to run with the following database servers:
* **Sql Server 2008**
* **Sql Server 2008 R2**
* **Sql Server 2012**
* **Sql Server 2012 R2**
* **Sql Server 2014**
* **Sql Server 2016**
* **Sql Server 2017**

## Failures
The final version of SUM **MUST** has at least a minimal level of failure tolerance with error logs provided.

## Extensions
The application **SHOULD** be designed in the way, that will allow applications extension with new features with minimal effort.

## Unit tests
All of the application code **SHOULD** be covered with unit tests. Tests **MAY** be developed after the initial stable build is done.

## Documentation
For the final version of the application **MUST** be written the documentation. Access to the documentation **SHOULD** presented at the following sources:
* **Internet pages**
* **Application internal**
* **Local files**
