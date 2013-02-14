using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using System.Web;

namespace HttpSecureCookie.Tests
{
	[TestFixture]
	class HttpCookieEncryptionTests
	{
		[Test]
		public void static_ctor_worked()
		{
			//just have to call something on the type itself.
			System.Web.HttpCookieEncryption.Equals(new object(), new object());
		}

		[Test]
		public void Encrypt_Cookie()
		{
			string startingValue = "asdf";

			HttpCookie cookie = new HttpCookie("test");
			cookie.Value = startingValue;

			cookie = HttpCookieEncryption.Encrypt(cookie);
			string actualValue = cookie.Value;

			Assert.AreNotEqual(startingValue, actualValue);
		}

		[Test]
		public void Encrypt_Decrypt_roundtrip()
		{
			string startingValue = "asdf";

			HttpCookie cookie = new HttpCookie("test");
			cookie.Value = startingValue;

			HttpCookie encrypted = HttpCookieEncryption.Encrypt(cookie);

			HttpCookie decrypted = HttpCookieEncryption.Decrypt(encrypted);

			Assert.AreEqual(decrypted.Value, cookie.Value);
		}

	}
}
