using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
using Homeclick.vn.Module.BusinessObjects;
using DevExpress.ExpressApp.Security.Strategy;

namespace Homeclick.vn.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();


            User userAdmin = ObjectSpace.FindObject<User>(new BinaryOperator("UserName", "Admin"));
            if(userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<User>();
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");
            }
			// If a role with the Administrators name doesn't exist in the database, create this role
            CustomSecurityRole adminRole = ObjectSpace.FindObject<CustomSecurityRole>(new BinaryOperator("Name", "Administrators"));
            if(adminRole == null) {
                adminRole = ObjectSpace.CreateObject<CustomSecurityRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
			userAdmin.SecurityRoles.Add(adminRole);
     		ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
        }
        
    }
}
