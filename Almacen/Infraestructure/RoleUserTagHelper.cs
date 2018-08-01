using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Almacen.Models.Data;

namespace Almacen.Infraestructure
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUserTagHelper : TagHelper
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public RoleUserTagHelper(UserManager<User> usermgr,
                                 RoleManager<IdentityRole> rolemgr)
        {
            userManager = usermgr;
            roleManager = rolemgr;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context,
                                                TagHelperOutput output)
        {
            int quantity = 0;
            IdentityRole role = await roleManager.FindByIdAsync(Role);
            if(role != null)
            {
                IList<User> users = await userManager.GetUsersInRoleAsync(role.Name);
                quantity = users.Count;
            }
            output.Content.SetContent(quantity == 0 ? "No hay usuarios" : "" + quantity + " usuario(s)");
        }
    }
}
