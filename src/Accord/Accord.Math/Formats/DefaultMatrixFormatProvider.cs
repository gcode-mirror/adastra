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
    using System;
    using System.Globalization;

    /// <summary>
    ///   Gets the default matrix representation, where each row
    ///   is separated by a new line, and columns are separated by spaces.
    /// </summary>
    /// 
    public sealed class DefaultMatrixFormatProvider : MatrixFormatProviderBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMatrixFormatProvider"/> class.
        /// </summary>
        public DefaultMatrixFormatProvider(CultureInfo culture)
            : base(culture)
        {
            FormatMatrixStart = String.Empty;
            FormatMatrixEnd = String.Empty;
            FormatRowStart = String.Empty;
            FormatRowEnd = String.Empty;
            FormatColStart = String.Empty;
            FormatColEnd = String.Empty;
            FormatRowDelimiter = " \n";
            FormatColDelimiter = " ";

            ParseMatrixStart = String.Empty;
            ParseMatrixEnd = String.Empty;
            ParseRowStart = String.Empty;
            ParseRowEnd = String.Empty;
            ParseColStart = String.Empty;
            ParseColEnd = String.Empty;
            ParseRowDelimiter = "\n";
            ParseColDelimiter = " ";
        }

        /// <summary>
        ///   Gets the IMatrixFormatProvider which uses the CultureInfo used by the current thread.
        /// </summary>
        /// 
        public static DefaultMatrixFormatProvider CurrentCulture
        {
            get { return currentCulture; }
        }
        
        /// <summary>
        ///   Gets the IMatrixFormatProvider which uses the invariant system culture.
        /// </summary>
        /// 
        public static DefaultMatrixFormatProvider InvariantCulture
        {
            get { return invariantCulture; }
        }


        private static readonly DefaultMatrixFormatProvider currentCulture =
            new DefaultMatrixFormatProvider(CultureInfo.CurrentCulture);

        private static readonly DefaultMatrixFormatProvider invariantCulture = 
            new DefaultMatrixFormatProvider(CultureInfo.InvariantCulture);

    }
}
