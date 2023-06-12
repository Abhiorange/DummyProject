using DataModels.DataModel;
using DataModels.ViewModels;
using DataRepository.Interface;

namespace DataRepository
{
    public class UserRepository:IUser
    {
        private readonly AramexContext _aramexContext;

        public UserRepository(AramexContext aramexContext)
        {
            _aramexContext = aramexContext;
        }

        public UserViewModel Login(LoginViewModel model)
        {
            var user = _aramexContext.Users.FirstOrDefault(u => u.Email.Equals(model.Email.ToLower()));
            if (user == null)
            {
                return null;
            }
            var userpassword = _aramexContext.Users.FirstOrDefault(u => u.Password.Equals(model.Password) && u.Email.Equals(model.Email.ToLower()));
            if (userpassword == null)
            {
                return null;
            }
            else
            {
                var passwordsMatch = string.Compare(model.Password, userpassword.Password, StringComparison.Ordinal) == 0;
                if (passwordsMatch == false)
                {
                    return null;
                }
            }
            UserViewModel authuser = new UserViewModel
            {
                UserId=userpassword.UserId,
                Email=userpassword.Email,
                Password=userpassword.Password,
                UserName=userpassword.UserName,
            };
            return authuser;
        }
        public bool Register(RegisterViewModel model)
        {
            var existingUser = _aramexContext.Users.FirstOrDefault(u => u.Email == model.Email);

            if (existingUser != null)
            {
                return false;
            }
            else
            {
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = model.Password,
                };
                _aramexContext.Add(user);
                _aramexContext.SaveChanges();
                return true;
            }
        }

    }
}