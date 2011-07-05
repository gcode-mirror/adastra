// Accord Core Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///   Algorithm Convergence Exception.
    /// </summary>
    /// 
    /// <remarks><para>The algorithm convergence exception is thrown in cases where a iterative
    /// algorithm could not converge to a finite solution.</para>
    /// </remarks>
    /// 
    [Serializable]
    public class ConvergenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvergenceException"/> class.
        /// </summary>
        public ConvergenceException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvergenceException"/> class.
        /// </summary>
        /// 
        /// <param name="message">Message providing some additional information.</param>
        /// 
        public ConvergenceException(string message) :
            base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvergenceException"/> class.
        /// </summary>
        /// 
        /// <param name="message">Message providing some additional information.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// 
        public ConvergenceException(string message, Exception innerException) :
            base(message, innerException) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="ConvergenceException"/> class.
        /// </summary>
        /// 
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        /// 
        protected ConvergenceException(SerializationInfo info, StreamingContext context) :
            base(info, context) { }

    }
}
