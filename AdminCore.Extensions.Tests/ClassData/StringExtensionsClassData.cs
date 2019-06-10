using System;
using System.Collections;
using System.Collections.Generic;

namespace AdminCore.Extensions.Tests.ClassData
{
    public class StringExtensionsClassData
    {
        public class ValidStringWithSeparatorsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // ARGS: stringToSplit: string, separator: char, rangeMin: int, splitExpected: IList<string>
                yield return new object[]
                {
                    "A_B_C_D",
                    '_',
                    0,
                    new List<string> {"A", "B", "C", "D"}
                };
                yield return new object[]
                {
                    "A_B_C_D",
                    '_',
                    1,
                    new List<string> {"B", "C", "D"}
                };
                yield return new object[]
                {
                    "A_B_C_D",
                    '_',
                    2,
                    new List<string> {"C", "D"}
                };
                yield return new object[]
                {
                    "A_B_C_D",
                    '_',
                    3,
                    new List<string> {"D"}
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class ValidStringWithSeparatorsAndInvalidRangeMinClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // ARGS: stringToSplit: string, separator: char, rangeMin: int
                yield return new object[]
                {
                    "A_B_C_D",
                    '_',
                    5
                };
                yield return new object[]
                {
                    "A_B_C_D",
                    '_',
                    -1
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class ValidStringWithIncorrectSeparatorsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // ARGS: stringToSplit: string, separator: char, rangeMin: int

                yield return new object[]
                {
                    "A_B_C_D",
                    '/',
                    0,
                    new List<string>{"A_B_C_D"}
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
