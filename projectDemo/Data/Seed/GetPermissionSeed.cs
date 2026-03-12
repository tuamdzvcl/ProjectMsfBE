using EventTick.Model.Models;
using projectDemo.Entity.Models;

namespace projectDemo.Data.Seed
{
    public class GetPermissionSeed
    {
        public static List<Permissions> GetPermission()
        {
            return new List<Permissions> {
                //User
                    new Permissions{ Id =1,PermissonsName="user.create",PermissonsDescription="tạo mới user"},
                    new Permissions{ Id =2,PermissonsName="user.update",PermissonsDescription="sửa user"},
                    new Permissions{ Id =3,PermissonsName="user.delete",PermissonsDescription="xóa user"},
                    new Permissions{ Id =4,PermissonsName="user.view",PermissonsDescription="xem user"},
                //Role
                    new Permissions{ Id =5,PermissonsName="role.create",PermissonsDescription="tạo role mới"},
                    new Permissions{ Id =6,PermissonsName="role.update",PermissonsDescription=" sửa role"},
                    new Permissions{ Id =7,PermissonsName="role.delete",PermissonsDescription=" xóa role"},
                    new Permissions{ Id =8,PermissonsName="role.view",PermissonsDescription=" xem role"},
                //event
                    new Permissions{ Id =9,PermissonsName="event.create",PermissonsDescription="tạo mới event" },
                    new Permissions{ Id =10,PermissonsName="event.update",PermissonsDescription="sửa  event" },
                    new Permissions{ Id =11,PermissonsName="event.delete",PermissonsDescription="xóa  event" },
                    new Permissions{ Id =12,PermissonsName="event.view",PermissonsDescription=" xem event" },
                    new Permissions{ Id =13,PermissonsName="event.getTotalTickbyid",PermissonsDescription="xem tổng vé của event"},
                    new Permissions{ Id =14,PermissonsName="event.getTotalTickByUser",PermissonsDescription="xem tổng vé theo user"}

                };
        }
    }
}
