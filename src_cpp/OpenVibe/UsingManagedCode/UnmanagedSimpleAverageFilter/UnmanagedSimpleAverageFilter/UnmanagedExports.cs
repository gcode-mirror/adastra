using System;
using System.Collections.Generic;
using System.Text;
using RGiesecke.DllExport;

namespace UnmanagedSimpleAverageFilter
{
   internal static class UnmanagedExports
   {
      [DllExport("calculate", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
      static double calculate(double value)
      {
         value = value * value;

         return value;
      }
   }
}
