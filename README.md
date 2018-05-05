# AlgebraicTermRewriter

 - Parses and tokenizes mathematical sentences, then simplifies them by rewriting them, before solving them. It operates on algebraic expressions and equations.

 - The archetecture allows for solving inequalities and systems of equations as well, but I have not expressly handled these these types of equations yet.

 - Currently, the archetecture expects all numeric values to be whole-number integers. Negatives are allowed. 

### Here are some notes on the terminology and concepts used in this project:

A mathematical **sentence** is either an mathematical **expression** or an **equation**.

An **Expression** can contain a combination of numbers, variables and operations, but does not equate anything (does not contain an equals, greater-than or less-than symbol). For example: 7 - y

An **Equation** consists of two expressions separated by a comparative token, such as an equals symbol or a less-than symbol. For example: 7 - y = 0

The project allows you to supply it with a **Problem**, which is one or more mathematical sentences, seperated by a line break. An example would be a system of equations.
  
  
Both the Expression class and the Equation class derive from the **ISentence** interface, because they are mathematical sentences.

 - **INumber** and **IVariable** derive from the **ITerm** interface, which in turn, is derived from **IToken**. An example of an IToken that does not derive from ITerm, is **IOperator**.

 - **IVariable** is a variable, such as X or Y, that stand in for a numeric value in a mathematical expression or equation. Solutions will be presented as a Tuple of an IVariable and its numeric value.

 - **INumber** is an term in a mathematical sentence that holds numeric value, and can be directly operated upon by a mathematical operation. For our purposes, this will always be whole number integer.
 
 - **IOperator** is, of course, a mathematical operation such as addition, subtraction, multiplication or division.


A mathematical expression is parsed and tokenized into a collection of **ITokens**. An IToken is the most basic atomic element in a mathematical sentence.

When parsing, one cannot simply strip out whitespace and then convert each character in a string into a token, because a single token does not necessarily consits of a single character. An example of a token that is more than a single character would be a numeric value greater than 9.
