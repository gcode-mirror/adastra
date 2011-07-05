// Accord Math Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Math.Geometry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///   Convexity defect.
    /// </summary>
    /// 
    public class ConvexityDefect
    {

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConvexityDefect"/> class.
        /// </summary>
        /// <param name="point">The most distant point from the hull.</param>
        /// <param name="start">The starting index of the defect in the contour.</param>
        /// <param name="end">The ending index of the defect in the contour.</param>
        /// <param name="depth">The depth of the defect (highest distance from the hull to
        /// any of the contour points).</param>
        public ConvexityDefect(int point, int start, int end, double depth)
        {
            this.Point = point;
            this.Start = start;
            this.End = end;
            this.Depth = depth;
        }

        /// <summary>
        ///   Gets or sets the starting index of the defect in the contour.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the ending index of the defect in the contour.
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// Gets or sets the most distant point from the hull characterizing the defect.
        /// </summary>
        /// <value>The point.</value>
        public int Point { get; set; }

        /// <summary>
        /// Gets or sets the depth of the defect (highest distance
        /// from the hull to any of the points in the contour).
        /// </summary>
        public double Depth { get; set; }

    }
}
