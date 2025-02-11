using OmegaSudoku.Exceptions;
using OmegaSudoku.GameLogic;
using OmegaSudoku.Interfaces;
using OmegaSudoku.IO;


namespace SudokuTests
{
    [TestClass]
    public class ErrorTests
    {
        // In the tests for unsolvable boards the input should be the same as the output
        [TestMethod]
        public void Test1UnsolvableSize9() 
        {
            string input = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
            //Assert
            Assert.IsTrue(TestUtils.TestBoard(input) == "000005080000601043000000000010500000000106000300000005530000061000000004000000000");
        }

        [TestMethod]
        public void Test2UnsolvableSize9()
        {
            string input = "000030000060000400007050800000406000000900000050010300400000020000300000000000000";
            //Assert
            Assert.IsTrue(TestUtils.TestBoard(input) == "000030000060000400007050800000406000000900000050010300400000020000300000000000000"); 
        }

        [TestMethod]
        public void Test1UnsolvableSize16()
        {
            string input = ";0?0=>010690000000710000500:?0;4000000<0400070=005<3000800000000500@000:?80>10004<30>?8;00=20000>?8;270060000000000000900000000?0000?00000>0=000?3:0000>0026000000;>61029@0<00000100<0@00:40000800500:0?;>012600800?0;0000090<0@0;07000005<00?8:00003050:4080709";
            //Assert
            Assert.IsTrue(TestUtils.TestBoard(input) == ";0?0=>010690000000710000500:?0;4000000<0400070=005<3000800000000500@000:?80>10004<30>?8;00=20000>?8;270060000000000000900000000?0000?00000>0=000?3:0000>0026000000;>61029@0<00000100<0@00:40000800500:0?;>012600800?0;0000090<0@0;07000005<00?8:00003050:4080709");
        }

        // Tests for different Exceptions

        [TestMethod]
        public void TestInvalidBoard()
        {
            //Arrange
            string input = "100002000001000000000000000000000000000000000000000000000000000000000000000000000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            //Act
            Exception exc = null;
            try
            {
                ISolver solver = new SudokuSolver(board);
            }
            catch (Exception e) 
            {
                exc = e;
            }
            //Assert
            Assert.IsInstanceOfType(exc, typeof(InvalidInitialBoardException));
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            //Arrange
            string input = "";
            //Act
            Exception exc = null;
            try
            {
                InputValidator.CheckInput(input);
            }
            catch (Exception e)
            {
                exc = e;
            }
            //Assert
            Assert.IsInstanceOfType(exc, typeof(EmptyInputException));
        }

        [TestMethod]
        public void TestInvalidBoardSize()
        {
            //Arrange
            string input = "00";
            //Act
            Exception exc = null;
            try
            {
                InputValidator.CheckInput(input);
            }
            catch (Exception e)
            {
                exc = e;
            }
            //Assert
            Assert.IsInstanceOfType(exc, typeof(BoardSizeException));
        }

        [TestMethod]
        public void TestInvalidCellInfo()
        {
            //Arrange
            string input = "0000000000005000";
            //Act
            Exception exc = null;
            try
            {
                InputValidator.SetUpBoard(input);
            }
            catch (Exception e)
            {
                exc = e;
            }
            //Assert
            Assert.IsInstanceOfType(exc, typeof(CellInfoExeption));
        }
    }
}
