namespace Domain.Cache.Abstractions.Dtos
{
    public class CacheRequestDto
    {
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Resolution { get; set; }
        public string Watermark { get; set; }
        public string BackgroundColour { get; set; }

    }
}
