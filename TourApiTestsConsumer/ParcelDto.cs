using System;

namespace DotnetSampleApi2Test
{
    internal class ParcelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Variant { get; set; }
    }
}