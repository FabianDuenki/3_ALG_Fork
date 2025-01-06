namespace FloatingPoint {
    public static class Integer {
        /// <summary>
        /// Java: Integer.highestOneBit()
        /// Returns an {@code int} value with at most a single one-bit, in the
        /// position of the highest-order("leftmost") one-bit in the specified
        /// int-value. Returns zero if the specified value has no
        /// one-bits in its two's complement binary representation, that is, if it
        /// is equal to zero.
        /// </summary>
        /// <param name="i">the value whose highest one bit is to be computed</param>
        /// <returns>an int-value with a single one-bit, in the position
        /// of the highest-order one-bit in the specified value, or zero if
        /// the specified value is itself equal to zero.</returns>
        public static int HighestOneBit(int i) {
            return i & (int.MinValue >>> NumberOfLeadingZeros(i));
        }

        /// <summary>
        /// Java: Integer.numberOfLeadingZeros
        /// Returns the number of zero bits preceding the highest-order
        /// ("leftmost") one-bit in the two's complement binary representation
        /// of the specified int-value.Returns 32 if the
        /// specified value has no one-bits in its two's complement representation,
        /// in other words if it is equal to zero.
        /// 
        /// <p>Note that this method is closely related to the logarithm base 2.
        /// For all positive int-values x:
        /// <ul>
        /// <li>floor(log<sub>2</sub>(x)) = 31 - numberOfLeadingZeros(x)
        /// <li> ceil(log <sub> 2 </sub> (x)) = 32 - numberOfLeadingZeros(x - 1)
        /// </ul>
        /// </summary>
        /// <param name="i">the value whose number of leading zeros is to be computed</param>
        /// <returns>the number of zero bits preceding the highest-order
        ///      ("leftmost") one-bit in the two's complement binary representation
        ///      of the specified int-value, or 32 if the value
        ///      is equal to zero.</returns>
        public static int NumberOfLeadingZeros(int i) {
            // HD, Count leading 0's
            if (i <= 0)
                return i == 0 ? 32 : 0;
            int n = 31;
            if (i >= 1 << 16) { n -= 16; i >>>= 16; }
            if (i >= 1 << 8) { n -= 8; i >>>= 8; }
            if (i >= 1 << 4) { n -= 4; i >>>= 4; }
            if (i >= 1 << 2) { n -= 2; i >>>= 2; }
            return n - (i >>> 1);
        }

        /// <summary>
        /// Java: Integer.numberOfTrailingZeros
        /// Returns the number of zero bits following the lowest-order ("rightmost")
        /// one-bit in the two's complement binary representation of the specified
        /// int-value.Returns 32 if the specified value has no
        /// one-bits in its two's complement representation, in other words if it is
        /// equal to zero.
        /// </summary>
        /// <param name="i">i the value whose number of trailing zeros is to be computed</param>
        /// <returns>the number of zero bits following the lowest-order ("rightmost")
        ///     one-bit in the two's complement binary representation of the
        ///     specified int-value, or 32 if the value is equal
        ///     to zero.</returns>
        public static int NumberOfTrailingZeros(int i) {
            // HD, Count trailing 0's
            i = ~i & (i - 1);
            if (i <= 0) return i & 32;
            int n = 1;
            if (i > 1 << 16) { n += 16; i >>>= 16; }
            if (i > 1 << 8) { n += 8; i >>>= 8; }
            if (i > 1 << 4) { n += 4; i >>>= 4; }
            if (i > 1 << 2) { n += 2; i >>>= 2; }
            return n + (i >>> 1);
        }
    }
}
