## Code Standards
- Test driven development add test, check failing, do implentation leave test code unchanged
- Best code practices for the SOLID patern
- Use Solid principles
- Use MVVM design pattern for UI code
- View-ViewModel Decoupling
- Keep code files under 600 lines of code
- Keep the [architecture](../../Architecture.md) DOC updated with any changes to the project structure or design decisions

## Project code format
- use explicit this. qualifier for instance members.
- private properties should be changed to fields and moved to the top of the file
- properties and fields not modified out side of the constructor should be read only 
- private fields to be lower camel case and public properties to be upper camel case
- all methods shoud have the highest level of access modifier possible (private,private protected,protected,internal,protected internal,public)
- all methods should be Upper camel case
- all non inherited classes to be sealed 
- No comments, code to be readable no short hand varible names 
- A blank line between properties functions and constructors
- No unused interfaces types
- Correct spelling at all times
- Control statements to have on a line of its own { and also on a line of its own }
- No embedded classes they should be in their own files
- File names should match thier contained class or interface use git rename to correct this 
- use modern key word var when possible 
- Unreferenced methods, properties, fields should be removed