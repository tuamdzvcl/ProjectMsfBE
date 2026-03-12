using EventTick.Model.Enum;
using projectDemo.Entity.Enum;

namespace projectDemo.DTO.UpdateRequest
{
    public class TypeTicketUpdate
    {
        public EnumNameTickType Name { get; set; }
        public int TotalQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public EnumStatusTickType Status { get; set; }
    }
}
