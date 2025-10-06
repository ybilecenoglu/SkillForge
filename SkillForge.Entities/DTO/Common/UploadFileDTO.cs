namespace SkillForge.Entities.DTO.Common
{
    public class UploadFileDTO 
    {
        public int id { get; set; }
		public int order { get; set; }
		public string name { get; set; }
		public string originalName { get; set; }
		public string uuid { get; set; }
		public int size { get; set; }
		public string status { get; set; }
    }
}