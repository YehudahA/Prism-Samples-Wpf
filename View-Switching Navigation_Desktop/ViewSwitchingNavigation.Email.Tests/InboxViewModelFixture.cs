

using System;
using Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Email.ViewModels;
using ViewSwitchingNavigation.Infrastructure;
using System.Windows.Data;

namespace ViewSwitchingNavigation.Email.Tests
{
    [TestClass]
    public class InboxViewModelFixture
    {
        [TestMethod]
        public void WhenCreated_ThenRequestsEmailsToService()
        {
            var emailServiceMock = new Mock<IEmailService>();
            var requested = false;
            emailServiceMock
                .Setup(svc => svc.GetEmailDocumentsAsync())
                .Callback(() => requested = true);

            var viewModel = new InboxViewModel(emailServiceMock.Object, new Mock<IRegionManager>().Object);

            Assert.IsTrue(requested);
        }

        [TestMethod]
        public void WhenExecutingTheNewCommand_ThenNavigatesToComposeEmailView()
        {
            var emailServiceMock = new Mock<IEmailService>();


            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(RegionNames.MainContentRegion, new Uri("ComposeEmailView", UriKind.Relative))).Verifiable();

            var viewModel = new InboxViewModel(emailServiceMock.Object, regionManagerMock.Object);

            viewModel.ComposeMessageCommand.Execute(null);

            regionManagerMock.VerifyAll();
        }

        [TestMethod]
        public void WhenExecutingTheReplyCommand_ThenNavigatesToComposeEmailViewWithId()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock
                .Setup(svc => svc.GetEmailDocumentsAsync())
                .ReturnsAsync(new[] { email });

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(RegionNames.MainContentRegion, @"ComposeEmailView?ReplyTo=" + email.Id.ToString("N")))
                .Verifiable();

            EmailDocument[] emails = new EmailDocument[]{ email };
            var viewModel = new TestInboxViewModel(emailServiceMock.Object, regionManagerMock.Object, emails);

            Assert.IsFalse(viewModel.Messages.IsEmpty);

            viewModel.Messages.MoveCurrentToFirst();

            viewModel.ReplyMessageCommand.Execute(null);

            regionManagerMock.VerifyAll();
        }

        [TestMethod]
        public void WhenExecutingTheOpenCommand_ThenNavigatesToEmailView()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(RegionNames.MainContentRegion, new Uri(@"EmailView?EmailId=" + email.Id.ToString("N"), UriKind.Relative))).Verifiable();

            var viewModel = new InboxViewModel(emailServiceMock.Object, regionManagerMock.Object);

            viewModel.OpenMessageCommand.Execute(email);

            regionManagerMock.VerifyAll();
        }
    }

    class TestInboxViewModel : InboxViewModel
    {
        public TestInboxViewModel(IEmailService emailService, IRegionManager regionManager, EmailDocument[] emails) :
            base(emailService, regionManager)
        {
            var viewCollection = this.Messages as ListCollectionView;

            foreach (var email in emails)
            {
                viewCollection.AddNewItem(email);
            }

            viewCollection.MoveCurrentTo(null);
        }
    }
}
