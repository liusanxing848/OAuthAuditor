using System;
using System.Collections.Generic;
using System.Text;

namespace AccessAdminAuditorV3.App
{
    public class TaskConfiguration
    {
        public static TaskConfiguration c = new TaskConfiguration();

        public enum Config
        {
            QA,
            Production,
            Gateway,
            Asterion,
        }

        public Config env;
        public Config endpoint;
    }
}
