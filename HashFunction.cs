using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Array;
using static System.Math;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Reflection;
using System.Numerics;

namespace HashTableExploration
{
    public class HashFunctionFactory
    {
        public HashFunction<string,int> CreateSelectDigitsHash(int[] selections)
        {
            Func<string, int> function = (x) =>
            {
                string k = (TransformString(x)).ToString();
                string temp = "";
                foreach (var selection in selections)
                {
                    temp += k[selection - 1];
                }
                return int.Parse(temp);
            };
            HashFunction<string, int> newHash = new HashFunction<string, int>(function);
            return newHash;
        }
        public HashFunction<string, int> CreateFoldingHash(int digitGroupSize, bool folding,int hashTableLength)
        {

            Func<string, int> function = (x) =>
            {
                int i = 0;
                int hashIndex = 0;
                char[] temp;
                
                string k = (TransformString(x)).ToString();
                
                do
                {
                    if (i % 2 == 0 || !folding)
                    {                        
                        hashIndex += int.Parse(k.Substring(i * digitGroupSize, digitGroupSize));
                    }
                    else
                    {
                        temp = k.Substring(i * digitGroupSize, digitGroupSize).ToCharArray();
                        Array.Reverse(temp);                        
                        hashIndex += int.Parse(string.Join("", temp));
                    }
                    i++;


                } while ((i + 1) * digitGroupSize < k.Length);

                if ( k.Length==1)
                {
                    return CollectLowerMagnitudeDigits(hashTableLength, hashIndex);
                }
                

                if (i % 2 == 0 || !folding)
                {                    
                    hashIndex += int.Parse(k.Substring(i * digitGroupSize));
                }
                else
                {
                    temp = k.Substring(i * digitGroupSize).ToCharArray();
                    Array.Reverse(temp);                    
                    hashIndex += int.Parse(string.Join("", temp));
                }
                return CollectLowerMagnitudeDigits(hashTableLength,hashIndex);
            };
            HashFunction<string, int> newHash = new HashFunction<string, int>(function);
            return newHash;
        }

        public HashFunction<string, int> CreateMiddleSquareHash(int digits)
        {
            // Does not work for a digits value of 10 or greater
            if (digits < 10)
            {
                Func<string, int> function = (x) =>
                {
                    string result = ((int)BigInteger.Pow(TransformString(x), 2)).ToString();                                        
                    if (result.Length > digits)
                    {
                        int j = (int)Math.Ceiling((float)((result.Length - digits) / 2));                        
                        int index = int.Parse(result.Substring(j, digits));
                        return index;
                    }
                    else
                    {
                        return int.Parse(result);
                    }
                };
                HashFunction<string, int> newHash = new HashFunction<string, int>(function);
                return newHash;
            }
            else { return null; };
        }

        public HashFunction<string,int> CreateDivisionHash(int divisor)            
        {   
            Func<string, int> function = (x) => (int)(TransformString(x) % divisor);                
            HashFunction<string, int> newHash = new HashFunction<string,int>(function);
            return newHash;           
        }
        public BigInteger TransformString(string input)
        {
            string transformedString = string.Empty;

            foreach (char c in input)
            {                
                if (char.IsLetter(c))
                {
                    char lowerCaseChar = char.ToLower(c);
                    long position = lowerCaseChar - 'a' + 1;
                    transformedString += position.ToString();
                }
                else if (char.IsDigit(c))
                {
                    
                    transformedString += c;
                }                
            }

            if (BigInteger.TryParse(transformedString, out BigInteger result))
            {                
                return result;
            }
            else
            {
                throw new FormatException("Failed to parse the transformed string to a long integer format.");
            }
        }

        public static int CollectLowerMagnitudeDigits(int hashTableLength, int foldedValue)
        {
            string foldedValueString = foldedValue.ToString();
            string hashTableLengthString = hashTableLength.ToString();

            int orderOfMagnitude = hashTableLengthString.Length;

            string lowerMagnitudeDigits = string.Empty;

            if (foldedValueString.Length > orderOfMagnitude)
            {
                lowerMagnitudeDigits = foldedValueString.Substring(foldedValueString.Length - orderOfMagnitude);
            }
            else if (foldedValueString.Length < orderOfMagnitude)
            {
                lowerMagnitudeDigits = foldedValueString;
            }
            else if (foldedValueString.Length == orderOfMagnitude)
            {
                lowerMagnitudeDigits = foldedValueString.Substring(1);
            }
            //WriteLine($"The inputs are table length {hashTableLength} and foldedVal {foldedValue} we've calculated {lowerMagnitudeDigits} as the returnable digits");
            //ReadKey();
            return int.Parse(lowerMagnitudeDigits);
        }
    }

    public class HashFunction<TInputs, TOutput>
    {
        private Func<TInputs, TOutput> hashFunction;

        public HashFunction(Func<TInputs, TOutput> function)
        {
            this.hashFunction = function;
        }
        public TOutput Hash(TInputs inputs)
        {
            return this.hashFunction(inputs);
        }


    }
}
