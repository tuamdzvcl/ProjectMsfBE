using EventTick.Model.Enum;
using EventTick.Model.Models;

namespace projectDemo.Data.Seed
{
    public  class GetRoleSeed
    {
        public static List<Role> GetRole() { 
            return new List<Role>() 
            { 
                new Role { Id = 1, RoleName = EnumRoleName.ADMIN},
                new Role { Id = 2, RoleName = EnumRoleName.ORGANIZER},
                new Role { Id = 3, RoleName = EnumRoleName.CUSTOMER},
            }; 
        
        }
    }
}
