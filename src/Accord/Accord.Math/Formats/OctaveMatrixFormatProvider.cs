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
    using System;
    using System.Globalization;

    /// <summary>
    ///   Format provider for the matrix format used by Octave (and MATLAB).
    /// </summary>
    /// 
    public sealed class OctaveMatrixFormatProvider : MatrixFormatProviderBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="OctaveMatrixFormatProvider"/> class.
        /// </summary>
        public OctaveMatrixFormatProvider(CultureInfo culture)
            : base(culture)
        {
            FormatMatrixStart = "[";
            FormatMatrixEnd = "]";
            FormatRowStart = String.Empty;
            FormatRowEnd = String.Empty;
            FormatColStart = String.Empty;
            FormatColEnd = String.Empty;
            FormatRowDelimiter = "; ";
            FormatColDelimiter = " ";

            ParseMatrixStart = "[";
            ParseMatrixEnd = "]";
            ParseRowStart = String.Empty;
            ParseRowEnd = String.Empty;
            ParseColStart = String.Empty;
            ParseColEnd = String.Empty;
            ParseRowDelimiter = "; ";
            ParseColDelimiter = " ";
        }

        /// <summary>
        ///   Gets the IMatrixFormatProvider which uses the CultureInfo used by the current thread.
        /// </summary>
        /// 
        public static OctaveMatrixFormatProvider CurrentCulture
        {
            get { return currentCulture; }
        }

        /// <summary>
        ///   Gets the IMatrixFormatProvider which uses the invariant system culture.
        /// </summary>
        /// 
        public static OctaveMatrixFormatProvider InvariantCulture
        {
            get { return invariantCulture; }
        }


        private static readonly OctaveMatrixFormatProvider invariantCulture =
            new OctaveMatrixFormatProvider(CultureInfo.InvariantCulture);

        private static readonly OctaveMatrixFormatProvider currentCulture =
            new OctaveMatrixFormatProvider(CultureInfo.CurrentCulture);

    }
}
