using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.ErrorHandling
{
    public class FileNotPatientDataException : Exception
    {
        public FileNotPatientDataException() { }
        public FileNotPatientDataException(string message) : base(message) { }
        public FileNotPatientDataException(string message, Exception inner) : base(message, inner) { }
        protected FileNotPatientDataException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
 
}
