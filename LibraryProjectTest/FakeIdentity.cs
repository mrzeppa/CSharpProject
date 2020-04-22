// using System;
// using System.Collections.Generic;
// using System.Text;
// using LibraryProject.Models;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using Moq;
//
// namespace LibraryProjectTest
// {
//     public class FakeUserManager : UserManager<ApplicationUser>
//     {
//         public FakeUserManager()
//             : base(new Mock<IUserStore<ApplicationUser>>().Object,
//                 new Mock<IOptions<IdentityOptions>>().Object,
//                 new Mock<IPasswordHasher<ApplicationUser>>().Object,
//                 new IUserValidator<ApplicationUser>[0],
//                 new IPasswordValidator<ApplicationUser>[0],
//                 new Mock<ILookupNormalizer>().Object,
//                 new Mock<IdentityErrorDescriber>().Object,
//                 new Mock<IServiceProvider>().Object,
//                 new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
//         {
//         }
//     }
//     public class FakeSignInManager : SignInManager<ApplicationUser>
//     {
//         public FakeSignInManager()
//             : base(new FakeUserManager(),
//                 new Mock<IHttpContextAccessor>().Object,
//                 new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
//                 new Mock<IOptions<IdentityOptions>>().Object,
//                 new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
//                 new Mock<IAuthenticationSchemeProvider>().Object, new Mock<IDefaultUserConfirmation<ApplicationUser>> ().Object)
//         { }
//     }
// }
