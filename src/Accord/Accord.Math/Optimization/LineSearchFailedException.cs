﻿// Accord Math Library
// The Accord.NET Framework
// http://accord-net.origo.ethz.ch
//
// Copyright © César Souza, 2009-2011
// cesarsouza at gmail.com
// http://www.crsouza.com
//

namespace Accord.Math.Optimization
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;


    /// <summary>
    ///   Line Search Failed Exception.
    /// </summary>
    /// 
    /// <remarks>
    ///   This exception may be thrown by the <see cref="LbfgsOptimizer">L-BFGS Optimizer</see>
    ///   when the line search routine used by the optimization method fails.
    /// </remarks>
    /// 
    [Serializable]
    public class LineSearchFailedException : Exception
    {
        int info;

        /// <summary>
        ///   Gets the error code information returned by the line search routine.
        /// </summary>
        /// 
        /// <value>The error code information returned by the line search routine.</value>
        /// 
        public int Information { get { return info; } }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LineSearchFailedException"/> class.
        /// </summary>
        /// 
        public LineSearchFailedException()
            : base()
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LineSearchFailedException"/> class.
        /// </summary>
        /// 
        /// <param name="info">The error code information of the line search routine.</param>
        /// <param name="message">Message providing some additional information.</param>
        /// 
        public LineSearchFailedException(int info, string message)
            : base(message)
        {
            this.info = info;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LineSearchFailedException"/> class.
        /// </summary>
        /// 
        /// <param name="message">Message providing some additional information.</param>
        /// 
        public LineSearchFailedException(string message)
            : base(message)
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LineSearchFailedException"/> class.
        /// </summary>
        /// 
        /// <param name="message">Message providing some additional information.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// 
        public LineSearchFailedException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LineSearchFailedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        /// 
        protected LineSearchFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        ///   When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>
        /// 
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            base.GetObjectData(info, context);

            info.AddValue("Information", this.info);
        }
    }

}
