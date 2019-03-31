using System;

namespace NBL.Models.EntityModels.Clients
{
   public class ClientAttachment
    {
        public int Id { get; set; }
        public string AttachmentName { get; set; }
        public int ClientId { get; set; }

        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public int UploadedByUserId { get; set; }
        public DateTime SysDatTime { get; set; }
    }
}
