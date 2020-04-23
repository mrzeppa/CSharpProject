using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Controllers;
using LibraryProject.Fakes;
using Xunit;

namespace LibraryProjectTest
{
    public class TestDoubles : IDisposable
    {
        public TestDoubles() { }

        [Fact]
        public async Task a()
        {
            var chuj = new FakeAuthorRepository();
            var cont = new AuthorsController(chuj);

            var result = await cont.Index();

            result = result;
        }

        public void Dispose()
        {
        }
    }
}
