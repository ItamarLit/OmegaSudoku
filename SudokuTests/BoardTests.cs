using OmegaSudoku.GameLogic;
using OmegaSudoku.Interfaces;
using OmegaSudoku.IO;

namespace SudokuTests
{
    [TestClass]
    public class BoardTests
    {
        // Tests for solvable 9*9 boards
        [TestMethod]
        public void boardTest1SizeNine()
        {
            //Arrange
            string input = "900800000000000500000000000020010003010000060000400070708600000000030100400000200";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "972853614146279538583146729624718953817395462359462871798621345265934187431587296");
        }

        [TestMethod]
        public void boardTest2SizeNine()
        {
            //Arrange
            string input = "400000805030000000000700000020000060000080400000010000000603070500200000104000000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "417369825632158947958724316825437169791586432346912758289643571573291684164875293");
        }

        [TestMethod]
        public void boardTest3SizeNine()
        {
            //Arrange
            string input = "507084000008000070000100000000040002000000000900020000000001000070000200000350708";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "567284931218936574349175826735849162624517389981623457852791643173468295496352718");
        }

        [TestMethod]
        public void boardTest4SizeNine()
        {
            //Arrange
            string input = "005300000800000020070010500400005300010070006003200080060500009004000030000009700";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "145327698839654127672918543496185372218473956753296481367542819984761235521839764");
        }

        [TestMethod]
        public void boardTest5SizeNine()
        {
            //Arrange
            string input = "000006000059000008200008000045000000003000000006003054000325006000000000000000000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "487156329659732148231498567945261873823574691716983254174325986362849715598617432");
        }

        // Tests for solvable 16*16 boards
        [TestMethod]
        public void boardTest1SizeSixteen()
        {
            //Arrange
            string input = "000=5;7000<6304150800000000?0000030000>00;20000570;00000>00000002000900:0750000;0090400>0200<700;00?600=00>901030000?00080=000>0010000<640007?0=000029000300;0000<7003000008402000000:=0007<0069300>0<805:120076000;00043600500>005:0001009000;40600000008000000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "@>?=5;72:9<638415284<6:@=13?>;97<3618?>97;24@=:57:;9=413>58@26?<2=><983:?75164@;18934@5>62;:<7=?;@:?672=<4>981536547?1;<8@=39:>29128@5<64>:;7?3=:?=@29471365;><8><7613?;9=@8452:4;35>:=82?7<1@6934@>;<8?5:12=976=91;:2@436?75<8>875:3>61@<9=?2;4?6<27=95;84>:31@");
        }

        [TestMethod]
        public void boardTest2SizeSixteen()
        {
            //Arrange
            string input = "01002=:000<80>@00965@>?0=70010000@3000100000<=7;00>=53000000?090@>?000;326=901000=10000@5000000058:00400300702007009:0=>0810;<300;5>0:09000<=06700<01?500;7:000>1000007000402;032400<000@>80501:0<@0000000203000020007@:0?0300010?4309600050@:000001004080:09000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "41;?2=:793<86>@5<965@>?4=7;213:8:@329618>5?4<=7;87>=53<;:@61?492@>?<78;326=9:1543=146<2@5:>;78?958:;?4913<@7>2=67629:5=>481?;<3@?;5>4:89123<=@67=3<@1?526;7:894>1:86>@7=?9452;<32497<;36@>8=5?1:9<@:=1>5;426378?62=8;7@:<?9345>1;?43896<715>@:2=>571324?8=:@96;<");
        }

        [TestMethod]
        public void boardTest3SizeSixteen()
        {
            //Arrange
            string input = "0<00;04000>0@0070600000?;1308>20@0;01060082=<?00080>9250@0?006;:80?950;06000:@00475=6380000@01<000<:?=100;9800040@620000<30?98=00?7@80904063=05>002040:700000060000600009007300@5;000?0080=04002?0000074390:5<00:001<930=7@600?;6400:@?8>0;270000000>6=0?0010008";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "2<1?;84=:6>9@53796:4@7<?;1358>2=@5;31:6>782=<?4978=>9253@4?<16;:81?95<;26=74:@>3475=63892>:@;1<?3><:?=1@5;986274;@6274>:<31?98=51?7@8;9<4263=:5>=3284>:71@5;?96<>:46=5219?<73;8@5;9<3?@68:=>4712?=>;2174398:5<@6:281<935=7@6>4?;6435:@?8><;27=91<9@7>6=;?54123:8");
        }

        [TestMethod]
        public void boardTest4SizeSixteen()
        {
            //Arrange
            string input = "00;:00007050@000002008;000300:09000001:39008000060004@050>:0;00<00800?0001005000?000000400;<00030:00008@0000400>020000003=95000:4@00?0=0000092570000@4100000<000907>0006000:=00000000020000006>00608<0?0052000700000030:@0000900000=02>000?00300>00009000800000=";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "83;:269<745?@>=15>21=8;?<@367:497<@4>1:39;=8?5626=?94@751>:2;83<;987:?3241>@5=<6?1>69=548:;<27@33:=5;<8@2?67419><24@176>3=958;?:4@:3?>=86<1;9257=862@419537><?:;9;7>35<6?28:=41@1?5<7:2;=9@436>8:698<;?=>5231@74271;534:@6<=>98?@4<=82>1:7?963;5>53?69@7;841:<2=");
        }

        [TestMethod]
        public void boardTest5SizeSixteen()
        {
            //Arrange
            string input = "60400;>1090<=2701000700?000:000>>00000=0?01030808:9;45300070001097;6100><30?080000000:<000050060@0006700:8=000?900:504?0@100003008000002679000=30000507:2000@<000003<986000004000000?00@500010>00@09=0:73<;800003;0:00000>0600@0?1040008=@200090=0670?@00501002;";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "634?@;>1895<=27:12@=786?;43:95<>>57<:2=9?61@3;848:9;453<>27=?61@97;61@2><34?:85=2=?89:<37;>54@61@431675;:8=2<>?9<>:584?=@169;732:8<@;>1267945?=3491>5=7:2?83@<;6;?53<9861=@>24:776=2?34@5:<;19>85@29=6:73<;8>14?3;8:21954>?67=@<?1>43<;8=@276:95=<67>?@495:1832;");
        }

        // Tests for solvable 25*25 boards
        [TestMethod]
        public void boardTest1SizeTwentyFive()
        {
            //Arrange
            string input = "0E487:009200I300000=<;0?0090:50>00G=1B00;60A<87FE003000=1BC00;0?070008:5@9200D=1<00?080FE450920000006?A0;80FE400092>I0G0010C0E48705I000003G00000100?0<90:0I>B30H10C00F00<;00E4000H>B1000=0000<@E4075I92:CD=060F?A07@E40I00:50B30H00000000000I02:B0GH>10000D=00000A00@94070000I00GH>A00FE00487030:5C0H00600=000009030:00C000?0006000<;2:5I0B000>6?0=1EA<;0@9000G0>B060D01FEA<;94870002:0:5000CD00B?A=004<0F092870H0BC00A010040;02800930:50=1000000000200@0:0I3CDH>000FE40087030:000000C0A=108709230:0IC00>BA=06000<007090:GH5I0D0>B00160A08;F05I00H00>00A006080FE00:000>000=A000048;0E00002G000006?A000;FE2:7@905I3G000000FE402:0@90H003000CDA<100";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "FE487:5@92H>I3G1BCD=<;6?A@92:5H>I3G=1BCD;6?A<87FE4I3GH>=1BCD<;6?A7FE48:5@92BCD=1<;6?A87FE45@92:H>I3G6?A<;87FE4:5@92>I3GH=1BCDE487@5I92:>B3GH6CD=1;F?A<92:5I>B3GH16CD=F?A<;7@E483GH>B16CD=;F?A<@E4875I92:CD=16;F?A<7@E48I92:5>B3GH?A<;F7@E485I92:B3GH>16CD=D=16?FEA<;@948732:5IBCGH>A<;FE@9487I32:5CGH>B6?D=1487@9I32:5BCGH>?D=16FEA<;2:5I3BCGH>6?D=1EA<;F@9487GH>BC6?D=1FEA<;9487@I32:5:5I3GCDH>B?A=164<;FE9287@H>BCD?A=16E4<;F287@93G:5I=16?AE4<;F9287@G:5I3CDH>B<;FE49287@3G:5IDH>BC?A=1687@923G:5ICDH>BA=16?E4<;F7@92:GH5I3D=>BC<16?A48;FE5I3GHD=>BCA<16?8;FE42:7@9>BCD=A<16?48;FE:7@92GH5I316?A<48;FE2:7@9H5I3GD=>BC;FE482:7@9GH5I3=>BCDA<16?");
        }

        [TestMethod]
        public void boardTest2SizeTwentyFive()
        {
            //Arrange
            string input = "00085:002900H400000;>=0?0020:30<00G;1F00=600005B0004000;0FC00=0?050008:3@2900D;1>00?080B0730290000006?A0=80B0700029<H0G0010C0078503H000004G00000100?0>20:0H<F40I10C00B00>=0007000I<F1000;0000>@07053H29:0D;060B?A05@070H00:30F40I00000000000H09:F0GI<10000D;00000A00@27050000H00GI<A00BE00785040:3C0I00600;000002040:00C000?0006000>=900H0F000<6?0;1E0>=0@2000G0<F060D01BE0>=27850009:0:3000CD00F?0;007>0B029850I0FC000010070=09800240:30;1000000000900@0:0H4CDI<000BE70085040:000000C0A;108502940:0HC00<FA;06000>005020:GI3H0D0<F00160A08=B03H00I00<00A0060800E00:000<000;A000078=0E00009G000006?A000=009:5@203H4G000000BE709:0@20I004000CDA>100";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "BE785:3@29I<H4G1FCD;>=6?A@29:3I<H4G;1FCD=6?A>85BE7H4GI<;1FCD>=6?A5BE78:3@29FCD;1>=6?A85BE73@29:I<H4G6?A>=85BE7:3@29<H4GI;1FCDE785@3HC9:<F4GI62D;1=B?A>29:3H<F4GI16CD;B?A>=5@E784GI<F16ED;=B?A>@C7853H29:CD;16=B?A>5@278HE9:3<F4GI?A>=B5@2783HE9:F4GI<16CD;D;16?BEA>=@278549:3HFCGI<A>=BE@2785H49:3CGI<F6?D;1785@2H49:3FCGI<?D;16BEA>=9:3H4FCGI<6?D;1EA>=B@2785GI<FC6?D;1BEA>=2785@H49:3:3H4GCDI<F?A;167>=BE2985@I<FCD?A;16E7>=B985@24G:3H;16?AE7>=B2985@G:3H4CDI<F>=BE72985@4G:3HDI<FC?A;1685@294G:3HCDI<FA;16?E7>=B5@29:GI3HED;<FC>16?A78=B43HCGID;<F4A>16?8=BE79:5@2<F4D;A>16?78=BE:5@29GI3HC16?A>78=BC9:5@2I3H4GD;<FE=BE789:5@2GI3H4;<FCDA>16?");
        }

        [TestMethod]
        public void EmptyBoardTestSize1()
        {
            //Arrange
            string input = "0";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "1");
        }

        [TestMethod]
        public void EmptyBoardTestSize4()
        {
            //Arrange
            string input = "0000000000000000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "1234431221433421");
        }

        [TestMethod]
        public void EmptyBoardTestSize9()
        {
            //Arrange
            string input = "000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "123456789789231456456879123548917632361528974297364518612743895834195267975682341");
        }

        [TestMethod]
        public void EmptyBoardTestSize16()
        {
            //Arrange
            string input = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "123456789:;<=>?@=>?@234156789:;<9:78;<?@4=>2365156;<:9=>3?1@47283=9671;582?>@<:4>@82=?6<:345;179:;<5849367@12?>=?417@>:2<;9=5386@74:6829>153?=<;;9=?<5>:24867@138<213;@=?9:7654>65>3471?@<=;:892736>?@541829<;=:2?@=1:<;7>6489354859>=37;@<:126?<1:;9286=53?>4@7");
        }

        [TestMethod]
        public void EmptyBoardTestSize25()
        {
            //Arrange
            string input = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "123456789:;<=>?@ABCDEFGHIEFGHI234516789:;<=>?@ABCD<=>?@ADEFGBCHI231456:;9876;ABC=<>HIDEF@G789:2345?189:D7;?@BC1345AFGHEI62<=>=5;<:F@9D8GH16I472AC?B3>E93@1A<>;6H7DCBE5:?IF8G24=C7BF235:E=84>?@6;DHGI<A19G4DI>?C27A:9<;38=E1BF6@5HH6E8?1IBG4F=2A593>@<7:D;C7:C9;5HAI?@BG3<D>16428=EF2@F648E7C9HI;=>GBA<51D?:3DB153@:G;2A?E4FHC7=8>9I6<I<HA=BFD1>98627:E;?3GC4@5>?8GE46<=351:CDIF@29B7HA;@G4EHC9F<B3A7D6?5:8=;1>I23A5C671?@;EGI:=29FB>DH8<4?I=>9:83254@B1;AH<D7CEFG6BD72FG=64E<>5H8CI3;1A?:9@:1<;8IAH>DC2?F9E46G@=573BA89@<EB13FI;DGH=2C4:5>67?F>?3DHGI8<=:9EB1657;4@C2A4C67BD;=:@253<1>?89AHIEFG5HI:192C?6>FA74B@G3E<=;D8;E2=G>45A7?6@8C<DIFH931B:");
        }

        // Solved Board Test

        [TestMethod]
        public void SolvedBoardTest()
        {
            //Arrange
            string input = "972853614146279538583146729624718953817395462359462871798621345265934187431587296";
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            //Assert
            Assert.IsTrue(OutputHandler.GetBoardStr(board) == "972853614146279538583146729624718953817395462359462871798621345265934187431587296");
        }
    }
}
