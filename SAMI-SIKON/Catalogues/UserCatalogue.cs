using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAMI_SIKON.Interfaces;

namespace SAMI_SIKON.Catalogues
{
    public class UserCatalogue: ICatalogue<IUser>
    {
        private List<IUser> _users;
        private IUser _currentUser;
        
        
        public void Create(IUser u)
        {
            throw new NotImplementedException();
        }
            
        public IUser Read(int i)
        {
            IUser result = null;
            foreach (var user in _users)
            {
                if (user.Id == i)
                {
                    result = user;
                }
            }
            return result;
        }

        public void Update(IUser pre, IUser post)
        {
            foreach (var user in _users)
            {
                if (user.Id == post.Id)
                {
                    user.Id = post.Id;
                    user.Email = post.Email;
                }
            }
        }

        public IUser Delete(int i)
        {
            throw new NotImplementedException();
        }

        public List<IUser> ReadAll()
        {
            throw new NotImplementedException();
        }
    }
}
