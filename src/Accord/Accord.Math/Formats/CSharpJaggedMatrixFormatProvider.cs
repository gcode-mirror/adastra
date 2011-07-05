﻿// Accord Math Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Math.Formats
{
    using System.Globalization;

    /// <summary>
    ///   Gets the matrix representation used in C# jagged arrays.
    /// </summary>
    /// 
    public sealed class CSharpJaggedMatrixFormatProvider : MatrixFormatProviderBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpJaggedMatrixFormatProvider"/> class.
        /// </summary>
        public CSharpJaggedMatrixFormatProvider(CultureInfo culture)
            : base(culture)
        {
            FormatMatrixStart = "new double[][] {\n";
            FormatMatrixEnd = " \n};";
            FormatRowStart = "    new double[] { ";
            FormatRowEnd = " }";
            FormatColStart = ", ";
            FormatColEnd = ", ";
            FormatRowDelimiter = ",\n";
            FormatColDelimiter = ", ";

            ParseMatrixStart = "new double[][] {";
            ParseMatrixEnd = "};";
            ParseRowStart = "new double[] {";
            ParseRowEnd = "}";
            ParseColStart = ",";
            ParseColEnd = ",";
            ParseRowDelimiter = "},";
            ParseColDelimiter = ",";
        }

        /// <summary>
        ///   Gets the IMatrixFormatProvider which uses the CultureInfo used by the current thread.
        /// </summary>
        /// 
        public static CSharpJaggedMatrixFormatProvider CurrentCulture
        {
            get { return currentCulture; }
        }

        /// <summary>
        ///   Gets the IMatrixFormatProvider which uses the invariant system culture.
        /// </summary>
        /// 
        public static CSharpJaggedMatrixFormatProvider InvariantCulture
        {
            get { return invariantCulture; }
        }

        private static readonly CSharpJaggedMatrixFormatProvider currentCulture =
            new CSharpJaggedMatrixFormatProvider(CultureInfo.CurrentCulture);

        private static readonly CSharpJaggedMatrixFormatProvider invariantCulture =
            new CSharpJaggedMatrixFormatProvider(CultureInfo.InvariantCulture);

    }
}
