// Accord Math Library
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
    ///   Gets the matrix representation used in C# multi-dimensional arrays.
    /// </summary>
    /// 
    public sealed class CSharpMatrixFormatProvider : MatrixFormatProviderBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpMatrixFormatProvider"/> class.
        /// </summary>
        public CSharpMatrixFormatProvider(CultureInfo culture)
            : base(culture)
        {
            FormatMatrixStart = "new double[,] {\n";
            FormatMatrixEnd = " \n};";
            FormatRowStart = "    { ";
            FormatRowEnd = " }";
            FormatColStart = ", ";
            FormatColEnd = ", ";
            FormatRowDelimiter = ",\n";
            FormatColDelimiter = ", ";

            ParseMatrixStart = "new double[,] {";
            ParseMatrixEnd = "};";
            ParseRowStart = "{";
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
        public static CSharpMatrixFormatProvider CurrentCulture 
        {
            get { return currentCulture; }
        }

        /// <summary>
        ///   Gets the IMatrixFormatProvider which uses the invariant system culture.
        /// </summary>
        /// 
        public static CSharpMatrixFormatProvider InvariantCulture
        {
            get { return invariantCulture; }
        }

        
        private static readonly CSharpMatrixFormatProvider currentCulture =
            new CSharpMatrixFormatProvider(CultureInfo.CurrentCulture);

        private static readonly CSharpMatrixFormatProvider invariantCulture =
            new CSharpMatrixFormatProvider(CultureInfo.InvariantCulture);

    }
}
