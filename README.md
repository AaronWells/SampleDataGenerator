#Ed-Fi Sample Data Generator

##Tracer Status 1/23/2015
###Prerequesites
In order to run the application, you need to:

*   deploy the database locally with the default settings
    *   should be called "edfi.sdg.database" in your default SQL instance
*   install Microsoft Message Queue (MSMQ)
*   add "-bootstrap:true" to the application command line
    *   This step runs the first Work Item in the configuration to "bootstrap" the generation process

###Settings and Configuration
You can change the configuration settings to cause the data to be generated differently

*   The database connection string is in the app.config file
*   Other settings are in the Configuration
    *   ThreadCount - the number of threads used for work items (recommend 1 for Debugging)
    *   WorkQueueName - a valad MSMQ connection string
    *   ValueRules - individual rules used to identify how object properties get set
    *   WorkFlow - the order of object creation (the first one is the bootstrapper)

###Additional Functionality
Some of the unit tests represent utility functions that should be integrated into the application as different command line parameters.

*   GenerateDefaultConfiguration - serializes the default configuration into the test result screen. The output of this test may be used as a starting point for future custom settings.
*   WriteInterchangeStudentParentFromDatabase - creates the InterchangeStudentParent.xml file in the Ed-Fi-SampleDataGenerator\edfi.sdg.test\bin\Debug directory from all the student parent studentParentAssociation data in the database

###Clean Up
If you are debugging, or running the application, you may need to "clean up" intermediate or final output data.
*   delete or remove all the messages from the private\edfi.sdg message queue (or whatever you changed it to)
*   truncate the StatTemplate table from the database ("Truncate Table [edfi.sdg.database].[dbo].[StatTemplate]")

###Current Functionality
The tracer represents a minimal vertical slice of functionality through the application. The scope of the tracer proves the following concepts and lays a groundwork for future development.

*   Distributed processing to improve scalability while limiting memory use
*   Value Providers, Value Rules, and Work Items as the basis for detailed rule configuration
    *   Ability to generate values based on other values in an object
    *   Ability to create additional objects using an input object (Student -> StudentParentAssociation(s) -> Parent(s))
    *   Smart execution of value rules using dependencies
    *   Establishment of a database model to handle statistical generation of properties from realistic data
*   Serialization using code-generated classes
*   Configuration file as basis for customization
*   Extension of these classes via additional T4 templates to provide additional support where the XSD itself is insufficient

###Next Steps
The following list is not exhaustive, but represents the current backlog

*   Improve value provider mechanisms to populate array properties in objects (currently ignored)
    *   when populating array properties, use a shuffled distribution to retrieve values within an object instead of a single random
*   Reference related object properties in order to provide new property values (i.e. assign a parents name based in part on a student's name)
*   Retrieve a random set of objects based on an XPath match of properties
*   Create additional value provider types:
    *   Formatted output value provider - Take the output from more than one value provider and create a formatted string (i.e. addresses, email, etc.)
    *   A value provider that uses an existing value from somewhere else, or another random value with weightings (take childs family name (80%) or random family name (20%))
    *   Others as required
*   Read configuration from file instead of using default configuration
*   Add ability to Deserialize entire objects from the Stat tables.