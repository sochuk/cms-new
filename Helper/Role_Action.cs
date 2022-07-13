using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    public class Role_Action
    {
        private DevExpress.Utils.DefaultBoolean allow_create;
        private DevExpress.Utils.DefaultBoolean allow_update;
        private DevExpress.Utils.DefaultBoolean allow_delete;
        private DevExpress.Utils.DefaultBoolean allow_export;
        private DevExpress.Utils.DefaultBoolean allow_import;
        private DevExpress.Utils.DefaultBoolean allow_enabledisable;

        public DevExpress.Utils.DefaultBoolean Allow_Create {
            get { return allow_create; }
            set { allow_create = value; }
        }
        public DevExpress.Utils.DefaultBoolean Allow_Update
        {
            get { return allow_update; }
            set { allow_update = value; }
        }
        public DevExpress.Utils.DefaultBoolean Allow_Delete
        {
            get { return allow_delete; }
            set { allow_delete = value; }
        }
        public DevExpress.Utils.DefaultBoolean Allow_Export
        {
            get { return allow_export; }
            set { allow_export = value; }
        }
        public DevExpress.Utils.DefaultBoolean Allow_Import
        {
            get { return allow_import; }
            set { allow_import = value; }
        }
        public DevExpress.Utils.DefaultBoolean Allow_EnableDisable
        {
            get { return allow_enabledisable; }
            set { allow_enabledisable = value; }
        }
    }

}