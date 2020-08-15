namespace Cornerstone
{
    public static class ContentSerializersExtensions
    {
        public static string ToMimeString(this ContentSerializers serializer)
        {
            switch (serializer)
            {
                case ContentSerializers.Json:
                    return "application/json";

                case ContentSerializers.Xml:
                    return "application/xml";

                default:
                    return "application/octet-stream";
            }
        }
    }
}
