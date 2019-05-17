using NUnit.Framework;
using Infrastructure.Auth.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth.Imp.Tests
{
    [TestFixture()]
    public class AuthDbContextTests
    {
        [Test()]
        public void AuthDbContextTest()
        {
            using (var context = new AuthDbContext())
            {
                context.SaveChanges();
            }
        }
    }
}