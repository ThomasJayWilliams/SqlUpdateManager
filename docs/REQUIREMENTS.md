# SQL Update Manager Requirements
This file contains all of the requirements for the final version of the application.
Not all of these requirements will be satisfied with the first stable build.
The following document contains different pointing words, such as **SHOULD**, **MAY** and **MUST**.
* **MUST** means that requirement **MUST** be absolutely satisfied by following all of the points described;
* **SHOULD** means that requirement **SHOULD** be satisfied with some changes and deviations allowed;
* **MAY** means that requirement is optional and **MAY** be satisfied or not.
## Contents
* **[Input/Output](#input/output)**

## Input/Output
### Input
The final version of the SQL Update Manager (SUM) **MUST** provide the following interfaces:
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
All output data of application should be presented as text information. The format of text **MUST** depended on the interface.
