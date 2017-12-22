using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    class Operations
    {
        private char[] operators = { '+', '-', '*', '/', '%' };
        private char[] mulOperators = { '*', '/', '%' };
        private char[] addOperators = { '+', '-' };
        private char[] validOperators1 = { '*', '/', '%', '-' };
        private char[] validOperators2 = { '*', '/', '%', '+' };        
        private string result = string.Empty;
        private string[] numbers = new string[] { };
        private string[] numbers1 = new string[] { };
        private string[] numbers2 = new string[] { };

        //Takes the input from main() class and passes it to Calculator method
        // and prints the final result
        public void CalculateTheInput(string text)
        {
            result = Calculator(text);     
            int finalResult = 0;  
                  
            if(Int32.TryParse(result, out finalResult))
            {
                Console.WriteLine("Result = {0}",finalResult);
            }
            else
            {
                Console.WriteLine(result);
            }
        }

        //Passes the input for validation and then to Multiplicative and Additive 
        //operation and returns the final result as a string
        private string Calculator(string text)
        {            
            string validation = string.Empty;
            string sub = string.Empty;            

            validation = Validation(text);
            if (!(string.IsNullOrEmpty(validation))) return validation;   
                     
            text = text.Replace("++", "+").Replace("--", "+").Replace("-+", "-")
                       .Replace("+-", "-");

            if (text.IndexOf('+') == 0)
            {
                text = text.Remove(0, 1);
            }

            if ((text.IndexOf('*') > 0) || (text.IndexOf('/') > 0) || (text.IndexOf('%') > 0))
            {
                text = MultiplicativeOperation(text); 
            }
            if ((text.IndexOf('+') < 0) && (text.IndexOf('-') == 0) && 
                (text.Split('-').Length-1) == 1)
            {
                sub = text.Substring(0, 1);
                text = text.Remove(0, 1);
            }
            if ((text.IndexOf('+') > 0) || (text.IndexOf('-') >= 0))
            {
                text = AdditiveOperation(text);
            }
            if (sub != null) text = sub + text;
            return result=text;
        }

        //Validates the input
        private string Validation(string text)
        {
            string valid = string.Empty;

            //checks for any operator at the end and for multiplicative operator at the front
            if (((text.Substring(text.Length - 1, 1)).IndexOfAny(operators) >= 0) ||
                (text.IndexOfAny(mulOperators) == 0))
            {                
                return valid = "Invalid Input";
            }

            //Checks for input with only integer
            int result = 0;            
            if ((text.IndexOfAny(validOperators2) < 0) && (Int32.TryParse(text, out result)))
            {                
                return Convert.ToString(result);
            }

            if ((text.IndexOf("-*") >= 0) || (text.IndexOf("-/") >= 0) ||
                (text.IndexOf("-%") >= 0) || (text.IndexOf("+*") >= 0) ||
                (text.IndexOf("+/") >= 0) || (text.IndexOf("+%") >= 0) ||
                (text.IndexOf("---") >= 0) || (text.IndexOf("+++") >= 0) ||
                (text.IndexOf("+--") >= 0) || (text.IndexOf("--+") >= 0) ||
                (text.IndexOf("+-+") >= 0) || (text.IndexOf("-+-") >= 0) ||
                (text.IndexOf("++-") >= 0) || (text.IndexOf("-++") >= 0))
            {
                return valid = "Invalid Input";
            }

            string[] vtext = text.Split(operators, StringSplitOptions.RemoveEmptyEntries);
            long number = 0;

            foreach (string a in vtext)
            {
                if (!long.TryParse(a, out number) || number > Int32.MaxValue)
                {                    
                    return valid = "Invalid Input";
                }
            }

            if ((text.IndexOf("%0") > 0) || (text.IndexOf("/0") > 0))
            {                
                return valid = "Division by zero";
            }

            return valid;
        }

        //Splits the input according to operators and passes them to MultiplicativeCalculation
        //method for calculation 
        private string MultiplicativeOperation(string text)
        {
            string sub = string.Empty;
            string sub2 = string.Empty;

            //Loops through the input untill all the mulplicative operators are calculated
            do
            {                
                numbers = text.Split(mulOperators, 2, StringSplitOptions.RemoveEmptyEntries);

                //Checks for input like a-b*+c, -b*-c etc.
                if ((numbers[1].Substring(0, 1) == "+") || (numbers[1].Substring(0, 1) == "-"))
                {
                    sub2 = numbers[1].Substring(0, 1);
                    numbers[1] = numbers[1].Remove(0, 1);
                }

                numbers1 = numbers[0].Split(operators, StringSplitOptions.RemoveEmptyEntries);
                numbers2 = numbers[1].Split(operators, StringSplitOptions.RemoveEmptyEntries);

                int x = numbers1.Length;
                long[] temp1 = new long[x];
                temp1 = Array.ConvertAll(numbers1, long.Parse);
                int y = numbers2.Length;
                long[] temp2 = new long[y];
                temp2 = Array.ConvertAll(numbers2, long.Parse);
                long temp3 = 0;

                temp3 = MultiplicativeCalculation(text, numbers, numbers1, numbers2, temp1, 
                                                  temp2, sub2);            
                sub2 = null;
                if (temp3 > Int32.MaxValue || temp3 < -Int32.MaxValue) return "Out of Range";

                //combines into one string with the calculation result
                string text2 = Convert.ToString(temp3);
                string text1 = numbers[0].Substring(0,
                                          (numbers[0].Length - numbers1[x - 1].Length));
                string text3 = numbers[1].Substring(0 + (numbers2[0].Length));                
                text = text1 + text2 + text3;
                
                if((text.IndexOfAny(validOperators1) < 0) && (text.IndexOf('+') == 0))
                {
                    text = text.Remove(0, 1);
                }
                if ((text.IndexOfAny(validOperators2) < 0) && (text.IndexOf('-') == 0))
                {
                    sub = text.Substring(0, 1);
                    text = text.Remove(0, 1);
                }
            } while ((text.IndexOf('*') >= 0) || (text.IndexOf('/') >= 0) || 
                     (text.IndexOf('%') >= 0));

            if (sub != null) text = sub + text;            

            return result=text;
        }

        //Calculates for multiplicative operators
        private long MultiplicativeCalculation(string text, string[] numbers, string[] numbers1, 
                                               string[] numbers2, long[] temp1, long[] temp2,
                                               string sub2)
        {
            long temp3 = 0;
            int nLength1 = numbers1.Length;
                        
            //For calculation like a+b*c, a-b*c etc
            if ((text.Substring(numbers[0].Length, 1) == "*") && string.IsNullOrEmpty(sub2))
            {
                temp3 = temp1[nLength1 - 1] * temp2[0];
            }

            //For calculation like a+b*-c, -b*-c etc
            else if ((numbers[0].Length > numbers1[nLength1 - 1].Length) &&
                    (text.Substring(numbers[0].Length, 1) == "*"))
            {
                temp3 = (sub2 != numbers[0].Substring(
                         numbers[0].Length - numbers1[nLength1 - 1].Length - 1, 1)) ?
                         -temp1[nLength1 - 1] * temp2[0] : temp1[nLength1 - 1] * temp2[0];
                numbers[0] = numbers[0].Remove(
                             numbers[0].Length - numbers1[nLength1 - 1].Length - 1, 1);
                numbers[0] = (temp3 >= 0) ? numbers[0].Insert(
                          numbers[0].Length - numbers1[nLength1 - 1].Length, "+") : numbers[0];
            }

            //For calculation like a*-b, a*+b etc
            else if ((text.Substring(numbers[0].Length, 1) == "*") &&
                     !(string.IsNullOrEmpty(sub2)))
            {
                temp3 = (sub2 == "-") ? -temp1[nLength1 - 1] * temp2[0] : 
                                         temp1[nLength1 - 1] * temp2[0];
            }
            else if ((text.Substring(numbers[0].Length, 1) == "/") &&
               string.IsNullOrEmpty(sub2))
            {
                temp3 = temp1[nLength1 - 1] / temp2[0];
            }
            else if ((numbers[0].Length > numbers1[nLength1 - 1].Length) &&
                    (text.Substring(numbers[0].Length, 1) == "/"))
            {
                temp3 = (sub2 != numbers[0].Substring(
                         numbers[0].Length - numbers1[nLength1 - 1].Length - 1, 1)) ?
                          -temp1[nLength1 - 1] / temp2[0] : temp1[nLength1 - 1] / temp2[0];
                numbers[0] = numbers[0].Remove(
                             numbers[0].Length - numbers1[nLength1 - 1].Length - 1, 1);
                numbers[0] = temp3 >= 0 ? numbers[0].Insert(
                          numbers[0].Length - numbers1[nLength1 - 1].Length, "+") : numbers[0];
            }
            else if ((text.Substring(numbers[0].Length, 1) == "/") && 
                     !(string.IsNullOrEmpty(sub2)))
            {
                temp3 = (sub2 == "-") ? -temp1[nLength1 - 1] / temp2[0] : 
                                        temp1[nLength1 - 1] / temp2[0];
            }
            else if (text.Substring(numbers[0].Length, 1) == "%")
            {
                temp3 = temp1[nLength1 - 1] % temp2[0];
            }
            return temp3;
        }

        //Splits the input according to operators and passes them to Additivecalculation
        //method for calculation 
        private string AdditiveOperation(string text)
        {
            string sub = string.Empty;
            string sub1 = string.Empty;

            //Loops through the input untill all the additive operators are calculated
            do
            {
                if ((text.IndexOf('-') == 0))
                {
                    sub1 = text.Substring(0, 1);
                    text = text.Remove(0, 1);                    
                }

                numbers = text.Split(addOperators, 2, StringSplitOptions.RemoveEmptyEntries);
                
                if (!(string.IsNullOrEmpty(sub1)))
                {
                    numbers[0] = sub1 + numbers[0];                    
                    text = sub1 + text;                                        
                }
                sub1 = null;

                numbers1 = numbers[0].Split(operators, StringSplitOptions.RemoveEmptyEntries);
                numbers2 = numbers[1].Split(operators, StringSplitOptions.RemoveEmptyEntries);

                int x = numbers1.Length;
                long[] temp1 = new long[x];
                temp1 = Array.ConvertAll(numbers1, long.Parse);
                int y = numbers2.Length;
                long[] temp2 = new long[y];
                temp2 = Array.ConvertAll(numbers2, long.Parse);
                long temp3 = 0;

                temp3= AdditiveCalculation(text, numbers, numbers1, numbers2, temp1, temp2);

                if (temp3 > Int32.MaxValue || temp3 < -Int32.MaxValue) return "Out of Range";

                // combining into one string with the result
                string text2 = Convert.ToString(temp3);                
                string text1 = numbers[0].Substring(0, 
                                          (numbers[0].Length - numbers1[x - 1].Length));
                string text3 = numbers[1].Substring(0 + (numbers2[0].Length));             
                text = text1 + text2 + text3;

                if ((text.IndexOfAny(validOperators1) < 0) && (text.IndexOf('+') == 0))
                {
                    text = text.Remove(0, 1);
                }
                if ((text.IndexOfAny(validOperators2) < 0) && (text.IndexOf('-') == 0))
                {
                    sub = text.Substring(0, 1);
                    text = text.Remove(0, 1);
                }
            } while ((text.IndexOf('+') >= 0) || (text.IndexOf('-') >= 0));

            if (sub != null) text = sub + text;

            return result = text;
        }

        //Calculates for additive operators
        private long AdditiveCalculation(string text, string[] numbers, string[] numbers1,
                                         string[] numbers2, long[] temp1, long[] temp2)
        {
            long temp3 = 0;
            int nLength1 = numbers1.Length;

            //Following if else block calculates for addition and substraction
            if ((text.Substring(numbers[0].Length, 1) == "+") && (numbers[0].IndexOf('-') == 0))
            {
                temp3 = -temp1[nLength1 - 1] + temp2[0];
                numbers[0] = numbers[0].Remove(0, 1);               
            }
            else if (text.Substring(numbers[0].Length, 1) == "+")
            {
                temp3 = temp1[nLength1 - 1] + temp2[0];
            }
            else if ((text.Substring(numbers[0].Length, 1) == "-") &&
                    (numbers[0].IndexOf('-') != 0))
            {
                temp3 = temp1[nLength1 - 1] - temp2[0];
            }
            else
            {
                temp3 = -temp1[nLength1 - 1] - temp2[0];
                numbers[0] = numbers[0].Remove(0, 1);
            }

            return temp3;
        }

    }
}
