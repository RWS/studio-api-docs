Working with Filters
=======
The section describes how to create translation unit filters that can be used with operations such as import, export and search.

Overview
----
Translation unit filters are represented by the abstract **FilterExpression** class, which represents a boolean expression that works on translation units fields. There are two concrete classes that derive from this:

* **AtomicExpression**: A filter expression that compares a single translation unit field against a specified value using the specified operator.
* **ComposedExpression**: A filter expression that can be used to build complex filter expression from other filter expression, either by combining two filter expressions using the AND or NOT operator, or by negating a filter expression. Composed filter expressions can be nested.
The field values (**FieldValue**) that can be used in an atomic filter expression include:

* **User-defined fields**: See Working with Field Definitions.
* **System fields**: These are a number of built-in translation unit fields as defined in SystemFieldDefinitions.



<img style="display:block; " src="images/Filters.png"/>