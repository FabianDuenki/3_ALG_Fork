namespace FloatingPoint.Tests {
    [TestClass]
    public sealed class FloatingPointAdditionTests {
        [TestMethod]
        public void TestPositiveFloats() {
            TestTwoFloats(5.1f, 2.8f);
            TestTwoFloats(2.8f, 5.1f);
            TestTwoFloats(1.5f, 3.25f);
            TestTwoFloats(3.25f, 1.5f);
        }

        [TestMethod]
        public void TestNegativeFloats() {
            TestTwoFloats(-5.1f, 2.8f);
            TestTwoFloats(2.8f, -5.1f);
            TestTwoFloats(1.5f, -3.25f);
            TestTwoFloats(-3.25f, 1.5f);
            TestTwoFloats(-1.5f, -3.25f);
            TestTwoFloats(-5.1f, -2.8f);
        }

        [TestMethod]
        public void TestSubnormalNumbers() {
            float zero = 0.0f;
            float value = 419037.1875f;
            float nonZeroMantissa1 = 5.90381056005e-41f;
            float nonZeroMantissa2 = 7.35996705665e-39f;

            TestTwoFloats(zero, value);
            TestTwoFloats(value, zero);
            TestTwoFloats(zero, zero);
            TestTwoFloats(nonZeroMantissa1, nonZeroMantissa2);
            TestTwoFloats(nonZeroMantissa2, zero);
        }

        private void TestTwoFloats(float firstValue, float secondValue) {
            float expectedResult = firstValue + secondValue;
            int val1 = BitConverter.SingleToInt32Bits(firstValue);
            int val2 = BitConverter.SingleToInt32Bits(secondValue);
            int result = FloatingPointAddition.Addition(val1, val2);

            float actualResult = BitConverter.Int32BitsToSingle(result);

            Assert.AreEqual(expectedResult, actualResult, 0.0001, $"Addition of {firstValue} and {secondValue} failed.");
        }
    }
}
