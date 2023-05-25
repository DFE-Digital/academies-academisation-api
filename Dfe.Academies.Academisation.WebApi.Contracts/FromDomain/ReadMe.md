# Todo
The types in this folder were originally from the Domain.Core
I wasn't sure if it was a good idea to be sending objects to/from the webapi that are declared in Domain.Core because that means clients also get the other types in there, which may be harmless but is also adding unnecessarily to the surface area of types exposed.
I think it's better to include only the types needed and map between the two
