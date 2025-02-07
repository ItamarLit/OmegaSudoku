# Sudoku solver
**Welcome to the amazing sudoku solver**

![image](https://github.com/user-attachments/assets/0f0ae0fa-c6be-4623-b62f-25f67c8e4e57)

This solver can solve any board that is smaller than 25 by 25 in less than a second, and it can also solve 25 by 25 boards fast.
The solver also tells you if the inputed board is unsolvable or illegal.

## Features
The solver uses bitwise operations and advanced sudoku heuristics to solve the boards fast.

1. Naked pairs

2. Naked singles

3. Hidden pairs

4. Hidden singles

5. Minimum remaining values with most filled spot heuristic

The project is written in a generic way, I chose to use bitwise actions to make the solver faster but there is an interface for Cells on the board so it doesnt matter how the cells are implemented, the same goes for the solver.

## How to run

**In order to run this project, clone the repo and run the sln.**

In this project you can give input in two ways:

1. By console, write "solve_c" input the board and watch the magic happen.
   
3. By file, write "solve_f" input the path, you can see the output by console or go into the input file and see the output message there.

![image](https://github.com/user-attachments/assets/8b02f032-dbf1-4d0b-b6c0-acd2bd0f2017)


**To see the rules of the project while running write 'rules'**
## Tests
This project has tests using mstest, to run the tests go to SudokuTests right click and click "Run Tests", I have written many tests, including tests for invalid boards and exceptions.

![image](https://github.com/user-attachments/assets/e5977062-442b-4d6d-a726-1df6a7d11755)
