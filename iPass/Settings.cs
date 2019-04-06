using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace iPass
{
    class Settings
    {
        public String getUsername()
        {
            return UserData.Default.username;
        }

        public String getPassword()
        {
            return UserData.Default.password;
        }

        public void setUsername(String username)
        {
            UserData.Default.username = username;
        }

        public void setPassword(String password)
        {
            UserData.Default.password = password;
        }

        public Boolean hasUsername()
        {
            return !String.IsNullOrEmpty(getUsername());
        }

        public Boolean hasPassword()
        {
            return !String.IsNullOrEmpty(getPassword());
        }

        public void save()
        {
            UserData.Default.Save();
        }
    }
}
