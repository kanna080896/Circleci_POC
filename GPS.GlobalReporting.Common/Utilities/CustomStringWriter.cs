using System.Text;

namespace GPS.GlobalReporting.Common.Utilities
{
    /// <summary>
    /// The purpose of this seemingly odd class is to enable the application to specify the encoding of an XML Serializer; default is UTF-16
    /// </summary>
    public class CustomStringWriter : StringWriter
    {
        private Encoding CustomEncoding { get; set; }
        public override Encoding Encoding => CustomEncoding; // this is an existing 'get'-only property in StringWriter class

        public CustomStringWriter(Encoding customEncoding)
        {
            CustomEncoding = customEncoding;
        }
    }
}
