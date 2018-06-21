# ConsoleProvinceEditor
A small hack to quickly change many file's properties.

### CURRENT VERSION: 0.3.0 Changing Values
* User enters directory, and can run the 'regions' command to verify the region is in resources.
* No way to enter directory at run time yet.
* Clear and write are not yet commands, but their code exists.

### Version History

##### 0.3 Changing Values (Current Version)
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

##### 0.4 Anonymous Regions
* Allow a region clause ({ # # # }) to be defined and used in commands

##### 0.5 Conditionals
* Allow conditional clause to restrict regions. (where command.)
* Allow conditional clauses to change action. (limit command.)

##### 1.0 release
* Polish glaring errors
* Compile Bug list
* Release
