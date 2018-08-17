# ConsoleProvinceEditor
A small hack to quickly change many file's properties.

### CURRENT VERSION: 0.4 Aliases

### Version History

#### 0.4 Aliases
* Allow an anonymous region clause ({ # # # }) to be defined and used in commands
* Allow region nesting.

#### 0.3.1.1 Bug Fix for Comments (Released)
* Fixed a bug where the write method did not create its own line on a consistant basis.

##### 0.3.1 Comments
* Allows comments in region and target files.

##### 0.3 Changing Values
* Added Kill method to eliminate item.
* Added Write method to write to a file.
* Added Remove method to remove a phrase from a file.
* Added Replace method to replace one phrase with another.
* Changed direction of the project.

* WARNING: No error checking currently exists. Mistakes are difficult to fix.

##### 0.2 Regions 
* Add a text file that defines aliases for regions.

##### 0.1 Initial Commit
* Can enter regions (set of provinces) into the solution.
* Can clear all files in a region.
* Can write to all files in a region.

### Upcoming Versions

##### 0.5 Conditionals
* Allow conditional clause to restrict regions. (where command.)
* Allow conditional clauses to change action. (limit command.)

##### 0.6 Base Regions
* Compile a set of starting regions

##### 0.7 Logging
* Introduce an error log.

##### 0.8 Scripting
* Allow a user to run a script that performs multiple commands at once.

##### 1.0 release
* Polish glaring errors
* Compile Bug list
