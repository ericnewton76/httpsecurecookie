using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace HttpSecureCookie.Tests
{
	[TestFixture]
	class MachineKeyWrapperTests
	{
		[Test]
		public void static_ctor_worked()
		{
			//just have to call something on the type itself.
			System.Web.Security.MachineKeyWrapper.ReferenceEquals(new object(), new object());
		}

		[Test]
		public void EncryptOrDecrypt()
		{
			byte[] data = System.Text.Encoding.UTF8.GetBytes("This is a test string.");
			byte[] mod = new byte[] { };
			System.Web.Security.MachineKeyWrapper.EncryptOrDecryptData(true, data, mod, 0, data.Length);
		}

	}
}
