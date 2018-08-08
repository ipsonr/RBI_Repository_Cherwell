using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CherwellApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cherwell Coding Challenge\n");
            Console.WriteLine("Challenge 1.A: Calculate the vertex coordinates for a given Row/Column label.\n");
            Console.WriteLine("Challenge 1.B: Calculate the Row/Column label for given vertex coordinates.\n");
            //string myInput = Console.ReadLine();
            string myInput = "";
            bool isExit = false;
            //Console.WriteLine("Enter A to run Challenge 1.A or B to run Challenge 1.B:");
            //myInput = Console.ReadLine();
            //while (myInput.ToUpper() != "A" || myInput.ToUpper() != "B")
            while (!isExit)
            {
                Console.WriteLine("Enter 'A' to run Challenge 1.A or 'B' to run Challenge 1.B; any other key to exit:");
                myInput = Console.ReadLine().ToUpper();
                switch (myInput)
                {
                    case "A":
                        DoChallenge1A();
                        break;
                    case "B":
                        DoChallenge1B();
                        break;
                    default:
                        isExit = true;
                        break;
                }
            }
        }

        public static void DoChallenge1A()
        {
            do
            {
                Console.WriteLine("1.A: Enter values for Row (A-F upper or lower case) and Column (1-12 no decimals)");
                Console.WriteLine("Enter a letter for the Row:");
                string myRowInput = Console.ReadLine().ToUpper();
                Console.WriteLine("Enter a number for the Column:");
                string myColumnInput = Console.ReadLine().ToUpper();
                string myError = null;
                try
                {
                    var myTuple = MyTriangle.GetRowAndColumnNumbersFromInput(myRowInput, myColumnInput, out myError);
                    if (myError == null)
                    {
                        if (myTuple != null)
                        {
                            int myRowNumber = myTuple.Item1;
                            int myColumnNumber = myTuple.Item2;
                            string myLabel = MyTriangle.GetCoordinatesFromRowAndColumn(myRowNumber, myColumnNumber, out myError);
                            if (myError == null)
                                Console.WriteLine("The coordinates for Triangle " + myRowInput + myColumnInput + " are: " + myLabel);
                        }
                    }
                    else
                        Console.WriteLine("Error: " + myError);
                }
                catch (Exception ex)
                {
                    myError = "Exception thrown: " + ex.Message;
                }
                finally
                {
                    if (myError != null)
                        Console.WriteLine("Error: " + myError);
                }


                Console.WriteLine("Press A to enter another row and column, or any other key to exit to main menu.");
            }
            while (Console.ReadLine().ToUpper().StartsWith("A"));
        }

        public static void DoChallenge1B()
        {
            do
            {
                Console.WriteLine("1.B: Triangle coordinates can be entered in any order. \nAll must be multiples of 10 from 0 to 60, separted by a comma. e.g. 10,20");
                Console.WriteLine("Enter X-Y coordinates for first vertex:");
                string myXY0 = Console.ReadLine();
                Console.WriteLine("Enter X-Y coordinates for second vertex:");
                string myXY1 = Console.ReadLine();
                Console.WriteLine("Enter X-Y coordinates for third vertex:");
                string myXY2 = Console.ReadLine();
                string myError = null;
                try
                {
                    List<Tuple<int, int>> myCorners = MyTriangle.GetCornersFromInput(new List<string> { myXY0, myXY1, myXY2 }, out myError);
                    if (myError == null)
                    {
                        if (myCorners != null)
                        {
                            MyTriangle myTest = new MyTriangle(myCorners);
                            if (myTest.HasValidCorners(out myError))
                                Console.WriteLine("The label for triangle bounded by " + myTest.MyCorners + " is: " + myTest.GetTriangleName());
                            else
                                Console.WriteLine("Error: " + myError);
                        }
                    }
                    else
                        Console.WriteLine("Error: " + myError);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception thrown: " + ex.Message);
                }
                Console.WriteLine("Press B to enter another set of vertices, or any other key to exit to main menu.");
            }
            while (Console.ReadLine().ToUpper().StartsWith("B"));
        }
    }

    public class MyTriangle
    {
        List<Tuple<int, int>> _corners;
        int myX0, myX1, myX2,myY0,myY1, myY2;
        public string MyCorners { get; set; }

        public MyTriangle(List<Tuple<int, int>> corners) 
        {
            _corners = corners;
            _corners.Sort();
            myX0 = _corners[0].Item1;
            myX1 = _corners[1].Item1;
            myX2 = _corners[2].Item1;
            myY0 = _corners[0].Item2;
            myY1 = _corners[1].Item2;
            myY2 = _corners[2].Item2;
            MyCorners = "(" + myX0.ToString() + "," + myY0.ToString() + ")-" +
                "(" + myX1.ToString() + "," + myY1.ToString() + ")-" +
                "(" + myX2.ToString() + "," + myY2.ToString() + ")";
        }

        //Tuple's Item1 is the X dimension; Item2 is the Y dimension
        private Tuple<int, int> _myRightAngleCorner;
        private bool _isRightangleCornerOdd;
        enum _myRows {A, B, C, D, E, F };

        public bool HasValidCorners(out string errorMessage)
        {
            errorMessage = "";
            if (_corners == null)
            {
                errorMessage = "Input corners cannot be null.";
                return false;
            }
            if (_corners.Count != 3)
            {
                errorMessage = "Must enter 3 inputs corners.";
                return false;
            }

            if (myX0 % 10 != 0 || myX1 % 10 != 0 || myX2 % 10 != 0 || myY0 % 10 != 0 || myY1 % 10 != 0 || myY2 % 10 != 0)
            {
                errorMessage = "Input corners must be multiples of 10.";
                return false;
            }

            if (myX0 < 0 || myX0 > 60 || myX1 < 0 || myX1 > 60 || myX2 < 0 || myX2 > 60 || myY0 < 0 || myY0 > 60 || myY1 < 0 || myY1 > 60 || myY2 < 0 || myY2 > 60)
            {
                errorMessage = "Input corners must be between 0 and 60.";
                return false;
            }

            if ((myX0 == myX1 && myY0 == myY1) || (myX0 == myX2 && myY0 == myY2) || (myX1 == myX2 && myY1 == myY2))
            {
                errorMessage = "Input corners must be unique.";
                return false;
            }

            if (((Math.Abs(myX0 - myX1) == 0 || Math.Abs(myX0 - myX1) == 10) && (Math.Abs(myX0 - myX2) == 0 || Math.Abs(myX0 - myX2) == 10) && (Math.Abs(myX1 - myX2) == 0 || Math.Abs(myX1 - myX2) == 10)) == false)
            {
                errorMessage = "Input corners cannot be more than 10 pixels apart horizontally.";
                return false;
            }

            if (((Math.Abs(myY0 - myY1) == 0 || Math.Abs(myY0 - myY1) == 10) && (Math.Abs(myY0 - myY2) == 0 || Math.Abs(myY0 - myY2) == 10) && (Math.Abs(myY1 - myY2) == 0 || Math.Abs(myY1 - myY2) == 10)) == false)
            {
                errorMessage = "Input corners cannot be more than 10 pixels apart vertically.";
                return false;
            }

            int sum0 = myX0 + myY0;
            int sum1 = myX1 + myY1;
            int sum2 = myX2 + myY2;
            if (sum0 == sum1 || sum0 == sum2 || sum1 == sum2)
            {
                errorMessage = "Triangle hypotenuse must run NorthWest to SouthEast.";
                return false;
            }

            if (myX0 + 10 == myX2 && myY0 + 10 == myY2)
            {
                _myRightAngleCorner = new Tuple<int, int>(myX1, myY1);
                if (myX0 + 10 == myX1 && myY0 == myY1)
                    _isRightangleCornerOdd = false;
                else //if (myX0 == myX1 && myY0 + 10 == myY1)
                    _isRightangleCornerOdd = true;
                return true;
            }
            else
            {
                errorMessage = "Triangle hypotenuse must run NorthWest to SouthEast.";
                return false;
            }

        }

        public string GetTriangleName()
        {
            string result = "Error";
            int myRowNum = _isRightangleCornerOdd ? _myRightAngleCorner.Item2/10 - 1 : _myRightAngleCorner.Item2 / 10;
            int myColumnNum = _isRightangleCornerOdd ? _myRightAngleCorner.Item1/5 + 1 : _myRightAngleCorner.Item1/5;
            _myRows myRow = (_myRows)Enum.ToObject(typeof(_myRows), myRowNum);
            result = myRow.ToString() + myColumnNum.ToString();
            return result;
        }

        public static Tuple<int, int> GetRowAndColumnNumbersFromInput(string myRowInput, string myColumnInput, out string errorMessage)
        {
            errorMessage = null;
            Tuple<int, int> result = new Tuple<int, int>(-1,-1);
            try
            {
                if (String.IsNullOrWhiteSpace(myRowInput))
                {
                    errorMessage = "Row input '" + myRowInput + "' is invalid.";
                    return null;
                }
                if (String.IsNullOrWhiteSpace(myColumnInput))
                {
                    errorMessage = "Column input '" + myColumnInput + "' is invalid.";
                    return null;
                }
                if (myRowInput != "A" && myRowInput != "B" && myRowInput != "C" && myRowInput != "D" && myRowInput != "E" && myRowInput != "F")
                {
                    errorMessage = "Row input '" + myRowInput + "' is invalid.";
                    return null;
                }
                int myColumnIndex = -1;
                if (!Int32.TryParse(myColumnInput, out myColumnIndex) || (myColumnIndex < 1 || myColumnIndex > 12))
                {
                    errorMessage = "Column input '" + myColumnInput + "' is invalid.";
                    return null;
                }
                myColumnIndex -= 1;
                _myRows myRowEnum;
                int myRowIndex = -1;
                if (Enum.TryParse(myRowInput, out myRowEnum))
                    myRowIndex = (int)myRowEnum;
                else
                {
                    errorMessage = "Row input '" + myRowInput + "' did not return a valid row number.";
                    return null;
                }

                result = new Tuple<int, int>(myRowIndex, myColumnIndex);//both are 0-based
            }
            catch (Exception ex)
            {
                errorMessage = "Invalid input.\n" + ex.Message;
            }
            return result;
        }

        internal static List<Tuple<int, int>> GetCornersFromInput(List<string> myInputs, out string errorMessage)
        {
            errorMessage = null;
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            try
            {
                if (myInputs.Count != 3)
                {
                    errorMessage = "Must enter 3 points.";
                    return null;
                }
                foreach (string myString in myInputs)
                {
                    if (!myString.Contains(","))
                        errorMessage = myString + " does not contain a comma.";
                    int myXNumber, myYNumber;
                    string myXcheck = myString.Split(',')[0];
                    string myYcheck = myString.Split(',')[1];
                    if (Int32.TryParse(myXcheck, out myXNumber) && Int32.TryParse(myYcheck, out myYNumber))
                        result.Add(new Tuple<int, int>(myXNumber, myYNumber));
                    else
                    {
                        errorMessage = myString + " is not a valid point.";
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Invalid input.\n" + ex.Message;
            }
            return result;
        }

        internal static string GetCoordinatesFromRowAndColumn(int myRowNumber, int myColumnNumber, out string myError)
        {
            string result = null;
            myError = null;
            bool isColumnOdd = myColumnNumber % 2 == 0;//0-based so appears reversed
            int myBaseX = isColumnOdd ? ((myColumnNumber) / 2) * 10 : ((myColumnNumber - 1) / 2) * 10;
            int myBaseY = myRowNumber * 10;
            string myStart = "(" + myBaseX.ToString() + "," + myBaseY.ToString() + ")";
            string myMiddle = isColumnOdd ? "(" + myBaseX.ToString() + "," + (myBaseY + 10).ToString() + ")" : "(" + (myBaseX + 10).ToString() + "," + myBaseY.ToString() + ")";
            string myEnd = "(" + (myBaseX + 10).ToString() + "," + (myBaseY + 10).ToString() + ")";
            result = myStart + "-" + myMiddle + "-" + myEnd;
            return result;
        }
    }
}
