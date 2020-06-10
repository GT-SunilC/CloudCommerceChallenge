# CloudCommerceChallenge
Cloud Commerce Technical Challenge

Create a C# console application that can be given a CSV file and convert it to XML or JSON.  The idea is to show off your knowledge of C# and OOP principles, rather than to do it as quickly as possible.
Feel free to use a library or framework, e.g. to help handle the interaction with the command line.

Your solution should be written with possible future requirements in mind e.g. converting back to CSV from other XML or JSON, converting between XML and JSON, or possibly taking input from a different source e.g. database.

CSV file format

* The first line will contain column headings
* The headings should form keys in the XML or JSON output
* Underscores should be used to group headings e.g.:

    name, address_line1, address_line2
    Dave, Street, Town

    should convert to

    {
        name: Dave,
        address: {
            line1: Street,
            line2: Town
        }
    }

My comments

The challenge here is that we can have ANY CSV file as the source, therefore we don't know what we'll be reading and we cannot create a class to hold this information.
This means that we need to build a dynamic collection and an ExpandoObject could work here.
I wanted to use Newtonsoft.JSON library because it wil handle serialization to JSON better than I can so the plan was to be able to turn the CSV into an ExpandoObject which once it exists can easily be
transformed to JSON using the SerializeObjects method.
Potentially there is a library to handle the XML conversion as well but I have implemented my own version which probably has far to go to deal with all potential pitfalls but gives you more code to look at.


* If we are prepared to accept any CSV file then that file could contain data with commas, enclosed in quotes.  I'm going to use the Visual Basic TextFieldParser to handle this.
* Since the future requirement is to be able to convert any kind of source into any kind of output, I'd like to follow the JSON.Net example and have dedicated classes that will deserialize to object and serialize to their dedicated output.  This way we can take any source and use the dedicated object to turn it into an object.  Then take another dedicated object to convert it to the output of choice.  In order to enforce some kind of rules on that having each dedicated class implement an interface will ensure that they implement these 2 key methods.
* In the Program.cs I create an object of ISourceConverter and then initialise it it to either JSON or XML depending on the user choice - this is an indicator for using DependencyInjection frameworks or where other code wants an ISourceConverter but it doesn't need to know about which one it's getting.

Improvements

* Better Exception handling and logging to give us a better indication of where failure occurred.  Since we're using dynamic objects failure will only be known at runtime.
* XmlConverter is forcing the root element to be called root and each object is wrapped in a line element. Improvments need to be made to allow this to be modified programmatically.
* DelimitedFileConverter could be made a base class and then I can create a CSVFileConverter which makes the intention of the class clearer while still keeping flexibility underneath.
* Since we're using dynamic object failures will only occur at runtime so Unit Tests are essential here but the DelimitedFileConverter works with a file so some refactoring is required here to move the file reading elsewhere and allow DeSerializeObject to take in a string of csv and the header.
* Speed/Size of file has been ignored here.  Realistically we should prepare to accept large files and the best way to deal with those is with a StreamReader, reading the file a line at a time and processing as we go. Breaking apart the file reading from deserialize (above) will help in this matter.