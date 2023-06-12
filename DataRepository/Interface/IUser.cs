using DataModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Interface
{
    public interface IUser
    {
        public UserViewModel Login(LoginViewModel model);
        public bool Register(RegisterViewModel model);
    }
}
