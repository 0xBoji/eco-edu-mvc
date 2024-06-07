namespace eco_edu_mvc.Models.Entities;

public class Conversation
{
  
        public Guid Group_Id { get; set; }
        public Guid Message_Id { get; set; }
        public Group Group { get; set; }
        public Message Message { get; set; }
    
}
