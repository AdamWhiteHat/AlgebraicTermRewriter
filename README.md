# AlgebraicTermRewriter

 - Parses and tokenizes mathematical sentences, then simplifies them by rewriting them, before solving them. It operates on algebraic expressions and equations.

 - The architecture allows for solving inequalities and systems of equations as well, but I have not expressly handled these these types of equations yet.

 - Currently, the architecture expects all numeric values to be whole-number integers. Negatives are allowed. 

### Here are some notes on the terminology and concepts used in this project:

A mathematical **sentence** is either an mathematical **expression** or an **equation**.

An **Expression** can contain a combination of numbers, variables and operations, but does not equate anything (does not contain an equals, greater-than or less-than symbol). For example: 7 - y

An **Equation** consists of two expressions separated by a comparison token, such as an equals symbol or a less-than symbol. For example: 7 - y = 0

The project allows you to supply it with a **Problem**, which is one or more mathematical sentences, separated by a line break. An example would be a system of equations.
  
  
Both the Expression class and the Equation class derive from the **ISentence** interface, because they are mathematical sentences.

 - **INumber** and **IVariable** derive from the **ITerm** interface, which in turn, is derived from **IToken**. An example of an IToken that does not derive from ITerm, is **IOperator**.

 - **IVariable** is a variable, such as X or Y, that stand in for a numeric value in a mathematical expression or equation. Solutions will be presented as a Tuple of an IVariable and its numeric value.

 - **INumber** is an term in a mathematical sentence that holds numeric value, and can be directly operated upon by a mathematical operation. For our purposes, this will always be whole number integer.
 
 - **IOperator** is, of course, a mathematical operation such as addition, subtraction, multiplication or division.


A mathematical expression is parsed and tokenized into a collection of **ITokens**. An IToken is the most basic atomic element in a mathematical sentence.

Once all the sentences in a problem have been tokenized, each sentence is passed through a number of rewrite rules, which are visited in a particular order. Each rewrite rule has some condition it looks for, and will run if that condition is present in the sentence. For example, there is a rewrite rule to isolate variables on one side of the equality symbol, and it looks for the condition that all variables will have at least one other term with it on the same side of the equality symbol. It will then choose the symbol that has the fewest terms sharing the same side of the equality symbol with it and attempt to isolate that one first. It does this by looking for operator-term pairs and sending them to the other side, preferring to do select terms with the addition or subtraction operator first, then multiplication or division, then power or root. There is special built-in function for sending operator-term pairs to the other side, which functions by looking at the operator and replacing it with its inverse operation (e.g. subtraction for addition, or division for multiplication) and placing it either at the beginning or the end of the expression on the other side of the equality symbol, depending on the operator associativity.

The first sentence will be sent to each rewrite rule in order, then the second sentence will be sent to each rule, and so on, until each sentence has made 1 pass through all the rules. Then the solver will then start with the first sentence again, sending it to each rewrite rule again, so that the rewrite rules will get applied iteratively, simplifying the sentence more and more with each pass. The solver will continue like this until a complete pass is made without any rewrites taking place on any of the sentences. At that point, the solver will halt, the transformed sentences will be displayed and (hopefully) the equations will be solved OR a contradiction will be readily apparent.
